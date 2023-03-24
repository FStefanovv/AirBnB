using Flights.Model;
using Flights.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Service
{
    public class UsersService
    {
        private readonly UsersRepository _usersRepository;

        public UsersService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public List<User> GetAll()
        {
            return _usersRepository.GetAll();
        }
    }
}
