﻿// Quarrel © 2022

using Discord.API.Exceptions;
using Discord.API.Gateways.Models;
using Discord.API.Gateways.Models.Channels;
using Discord.API.Gateways.Models.GuildMember;
using Discord.API.Gateways.Models.Guilds;
using Discord.API.Gateways.Models.Handshake;
using Discord.API.Gateways.Models.Messages;
using System;
using Discord.API.JsonConverters;
using Discord.API.Models.Json.Channels;
using Discord.API.Models.Json.Gateway;
using Discord.API.Models.Json.Guilds;
using Discord.API.Models.Json.Messages;
using Discord.API.Models.Json.Settings;
using Discord.API.Models.Json.Users;
using Discord.API.Models.Json.Voice;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace Discord.API.Gateways
{
    internal partial class Gateway
    {
        private delegate void GatewayEventHandler(SocketFrame gatewayEvent);

        private readonly GatewayConfig _gatewayConfig;
        private readonly string _token;

        private GatewayStatus _gatewayStatus;
        private string? _connectionUrl;
        private string? _sessionId;
        private int _lastEventSequenceNumber;

        public Gateway(GatewayConfig config,
            string token,
            Action<SocketFrameException> unhandledMessageEncountered,
            Action<string> unknownEventEncountered,
            Action<int> unknownOperationEncountered,
            Action<string> knownEventEncountered,
            Action<GatewayOperation> unhandledOperationEncountered,
            Action<GatewayEvent> unhandledEventEncountered,
            Action<Ready> ready,
            Action<Resumed> resumed,
            Action<InvalidSession> invalidSession,
            Action<Exception> gatewayClosed,
            Action<JsonGuild> guildCreated,
            Action<JsonGuild> guildUpdated,
            Action<GuildDeleted> guildDeleted,
            Action<GuildBanUpdate> guildBanAdded,
            Action<GuildBanUpdate> guildBanRemoved,
            Action<JsonChannel> channelCreated,
            Action<JsonChannel> channelUpdated,
            Action<JsonChannel> channelDeleted,
            Action<ChannelRecipientUpdate> channelRecipientAdded,
            Action<ChannelRecipientUpdate> channelRecipientRemoved,
            Action<JsonMessageAck> messageAck,
            Action<JsonMessage> messageCreated,
            Action<JsonMessage> messageUpdated,
            Action<JsonMessageDeleted> messageDeleted,
            Action<MessageReactionUpdated> messageReactionAdded,
            Action<MessageReactionUpdated> messageReactionRemoved,
            Action<MessageReactionRemoveAll> messageReactionRemovedAll,
            Action<JsonGuildMember> guildMemberAdded,
            Action<JsonGuildMember> guildMemberUpdated,
            Action<GuildMemberRemoved> guildMemberRemoved,
            Action<GuildMemberListUpdated> guildMemberListUpdated,
            Action<GuildMembersChunk> guildMembersChunk,
            Action<JsonRelationship> relationshipAdded,
            Action<JsonRelationship> relationshipUpdated,
            Action<JsonRelationship> relationshipRemoved,
            Action<TypingStart> typingStarted,
            Action<JsonPresence> presenceUpdated,
            Action<UserNote> userNoteUpdated,
            Action<JsonUserSettings> userSettingsUpdated,
            Action<JsonGuildSettings> userGuildSettingsUpdated,
            Action<JsonVoiceState> voiceStateUpdated,
            Action<VoiceServerUpdate> voiceServerUpdated,
            Action<SessionReplace[]> sessionReplaced)
        {
            _gatewayConfig = config;
            _token = token;

            SessionReplaced = sessionReplaced;
            VoiceServerUpdated = voiceServerUpdated;
            VoiceStateUpdated = voiceStateUpdated;
            UserGuildSettingsUpdated = userGuildSettingsUpdated;
            UserSettingsUpdated = userSettingsUpdated;
            UserNoteUpdated = userNoteUpdated;
            PresenceUpdated = presenceUpdated;
            TypingStarted = typingStarted;
            RelationshipRemoved = relationshipRemoved;
            RelationshipUpdated = relationshipUpdated;
            RelationshipAdded = relationshipAdded;
            GuildMembersChunk = guildMembersChunk;
            GuildMemberListUpdated = guildMemberListUpdated;
            GuildMemberRemoved = guildMemberRemoved;
            GuildMemberUpdated = guildMemberUpdated;
            GuildMemberAdded = guildMemberAdded;
            MessageReactionRemovedAll = messageReactionRemovedAll;
            MessageReactionRemoved = messageReactionRemoved;
            MessageReactionAdded = messageReactionAdded;
            MessageDeleted = messageDeleted;
            MessageUpdated = messageUpdated;
            MessageCreated = messageCreated;
            MessageAck = messageAck;
            ChannelRecipientRemoved = channelRecipientRemoved;
            ChannelRecipientAdded = channelRecipientAdded;
            ChannelDeleted = channelDeleted;
            ChannelUpdated = channelUpdated;
            ChannelCreated = channelCreated;
            GuildBanRemoved = guildBanRemoved;
            GuildBanAdded = guildBanAdded;
            GuildDeleted = guildDeleted;
            GuildUpdated = guildUpdated;
            GuildCreated = guildCreated;
            GatewayClosed = gatewayClosed;
            InvalidSession = invalidSession;
            Resumed = resumed;
            Ready = ready;
            UnhandledEventEncountered = unhandledEventEncountered;
            UnhandledOperationEncountered = unhandledOperationEncountered;
            KnownEventEncountered = knownEventEncountered;
            UnknownOperationEncountered = unknownOperationEncountered;
            UnknownEventEncountered = unknownEventEncountered;
            UnhandledMessageEncountered = unhandledMessageEncountered;

            _socket = CreateSocket();
            SetupCompression();

            _gatewayStatus = GatewayStatus.Initialized;

            _serialiseOptions = new JsonSerializerOptions();
            _serialiseOptions.AddContext<JsonModelsContext>();

            _deserialiseOptions = new JsonSerializerOptions { Converters = { new SocketFrameConverter() } };
        }

        /// <summary>
        /// Sets up a connection to the gateway.
        /// </summary>
        /// <exception cref="Exception">An exception will be thrown when connection fails, but not when the handshake fails.</exception>
        public async Task ConnectAsync()
        {
            _gatewayStatus = GatewayStatus.Connecting;
            await ConnectAsync(_gatewayConfig.GetFullGatewayUrl("json", "9", "&compress=zlib-stream"));
        }

        /// <summary>
        /// Resumes a connection to the gateway.
        /// </summary>
        /// <exception cref="Exception">An exception will be thrown when connection fails, but not when the handshake fails.</exception>
        public async Task ResumeAsync()
        {
            _gatewayStatus = GatewayStatus.Resuming;
            _socket = CreateSocket();
            await ConnectAsync();
        }

        private async Task ConnectAsync(string connectionUrl)
        {
            _connectionUrl = connectionUrl;
            await _socket.ConnectAsync(connectionUrl);
        }
    }
}
