using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using StationSearchAlgorithm;

namespace StationSearchAlgorithmTests
{
	[TestFixture]
	public class GetStationLookupsTests
	{
		[Test]
		public void GivenNull_ThrowsArgNullException()
		{
			var preprocessor = new DefaultStationPreprocessor();
			Assert.Throws<ArgumentNullException>(() => preprocessor.GetStationsLookups(null));
		}

		[Test]
		public void GivenEmptyList_ThrowsArgumentException()
		{
			var preprocessor = new DefaultStationPreprocessor();
			Assert.Throws<ArgumentException>(() => preprocessor.GetStationsLookups(new List<string>()));
		}

		[Test]
		public void GivenEmptyList_ThrowsArgumentExceptionWithUsefulMessage()
		{
			var preprocessor = new DefaultStationPreprocessor();
			ArgumentException exception = null;
			try
			{
				preprocessor.GetStationsLookups(new List<string>());
			}
			catch (ArgumentException e)
			{
				exception = e;
			}

			Assert.That(exception.Message, Is.StringContaining("The list of stations was empty. We cannot possibly query an empty list of station names."));
		}

		[Test]
		public void GivenListOf1StationNameWith1Letter_Returns1KeyValuePair()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "a" });

			Assert.That(result.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenListOf1StationNameWith1Letter_ReturnsCorrectKey()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "a" });

			Assert.That(result.Single().Key, Is.EqualTo("a"));
		}

		[Test]
		public void GivenCapitolA_ResultIsCaseInsensetive()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "A" });

			Assert.That(result.ContainsKey("a"));
		}

		[Test]
		public void GivenLowecaseA_ResultIsCaseInsensetive()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "a" });

			Assert.That(result.ContainsKey("A"));
		}

		[Test]
		public void GivenListOf1StationNameWith1Letter_ReturnsCorrectValue()
		{
			var preprocessor = new DefaultStationPreprocessor();
			string expected = "a";

			var result = preprocessor.GetStationsLookups(new List<string> { expected });

			Assert.That(result.Single().Value.Single(), Is.EqualTo(expected));
		}

		[Test]
		public void GivenListOf2StationsWith1Letter_Returns2KeyValuePairs()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "a", "b" });

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void GivenListOf2StationsWith1Letter_ReturnsLastStationWithCorrectKey()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "a", "b" });

			Assert.That(result.SingleOrDefault(x => x.Key.Equals("b")), Is.Not.Null);
		}

		[Test]
		public void GivenListOf2StationsWith1Letter_Returns1Suggestion()
		{
			var preprocessor = new DefaultStationPreprocessor();
			Dictionary<string, Dictionary<char?, List<string>>> result = preprocessor.GetStationsLookups(new List<string> { "a", "b" });

			Assert.That(result["b"].Keys.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenTokyo_Returns5Pairs()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "Tokyo" });

			Assert.That(result.Count(), Is.EqualTo(5));
		}

		[Test]
		public void GivenTokyoTwice_Returns5Pairs()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "Tokyo", "Tokyo" });

			Assert.That(result.Count(), Is.EqualTo(5));
		}

		[Test]
		public void GivenTokyoAndTibet_ReturnsTWithTwoValues()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "Tokyo", "Tibet" });

			Assert.That(result["t"].Count, Is.EqualTo(2) );
		}

		[Test]
		public void GivenTokyoAndTibet_ReturnsTWithTokyo()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "Tokyo", "Tibet" });

			Assert.That(result["t"]['o'].Count , Is.EqualTo(1));
		}

		[Test]
		public void GivenTokyoAndTibet_ReturnsTWithTibet()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationsLookups(new List<string> { "Tokyo", "Tibet" });

			Assert.That(result["t"]['i'].Count, Is.EqualTo(1) );
		}
	}

	[TestFixture]
	public class GetStationBeginningsTests
	{
		[Test]
		public void GivenNull_ThrowsArgumentNullException()
		{
			var preprocessor = new DefaultStationPreprocessor();
			Assert.Throws<ArgumentNullException>(() => preprocessor.GetStationBeginnings(null));
		}

		[Test]
		public void GivenEmptyString_ReturnsEmptyList()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings(string.Empty);

			Assert.That(result, Is.Empty);
		}

		[Test]
		public void GivenWhitespace_ReturnsEmptyList()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("         \t          \n       ");

			Assert.That(result, Is.Empty);
		}

		[Test]
		public void GivenA_Returns1KeyValuePair()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("a");

			Assert.That(result.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenA_ReturnsWithKeyA()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("a");

			Assert.That(result.Single().Key, Is.EqualTo("a"));
		}

		[Test]
		public void GivenA_ReturnsWithValueA()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("a");

			Assert.That(result.Single().Value, Is.EqualTo("a"));
		}

		[Test]
		public void GivenAB_Returns2Pairs()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("ab");

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void GivenAB_ReturnsPairWithKeyA()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("ab");

			Assert.That(result.SingleOrDefault(x => x.Key == "a"), Is.Not.Null);
		}

		[Test]
		public void GivenAB_ReturnsPairWithKeyAMappedToAB()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("ab");

			Assert.That(result.SingleOrDefault(x => x.Key == "a").Value, Is.EqualTo("ab"));
		}

		[Test]
		public void GivenAB_ReturnsPairWithKeyAB()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("ab");

			Assert.That(result.SingleOrDefault(x => x.Key == "ab"), Is.Not.Null);
		}

		[Test]
		public void GivenABCDE_Returns5Pairs()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("abcde");

			Assert.That(result.Count, Is.EqualTo(5));
		}

		[Test]
		public void GivenABCDE_ReturnsPairWithKeyABMappedToAB()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("abcde");

			Assert.That(result.SingleOrDefault(x => x.Key == "abcd"), Is.Not.Null);
		}

		[Test]
		public void GivenABCDE_ReturnsPairWithKeyABCDMappedToABCDE()
		{
			var preprocessor = new DefaultStationPreprocessor();
			var result = preprocessor.GetStationBeginnings("abcde");

			Assert.That(result.SingleOrDefault(x => x.Key == "abcd").Value, Is.EqualTo("abcde"));
		}
	}
}
