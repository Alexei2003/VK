namespace LikesRepostsBots.Classes
{
    internal sealed class BotsWorksParams
    {
        public bool MakeRepost { get; set; } = false;
        public int AddFriendsCount { get; set; } = 0;
        public ClearFriendsType ClearFriends { get; set; } = ClearFriendsType.None;
        public enum ClearFriendsType
        {
            None, BanAccount, BanAndMathAccount
        }
        public bool BanPeopleFromGroup { get; set; } = false;
        public long? GroupIdForGood { get; set; } = null;
        public long? GroupIdForBad { get; set; } = null;
    }
}
