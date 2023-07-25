using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

using Users.Adapters;
using Users.DTO;
using Users.Model;
using Users.Repository;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using Grpc.Net.Client;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;

using Grpc.Core;
using OpenTracing;

using Users.RabbitMQ;
using MassTransit;


namespace Users.Services
{
    public class UserService : UserGRPCService.UserGRPCServiceBase, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string key;
        private readonly ITracer _tracer;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public UserService(IUserRepository userRepository, IConfiguration configuration, ISendEndpointProvider sendEndpointProvider, ITracer tracer)
        {
            _userRepository = userRepository;
            _sendEndpointProvider = sendEndpointProvider;
            key = configuration.GetSection("JwtKey").ToString();
            _tracer = tracer;
        }

        public TokenDTO Authenticate(LoginCredentialsDTO credentials)
        {
            var user = _userRepository.GetUserWithCredentials(credentials);

            if (user == null) return null;

            DateTime expires = DateTime.Now.AddHours(1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim("UserId", user.Id as string),
                    new Claim("Email", user.Email as string),
                    new Claim("Username", user.Username as string),
                    new Claim("Role", user.Role as string)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenDTO(tokenHandler.WriteToken(token));
        }


        public SuccessfulRegistrationDTO Register(RegistrationDTO registrationData)
        {
            try
            {
                Validate(registrationData);
                User user = UserAdapter.RegistrationDtoToUser(registrationData);
                _userRepository.Create(user);
                return new SuccessfulRegistrationDTO(user.Username);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Validate(RegistrationDTO registrationData)
        {
            if (_userRepository.CheckIfEMailInUse(registrationData.Email))
                throw new Exception("The entered email is already in use!");
            else if (_userRepository.CheckIfUsernameInUse(registrationData.Username))
                throw new Exception("The entered username is already in use!");
            else if (registrationData.Password != registrationData.ConfirmPassword)
                throw new Exception("Password and confirmation password need to be the same!");
            else if (registrationData.Password.Length <= 6)
                throw new Exception("Password is too short");
            else if (registrationData.Role.Length == 0)
                throw new Exception("Choose type");
        }


        public User GetUser(StringValues userId)
        {
            return _userRepository.GetUser(userId);
        }

        public User UpdateUser(StringValues userId, UserChangeInfoDTO changeData)
        {
            try
            {
                User user = _userRepository.GetUser(userId);
                User changedUser = UserAdapter.UpdateUserDTOToUser(user, changeData);
                _userRepository.UpdateUser(changedUser);

                return changedUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> DeleteAsGuest(StringValues id)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://localhost:5003",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new ReservationGRPCService.ReservationGRPCServiceClient(channel);
            var reply = await client.GuestHasActiveReservationsAsync(new UserData
            {
                Id = id,

            });

            if (reply.IsReservationActive == false)
            {
                User user = _userRepository.GetById(id);
                _userRepository.Delete(user);
                bool isUpdated = await UpdateRequestsPostUserDeletion(id);
                return isUpdated;
            }
            else
            {
                throw new Exception("You have active reservation");
            }

        }

        public async Task<bool> UpdateRequestsPostUserDeletion(StringValues id)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://localhost:5003",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new ReservationGRPCService.ReservationGRPCServiceClient(channel);
            var reply = await client.UpdateRequestsPostUserDeletionAsync(new UserData
            {
                Id = id,

            });
            return reply.IsUpdated;

        }


        public async Task<bool> DeleteAsHost(StringValues id)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://localhost:5003",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new ReservationGRPCService.ReservationGRPCServiceClient(channel);
            var reply = await client.HostHasActiveReservationsAsync(new UserData
            {
                Id = id,

            });

            if (reply.IsReservationActive == false)
            {
                User user = _userRepository.GetById(id);

                bool isDeletedAccomodation = await DeleteAccWithoutHost(id);
                _userRepository.Delete(user);


                return isDeletedAccomodation;
            }
            else
            {
                throw new Exception("You have active reservation");
            }


        }

        public async Task<bool> DeleteAccWithoutHost(StringValues id)
        {

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://localhost:5002",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new AccommodationGRPCService.AccommodationGRPCServiceClient(channel);
            var reply = await client.DeleteAccwithoutHostAsync(new UserId
            {
                Id = id,

            });

            return reply.IsDeleted;
        }



        public override Task<ReservationPartUpdated> IsReservationPartSatisfied(ReservationSatisfied isReservationSatisfied, ServerCallContext context)
        {
            using var scope = _tracer.BuildSpan("IsReservationPartSatisfied").StartActive(true);
            User host = _userRepository.GetUserById(isReservationSatisfied.Id);
            if (host.Role == "HOST")
            {
                if (isReservationSatisfied.ReservationPartSatisfied)
                {
                    host.IsReservationPartSatisfied = true;
                    _userRepository.UpdateUser(host);
                    return Task.FromResult(new ReservationPartUpdated
                    {
                        IsReservationSatisfied = true
                    });
                }
                else
                {
                    host.IsReservationPartSatisfied = false;
                    _userRepository.UpdateUser(host);
                    return Task.FromResult(new ReservationPartUpdated
                    {
                        IsReservationSatisfied = false
                    });
                }
            }
            else
            {
                return Task.FromResult(new ReservationPartUpdated
                {
                    IsReservationSatisfied = false
                });
            }
        }


        private async Task<string> UpdateAccomodationsByDistinguishedHost(String id, bool change)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://localhost:5002",
                    new GrpcChannelOptions { HttpHandler = handler });
            var client = new AccommodationGRPCService.AccommodationGRPCServiceClient(channel);
            var reply = await client.UpdateDistinguishedHostAppointmentsAsync(new HostIdAndDistinguishedStatus
            {
                Id = id,
                HostStatus = change
            });
            return reply.Id;

        }
        public async Task<bool> DeleteAsHostSaga(string id)
        {
            SagaState state = SagaState.PENDING_DELETE;
            bool userUpdated = _userRepository.UpdateUserSaga(id,state);
            if (userUpdated)
            {
                var endPoint = await _sendEndpointProvider.
                GetSendEndpoint(new Uri("queue:" + BusConstants.StartDeleteQueue));
                await endPoint.Send<IUserMessage>(new { Id = id });
            }


            return true;


        }
    }
}
