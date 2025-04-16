using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;

public static class AuthenticationWrapper
{
    public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;
    public static async Task<AuthState> DoAuth(int maxRetries = 5)
    {
        if (AuthState == AuthState.Authenticated)
        {
            return AuthState;
        }

        if (AuthState == AuthState.Authenticating)
        {
            Debug.Log("Already authenticating...");
            await Authenticating();
            return AuthState;
        }

        await SignInAnonymouslyAsync(maxRetries);
        return AuthState;
    }

    private static async Task<AuthState> Authenticating()
    {
        while (AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }
        return AuthState;
    }

    private static async Task SignInAnonymouslyAsync(int maxRetries = 5)
    {
        AuthState = AuthState.Authenticating;
        int attempts = 0;
        while (attempts < maxRetries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException ae)
            {
                AuthState = AuthState.Error;
                Debug.LogError(ae);
            }
            catch (RequestFailedException rfe)
            {
                AuthState = AuthState.Error;
                Debug.LogError(rfe);
            }
            
            attempts++;
            await Task.Delay(1000);
        }
        if (AuthState != AuthState.Authenticated)
        {
            AuthState = AuthState.TimeOut;
            Debug.LogWarning("Authentication timed out.");
        }
    }
}

public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}
