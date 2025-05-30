﻿using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Hotel_Management.Providers
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
