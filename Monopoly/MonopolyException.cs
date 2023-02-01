using System;

namespace Monopoly
{
    public class MonopolyException : Exception
    {
        public MonopolyException(string message) : base(message) { }
    }
}
