﻿using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
using CnGalWebSite.Extensions;
using IdentityModel.AspNetCore.AccessTokenManagement;

namespace CnGalWebSite.Server.Plumbing;

/// <summary>
/// Simplified implementation of a server-side token store.
/// Probably want somehting more robust IRL
/// </summary>
public class ServerSideTokenStore : IUserAccessTokenStore
{
    private readonly ConcurrentDictionary<string, UserAccessToken> _tokens = new ConcurrentDictionary<string, UserAccessToken>();

    public Task ClearTokenAsync(ClaimsPrincipal user, UserAccessTokenParameters? parameters = null)
    {
        var sub = user?.Claims?.GetUserId() ?? throw new InvalidOperationException("no sub claim");
        
        _tokens.TryRemove(sub, out _);
        return Task.CompletedTask;
    }

    public Task<UserAccessToken?> GetTokenAsync(ClaimsPrincipal user, UserAccessTokenParameters? parameters = null)
    {
        var sub = user?.Claims?.GetUserId() ?? throw new InvalidOperationException("no sub claim");
        
        _tokens.TryGetValue(sub, out var value);
        
        return Task.FromResult(value);
    }

    public Task StoreTokenAsync(ClaimsPrincipal user, string accessToken, DateTimeOffset expiration, string? refreshToken = null, UserAccessTokenParameters? parameters = null)
    {
        var sub = user?.Claims?.GetUserId() ?? throw new InvalidOperationException("no sub claim");
        
        var token = new UserAccessToken
        {
            AccessToken = accessToken,
            Expiration = expiration,
            RefreshToken = refreshToken
        };
        
        _tokens[sub] = token;
        
        return Task.CompletedTask;
    }
}
