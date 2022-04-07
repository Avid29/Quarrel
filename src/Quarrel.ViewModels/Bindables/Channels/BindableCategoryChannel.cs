﻿// Adam Dernis © 2022

using Discord.API.Models.Channels.Abstract;
using Quarrel.Bindables.Channels.Abstract;

namespace Quarrel.Bindables.Channels
{
    public class BindableCategoryChannel : BindableChannel
    {
        internal BindableCategoryChannel(Channel channel) : base(channel)
        {
        }
    }
}
