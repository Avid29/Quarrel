﻿using CommonServiceLocator;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Quarrel.Models.Bindables;
using Quarrel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Quarrel.Helpers;
using DiscordAPI.Models;

namespace DiscordAPI.Models
{
    internal static class GuildMemberChunkExtentions
    {
        public static void Cache(this GuildMemberChunk chunk)
        {
            foreach (var user in chunk.Members)
            {
                BindableUser bgMember = new BindableUser(user);
                bgMember.GuildId = chunk.GuildId;
                // TODO: add to list
            }
        }
    }
}
