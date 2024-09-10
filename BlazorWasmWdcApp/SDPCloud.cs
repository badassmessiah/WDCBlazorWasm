using Newtonsoft.Json;
using Microsoft.JSInterop;


namespace BlazorWasmWdcApp
{
	public static class SDPCloud
	{
		private static SDPCloudSettings _settings;

		public static void Initialize(IConfiguration configuration)
		{
			_settings = configuration.GetSection("SDPCloud").Get<SDPCloudSettings>();
		}

		public static async Task GetAccessTokenAsync(IJSRuntime runtime)
		{
			var authRequestUrl = $"{_settings.AuthUrl}?client_id={_settings.ClientId}&response_type=token&scope={_settings.Scope}&redirect_uri={_settings.RedirectUri}";
			await runtime.InvokeVoidAsync("openAuthPopup", authRequestUrl);
		}

		public static async Task CreateNewProject(string accessToken, Project project)
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.SdpUrl}api/v3/projects");

			request.Headers.Add("Accept", "application/vnd.manageengine.sdp.v3+json");
			request.Headers.Add("Authorization", $"Zoho-oauthtoken {accessToken}");

			var projectData = new
			{
				project = new
				{
					title = project.title,
					actual_end_time = project.actual_end_time,
					actual_start_time = project.actual_start_time,
					scheduled_end_time = project.scheduled_end_time,
					projected_end = project.projected_end,
					scheduled_start_time = project.scheduled_start_time
				}
			};

			var collection = new List<KeyValuePair<string, string>>
				{
					new("input_data", JsonConvert.SerializeObject(projectData))
				};

			var content = new FormUrlEncodedContent(collection);
			request.Content = content;

			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			Console.WriteLine(await response.Content.ReadAsStringAsync());
		}
	}


}
