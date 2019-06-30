﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Quarrel.Models.Bindables;
using UICompositionAnimations.Helpers;
using Quarrel.Helpers;
using Quarrel.Messages.Gateway;
using Quarrel.Messages.Posts.Requests;
using Quarrel.Services;
using DiscordAPI.Models;
using Quarrel.Converters.Base;

namespace Quarrel.ViewModels
{
    public class MemberViewModel : ViewModelBase
    {
        public MemberViewModel()
        {
            Messenger.Default.Register<GatewayGuildSyncMessage>(this, async m =>
            {
                await DispatcherHelper.RunAsync(() =>
                {
                    Source.Clear();

                    // Load guild list
                    var guildMemberList = ServicesManager.Cache.Runtime.TryGetValue<List<BindableUser>>(Constants.Cache.Keys.GuildMemberList, m.GuildId);

                    // Show members
                    foreach (var member in guildMemberList)
                    {
                        Source.Add(member);
                    }
                });
            });

            Messenger.Default.Register<BindableUserRequestMessage>(this, m => m.ReportResult(Source.Elements.FirstOrDefault(x => x.Model.User.Id == m.UserId)));

            Messenger.Default.Register<CurrentMemberListRequestMessage>(this, m => m.ReportResult(Source.Elements.ToList()));

            Source = new GroupedObservableCollection<Role, BindableUser>(x => x.TopHoistRole);
            ViewSource = new CollectionViewSource() { Source = Source, IsSourceGrouped = true };
        }

        public CollectionViewSource ViewSource { get; }

        /// <summary>
        /// Gets the collection of grouped feeds to display
        /// </summary>
        [NotNull]
        public GroupedObservableCollection<Role, BindableUser> Source { get; set; }
    }
}
