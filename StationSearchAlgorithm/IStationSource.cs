using System.Collections.Generic;

namespace StationSearchAlgorithm
{
	public interface IStationSource
	{
		List<string> Get();
	}
}