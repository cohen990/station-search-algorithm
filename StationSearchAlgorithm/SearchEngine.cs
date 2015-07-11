using System;
using System.Collections.Generic;
using System.Linq;

namespace StationSearchAlgorithm
{
	public class SearchEngine
	{
		private readonly Dictionary<string, Dictionary<char?, List<string>>> _lookupTable;

		public SearchEngine(Dictionary<string, Dictionary<char?, List<string>>> lookups)
		{
			if (lookups == null)
				throw new ArgumentNullException("lookups");
			if (lookups.Count == 0)
				throw new ArgumentException("The lookups must not be empty.", "lookups");
			if (lookups.All(x => x.Value.Count == 0))
				throw new ArgumentException("None of the lookups (keys) have results (values). This is invalid as a lookup table.");

			_lookupTable = lookups;
		}

		public SearchResult Search(string searchTerm)
		{
			if(string.IsNullOrWhiteSpace(searchTerm))
				return SearchResult.Empty;
			if(!_lookupTable.ContainsKey(searchTerm))
				return SearchResult.Empty;

			List<char?> suggestions = _lookupTable[searchTerm].Keys.ToList();
			var matches = _lookupTable[searchTerm].Values.SelectMany(x => x).ToList();

			return new SearchResult(matches, suggestions);
		}
	}
}