﻿// Adam Dernis © 2022

using System.Text.Json.Serialization;

// JSON models don't need to respect standard nullable rules.
#pragma warning disable CS8618

namespace Discord.API.Gateways.Models.Handshake
{
    internal class InvalidSession
    {
        [JsonPropertyName("d")]
        public bool ConnectedState { get; set; }
    }
}
