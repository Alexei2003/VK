namespace LikesRepostsBots.Classes
{
    internal class BotsWorksParams
    {
        public bool MakeRepost { get; set; } = false;
        public int AddFriendsCount { get; set; } = 0;
        public ClearFriendsType ClearFriends { get; set; } = 0;
        public enum ClearFriendsType
        {
            BanAccount,BanAndMathAccount
        }
        public bool AddPeopleFromGroupInBlacklist { get; set; } = false;
        public long? GroupIdForGood { get; set; } = null;
        public long? GroupIdForBad { get; set; } = null;
    }
}
