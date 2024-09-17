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
			builder.Services.AddScoped<SessionStorageAccessor>();
            

            builder.Services.AddMsalAuthentication(options =>
            {
				builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
				options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/Calendars.ReadWrite");

			});

			SDPCloud.Initialize(builder.Configuration);

			await builder.Build().RunAsync();
        }

        
    }
}
