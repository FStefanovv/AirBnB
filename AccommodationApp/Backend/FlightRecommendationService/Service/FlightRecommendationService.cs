using FlightRecommendationService.Model;
using Google.Protobuf.Collections;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace FlightRecommendationService.Service
{
    public class FlightRecommendationService
    {
        public async Task<List<FlightRecommendation>> GetRecommendationsFor(FlightRequirements requirements)
        {

            List<FlightRecommendation> recommendations = await GetRecommendationsGrpc(requirements);

            return recommendations;
        }

        private async Task<List<FlightRecommendation>> GetRecommendationsGrpc(FlightRequirements requirements)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://localhost:5010",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new FlightGRPCService.FlightGRPCServiceClient(channel);
            var reply = await client.GetRecommendationsAsync(new FlightRequirementsGrpc
            {
                DeparturePoint = requirements.DeparturePoint,
                DepartureDate = Timestamp.FromDateTime(requirements.DepartureDate)
            });

            return ConvertToFlightRecommendations(reply.Recommendations_);
        }

        private List<FlightRecommendation> ConvertToFlightRecommendations(RepeatedField<Recommendation> grpcRecommendations_)
        {
            List<FlightRecommendation> recommendations = new List<FlightRecommendation>();
            foreach (var grpcRecommendation in grpcRecommendations_)
            {
                var recommendation = new FlightRecommendation(grpcRecommendation.FlightId, grpcRecommendation.DepartureTime.ToDateTime(), grpcRecommendation.Duration, grpcRecommendation.TicketPrice);

                recommendations.Add(recommendation);
            }
            return recommendations;
        }
    }
}
