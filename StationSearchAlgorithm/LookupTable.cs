using System;
using System.Collections.Generic;
using System.Linq;

namespace StationSearchAlgorithm
{
	public class LookupTable : Dictionary<string, SuggestionResultTable>
	{
		public LookupTable()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		public List<SuggestionResultTable> SuggestionResultTables
		{
			get { return Values.ToList(); }
		}
	}
}
