using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

			var engine = new SearchEngine(lookups);

			while (true)
			{
				Console.WriteLine(Environment.NewLine + "Enter your search term or hit 'Ctrl' + 'C' to quit");
				var searchTerm = Console.ReadLine();

				watch = Stopwatch.StartNew();
				var result = engine.Search(searchTerm);
				watch.Stop();

				Console.WriteLine("Search completed in {0} miliseconds", watch.ElapsedMilliseconds);

				if(!result.Matches.Any())
				{
					Console.WriteLine("No matches found...");
					continue;
				}

				Console.WriteLine("Matches: {0}", string.Join(", ", result.Matches));
			}
		}
	}
}
