using System.Collections.Generic;

namespace Cirrious.CrossCore.Parse
{
    public class Delimiters
    {
        private const int MapThreshold = 4;

        private readonly HashSet<char> _set;
        private readonly char[] _array;

        public bool Contains(char c)
        {
            if (_array != null)
            {
                for (int i = 0; i < _array.Length; ++i)
                    if (_array[i] == c)
                        return true;
                return false;
            }

            return _set.Contains(c);
        }

        private Delimiters(HashSet<char> set)
        {
            _set = set;
        }

        public Delimiters(IEnumerable<char> chars)
        {
            _set = new HashSet<char>(chars);
        }

        public Delimiters(params char[] chars)
        {
            if (chars.Length < MapThreshold)
                _array = chars;
            else
                _set = new HashSet<char>(chars);
        }

        public Delimiters(string chars) : this(chars.ToCharArray())
        {
        }

        public Delimiters Union(params char[] chars)
        {
            HashSet<char> set;
            set = _set != null ? new HashSet<char>(_set) : new HashSet<char>(_array);
            set.UnionWith(chars);
            return new Delimiters(set);
        }
    }
}
