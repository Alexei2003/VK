namespace Other.Tags
{
    public static class TagValidator
    {
        private static readonly string[] _badTagArr = { "bad_drawing", "nsfw" };

        public static bool CheckBadTag(string[] tagsArr)
        {
            return tagsArr.Intersect(_badTagArr).Any();
        }
    }
}