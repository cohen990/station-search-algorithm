using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using StationSearchAlgorithm.FluentApi;

namespace StationSearchAlgorithm
{
	class Program
	{
		static void Main()
		{
			var watch = Stopwatch.StartNew();
			Dictionary<string, List<string>> lookups = StationLookup.Get()
				.With(new FileStationSource("station-names.txt"))
				//.With(new FileStationSource("BIGdata.txt")) // Swap this with previous line for a file with 22,000 GUIDs
				.And(new DefaultStationPreprocessor());

			watch.Stop();

			Console.WriteLine("{0} lookups found in {1} miliseconds", lookups.Count, watch.ElapsedMilliseconds);

			var engine = new SearchEngine(lookups);

			while (true)
			{
				Console.WriteLine(Environment.NewLine + "Enter your search term or hit 'Ctrl' + 'C' to quit");
				var searchTerm = Console.ReadLine();

				watch = new Stopwatch();

				watch.Start();
				var result = engine.Search(searchTerm);
				watch.Stop();

				Console.WriteLine("Search completed in {0} miliseconds, {1} ticks", watch.ElapsedMilliseconds, watch.ElapsedTicks);

				if(!result.Matches.Any())
				{
					Console.WriteLine("No matches found...");
					continue;
				}

				Console.WriteLine("Matches: {0}", string.Join(", ", result.Matches));

				if (!result.Suggestions.Any())
				{
					Console.WriteLine("No suggestions...");
					continue;
				}

				Console.WriteLine("Suggestions: '{0}'", string.Join("', ", result.Suggestions));

				Console.WriteLine("Search completed in {0} miliseconds, {1} ticks", watch.ElapsedMilliseconds, watch.ElapsedTicks);
			}
		}
	}
}
