using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

using Tripbuk.Server.Models;

namespace Tripbuk.Client
{
    public partial class ViatorService
    {
        private readonly HttpClient httpClient;
        private readonly NavigationManager navigationManager;

        public ViatorService(NavigationManager navigationManager, IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient("Viator");
            this.navigationManager = navigationManager;
        }

        private async Task AuthorizeRequest(HttpRequestMessage request)
        {
            request.Headers.Add("exp-api-key", "f8ffa55a-1942-41ae-9a19-a1c88aa8649c");
        }

        partial void OnSearchFreeText(HttpRequestMessage request);
        partial void OnSearchFreeTextResponse(HttpResponseMessage response);

        public async Task SearchFreeText(string language)
        {
            var uri = new Uri(httpClient.BaseAddress, $"search/freetext");

            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.TryAddWithoutValidation("Accept-Language", $"{language}");

            OnSearchFreeText(request);

            await AuthorizeRequest(request);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            OnSearchFreeTextResponse(response);
        }
    }
}