using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace StationSearchAlgorithmTests
{
	[TestFixture]
	public class PreprocessStationsTests
	{
		[Test]
		public void GivenNull_ThrowsArgNullException()
		{
			Assert.Throws<ArgumentNullException>(() => ProgramInitialize.PreprocessStations(null));
		}

		[Test]
		public void GivenEmptyList_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => ProgramInitialize.PreprocessStations(new List<string>()));
		}

		[Test]
		public void GivenEmptyList_ThrowsArgumentExceptionWithUsefulMessage()
		{
			ArgumentException exception = null;
			try
			{
				ProgramInitialize.PreprocessStations(new List<string>());
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
			List<KeyValuePair<string, string>> result = ProgramInitialize.PreprocessStations(new List<string>{"a"});

			Assert.That(result.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenListOf1StationNameWith1Letter_ReturnsCorrectKey()
		{
			List<KeyValuePair<string, string>> result = ProgramInitialize.PreprocessStations(new List<string>{"a"});

			Assert.That(result.Single().Key, Is.EqualTo("a"));
		}

		[Test]
		public void GivenListOf1StationNameWith1Letter_ReturnsCorrectValue()
		{
			List<KeyValuePair<string, string>> result = ProgramInitialize.PreprocessStations(new List<string>{"a"});

			Assert.That(result.Single().Value, Is.EqualTo("a"));
		}

		[Test]
		public void GivenListOf2StationsWith1Letter_Returns2KeyValuePairs()
		{
			List<KeyValuePair<string, string>> result = ProgramInitialize.PreprocessStations(new List<string>{"a", "b"});

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void GivenListOf2StationsWith1Letter_ReturnsLastStationWithCorrectKey()
		{
			IEnumerable<KeyValuePair<string, string>> result = ProgramInitialize.PreprocessStations(new List<string> { "a", "b" });

			Assert.That(result.SingleOrDefault(x => x.Key.Equals("b")), Is.Not.Null);
		}

		[Test]
		public void GivenListOf2StationsWith1Letter_ReturnsLastStationWithCorrectValue()
		{
			IEnumerable<KeyValuePair<string, string>> result = ProgramInitialize.PreprocessStations(new List<string> { "a", "b" });

			Assert.That(result.SingleOrDefault(x => x.Value.Equals("b")), Is.Not.Null);
		}
	}

	[TestFixture]
	public class GetStationBeginningsTests
	{
		[Test]
		public void GivenNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => ProgramInitialize.GetStationBeginnings(null));
		}

		[Test]
		public void GivenEmptyString_ReturnsEmptyList()
		{
			var result =  ProgramInitialize.GetStationBeginnings(string.Empty);

			Assert.That(result, Is.Empty);
		}

		[Test]
		public void GivenWhitespace_ReturnsEmptyList()
		{
			var result =  ProgramInitialize.GetStationBeginnings("         \t          \n       ");

			Assert.That(result, Is.Empty);
		}

		[Test]
		public void Given1LetterString_Returns1KeyValuePair()
		{
			var result =  ProgramInitialize.GetStationBeginnings("a");

			Assert.That(result.Count, Is.EqualTo(1));
		}
	}

	public static class ProgramInitialize
	{
		public static List<KeyValuePair<string, string>> PreprocessStations(List<string> stations)
		{
			if(stations == null)
				throw new ArgumentNullException("stations");

			if(stations.Count == 0)
				throw new ArgumentException("The list of stations was empty. We cannot possibly query an empty list of station names.");

			var result = stations.Select(x => new KeyValuePair<string, string>(x, x));

			return result.ToList();
		}

		public static List<KeyValuePair<string, string>> GetStationBeginnings(string stationName)
		{
			if(stationName == null)
				throw new ArgumentNullException();

			if(string.IsNullOrWhiteSpace(stationName))
				return new List<KeyValuePair<string, string>>();

			return new List<KeyValuePair<string, string>> {new KeyValuePair<string, string>()};
		}
	}
}
