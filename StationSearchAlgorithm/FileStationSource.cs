using System;
using System.Collections.Generic;
using System.IO;

namespace StationSearchAlgorithm
{
	public class FileStationSource : IStationSource
	{
		private readonly string _stationSource;

		public FileStationSource(string stationSource)
		{
			if (string.IsNullOrWhiteSpace(stationSource))
				throw new ArgumentNullException("stationSource");

			_stationSource = stationSource;
		}

		public List<string> Get()
		{

			var file = File.OpenText(_stationSource);

			var result = new List<string>();

			while (!file.EndOfStream)
			{
				result.Add(file.ReadLine());
			}

			return result;
		}
	}
}
