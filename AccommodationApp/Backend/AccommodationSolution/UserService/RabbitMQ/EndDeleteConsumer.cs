using MassTransit;
using System.Threading.Tasks;
using Users.Model;
using Users.Repository;

namespace Users.RabbitMQ
{
    public class EndDeleteConsumer  : IConsumer<IUserMessage>
    {
        private readonly IUserRepository _userRepository;

        public EndDeleteConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task Consume(ConsumeContext<IUserMessage> context)
        {
            var data = context.Message;
            User user = _userRepository.GetById(data.Id);
            _userRepository.Delete(user);
        }
    }
}
