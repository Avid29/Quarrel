﻿using System.Collections.Generic;
using Quarrel.Messages.Abstract;
using Quarrel.Models.Bindables;

namespace Quarrel.Messages.Posts.Requests
{
    /// <summary>
    /// A request message to retrieve a user currently loaded in the memberlist being displayed
    /// </summary>
    public sealed class CurrentMemberListRequestMessage : RequestMessageBase<List<BindableGuildMember>> { }
}
