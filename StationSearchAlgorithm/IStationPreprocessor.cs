using System.Collections.Generic;

namespace StationSearchAlgorithm
{
	public interface IStationPreprocessor
	{
		List<KeyValuePair<string, string>> GetStationsLookups(List<string> stations);
	}
}