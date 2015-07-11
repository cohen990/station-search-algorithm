using System;
using StationSearchAlgorithm.FluentApi;

namespace StationSearchAlgorithm
{
	class Program
	{
		static void Main(string[] args)
		{
			var lookups = StationLookup.Get()
										.With(new FileStationSource("station-names.txt"))
										.And(new StationPreprocessor());

			Console.WriteLine("{0} lookups found", lookups.Count);

			while (true)
			{
				Console.WriteLine("enter your search term");
				var search = Console.ReadLine();
			}
		}
	}
}
