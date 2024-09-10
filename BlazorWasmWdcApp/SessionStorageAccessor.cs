using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWasmWdcApp
{
	public class SessionStorageAccessor : IAsyncDisposable
	{
		private Lazy<Task<IJSObjectReference>> _accessorJsRef;
		private readonly IJSRuntime _jsRuntime;

		public SessionStorageAccessor(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			_accessorJsRef = new(() => _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/SessionStorageAccessor.js").AsTask());
		}

		private async Task<IJSObjectReference> GetJsReferenceAsync()
		{
			return await _accessorJsRef.Value;
		}

		public async Task<T> GetValueAsync<T>(string key)
		{
			var jsRef = await GetJsReferenceAsync();
			return await jsRef.InvokeAsync<T>("get", key);
		}

		public async Task SetValueAsync<T>(string key, T value)
		{
			var jsRef = await GetJsReferenceAsync();
			await jsRef.InvokeVoidAsync("set", key, value);
		}

		public async Task Clear()
		{
			var jsRef = await GetJsReferenceAsync();
			await jsRef.InvokeVoidAsync("clear");
		}

		public async Task RemoveAsync(string key)
		{
			var jsRef = await GetJsReferenceAsync();
			await jsRef.InvokeVoidAsync("remove", key);
		}

		public async ValueTask DisposeAsync()
		{
			if (_accessorJsRef.IsValueCreated)
			{
				var jsRef = await _accessorJsRef.Value;
				await jsRef.DisposeAsync();
			}
		}
	}
}
