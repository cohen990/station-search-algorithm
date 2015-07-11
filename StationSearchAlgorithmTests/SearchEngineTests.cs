using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using StationSearchAlgorithm;

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
		public void GivenEmptyLookup_ThrowsInvalidLookupTableException()
		{
			Assert.Throws<InvalidLookupTableException>(() => new SearchEngine(new LookupTable()));
		}

		[Test]
		public void GivenEmptyLookup_ThrowsInvalidLookupTableExceptionWithUsefulMessage()
		{
			var exception = Assert.Throws<InvalidLookupTableException>(() => new SearchEngine(new LookupTable()));

			Assert.That(exception.Message, Is.StringContaining("The lookups must not be empty."));
		}

		[Test]
		public void GivenNullSuggestions_ThrowsInvalidLookupTableException()
		{
			LookupTable dict = new LookupTable { { "lookup", null } };

			Assert.Throws<InvalidLookupTableException>(() => new SearchEngine(dict));
		}

		[Test]
		public void GivenNullSuggestions_ThrowsInvalidLookupTableExceptionWithUsefulMessage()
		{
			LookupTable dict = new LookupTable { { "lookup", null } };

			var exception = Assert.Throws<InvalidLookupTableException>(() => new SearchEngine(dict));

			Assert.That(exception.Message, Is.StringContaining("All of the SuggestionResultTable are null. This is invalid as a lookup table."));
		}

		[Test]
		public void GivenEmptySuggestions_ThrowsInvalidLookupTableException()
		{
			LookupTable dict = new LookupTable { { "lookup", new SuggestionResultTable() } };

			Assert.Throws<InvalidLookupTableException>(() => new SearchEngine(dict));
		}

		[Test]
		public void GivenEmptySuggestions_ThrowsInvalidLookupTableExceptionWithUsefulMessage()
		{
			LookupTable dict = new LookupTable { { "lookup", new SuggestionResultTable() } };

			var exception = Assert.Throws<InvalidLookupTableException>(() => new SearchEngine(dict));

			Assert.That(exception.Message, Is.StringContaining("None of the lookups have any suggestions. This is invalid as a lookup table."));
		}

		[Test]
		public void GivenValidDictionary_DoesntThrow()
		{
			LookupTable dict = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				}
			};

			Assert.DoesNotThrow(() => new SearchEngine(dict));
		}

		[Test]
		[Ignore("This should really be ignored, since it's more brittle than it's worth. If that property's name is changed then this will break every time - just wanted to show off.")]
		public void GivenValidDictionary_SetsLookupsToProvidedDictionary()
		{
			LookupTable dict = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
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
			LookupTable lookups = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search(null);

			Assert.That(result.Matches, Is.Empty);
		}
		[Test]
		public void GivenNull_ReturnsNoSuggestions()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search(null);

			Assert.That(result.Suggestions, Is.Empty);
		}

		[Test]
		public void GivenEmptyString_ReturnsNoMatches()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search(string.Empty);

			Assert.That(result.Matches, Is.Empty);
		}

		[Test]
		public void GivenWhitespace_ReturnsNoSuggestions()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("   \t\t\n   ");

			Assert.That(result.Suggestions, Is.Empty);
		}

		[Test]
		public void GivenWhitespace_ReturnsNoMatches()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("   \t\t\n   ");

			Assert.That(result.Matches, Is.Empty);
		}

		[Test]
		public void GivenEmptyString_ReturnsNoSuggestions()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search(string.Empty);

			Assert.That(result.Suggestions, Is.Empty);
		}

		[Test]
		public void GivenA_ReturnsNoMatches()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("a");

			Assert.That(result.Matches, Is.Empty);
		}

		[Test]
		public void GivenA_ReturnsNoSuggestions()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				}
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("a");

			Assert.That(result.Suggestions, Is.Empty);
		}

		[Test]
		public void GivenL_Returns1Match()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new SuggestionResultTable {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new SuggestionResultTable {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new SuggestionResultTable {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Matches.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenL_ReturnsResult()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new SuggestionResultTable {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new SuggestionResultTable {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new SuggestionResultTable {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Matches.Single(), Is.EqualTo("result"));
		}

		[Test]
		public void GivenL_Returns1Suggestion()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new SuggestionResultTable {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new SuggestionResultTable {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new SuggestionResultTable {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Suggestions.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenL_ReturnsO()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new SuggestionResultTable {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new SuggestionResultTable {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new SuggestionResultTable {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("l");

			Assert.That(result.Suggestions.Single(), Is.EqualTo('o'));
		}

		[Test]
		public void GivenLookup_Returns1Match()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new SuggestionResultTable {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new SuggestionResultTable {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new SuggestionResultTable {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("lookup");

			Assert.That(result.Matches.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenLookup_ReturnsResult()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new SuggestionResultTable {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new SuggestionResultTable {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new SuggestionResultTable {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("lookup");

			Assert.That(result.Matches.Single(), Is.EqualTo("result"));
		}

		[Test]
		public void GivenLookup_ReturnsNoSuggestion()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"lo",
					new SuggestionResultTable {{'o', new List<string> {"result"}}}
				},
				{
					"loo",
					new SuggestionResultTable {{'k', new List<string> {"result"}}}
				},
				{
					"look",
					new SuggestionResultTable {{'u', new List<string> {"result"}}}
				},
				{
					"looku",
					new SuggestionResultTable {{'p', new List<string> {"result"}}}
				},
				{
					"lookup",
					new SuggestionResultTable {{'\0', new List<string> {"result"}}}
				},
			};

			var engine = new SearchEngine(lookups);

			var result = engine.Search("lookup");

			Assert.That(result.Suggestions, Is.Empty );
		}

		[Test]
		public void GivenLWith3Matches_Returns3Matches()
		{
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable
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
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable
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
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable
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
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable
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
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable
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
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable
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
			LookupTable lookups = new LookupTable
			{
				{
					"l",
					new SuggestionResultTable
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
