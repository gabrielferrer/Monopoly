using System;

namespace Monopoly
{
    class MonopolyException : Exception
    {
        public MonopolyException(string message) : base(message) { }
    }
}
