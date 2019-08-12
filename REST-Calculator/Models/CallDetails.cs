using REST_Calculator.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST_Calculator.Models
{
    public class CallDetails
    {
        public int ID;
        public int ParentID;
        public DateTime InsertDate;
        public string Value;
        public MessageType messageType;
        public CallDetails(int callId, MessageType type)
        {
            InsertDate = DateTime.Now;
            messageType = type;
            Value = GetDetailsText(callId, messageType);
            ParentID = callId;
        }


        private string GetDetailsText(int callId, MessageType type)
        {
            return String.Format(messages[type], callId);
        }

        private Dictionary<MessageType, string> messages = new Dictionary<MessageType, string>
        {
            { MessageType.RequestToJson, "Call {0}. Request to JSON" },
            { MessageType.RequestToSoap, "Call {0}. Request to SOAP" },
            { MessageType.ResponseFromSoap, "Call {0}. Response from SOAP" },
        };
    }
}