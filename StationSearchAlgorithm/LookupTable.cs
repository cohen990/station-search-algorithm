using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StationSearchAlgorithm
{
	public class LookupTable : Dictionary<string, Suggestions>
	{
		public LookupTable()
			: base()
		{
		}

		public LookupTable(StringComparer comparer)
			: base(comparer)
		{
		}
	}

	public class Suggestions : IDictionary<char, List<string>> //IEnumerable<KeyValuePair<string, List<string>>>
	{
		private readonly Dictionary<char, List<string>> _internalDictionary;

		private List<string> NoChar { get; set; }

		public Suggestions()
		{
			NoChar = new List<string>();
			_internalDictionary = new Dictionary<char, List<string>>();
		}

		public Suggestions(Dictionary<char, List<string>> internalDictionary)
		{
			NoChar = new List<string>();
			_internalDictionary = internalDictionary;
		}

		public IEnumerator<KeyValuePair<char, List<string>>> GetEnumerator()
		{
			return _internalDictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool Remove(char key)
		{
			return _internalDictionary.Remove(key);
		}

		public bool Remove(char? key)
		{
			if (key == null)
			{
				NoChar = new List<string>();
				return true;
			}

			return Remove(key.Value);
		}

		public bool TryGetValue(char key, out List<string> value)
		{
			return _internalDictionary.TryGetValue(key, out value);
		}

		public bool TryGetValue(char? key, out List<string> value)
		{
			if (key == null)
			{
				value = NoChar;
				return true;
			}

			return TryGetValue(key.Value, out value);
		}

		public void Add(char c, List<string> list)
		{
			_internalDictionary.Add(c, list);
		}

		public void Add(char? c, List<string> list)
		{
			if (c == null)
			{
				NoChar = list;

				return;
			}

			Add(c.Value, list);
		}

		public bool ContainsKey(char key)
		{
			return _internalDictionary.ContainsKey(key);
		}

		public bool ContainsKey(char? key)
		{
			if (key == null)
			{
				return true;
			}

			return ContainsKey(key);
		}

		public List<string> this[char key]
		{
			get { return _internalDictionary[key]; }
			set { _internalDictionary[key] = value; }
		}

		public List<string> this[char? key]
		{
			get {
				if (key == null)
				{
					return NoChar;
				}
				return this[key.Value];
			}
			set
			{
				if (key == null)
				{
					NoChar = value;
					return;
				}
				
				this[key.Value] = value;
			}
		}

		public void Add(KeyValuePair<char, List<string>> item)
		{
			_internalDictionary.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			_internalDictionary.Clear();
			NoChar = new List<string>();
		}

		public bool Contains(KeyValuePair<char, List<string>> item)
		{
			return _internalDictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<char, List<string>>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(KeyValuePair<char, List<string>> item)
		{
			return Remove(item.Key);
		}

		public int Count
		{
			get { return GetCount(); }
		}

		private int GetCount()
		{
			var count = _internalDictionary.Count;

			if (NoChar.Any())
				count += 1;

			return count;
		}

		public bool IsReadOnly { get; private set; }

		public Dictionary<char, List<string>>.KeyCollection Keys
		{
			get { return _internalDictionary.Keys; }
		}

		ICollection<List<string>> IDictionary<char, List<string>>.Values
		{
			get { return Values; }
		}

		ICollection<char> IDictionary<char, List<string>>.Keys
		{
			get { return Keys; }
		}

		public Dictionary<char, List<string>>.ValueCollection Values
		{
			get { throw new NotImplementedException("Please use dict.AllValues()"); }
		}

		public List<string> AllValues()
		{
			var result = new List<string>();

			foreach (var value in _internalDictionary.Values)
			{
				result.AddRange(value);
			}
			result.AddRange(NoChar);

			return result;
		}
	}

	public class InvalidLookupTableException : Exception
	{
		public InvalidLookupTableException(string message)
			: base(message)
		{
		}
	}
}
