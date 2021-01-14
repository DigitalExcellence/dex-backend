using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBrokerPublisher
{
    public enum Subject
    {
        EMAIL,
        ELASTIC_CREATE_OR_UPDATE,
        ELASTIC_DELETE,
        ELASTIC_DELETE_ALL,
        ELASTIC_CREATE_INDEX
    }
}
