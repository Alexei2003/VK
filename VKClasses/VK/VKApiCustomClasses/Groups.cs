﻿using VkNet;
using VkNet.Model;
using VkNet.Utils;

namespace VKClasses.VK.VKApiCustomClasses
{
    public class Groups
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Groups(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public VkCollection<User> GetMembers(GroupsGetMembersParams @params)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Groups.GetMembers(@params);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public bool RemoveUser(long groupId, long userId)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Groups.RemoveUser(groupId, userId);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }
    }
}
