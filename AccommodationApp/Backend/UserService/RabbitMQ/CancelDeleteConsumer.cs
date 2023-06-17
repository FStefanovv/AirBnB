using MassTransit;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Users.Repository;

namespace Users.RabbitMQ
{
    public class CancelDeleteConsumer : IConsumer<IUserMessage>
    {
        private readonly IUserRepository _userRepository;

        public CancelDeleteConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task Consume(ConsumeContext<IUserMessage> context)
        {
            var data = context.Message;
            SagaState state = SagaState.NOT_DELETED;
            _userRepository.UpdateUserSaga(data.Id, state); 


        }
    }
}
