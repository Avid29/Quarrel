﻿// Quarrel © 2022

using CommunityToolkit.Diagnostics;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Quarrel.Bindables.Channels.Interfaces;
using Quarrel.Bindables.Messages;
using Quarrel.Client.Models.Messages;
using Quarrel.Messages.Discord.Messages;
using Quarrel.Messages.Navigation;
using Quarrel.Services.Discord;
using Quarrel.Services.Dispatcher;
using System.Collections.ObjectModel;

namespace Quarrel.ViewModels.Panels
{
    /// <summary>
    /// The view model for the message list in the app.
    /// </summary>
    public partial class MessagesViewModel : ObservableRecipient
    {
        private readonly IMessenger _messenger;
        private readonly IDiscordService _discordService;
        private readonly IDispatcherService _dispatcherService;

        private bool _isLoading;
        private IBindableSelectableChannel? _selectedChannel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesViewModel"/> class.
        /// </summary>
        public MessagesViewModel(IMessenger messenger, IDiscordService discordService, IDispatcherService dispatcherService)
        {
            _messenger = messenger;
            _discordService = discordService;
            _dispatcherService = dispatcherService;

            Source = new ObservableRangeCollection<BindableMessage>();
            IsLoading = false;

            _messenger.Register<NavigateToChannelMessage<IBindableSelectableChannel>>(this, (_, m) => SelectedChannel = m.Channel);
            _messenger.Register<MessageCreatedMessage>(this, (_, m) =>
            {
                if (SelectedChannel?.Id != m.Message.ChannelId) return;
                _dispatcherService.RunOnUIThread(() => AppendMessage(m.Message));
            });
        }

        /// <summary>
        /// The collection of loaded messages.
        /// </summary>
        public ObservableRangeCollection<BindableMessage> Source;

        public IBindableSelectableChannel? SelectedChannel
        {
            get => _selectedChannel;
            set
            {
                if (SetProperty(ref _selectedChannel, value))
                {
                    LoadChannel();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not messages are loading.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        /// <remarks>
        /// Must be called on the UI thread.
        /// </remarks>
        public async void LoadOlderMessages()
        {
            if (Source.Count == 0) return;

            ulong beforeId = Source[0].Message.Id;
            IBindableMessageChannel? channel = SelectedChannel as IBindableMessageChannel;
            Guard.IsNotNull(channel, nameof(channel));

            // Load messages
            IsLoading = true;
            var messages = await _discordService.GetChannelMessagesAsync(channel, beforeId);
            var bindableMessages = ParseMessages(messages);

            // Add messages to the UI and mark loading as finished
            Source.InsertRange(0, bindableMessages);
            IsLoading = false;
        }

        /// <remarks>
        /// Must be called on the UI thread.
        /// </remarks>
        private async void LoadInitialMessages()
        {
            IBindableMessageChannel? channel = SelectedChannel as IBindableMessageChannel;
            Guard.IsNotNull(channel, nameof(channel));
            // Clear the messages and begin loading
            Source.Clear();

            // Load messages
            IsLoading = true;
            var messages = await _discordService.GetChannelMessagesAsync(channel);
            var bindableMessages = ParseMessages(messages);

            // Add messages to the UI and mark loading as finished
            Source.AddRange(bindableMessages);
            IsLoading = false;
        }

        private void LoadChannel()
        {
            if (SelectedChannel is IBindableMessageChannel)
            {
                LoadInitialMessages();
            }
        }

        private BindableMessage[] ParseMessages(Message[] messages)
        {
            BindableMessage[] bindableMessages = new BindableMessage[messages.Length];
            if (bindableMessages.Length == 0) return bindableMessages;

            bindableMessages[0] = new BindableMessage(_messenger, _discordService, _dispatcherService, messages[messages.Length - 1]);
            for (int i = 1; i < messages.Length; i++)
            {
                bindableMessages[i] = new BindableMessage(_messenger, _discordService, _dispatcherService, messages[messages.Length - 1 - i], messages[messages.Length - i]);
            }
            return bindableMessages;
        }

        /// <remarks>
        /// Must be called on the UI thread.
        /// </remarks>
        private void AppendMessage(Message message)
        {
            Source.Add(new BindableMessage(_messenger, _discordService, _dispatcherService, message, Source.Count > 0 ? Source[Source.Count - 1].Message : null));
        }
    }
}
