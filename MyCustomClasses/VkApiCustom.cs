﻿using MyCustomClasses.VkApiCustomClasses;
using VkNet;
using VkNet.Model;

namespace MyCustomClasses
{
    public class VkApiCustom
    {
        private readonly Random rand;
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Account Account { get; }
        public Friends Friends { get; }
        public Groups Groups { get; }
        public VkApiCustomClasses.Likes Likes { get; }
        public VkApiCustomClasses.Photo Photo { get; }
        public Stats Stats { get; }
        public Users Users { get; }
        public VkApiCustomClasses.Wall Wall { get; }

        public VkApiCustom(Random rand)
        {
            ApiOriginal = new();
            Account = new(ApiOriginal, TIME_SLEEP);
            Friends = new(ApiOriginal, TIME_SLEEP);
            Groups = new(ApiOriginal, TIME_SLEEP);
            Likes = new(ApiOriginal, TIME_SLEEP);
            Photo = new(ApiOriginal, TIME_SLEEP);
            Stats = new(ApiOriginal, TIME_SLEEP);
            Users = new(ApiOriginal, TIME_SLEEP);
            Wall = new(ApiOriginal, TIME_SLEEP);
            this.rand = rand;

            TIME_SLEEP = TimeSpan.FromSeconds(rand.Next(3) + 1);
        }

        public void Authorize(IApiAuthParams @params)
        {
            while (true)
            {
                try
                {
                    ApiOriginal.Authorize(@params);
                    return;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

    }
}