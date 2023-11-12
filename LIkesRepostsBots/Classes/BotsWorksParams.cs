namespace LikesRepostsBots.Classes
{
    internal class BotsWorksParams
    {
        public int MakeRepost { get; set; } = 0;
        public int AddFriends { get; set; } = 0;
        public ClearFriendsType ClearFriends { get; set; } = 0;
        public enum ClearFriendsType
        {
            BanAccount,BanAndMathAccount
        }
        public int Memorial { get; set; } = 0;

    }
}
