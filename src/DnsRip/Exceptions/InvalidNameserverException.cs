using System;

namespace DnsRip.Exceptions
{
    public class InvalidNameserverException : Exception
    {
        public InvalidNameserverException(string server) : base("Invalid nameserver: " + server)
        {
        }
    }
}