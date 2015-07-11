using System;
using System.Collections.Generic;
using System.Linq;

namespace StationSearchAlgorithm
{
	public class SearchEngine
	{
		private readonly LookupTable _lookupTable;

		public SearchEngine(LookupTable lookups)
		{
			if (lookups == null)
				throw new ArgumentNullException("lookups");
			if (lookups.Count == 0)
				throw new InvalidLookupTableException("The lookups must not be empty.");
			if (lookups.Values.All(x => x == null))
				throw new InvalidLookupTableException("All of the SuggestionResultTable are null. This is invalid as a lookup table.");
			if (lookups.Values.All(x => x.Count == 0))
				throw new InvalidLookupTableException("None of the lookups have any suggestions. This is invalid as a lookup table.");

			_lookupTable = lookups;
		}

		public SearchResult Search(string searchTerm)
		{
			if(string.IsNullOrWhiteSpace(searchTerm))
				return SearchResult.Empty;
			if(!_lookupTable.ContainsKey(searchTerm))
				return SearchResult.Empty;

			List<char> suggestions = _lookupTable[searchTerm].Keys.Where(x => x != '\0').ToList();
			var matches = _lookupTable[searchTerm].Values.SelectMany(x=> x).ToList();

			return new SearchResult(matches, suggestions);
		}
	}
}