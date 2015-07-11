using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using StationSearchAlgorithm;

namespace StationSearchAlgorithmTests
{
	[TestFixture]
	public class LoadFromFileTests
	{
		[Test]
		public void GivenNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new FileStationSource(null));
		}

		[Test]
		public void GivenEmptyString_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(()=> new FileStationSource(string.Empty));
		}

		[Test]
		public void GivenIncorrectPath_ThrowsFileNotFoundException()
		{
			var source = new FileStationSource("wrong");
			Assert.Throws<FileNotFoundException>(() => source.Get());
		}

		[Test]
		public void GivenFileWith1Station_Returns1Station()
		{
			var source = new FileStationSource("1station.txt");
			var result = source.Get();

			Assert.That(result.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenFileWith1Station_ReturnsWitchetaw()
		{
			var source = new FileStationSource("1station.txt");
			var result = source.Get();

			Assert.That(result.Single(), Is.EqualTo("Witchetaw"));
		}

		[Test]
		public void GivenFileWith5Station_Returns5Stations()
		{
			var source = new FileStationSource("5stations.txt");
			var result = source.Get();

			Assert.That(result.Count, Is.EqualTo(5));
		}

		[Test]
		public void GivenFileWith5Station_ReturnsGrenoble()
		{
			var source = new FileStationSource("5stations.txt");
			var result = source.Get();

			Assert.That(result.Count(x=> x == "Grenoble"), Is.EqualTo(1));
		}

		[Test]
		public void GivenFileWith5Station_ReturnsFitchely()
		{
			var source = new FileStationSource("5stations.txt");
			var result = source.Get();

			Assert.That(result.Count(x => x == "Fitchely"), Is.EqualTo(1));
		}
	}

	[TestFixture]
	public class GetStationLookupsTests
	{
		[Test]
		public void GivenNull_ThrowsArgNullException()
		{
			var preprocessor = new StationPreprocessor();
			Assert.Throws<ArgumentNullException>(() => preprocessor.GetStationsLookups(null));
		}

		[Test]
		public void GivenEmptyList_ThrowsArgumentException()
		{
			var preprocessor = new StationPreprocessor();
			Assert.Throws<ArgumentException>(() => preprocessor.GetStationsLookups(new List<string>()));
		}

		[Test]
		public void GivenEmptyList_ThrowsArgumentExceptionWithUsefulMessage()
		{
			var preprocessor = new StationPreprocessor();
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
			var preprocessor = new StationPreprocessor();
			List<KeyValuePair<string, string>> result = preprocessor.GetStationsLookups(new List<string> { "a" });

			Assert.That(result.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenListOf1StationNameWith1Letter_ReturnsCorrectKey()
		{
			var preprocessor = new StationPreprocessor();
			List<KeyValuePair<string, string>> result = preprocessor.GetStationsLookups(new List<string> { "a" });

			Assert.That(result.Single().Key, Is.EqualTo("a"));
		}

		[Test]
		public void GivenListOf1StationNameWith1Letter_ReturnsCorrectValue()
		{
			var preprocessor = new StationPreprocessor();
			List<KeyValuePair<string, string>> result = preprocessor.GetStationsLookups(new List<string> { "a" });

			Assert.That(result.Single().Value, Is.EqualTo("a"));
		}

		[Test]
		public void GivenListOf2StationsWith1Letter_Returns2KeyValuePairs()
		{
			var preprocessor = new StationPreprocessor();
			List<KeyValuePair<string, string>> result = preprocessor.GetStationsLookups(new List<string> { "a", "b" });

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void GivenListOf2StationsWith1Letter_ReturnsLastStationWithCorrectKey()
		{
			var preprocessor = new StationPreprocessor();
			IEnumerable<KeyValuePair<string, string>> result = preprocessor.GetStationsLookups(new List<string> { "a", "b" });

			Assert.That(result.SingleOrDefault(x => x.Key.Equals("b")), Is.Not.Null);
		}

		[Test]
		public void GivenListOf2StationsWith1Letter_ReturnsLastStationWithCorrectValue()
		{
			var preprocessor = new StationPreprocessor();
			IEnumerable<KeyValuePair<string, string>> result = preprocessor.GetStationsLookups(new List<string> { "a", "b" });

			Assert.That(result.SingleOrDefault(x => x.Value.Equals("b")), Is.Not.Null);
		}

		[Test]
		public void GivenTokyo_Returns5Pairs()
		{
			var preprocessor = new StationPreprocessor();
			IEnumerable<KeyValuePair<string, string>> result = preprocessor.GetStationsLookups(new List<string> { "Tokyo" });

			Assert.That(result.Count(), Is.EqualTo(5));
		}

		[Test]
		public void GivenTokyoTwice_Returns5Pairs()
		{
			var preprocessor = new StationPreprocessor();
			IEnumerable<KeyValuePair<string, string>> result = preprocessor.GetStationsLookups(new List<string> { "Tokyo", "Tokyo" });

			Assert.That(result.Count(), Is.EqualTo(5));
		}
	}

	[TestFixture]
	public class GetStationBeginningsTests
	{
		[Test]
		public void GivenNull_ThrowsArgumentNullException()
		{
			var preprocessor = new StationPreprocessor();
			Assert.Throws<ArgumentNullException>(() => preprocessor.GetStationBeginnings(null));
		}

		[Test]
		public void GivenEmptyString_ReturnsEmptyList()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings(string.Empty);

			Assert.That(result, Is.Empty);
		}

		[Test]
		public void GivenWhitespace_ReturnsEmptyList()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("         \t          \n       ");

			Assert.That(result, Is.Empty);
		}

		[Test]
		public void GivenA_Returns1KeyValuePair()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("a");

			Assert.That(result.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenA_ReturnsWithKeyA()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("a");

			Assert.That(result.Single().Key, Is.EqualTo("a"));
		}

		[Test]
		public void GivenA_ReturnsWithValueA()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("a");

			Assert.That(result.Single().Value, Is.EqualTo("a"));
		}

		[Test]
		public void GivenAB_Returns2Pairs()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("ab");

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void GivenAB_ReturnsPairWithKeyA()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("ab");

			Assert.That(result.SingleOrDefault(x => x.Key == "a"), Is.Not.Null);
		}

		[Test]
		public void GivenAB_ReturnsPairWithKeyAMappedToAB()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("ab");

			Assert.That(result.SingleOrDefault(x => x.Key == "a").Value, Is.EqualTo("ab"));
		}

		[Test]
		public void GivenAB_ReturnsPairWithKeyAB()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("ab");

			Assert.That(result.SingleOrDefault(x => x.Key == "ab"), Is.Not.Null);
		}

		[Test]
		public void GivenABCDE_Returns5Pairs()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("abcde");

			Assert.That(result.Count, Is.EqualTo(5));
		}

		[Test]
		public void GivenABCDE_ReturnsPairWithKeyABMappedToAB()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("abcde");

			Assert.That(result.SingleOrDefault(x => x.Key == "abcd"), Is.Not.Null);
		}

		[Test]
		public void GivenABCDE_ReturnsPairWithKeyABCDMappedToABCDE()
		{
			var preprocessor = new StationPreprocessor();
			var result = preprocessor.GetStationBeginnings("abcde");

			Assert.That(result.SingleOrDefault(x => x.Key == "abcd").Value, Is.EqualTo("abcde"));
		}
	}
}
