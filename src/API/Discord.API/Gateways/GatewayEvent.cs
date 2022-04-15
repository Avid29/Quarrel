﻿// Adam Dernis © 2022

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Discord.API.Gateways
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum GatewayEvent
    {
        READY,
        IDENTIFY,
        RESUMED,
        CHANNEL_CREATE,
        CHANNEL_UPDATE,
        CHANNEL_DELETE,
        CHANNEL_PINS_UPDATE,
        CHANNEL_RECIPIENT_ADD,
        CHANNEL_RECIPIENT_REMOVE,

        GUILD_CREATE,
        GUILD_UPDATE,
        GUILD_DELETE,
        //Replaced by lazy guilds
        //GUILD_SYNC,

        GUILD_BAN_ADD,
        GUILD_BAN_REMOVE,
        GUILD_EMOJIS_UPDATE,
        GUILD_INTEGRATIONS_UPDATE,

        GUILD_MEMBER_ADD,
        GUILD_MEMBER_REMOVE,
        GUILD_MEMBER_UPDATE,
        GUILD_MEMBERS_CHUNK,
        GUILD_MEMBER_LIST_UPDATE,

        GUILD_ROLE_CREATE,
        GUILD_ROLE_UPDATE,
        GUILD_ROLE_DELETE,

        MESSAGE_CREATE,
        MESSAGE_UPDATE,
        MESSAGE_DELETE,
        MESSAGE_DELETE_BULK,
        MESSAGE_REACTION_ADD,
        MESSAGE_REACTION_REMOVE,
        MESSAGE_REACTION_REMOVE_ALL,
        MESSAGE_ACK,

        PRESENCE_UPDATE,
        TYPING_START,
        USER_GUILD_SETTINGS_UPDATE,
        USER_SETTINGS_UPDATE,
        USER_UPDATE,
        USER_NOTE_UPDATE,

        VOICE_STATE_UPDATE,
        VOICE_SERVER_UPDATE,

        RELATIONSHIP_ADD,
        RELATIONSHIP_REMOVE,
        RELATIONSHIP_UPDATE,

        SESSIONS_REPLACE,

        ACTIVITY_START,
        ACTIVITY_USER_ACTION,
        APPLICATION_COMMAND_AUTOCOMPLETE_RESPONSE,
        APPLICATION_COMMAND_CREATE,
        APPLICATION_COMMAND_DELETE,
        APPLICATION_COMMAND_UPDATE,
        BILLING_POPUP_BRIDGE_CALLBACK,
        CALL_CREATE,
        CALL_DELETE,
        CALL_UPDATE,
        CHANNEL_PINS_ACK,
        CHANNEL_UNREAD_UPDATE,
        EMBEDDED_ACTIVITY_UPDATE,
        ENTITLEMENT_CREATE,
        ENTITLEMENT_DELETE,
        ENTITLEMENT_UPDATE,
        FORUM_UNREADS,
        FRIEND_SUGGESTION_CREATE,
        FRIEND_SUGGESTION_DELETE,
        GENERIC_PUSH_NOTIFICATION_SENT,
        GIFT_CODE_CREATE,
        GIFT_CODE_UPDATE,
        GUILD_APPLICATION_COMMANDS_UPDATE,
        GUILD_APPLICATION_COMMAND_COUNTS_UPDATE,
        GUILD_DIRECTORY_ENTRY_CREATE,
        GUILD_DIRECTORY_ENTRY_DELETE,
        GUILD_DIRECTORY_ENTRY_UPDATE,
        GUILD_FEATURE_ACK,
        GUILD_JOIN_REQUEST_CREATE,
        GUILD_JOIN_REQUEST_DELETE,
        GUILD_JOIN_REQUEST_UPDATE,
        GUILD_SCHEDULED_EVENT_CREATE,
        GUILD_SCHEDULED_EVENT_DELETE,
        GUILD_SCHEDULED_EVENT_UPDATE,
        GUILD_SCHEDULED_EVENT_USER_ADD,
        GUILD_SCHEDULED_EVENT_USER_REMOVE,
        GUILD_STICKERS_UPDATE,
        INTEGRATION_CREATE,
        INTEGRATION_DELETE,
        INTEGRATION_UPDATE,
        INTERACTION_CREATE,
        INTERACTION_FAILURE,
        INTERACTION_MODAL_CREATE,
        INTERACTION_SUCCESS,
        LIBRARY_APPLICATION_UPDATE,
        LOBBY_CREATE,
        LOBBY_DELETE,
        LOBBY_MEMBER_CONNECT,
        LOBBY_MEMBER_DISCONNECT,
        LOBBY_MEMBER_UPDATE,
        LOBBY_MESSAGE,
        LOBBY_UPDATE,
        LOBBY_VOICE_SERVER_UPDATE,
        LOBBY_VOICE_STATE_UPDATE,
        MESSAGE_REACTION_REMOVE_EMOJI,
        OAUTH2_TOKEN_REVOKE,
        PAYMENT_UPDATE,
        PRESENCES_REPLACE,
        READY_SUPPLEMENTAL,
        RECENT_MENTION_DELETE,
        STAGE_INSTANCE_CREATE,
        STAGE_INSTANCE_DELETE,
        STAGE_INSTANCE_UPDATE,
        STREAM_CREATE,
        STREAM_DELETE,
        STREAM_SERVER_UPDATE,
        STREAM_UPDATE,
        THREAD_CREATE,
        THREAD_DELETE,
        THREAD_LIST_SYNC,
        THREAD_MEMBERS_UPDATE,
        THREAD_MEMBER_LIST_UPDATE,
        THREAD_MEMBER_UPDATE,
        THREAD_UPDATE,
        USER_ACHIEVEMENT_UPDATE,
        USER_CONNECTIONS_UPDATE,
        USER_PAYMENT_CLIENT_ADD,
        USER_PAYMENT_SOURCES_UPDATE,
        USER_PREMIUM_GUILD_SUBSCRIPTION_SLOT_CREATE,
        USER_PREMIUM_GUILD_SUBSCRIPTION_SLOT_UPDATE,
        USER_REQUIRED_ACTION_UPDATE,
        USER_SETTINGS_PROTO_UPDATE,
        USER_SUBSCRIPTIONS_UPDATE,
        WEBHOOKS_UPDATE,
    }
}
