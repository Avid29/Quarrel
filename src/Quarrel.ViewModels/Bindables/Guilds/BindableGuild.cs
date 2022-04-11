﻿// Adam Dernis © 2022

using Discord.API.Models.Guilds;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Quarrel.Bindables.Abstract;
using System;

namespace Quarrel.Bindables.Guilds
{
    /// <summary>
    /// A wrapper of a <see cref="Discord.API.Models.Guilds.Guild"/> that can be bound to the UI.
    /// </summary>
    public partial class BindableGuild : SelectableItem
    {
        [AlsoNotifyChangeFor(nameof(IconUrl))]
        [AlsoNotifyChangeFor(nameof(IconUri))]
        [ObservableProperty]
        private Guild _guild;

        internal BindableGuild(Guild guild)
        {
            _guild = guild;
        }

        /// <summary>
        /// The id of the selected channel in the guild.
        /// </summary>
        /// <remarks>
        /// This is used to reopen a channel when navigating to a guild.
        /// </remarks>
        public ulong? SelectedChannel { get; set; }

        /// <summary>
        /// Gets the url of the guild's icon.
        /// </summary>
        public string IconUrl => $"https://cdn.discordapp.com/icons/{Guild.Id}/{Guild.IconId}.png?size=128";

        /// <summary>
        /// Gets the url of the guild's icon as a <see cref="Uri"/>.
        /// </summary>
        public Uri IconUri => new Uri(IconUrl);
    }
}
