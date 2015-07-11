using System.Collections.Generic;

namespace StationSearchAlgorithm
{
	public interface IStationPreprocessor
	{
		LookupTable GetStationsLookups(List<string> stations);
	}
}