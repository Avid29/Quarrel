﻿// Quarrel © 2022

using Discord.API.Exceptions;
using Discord.API.Voice.Models.Handshake;
using System;

namespace Discord.API.Voice
{
    internal partial class VoiceConnection
    {
        private Action<VoiceReady> Ready { get; }

        private bool OnReady(VoiceSocketFrame<VoiceReady> frame)
        {
            var ready = frame.Payload;
            _ssrc = ready.SSRC;

            return FireEvent(frame, Ready);
        }

        private static bool FireEvent<T>(VoiceSocketFrame frame, Action<T> eventHandler)
        {
            var eventArgs = ((VoiceSocketFrame<T>)frame).Payload;
            eventHandler(eventArgs);
            return true;
        }

        public static bool FireEvent<T>(T data, Action<T> eventHandler)
        {
            eventHandler(data);
            return true;
        }

        protected override void ProcessEvents(VoiceSocketFrame frame)
        {
            bool succeeded = frame switch
            {
                UnknownOperationVoiceSocketFrame osf => FireEvent(osf.Operation, UnknownOperationEncountered),
                UnknownEventVoiceSocketFrame osf => FireEvent(osf.Event, UnknownEventEncountered),
                _ => frame.Operation switch
                {
                    VoiceOperation.Hello => OnHelloReceived((VoiceSocketFrame<VoiceHello>)frame),
                    VoiceOperation.Ready => OnReady((VoiceSocketFrame<VoiceReady>)frame),

                    _ => FireEvent(frame.Operation, UnhandledOperationEncountered),
                }
            };
            if (!succeeded) FireEvent(new SocketFrameException("Failed to handle socket frame.", (int?)frame.Operation, frame.Event.ToString()), UnhandledMessageEncountered);
        }
    }
}
