using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Graph.Models;
using System.Text;

namespace BlazorWasmWdcApp
{
	public static class SDPCloud
	{
		private static readonly string _clientId = "1000.1H74DTCN12M3IL5RKGOP3AZ2ECZB7Z";
		private static readonly string _authUrl = "https://accounts.zoho.com/oauth/v2/auth";
		private static readonly string _redirectUri = "https://localhost:7202";
		private static readonly string _scope = "SDPOnDemand.projects.ALL";
		private static readonly string _sdpUrl = "https://project.syntax.ge/";

		

		public static async Task GetAccessTokenAsync(IJSRuntime runtime)
		{
			var authRequestUrl = $"{_authUrl}?client_id={_clientId}&response_type=token&scope={_scope}&redirect_uri={_redirectUri}";
			await runtime.InvokeVoidAsync("openAuthPopup", authRequestUrl);
		}

		public static async Task CreateNewProject(string accessToken)
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, $"{_sdpUrl}api/v3/projects");

			request.Headers.Add("Accept", "application/vnd.manageengine.sdp.v3+json");
			request.Headers.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
			request.Headers.Add("Cookie", "_zcsr_tmp=ae4b5dbb-04a4-472c-be56-984043db7123; sdpcscook=ae4b5dbb-04a4-472c-be56-984043db7123; zalb_6bc9ae5955=6767e7ac871db7481716d79d97484427");

			try
			{
				var response = await client.SendAsync(request);
				response.EnsureSuccessStatusCode();
				Console.WriteLine(await response.Content.ReadAsStringAsync());
			}
			catch (HttpRequestException e)
			{
				// Log the exception or handle it as needed
				throw new Exception("Failed to fetch: " + e.Message);
			}
		}


	}

	
}
