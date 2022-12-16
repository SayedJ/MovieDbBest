using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using webapp_cloudrun.Repositories;

namespace webapp_cloudrun.Auth
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        private readonly IMoviesRepo _repo;

        public CustomAuthProvider(IMoviesRepo repo)
        {
            _repo = repo;
            _repo.OnAuthStateChanged += AuthStateChanged;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsPrincipal principal = await _repo.GetAuthAsync();

            return new AuthenticationState(principal);
        }

        private void AuthStateChanged(ClaimsPrincipal principal)
        {
            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(principal)
                )
            );
        }
    }

    }
