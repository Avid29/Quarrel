﻿// Special thanks to Sergio Pedri for the basis of this design

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarrel.Services.Gateway
{
    public interface IGatewayService
    {
        DiscordAPI.Gateway.Gateway Gateway { get; }

        Task<bool> InitializeGateway([NotNull] string accessToken);
    }
}
