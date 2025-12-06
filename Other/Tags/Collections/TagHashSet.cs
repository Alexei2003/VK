using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Other.Tags.Collections
{
    public class TagHashSet
    {
        private HashSet<string> _collection = [];

        public TagHashSet() 
        {
            _collection = [.. TagLoader.Load().Select(t => t.Name)];
        }

        public bool ContainTag(string tag)
        {
            if (_collection.Contains(tag))
            {
                return true;
            }
            return false;
        }
    }
}
