using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.HelperClasses;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingNotificationController : ControllerBase
    {

        private INotificationSender notificationSender = new NotificationSender();
        // POST api/<TestingNotificationController>
        [HttpPost]
        public string Post()
        {
            try
            {
                notificationSender.RegisterNotification("message to send", "email");
                return "succes";
            } catch (Exception e)
            {
                return e.Message;
            }
            
            
        }

    }
}
