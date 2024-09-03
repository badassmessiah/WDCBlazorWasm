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

		public static async Task CreateNewProject(Project project, string accessToken)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.manageengine.sdp.v3+json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);

				var projectData = new { project };
				var jsonContent = JsonConvert.SerializeObject(new { input_data = projectData });
				var content = new StringContent($"input_data={jsonContent}", Encoding.UTF8, "application/x-www-form-urlencoded");

				var response = await client.PostAsync($"{_sdpUrl}api/v3/projects", content);
				response.EnsureSuccessStatusCode();
			}
		}


	}

	
}
