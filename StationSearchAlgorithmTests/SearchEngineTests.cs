using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using StationSearchAlgorithm;
using StationSearchAlgorithm.FluentApi;

namespace StationSearchAlgorithmTests
{
	[TestFixture]
	class ConstructorTests
	{
		[Test]
		public void GivenNull_ThrowsArgNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new SearchEngine(null));
		}

		[Test]
		public void GivenEmptyDictionary_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => new SearchEngine(new Dictionary<string, Dictionary<char?, List<string>>>()));
		}

		[Test]
		public void GivenEmptyDictionary_ThrowsArgumentExceptionWithUsefulMessage()
		{
			var exception = Assert.Throws<ArgumentException>(() => new SearchEngine(new Dictionary<string, Dictionary<char?, List<string>>>()));

			Assert.That(exception.Message, Is.StringContaining("The lookups must not be empty."));
		}

		[Test]
		public void GivenDictionaryWithNoValues_ThrowsArgumentException()
		{
			Dictionary<string, Dictionary<char?, List<string>>> dict = new Dictionary<string, Dictionary<char?, List<string>>> { { "lookup", new Dictionary<char?, List<string>>() } };

			Assert.Throws<ArgumentException>(() => new SearchEngine(dict));
		}

		[Test]
		public void GivenDictionaryWithNoValues_ThrowsArgumentExceptionWithUsefulMessage()
		{
			Dictionary<string, Dictionary<char?, List<string>>> dict = new Dictionary<string, Dictionary<char?, List<string>>> { { "lookup", new Dictionary<char?, List<string>>() } };

			var exception = Assert.Throws<ArgumentException>(() => new SearchEngine(dict));

			Assert.That(exception.Message, Is.StringContaining("None of the lookups (keys) have results (values). This is invalid as a lookup table."));
		}

		[Test]
		public void GivenValidDictionary_DoesntThrow()
		{
			Dictionary<string, Dictionary<char?, List<string>>> dict = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			Assert.DoesNotThrow(() => new SearchEngine(dict));
		}

		[Test]
		[Ignore("This should really be ignored, since it's more brittle than it's worth. If that property's name is changed then this will break every time - just wanted to show off.")]
		public void GivenValidDictionary_SetsLookupsToProvidedDictionary()
		{
			Dictionary<string, Dictionary<char?, List<string>>> dict = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(dict);

			var type = typeof(SearchEngine);

			var property = type.GetField("_lookupTable", BindingFlags.NonPublic | BindingFlags.Instance);

			var actual = property.GetValue(engine);

			Assert.That(actual, Is.EqualTo(dict));
		}
	}

	[TestFixture]
	class SearchTests
	{
		[Test]
		public void GivenNull_ReturnsNoMatches()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search(null);

			Assert.That(result.Matches, Is.Empty);
		}
		[Test]
		public void GivenNull_ReturnsNoSuggestions()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search(null);

			Assert.That(result.Suggestions, Is.Empty);
		}

		[Test]
		public void GivenEmptyString_ReturnsNoMatches()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search(string.Empty);

			Assert.That(result.Matches, Is.Empty);
		}

		[Test]
		public void GivenWhitespace_ReturnsNoSuggestions()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("   \t\t\n   ");

			Assert.That(result.Suggestions, Is.Empty);
		}

		[Test]
		public void GivenWhitespace_ReturnsNoMatches()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("   \t\t\n   ");

			Assert.That(result.Matches, Is.Empty);
		}

		[Test]
		public void GivenEmptyString_ReturnsNoSuggestions()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search(string.Empty);

			Assert.That(result.Suggestions, Is.Empty);
		}

		[Test]
		public void GivenA_ReturnsNoMatches()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("a");

			Assert.That(result.Matches, Is.Empty);
		}

		[Test]
		public void GivenA_ReturnsNoSuggestions()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("a");

			Assert.That(result.Suggestions, Is.Empty);
		}

		[Test]
		public void GivenL_Returns1Match()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new Dictionary<char?, List<string>> {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new Dictionary<char?, List<string>> {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new Dictionary<char?, List<string>> {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Matches.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenL_ReturnsResult()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new Dictionary<char?, List<string>> {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new Dictionary<char?, List<string>> {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new Dictionary<char?, List<string>> {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Matches.Single(), Is.EqualTo("result"));
		}

		[Test]
		public void GivenL_Returns1Suggestion()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new Dictionary<char?, List<string>> {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new Dictionary<char?, List<string>> {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new Dictionary<char?, List<string>> {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Suggestions.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenL_ReturnsE()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new Dictionary<char?, List<string>> {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new Dictionary<char?, List<string>> {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new Dictionary<char?, List<string>> {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Suggestions.Single(), Is.EqualTo('e'));
		}

		[Test]
		public void GivenLookup_Returns1Match()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new Dictionary<char?, List<string>> {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new Dictionary<char?, List<string>> {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new Dictionary<char?, List<string>> {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("lookup");

			Assert.That(result.Matches.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenLookup_ReturnsResult()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new Dictionary<char?, List<string>> {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new Dictionary<char?, List<string>> {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new Dictionary<char?, List<string>> {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("lookup");

			Assert.That(result.Matches.Single(), Is.EqualTo("result"));
		}

		[Test]
		public void GivenLookup_Returns0Suggestion()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new Dictionary<char?, List<string>> {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new Dictionary<char?, List<string>> {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new Dictionary<char?, List<string>> {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new Dictionary<char?, List<string>> {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new Dictionary<char?, List<string>> {{null, new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("lookup");

			Assert.That(result.Suggestions, Is.Empty );
		}

		[Test]
		public void GivenLWith3Matches_Returns3Matches()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>>
					{
						{
							'o',
							new List<string> {"lookup", "lol"}
						},
						{
							'i',
							new List<string> {"link"}
						},
					}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Matches.Count, Is.EqualTo(3));
		}

		[Test]
		public void GivenLWith3Matches_ReturnsLookup()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>>
					{
						{
							'o',
							new List<string> {"lookup", "lol"}
						},
						{
							'i',
							new List<string> {"link"}
						},
					}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Matches.Count(x=> x == "lookup"), Is.EqualTo(1));
		}

		[Test]
		public void GivenLWith3Matches_ReturnsLol()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>>
					{
						{
							'o',
							new List<string> {"lookup", "lol"}
						},
						{
							'i',
							new List<string> {"link"}
						},
					}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Matches.Count(x=> x == "lol"), Is.EqualTo(1));
		}

		[Test]
		public void GivenLWith3Matches_ReturnsLink()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>>
					{
						{
							'o',
							new List<string> {"lookup", "lol"}
						},
						{
							'i',
							new List<string> {"link"}
						},
					}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Matches.Count(x=> x == "link"), Is.EqualTo(1));
		}

		[Test]
		public void GivenLWith3Matches_Returns2Suggestions()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>>
					{
						{
							'o',
							new List<string> {"lookup", "lol"}
						},
						{
							'i',
							new List<string> {"link"}
						},
					}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Suggestions.Count, Is.EqualTo(2));
		}

		[Test]
		public void GivenLWith3Matches_ReturnsO()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>>
					{
						{
							'o',
							new List<string> {"lookup", "lol"}
						},
						{
							'i',
							new List<string> {"link"}
						},
					}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Suggestions.Count(x => x.Equals('o')), Is.EqualTo(1));
		}

		[Test]
		public void GivenLWith3Matches_ReturnsI()
		{
			Dictionary<string, Dictionary<char?, List<string>>> lookups = new Dictionary<string, Dictionary<char?, List<string>>>
			{
				{
					"l",
					new Dictionary<char?, List<string>>
					{
						{
							'o',
							new List<string> {"lookup", "lol"}
						},
						{
							'i',
							new List<string> {"link"}
						},
					}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Suggestions.Count(x => x.Equals('i')), Is.EqualTo(1));
		}
	}
}
