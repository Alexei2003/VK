using VkNet.Model;

namespace RepetitionOfPostsBot
{
    [Serializable]
    public sealed class PhotoMy : MediaAttachment, IGroupUpdate
    {
        /// <inheritdoc />
        protected override string Alias => "photo";
    }
}
