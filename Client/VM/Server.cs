using ProtoBuf;

namespace Monopoly.VM
{
    [ProtoContract]
    public class Server : ViewModelBase
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public string Address { get; set; }

        [ProtoMember(3)]
        public int Port { get; set; }

        public static bool operator ==(Server a, Server b)
        {
            if (ReferenceEquals(null, a))
            {
                return ReferenceEquals(null, b);
            }

            return a.Equals(b);
        }

        public static bool operator !=(Server a, Server b)
        {
            if (ReferenceEquals(null, a))
            {
                return !ReferenceEquals(null, b);
            }

            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            return obj is Server server && server.Name == Name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashcode = 23;
                hashcode = (hashcode * 31) + Name?.GetHashCode() ?? 0;
                return hashcode;
            }
        }
    }
}
