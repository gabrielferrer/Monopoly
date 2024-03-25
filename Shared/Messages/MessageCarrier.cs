using ProtoBuf;
using System;

namespace Shared.Messages
{
    [ProtoContract]
    public class MessageCarrier
    {
        public MessageCarrier()
        {
        }

        [ProtoMember(1)]
        public Type MessageType { get; set; }

        [ProtoMember(2)]
        public byte[] Message { get; set; }
    }
}
