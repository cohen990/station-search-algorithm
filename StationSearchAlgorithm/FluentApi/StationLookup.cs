using System.Collections.Generic;

namespace StationSearchAlgorithm.FluentApi
{
	public class StationLookup : IRequiresStationSource, IRequiresPreprocessor
	{
		private IStationSource _source;

		private StationLookup()
		{
			
		}

		public static IRequiresStationSource Get()
		{
			return new StationLookup();
		}

		public IRequiresPreprocessor With(IStationSource source)
		{
			_source = source;

			return this;
		}

		public Dictionary<string, List<string>> And(IStationPreprocessor preprocessor)
		{
			var stations = _source.Get();

			var result = preprocessor.GetStationsLookups(stations);

			return result;
		}
	}

	public interface IRequiresStationSource
	{
		IRequiresPreprocessor With(IStationSource source);
	}

	public interface IRequiresPreprocessor
	{
		Dictionary<string, List<string>> And(IStationPreprocessor preprocessor);
	}
}