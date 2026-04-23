using System.Text.Json;

using Other.Tags.Collections;

namespace DownloaderDataSetPhoto.Downloaders
{
    public class LastViewedDictionary
    {
        private Dictionary<string, string> _collection = new Dictionary<string, string>();
        public const string PathFile = "E:\\WPS\\CommonData\\Gelbooru\\LastViewedDictionary.txt";

        public LastViewedDictionary()
        {
            Load();
        }

        public void Load()
        {
            try
            {
                string json = File.ReadAllText(PathFile);
                if (json?.Length != 0)
                {
                    _collection = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
                }
            }
            catch { }
        }

        public void Save()
        {
            const string PathFileOld = PathFile + ".old";
            if (File.Exists(PathFileOld)) File.Delete(PathFileOld);
            if (File.Exists(PathFile)) File.Move(PathFile, PathFileOld);
            string json = JsonSerializer.Serialize(_collection);
            File.WriteAllText(PathFile, json);
        }

        public string SwapUrl(string tag, string url)
        {
            if (_collection.TryGetValue(tag, out var lastUrl))
            {
                _collection[tag] = url;
                return lastUrl;
            }
            _collection.Add(tag, url);
            return "";
        }
    }
}
