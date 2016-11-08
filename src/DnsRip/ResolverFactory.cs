using DnsRip.Interfaces;
using System.Collections.Generic;

namespace DnsRip
{
    public class ResolverFactory : IResolverFactory
    {
        public Resolver Create(string server)
        {
            return new Resolver(server);
        }

        public Resolver Create(IEnumerable<string> servers)
        {
            return new Resolver(servers);
        }
    }
}