using System;

namespace StationSearchAlgorithm
{
	public class InvalidLookupTableException : Exception
	{
		public InvalidLookupTableException(string message)
			: base(message)
		{
		}
	}
}