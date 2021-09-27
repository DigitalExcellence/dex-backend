using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Resources
{
    public class SendGridParamaters
    {
        public Guid  Guid{ get; set; }
        public string Name { get; set; }

        public SendGridParamaters(Guid guid, string name)
        {
            Guid = guid;
            Name = name;
        }
    }

}
