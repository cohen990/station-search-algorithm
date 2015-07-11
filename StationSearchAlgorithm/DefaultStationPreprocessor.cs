using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StationSearchAlgorithm
{
	public class DefaultStationPreprocessor : IStationPreprocessor
	{

		public Dictionary<string, List<string>> GetStationsLookups(List<string> stations)
		{
			if (stations == null)
				throw new ArgumentNullException("stations");

			if (stations.Count == 0)
				throw new ArgumentException("The list of stations was empty. We cannot possibly query an empty list of station names.");

			var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

			var lookups = stations.Distinct().SelectMany(GetStationBeginnings);

			foreach (var lookup in lookups)
			{
				if (result.ContainsKey(lookup.Key))
				{
					result[lookup.Key].Add(lookup.Value);
				}
				else
				{
					result[lookup.Key] = new List<string>{lookup.Value};
				}
			}

			return result;
		}

		public List<KeyValuePair<string, string>> GetStationBeginnings(string stationName)
		{
			if (stationName == null)
				throw new ArgumentNullException("stationName");
			if (string.IsNullOrWhiteSpace(stationName))
				return new List<KeyValuePair<string, string>>();

			var result = new List<KeyValuePair<string, string>>();
			var beginsWith = new StringBuilder();
			foreach (var character in stationName)
			{
				beginsWith.Append(character);
				result.Add(new KeyValuePair<string, string>(beginsWith.ToString(), stationName));
			}

			return result;
		}
	}
}
