using System;
using System.Collections.Generic;
using System.Text;

namespace MessagebrokerPublisher.Contracts
{
    public interface INotification
    {
        public Subject Subject { get; }
    }
}
