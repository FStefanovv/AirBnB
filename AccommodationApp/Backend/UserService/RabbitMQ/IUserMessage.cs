namespace Users.RabbitMQ
{
    public class IUserMessage
    {
        public string Id { get; set;}

        public IUserMessage()
        {
        }

        public IUserMessage(string id)
        {
            Id = id;
        }
    }
}
