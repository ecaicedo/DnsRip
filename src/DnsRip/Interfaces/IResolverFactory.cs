using System.Collections.Generic;

namespace DnsRip.Interfaces
{
    public interface IResolverFactory
    {
        Resolver Create(string server);

        Resolver Create(IEnumerable<string> servers);
    }
}