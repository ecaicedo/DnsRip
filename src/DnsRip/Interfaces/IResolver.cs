using DnsRip.Models;
using DnsRip.Utilites;
using System;
using System.Collections.Generic;

namespace DnsRip.Interfaces
{
    public interface IResolver
    {
        int Retries { get; set; }
        IEnumerable<string> Servers { get; set; }
        TimeSpan Timeout { get; set; }
        Validator Validator { get; set; }

        IEnumerable<ResolveResponse> Resolve(string query, QueryType type);
    }
}