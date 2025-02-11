﻿namespace Users.RabbitMQ
{
    public class BusConstants
    {
        public const string RabbitMqUri = "rabbitmq://rabbit/";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string StartDeleteQueue = "reservations_queue";
        public const string CancelDeleteQueue = "hasReservation_queue";
        public const string EndDeleteQueue = "end_delte_queue";
        public const string NotificationQueue = "notification_queue";
    }
}
