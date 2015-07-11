using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using StationSearchAlgorithm;

namespace StationSearchAlgorithmTests
{
	[TestFixture]
	public class FileStationSourceTests
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
}