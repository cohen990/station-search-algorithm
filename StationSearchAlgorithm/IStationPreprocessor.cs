using System.Collections.Generic;

namespace StationSearchAlgorithm
{
	public interface IStationPreprocessor
	{
		Dictionary<string, Dictionary<char?, List<string>>> GetStationsLookups(List<string> stations);
	}
}