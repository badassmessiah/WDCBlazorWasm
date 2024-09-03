using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorWasmWdcApp
{
    public class Program
    {
		public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            

            var baseUrl = builder.Configuration.GetSection("MicrosoftGraph")["BaseUrl"];
            var scopes = builder.Configuration.GetSection("MicrosoftGraph:Scopes").Get<List<string>>();
            builder.Services.AddGraphClient(baseUrl, scopes);

            builder.Services.AddMsalAuthentication(options =>
            {
				builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
				options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/Calendars.ReadWrite");

			});

			await builder.Build().RunAsync();
        }

        
    }
}
