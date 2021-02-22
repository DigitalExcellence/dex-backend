/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using NotificationSystem.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace NotificationSystem.Services
{

    public interface IRabbitMQListener
    {

        EventingBasicConsumer CreateConsumer(INotificationService notificationService);

        void StartConsumer(EventingBasicConsumer consumer, string subject);

    }

    public class RabbitMQListener : IRabbitMQListener
    {

        private readonly IModel channel;

        public RabbitMQListener(IModel channel)
        {
            this.channel = channel;
        }

        public EventingBasicConsumer CreateConsumer(INotificationService notificationService)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            SetCallBack(notificationService, consumer);
            return consumer;
        }

        public void StartConsumer(EventingBasicConsumer consumer, string subject)
        {
            channel.BasicConsume(subject,
                                 false,
                                 consumer);
        }

        public EventingBasicConsumer SetCallBack(INotificationService notificationService, IBasicConsumer basicConsumer)
        {
            EventingBasicConsumer consumer = (EventingBasicConsumer) basicConsumer;
            consumer.Received += (sender, ea) => { CallBack(notificationService, ea); };
            return consumer;
        }

        public void CallBack(INotificationService notificationService, BasicDeliverEventArgs ea)
        {
            byte[] body = ea.Body.ToArray();
            string jsonBody = Encoding.UTF8.GetString(body);

            try
            {
                notificationService.ParsePayload(jsonBody);
                notificationService.ValidatePayload();
                notificationService.ExecuteTask();
                channel.BasicAck(ea.DeliveryTag, false);
                Console.WriteLine("Task executed");
            } catch(Exception e)
            {
                Console.WriteLine("Task failed");
                Console.WriteLine(e.Message);
            }
        }

    }

}
