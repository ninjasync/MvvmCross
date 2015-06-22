using System.Collections.Generic;
using System.Linq;
using Dot42.Collections.Specialized;

namespace Cirrious.CrossCore.Parse
{
    public class Delimiters
    {
        // Performance critical to use a specialized map in Dot42
        // to prevent boxing at every 'Contains' invocation.

        private const int MapThreshold = 4;

        private readonly IntIntMap _set;
        private readonly char[] _array;

        public bool Contains(char c)
        {
            if (_array != null)
            {
                var length = _array.Length;
                for(int i = 0; i < length; ++i)
                    if (_array[i] == c)
                        return true;
                return false;
            }

            return _set.Get(c) > 0;
        }

        private Delimiters(IntIntMap set)
        {
            _set = set;
        }

        public Delimiters(IEnumerable<char> chars)  : this(chars.ToArray())
        {
        }

        public Delimiters(string chars) : this(chars.ToCharArray())
        {
        }

        public Delimiters(params char[] chars)
        {
            if (chars.Length < MapThreshold)
                _array = chars;
            else
            {
                _set = new IntIntMap();
                for (int i = 0; i < chars.Length; ++i)
                    _set.Put(chars[i], 1);
            }
        }

        public Delimiters Union(params char[] chars)
        {
            IntIntMap set;
            if(_set != null)
                set = new IntIntMap(_set);
            else
            {
                set = new IntIntMap();
                for (int i = 0; i < _array.Length; ++i)
                    set.Put(chars[i], 1);
            }

            for(int i = 0; i < chars.Length; ++i)
                set.Put(chars[i], 1);

            return new Delimiters(set);
        }
    }
}
