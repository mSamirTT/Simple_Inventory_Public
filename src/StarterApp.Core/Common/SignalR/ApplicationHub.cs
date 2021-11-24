﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace StarterApp.Core.Common.SignalR
{
    public class ApplicationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
