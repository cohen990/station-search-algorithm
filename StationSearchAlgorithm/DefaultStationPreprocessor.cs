using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StationSearchAlgorithm
{
	public class DefaultStationPreprocessor : IStationPreprocessor
	{

		public Dictionary<string, Dictionary<char?, List<string>>> GetStationsLookups(List<string> stations)
		{
			if (stations == null)
				throw new ArgumentNullException("stations");

			if (stations.Count == 0)
				throw new ArgumentException("The list of stations was empty. We cannot possibly query an empty list of station names.");

			var result = new Dictionary<string, Dictionary<char?, List<string>>>(StringComparer.OrdinalIgnoreCase);

			var lookups = stations.Distinct().Select(GetStationBeginnings);

			foreach (Dictionary<string, Dictionary<char?, List<string>>> lookup in lookups)
			{
				foreach (string key in lookup.Keys)
				{
					Dictionary<char?, List<string>> suggestions = lookup[key];
					foreach (KeyValuePair<char?, List<string>> suggestion in suggestions)
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

		public Dictionary<string, Dictionary<char?, List<string>>> GetStationBeginnings(string stationName)
		{
			if (stationName == null)
				throw new ArgumentNullException("stationName");
			if (string.IsNullOrWhiteSpace(stationName))
				return new Dictionary<string, Dictionary<char?, List<string>>>();

			var result = new Dictionary<string, Dictionary<char?, List<string>>>();
			var beginsWith = new StringBuilder();

			int i;
			for (i = 0; i < stationName.Length - 1; i++)
			{
				var character = stationName[i];

				beginsWith.Append(character);

				var suggestions = new Dictionary<char?, List<string>> {{stationName[i + 1], new List<string> {stationName}}};

				result[beginsWith.ToString()] = suggestions;
			}

			return result;
		}
	}
}
