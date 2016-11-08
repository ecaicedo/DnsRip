using DnsRip.Models;

namespace DnsRip.Interfaces
{
    public interface IParser
    {
        ParseResult Parse(string input);
    }
}