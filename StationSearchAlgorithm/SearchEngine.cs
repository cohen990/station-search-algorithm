using System;
using System.Collections.Generic;
using System.Linq;

namespace StationSearchAlgorithm
{
	public class SearchEngine
	{
		private readonly Dictionary<string, List<string>> _lookupTable;

		public SearchEngine(Dictionary<string, List<string>> lookups)
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

			var matches = _lookupTable[searchTerm];
			var suggestions = new List<char>();

			suggestions = matches.Where(x => x.Length > searchTerm.Length)
				.Select(x => x[searchTerm.Length])
				.Distinct()
				.ToList();

			return new SearchResult(matches, suggestions);
		}
	}
}