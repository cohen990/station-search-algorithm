using System;
using System.Collections.Generic;
using System.Diagnostics;
using StationSearchAlgorithm.FluentApi;

namespace StationSearchAlgorithm
{
	class Program
	{
		static void Main(string[] args)
		{
			var watch = Stopwatch.StartNew();
			Dictionary<string, List<string>> lookups = StationLookup.Get()
				.With(new FileStationSource("station-names.txt"))
				.And(new DefaultStationPreprocessor());

			watch.Stop();

			Console.WriteLine("{0} lookups found in {1} miliseconds", lookups.Count, watch.ElapsedMilliseconds);

			while (true)
			{
				Console.WriteLine("enter your search term or hit 'Ctrl' + 'C' to quit");
				var search = Console.ReadLine();


			}
		}
	}
}
