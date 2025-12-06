using System;
using System.Collections.Generic;
using System.Text;

namespace Other.Tags.Collections
{
    public class TagDictionaryGT
    {
        private Dictionary<string, string> _collection = [];

        public TagDictionaryGT()
        {
            _collection = TagLoader.Load().Where(tag => tag.Gelbooru.Length > 0).ToDictionary(k => k.Gelbooru, v => v.Name);
        }

        public string? GetValue(string TagGelbooru)
        {
            _collection.TryGetValue(TagGelbooru, out var result);
            return result;
        }
    }
}
