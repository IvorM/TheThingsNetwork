using System;
using System.Collections.Generic;
using System.Text;

namespace TtnLib
{
    public class Message
    {
        public int port { get; set; }
        public bool confirmed { get; set; }
        public string payload_raw { get; set; }
        public Payload_Fields payload_fields { get; set; }
    }

    public class Payload_Fields
    {
        public string message { get; set; }
    }
}
