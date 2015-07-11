using System.Collections.Generic;

namespace StationSearchAlgorithm
{
	public interface IStationPreprocessor
	{
		Dictionary<string, List<string>> GetStationsLookups(List<string> stations);
	}
}