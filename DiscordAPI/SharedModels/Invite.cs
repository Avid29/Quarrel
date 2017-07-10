﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_UWP.SharedModels
{
    public class SimpleInvite : INotifyPropertyChanged
    {
        private Invite _invite;
        public Invite Invite
        {
            get { return _invite; }
            set { if (_invite.Equals(value)) return; _invite = value; OnPropertyChanged("Invite"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }
    public struct Invite
    {
        /// <summary>
        /// Current use count
        /// </summary>
        [JsonProperty("uses")]
        public int Uses { get; set; }
        /// <summary>
        /// Invite lifetime in seconds
        /// </summary>
        [JsonProperty("max_age")]
        public int MaxAge { get; set; }
        /// <summary>
        /// Maximum amount of uses in seconds
        /// </summary>
        [JsonProperty("max_uses")]
        public int MaxUses { get; set; }
        [JsonProperty("temporary")]
        public bool Temporary { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        /// <summary>
        /// The user who created the invite (only username, discrimnator, avatar and id values)
        /// </summary>
        [JsonProperty("inviter")]
        public User Inviter { get; set; }
        /// <summary>
        /// The invite code
        /// </summary>
        [JsonProperty("code")]
        public string String { get; set; }
        [JsonProperty("guild")]
        public InviteGuild Guild { get; set; }
        [JsonProperty("channel")]
        public InviteChannel Channel { get; set; }
    }

    public struct InviteGuild
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("splash_hash")]
        public string SplashHash { get; set; }
    }

    public struct InviteChannel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
