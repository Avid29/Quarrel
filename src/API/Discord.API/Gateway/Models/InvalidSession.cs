﻿// Adam Dernis © 2022

using System.Text.Json.Serialization;

namespace Discord.API.Gateway.Models
{
    internal class InvalidSession
    {
        [JsonPropertyName("d")]
        public bool ConnectedState { get; set; }
    }
}
