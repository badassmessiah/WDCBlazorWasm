using System.Net.Http;
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
		private static readonly string _clientId = "1000.SGLJOLCCSRPBFDPM31PYFTILSGOBVC";
		private static readonly string _clientSecret = "1d4115580f5b81d87e9cb37634aff6743b89f61552";
		private static readonly string _authUrl = "https://accounts.zoho.com/oauth/v2/auth";
		private static readonly string _tokenURL = "https://accounts.zoho.com/oauth/v2/token";
		private static readonly string _redirectUri = "https://localhost:7202/oauth";
		private static readonly string _scope = "SDPOnDemand.projects.ALL";

		//public static void GetAuthorizationCode(NavigationManager navigationManager)
		//{
		//	var authRequestUrl = $"{_authUrl}?response_type=code&client_id={_clientId}&redirect_uri={_redirectUri}&scope={_scope}&access_type=offline";
		//	navigationManager.NavigateTo(authRequestUrl, true);
		//}

		public static async Task GetAuthorizationCodeAsync(IJSRuntime jsRuntime)
		{
			var authRequestUrl = $"{_authUrl}?response_type=code&client_id={_clientId}&redirect_uri={_redirectUri}&scope={_scope}&access_type=offline";
			await jsRuntime.InvokeVoidAsync("openAuthPopup", authRequestUrl);
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
