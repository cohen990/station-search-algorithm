using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StationSearchAlgorithm
{
	public class DefaultStationPreprocessor : IStationPreprocessor
	{

		public LookupTable GetStationsLookups(List<string> stations)
		{
			if (stations == null)
				throw new ArgumentNullException("stations");

			if (stations.Count == 0)
				throw new ArgumentException("The list of stations was empty. We cannot possibly query an empty list of station names.");

			var result = new LookupTable(StringComparer.OrdinalIgnoreCase);

			var lookups = stations.Distinct().Select(GetStationBeginnings);

			foreach (var lookup in lookups)
			{
				foreach (string key in lookup.Keys)
				{
					Suggestions suggestions = lookup[key];
					foreach (KeyValuePair<char, List<string>> suggestion in suggestions)
					{
						if (result[key].ContainsKey(suggestion.Key))
						{
							result[key][suggestion.Key].AddRange(suggestion.Value);
						}
						else
						{
							result[key][suggestion.Key] = suggestion.Value;
						}
					}
				}
			}

			return result;
		}

		public LookupTable GetStationBeginnings(string stationName)
		{
			if (stationName == null)
				throw new ArgumentNullException("stationName");
			if (string.IsNullOrWhiteSpace(stationName))
				return new LookupTable();

			var result = new LookupTable();
			var beginsWith = new StringBuilder();

			int i;
			for (i = 0; i < stationName.Length - 1; i++)
			{
				var character = stationName[i];

				beginsWith.Append(character);

				var suggestions = new Suggestions {{stationName[i + 1], new List<string> {stationName}}};

				result[beginsWith.ToString()] = suggestions;
			}

			return result;
		}
	}
}
