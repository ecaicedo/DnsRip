using NUnit.Framework;
using System;

namespace DnsRip.Tests
{
    [TestFixture]
    public class TypeTest
    {
        [Test]
        public void Validate()
        {
            var input = "A";

            QueryType type;
            QueryType? parse = null;

            var result = Enum.TryParse(input, out type);

            if (result)
                parse = (QueryType)Enum.Parse(typeof(QueryType), input);

            Assert.That(result, Is.True);
            Assert.That(type, Is.EqualTo(QueryType.A));
            Assert.That(parse, Is.EqualTo(QueryType.A));
        }
    }
}