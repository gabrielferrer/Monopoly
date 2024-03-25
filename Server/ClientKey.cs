namespace Server
{
    class ClientKey
    {
        public ClientKey()
        {
        }

        public ClientKey(ClientKey clientKey)
        {
            Address = string.Copy(clientKey.Address);
            Port = clientKey.Port;
        }

        public override bool Equals(object obj)
        {
            return obj is ClientKey clientKey
                && Address == clientKey.Address
                && Port == clientKey.Port;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashcode = 23;
                hashcode = (hashcode * 31) + Address?.GetHashCode() ?? 0;
                hashcode = (hashcode * 31) + Port.GetHashCode();
                return hashcode;
            }
        }

        public string Address { get; set; }

        public int Port { get; set; }
    }
}
