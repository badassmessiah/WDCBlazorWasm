using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlazorWasmWdcApp
{
	public static class SDPCloud
	{
		private static readonly string _clientId = "1000.1H74DTCN12M3IL5RKGOP3AZ2ECZB7Z";
		private static readonly string _clientSecret = "50b39baef3e35e598907f48efd7be723712dc7a81c";
		private static readonly string _url = "https://accounts.zoho.com/oauth/v2/token";

		public static async Task<string> GetAccessToken(HttpClient httpClient)
		{
			var requestBody = new Dictionary<string, string>
				{
					{ "client_id", _clientId },
					{ "client_secret", _clientSecret },
					{ "grant_type", "authorization_code" }
				};

			var requestContent = new FormUrlEncodedContent(requestBody);
			var response = await httpClient.PostAsync(_url, requestContent);
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			var tokenResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

			if (tokenResponse != null && tokenResponse.TryGetValue("access_token", out var accessToken))
			{
				return accessToken;
			}

			throw new HttpRequestException("Failed to retrieve access token.");
		}
	}
}
