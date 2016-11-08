using DnsRip.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMethodReturnValue.Local

namespace DnsRip.Tests
{
    [TestFixture]
    public class ResolverTests
    {
        public class ResolveTest
        {
            public string Query { get; set; }
            public QueryType Type { get; set; }
            public IEnumerable<ResolveResponse> Expected { get; set; }

            public IEnumerable<string> Servers
            {
                get { return _servers ?? new[] { "8.8.4.4" }; }
                set { _servers = value; }
            }

            private IEnumerable<string> _servers;
        }

        public class TestResponse : ResolveResponse
        {
            public bool TtlIsInteger => true;
            public bool? RecordIsIp4 { get; set; }
            public bool? RecordIsIp6 { get; set; }
            public bool? RecordIsHostname { get; set; }
            public bool? RecordIsMxRecord { get; set; }
            public bool? RecordIsSoaRecord { get; set; }
            public bool? RecordIsNotEmpty { get; set; }
            public bool? ServerIsIp { get; set; }
        }

        private static IEnumerable<ResolveTest> GetResolveTests()
        {
            var tests = new List<ResolveTest>
            {
                new ResolveTest
                {
                    Query = "google.com",
                    Type = QueryType.A,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "yahoo.com",
                    Type = QueryType.A,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "www.yahoo.com",
                    Type = QueryType.A,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "fd-fp3.wg1.b.yahoo.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        },
                        new TestResponse
                        {
                            Host = "fd-fp3.wg1.b.yahoo.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        },
                        new TestResponse
                        {
                            Host = "www.yahoo.com",
                            Type = QueryType.CNAME,
                            RecordIsHostname = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "google.com",
                    Type = QueryType.AAAA,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.AAAA,
                            RecordIsIp6 = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "google.com",
                    Type = QueryType.NS,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        },
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        },
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        },
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "google.com",
                    Type = QueryType.MX,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.MX,
                            RecordIsMxRecord = true
                        },
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.MX,
                            RecordIsMxRecord = true
                        },
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.MX,
                            RecordIsMxRecord = true
                        },
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.MX,
                            RecordIsMxRecord = true
                        },
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.MX,
                            RecordIsMxRecord = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "google.com",
                    Type = QueryType.SOA,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.SOA,
                            RecordIsSoaRecord = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "google.com",
                    Type = QueryType.TXT,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.TXT,
                            RecordIsNotEmpty = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "8.8.4.4",
                    Type = QueryType.PTR,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "4.4.8.8.in-addr.arpa",
                            Type = QueryType.PTR,
                            RecordIsHostname = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "2607:f8b0:4009:808::200e",
                    Type = QueryType.PTR,
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "e.0.0.2.0.0.0.0.0.0.0.0.0.0.0.0.8.0.8.0.9.0.0.4.0.b.8.f.7.0.6.2.ip6.arpa",
                            Type = QueryType.PTR,
                            RecordIsHostname = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "google.com",
                    Type = QueryType.A,
                    Servers = new[] { "8.8.4.4", "208.67.222.222" },
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true,
                            ServerIsIp = true
                        },
                        new TestResponse
                        {
                            Host = "google.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true,
                            ServerIsIp = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "yahoo.com",
                    Type = QueryType.ANY,
                    Servers = new[] { "8.8.4.4" },
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.NS,
                            RecordIsHostname = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.SOA,
                            RecordIsSoaRecord = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.MX,
                            RecordIsMxRecord = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.MX,
                            RecordIsMxRecord = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.MX,
                            RecordIsMxRecord = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.TXT,
                            RecordIsNotEmpty = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.AAAA,
                            RecordIsIp6 = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.AAAA,
                            RecordIsIp6 = true
                        },
                        new TestResponse
                        {
                            Host = "yahoo.com",
                            Type = QueryType.AAAA,
                            RecordIsIp6 = true
                        }
                    }
                },
                new ResolveTest
                {
                    Query = "netsol.com",
                    Type = QueryType.A,
                    Servers = new[] { "ns1.netsol.com" },
                    Expected = new List<TestResponse>
                    {
                        new TestResponse
                        {
                            Host = "netsol.com",
                            Type = QueryType.A,
                            RecordIsIp4 = true
                        }
                    }
                }
            };

            foreach (var test in tests)
            {
                yield return test;
            }
        }

        [Test, TestCaseSource(nameof(GetResolveTests))]
        public void Resolve(ResolveTest resolveTest)
        {
            var dnsRip = new Resolver(resolveTest.Servers);
            var resultSet = dnsRip.Resolve(resolveTest.Query, resolveTest.Type);
            var expectedSet = resolveTest.Expected.ToList();
            var index = 0;

            resultSet = resultSet.OrderBy(r => r.Server)
                .ThenBy(r => r.Type)
                .ThenBy(r => r.Record);

            foreach (var result in resultSet)
            {
                Console.WriteLine(result.Server);
                Console.WriteLine(resolveTest.Query);
                Console.WriteLine(resolveTest.Type);
                Console.WriteLine(result.Host);
                Console.WriteLine(result.Ttl);
                Console.WriteLine(result.Type);
                Console.WriteLine(result.Record);
                Console.WriteLine();

                var expected = (TestResponse)expectedSet[index];

                Assert.That(result.Host, Is.EqualTo(expected.Host));
                Assert.That(result.Type, Is.EqualTo(expected.Type));
                Assert.That(dnsRip.Validator.IsInteger(result.Ttl), Is.EqualTo(expected.TtlIsInteger));

                if (expected.RecordIsIp4.HasValue)
                    Assert.That(dnsRip.Validator.IsIp4(result.Record), Is.EqualTo(expected.RecordIsIp4));

                if (expected.RecordIsIp6.HasValue)
                    Assert.That(dnsRip.Validator.IsIp6(result.Record), Is.EqualTo(expected.RecordIsIp6));

                if (expected.RecordIsHostname.HasValue)
                    Assert.That(dnsRip.Validator.IsDomain(result.Record), Is.EqualTo(expected.RecordIsHostname));

                if (expected.RecordIsMxRecord.HasValue)
                    Assert.That(dnsRip.Validator.IsMx(result.Record), Is.EqualTo(expected.RecordIsMxRecord));

                if (expected.RecordIsSoaRecord.HasValue)
                    Assert.That(dnsRip.Validator.IsSoa(result.Record), Is.EqualTo(expected.RecordIsSoaRecord));

                if (expected.RecordIsNotEmpty.HasValue)
                    Assert.That(!string.IsNullOrEmpty(result.Record), Is.EqualTo(expected.RecordIsNotEmpty));

                if (expected.ServerIsIp.HasValue)
                    Assert.That(dnsRip.Validator.IsIp4(result.Server), Is.EqualTo(expected.ServerIsIp));

                index++;
            }

            if (index == 0)
                Assert.Fail("No results");
        }
    }
}