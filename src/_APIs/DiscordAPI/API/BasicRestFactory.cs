﻿using DiscordAPI.API.Gateway;
using DiscordAPI.API.Login;
using Refit;
using System;
using System.Net.Http;

namespace DiscordAPI.API
{
    public class BasicRestFactory
    {
        private readonly DiscordApiConfiguration _apiConfig;

        public BasicRestFactory()
        {
            _apiConfig = new DiscordApiConfiguration() { BaseUrl = "https://discordapp.com/api" };
        }

        public BasicRestFactory(DiscordApiConfiguration config)
        {
            _apiConfig = config;
        }

        public IGatewayConfigService GetGatewayConfigService()
        {
            return RestService.For<IGatewayConfigService>(GetHttpClient());
        }

        public ILoginService GetLoginService()
        {
            return RestService.For<ILoginService>(GetHttpClient());
        }

        private HttpClient GetHttpClient()
        {
            return new HttpClient()
            {
                BaseAddress = new Uri(_apiConfig.BaseUrl)
            };
        }

    }
}
