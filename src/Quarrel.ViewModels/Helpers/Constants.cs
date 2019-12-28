﻿using DiscordAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarrel.Helpers
{
    public static class Constants
    {
        /// <summary>
        /// A <see langword="class"/> with some commonly used values for the cache services
        /// </summary>
        public static class Cache
        {
            /// <summary>
            /// A <see langword="class"/> with some commonly used keys for cached items
            /// </summary>
            public static class Keys
            {
                //public const string Guild = nameof(Guild);
                public const string GuildSettings = nameof(GuildSettings);
                public const string GuildList = nameof(GuildList);
                //public const string GuildMember = nameof(GuildMember);
                //public const string GuildMemberList = nameof(GuildMemberList);

                public const string GuildRole = nameof(GuildRole);
                public const string GuildRoleList = nameof(GuildRoleList);

                //public const string Channel = nameof(Channel);
                public const string ChannelSettings = nameof(ChannelSettings);
                //public const string ChannelList = nameof(ChannelList);

                //public const string ReadState = nameof(ReadState);
                //public const string Presence = nameof(Presence);

                public const string Friend = nameof(Friend);

                public const string CurrentUser = nameof(CurrentUser);
                public const string Note = nameof(Note);

                public const string AccessToken = nameof(AccessToken);
            }
        }

        public static class ConnectedAnimationKeys
        {
            public const string MemberFlyoutAnimation = nameof(MemberFlyoutAnimation);
        }

        public static class Regex
        {
            public const string YouTubeRegex = @"(?:https:\/\/)?(?:(?:www\.)?youtube\.com\/(?:(?:watch\?.*?v=)|(?:embed\/))([\w\-]+)|youtu\.be\/(?:embed\/)?([\w\-]+))";
        }

        public static class Store
        {
#if DEBUG
            public const string AppId = "d25517cb-12d4-4699-8bdc-52040c712cab";

            public const string NativeAdId = "test";
#else
            public const string AppId = "9nbrwj777c8r";

            public const string NativeAdId = "1100064845";
#endif
        }
    }
}
