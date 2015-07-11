using System.Collections.Generic;

namespace StationSearchAlgorithm
{
	public class SearchResult
	{
		public List<string> Matches { get; private set; }

		public List<char?> Suggestions { get; private set; }

		private SearchResult()
		{
			Matches = new List<string>();
			Suggestions = new List<char?>();
		}

		public SearchResult(List<string> matches, List<char?> suggestions)
		{
			Matches = matches;
			Suggestions = suggestions;
		}

		public static SearchResult Empty
		{
			get { return new SearchResult(); }
		}
	}
}