﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorWasmWdcApp
{
	public static class SDPCloud
	{
		private static readonly string _clientId = "1000.1H74DTCN12M3IL5RKGOP3AZ2ECZB7Z";
		private static readonly string _clientSecret = "50b39baef3e35e598907f48efd7be723712dc7a81c";
		private static readonly string _authUrl = "https://accounts.zoho.com/oauth/v2/auth";
		private static readonly string _tokenURL = "https://accounts.zoho.com/oauth/v2/token";
		private static readonly string _redirectUri = "https://localhost:7202/oauth";
		private static readonly string _scope = "SDPOnDemand.projects.ALL";
		
		public static async Task<string> GetAuthorizationCodeAsync(NavigationManager navigationManager)
		{
			var authRequestUrl = $"{_authUrl}?response_type=code&client_id={_clientId}&redirect_uri={_redirectUri}&scope={_scope}";
			//await jsRuntime.InvokeVoidAsync("open", authRequestUrl, "_blank");
			navigationManager.NavigateTo(authRequestUrl);
			//navigationManager.NavigateToLogin(authRequestUrl);
			return authRequestUrl;
		}

		public static async Task<string> GetAccessTokenAsync(string authorizationCode)
		{
			using (var client = new HttpClient())
			{
				var requestData = new Dictionary<string, string>
					{
						{ "grant_type", "authorization_code" },
						{ "client_id", _clientId },
						{ "client_secret", _clientSecret },
						{ "redirect_uri", _redirectUri },
						{ "code", authorizationCode }
					};

				var requestContent = new FormUrlEncodedContent(requestData);
				var response = await client.PostAsync(_tokenURL, requestContent);
				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();
				var tokenResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

				if (tokenResponse != null && tokenResponse.ContainsKey("access_token"))
				{
					return tokenResponse["access_token"];
				}

				return null;
			}
		}
	}
}
