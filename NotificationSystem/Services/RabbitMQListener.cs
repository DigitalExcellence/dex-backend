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

    /// <summary>
    ///     The interface of the rabbit MQ listener
    /// </summary>
    public interface IRabbitMQListener
    {

        /// <summary>
        /// The create consumer method
        /// </summary>
        /// <param name="notificationService"></param>
        /// <returns></returns>
        EventingBasicConsumer CreateConsumer(INotificationService notificationService);

        /// <summary>
        ///     The start consumer method
        /// </summary>
        /// <param name="consumer"></param>
        /// <param name="subject"></param>
        void StartConsumer(EventingBasicConsumer consumer, string subject);

    }

    /// <summary>
    ///     The rabbit MQ listener class
    /// </summary>
    public class RabbitMQListener : IRabbitMQListener
    {

        private readonly IModel channel;

        /// <summary>
        ///     The rabbit MQ listener constructor
        /// </summary>
        /// <param name="channel"></param>
        public RabbitMQListener(IModel channel)
        {
            this.channel = channel;
        }

        /// <summary>
        ///     The create consumer method start create a consumer
        /// </summary>
        /// <param name="notificationService"></param>
        /// <returns></returns>
        public EventingBasicConsumer CreateConsumer(INotificationService notificationService)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            SetCallBack(notificationService, consumer);
            return consumer;
        }

        /// <summary>
        ///     The start consumer starts listening to a channel
        /// </summary>
        /// <param name="consumer"></param>
        /// <param name="subject"></param>
        public void StartConsumer(EventingBasicConsumer consumer, string subject)
        {
            channel.BasicConsume(subject,
                                 false,
                                 consumer);
        }

        /// <summary>
        ///     The set callback method
        /// </summary>
        /// <param name="notificationService"></param>
        /// <param name="basicConsumer"></param>
        /// <returns></returns>
        public EventingBasicConsumer SetCallBack(INotificationService notificationService, IBasicConsumer basicConsumer)
        {
            EventingBasicConsumer consumer = (EventingBasicConsumer) basicConsumer;
            consumer.Received += (sender, ea) => { CallBack(notificationService, ea); };
            return consumer;
        }

        /// <summary>
        ///     The callback method
        /// </summary>
        /// <param name="notificationService"></param>
        /// <param name="ea"></param>
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
