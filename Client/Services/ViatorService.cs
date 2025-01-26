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
using Tripbuk.Server.Models.Viator;

namespace Tripbuk.Client.Services
{
    public partial class ViatorService(NavigationManager navigationManager, IHttpClientFactory httpClientFactory)
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Viator");
        private readonly NavigationManager _navigationManager = navigationManager;

        /// <summary>
        /// Authorize the request
        /// </summary>
        /// <param name="request"></param>
        private async Task AuthorizeRequest(HttpRequestMessage request)
        {
            request.Headers.Add("exp-api-key", "f8ffa55a-1942-41ae-9a19-a1c88aa8649c");
        }
        

        /// <summary>
        /// Search for products by free text
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Tripbuk.Server.Models.Viator.SearchFreeTextResponse> SearchFreeText(SearchFreetextRequest data)
        {
            var uri = new Uri(_httpClient.BaseAddress, $"products/tags");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US");
            request.Headers.TryAddWithoutValidation("Accept", "application/json;version=2.0");
            request.Content = JsonContent.Create<SearchFreetextRequest>(data);

            OnSearchFreeTextRequest(request);

            await AuthorizeRequest(request);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            OnSearchFreeTextResponse(response);

            return await response.Content.ReadFromJsonAsync<Tripbuk.Server.Models.Viator.SearchFreeTextResponse>();
        }
        partial void OnSearchFreeTextRequest(HttpRequestMessage request);
        partial void OnSearchFreeTextResponse(HttpResponseMessage response);

        
        /// <summary>
        /// Search for products destination id
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Tripbuk.Server.Models.Viator.ProductsSearchResponse> ProductSearch(ProductsSearchRequest data)
        {
            var uri = new Uri(_httpClient.BaseAddress, $"products/search");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US");
            request.Headers.TryAddWithoutValidation("Accept", "application/json;version=2.0");
            request.Content = JsonContent.Create<ProductsSearchRequest>(data);

            OnProductSearchRequest(request);

            await AuthorizeRequest(request);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            OnProductSearchResponse(response);

            return await response.Content.ReadFromJsonAsync<Tripbuk.Server.Models.Viator.ProductsSearchResponse>();
        }
        partial void OnProductSearchRequest(HttpRequestMessage request);
        partial void OnProductSearchResponse(HttpResponseMessage response);


        /// <summary>
        /// Get tags
        /// </summary>
        /// <returns></returns>
        public async Task<Tripbuk.Server.Models.Viator.TagsResponse> GetTags()
        {
            var uri = new Uri(_httpClient.BaseAddress, $"products/tags");
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US");
            request.Headers.TryAddWithoutValidation("Accept", "application/json;version=2.0");

            OnGetTagsRequest(request);

            await AuthorizeRequest(request);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            OnGetTagsResponse(response);

            return await response.Content.ReadFromJsonAsync<Tripbuk.Server.Models.Viator.TagsResponse>();
        }
        partial void OnGetTagsRequest(HttpRequestMessage request);
        partial void OnGetTagsResponse(HttpResponseMessage response);


        /// <summary>
        /// Get destinations
        /// </summary>
        /// <returns></returns>
        public async Task<Tripbuk.Server.Models.Viator.DestinationsResponse> GetDestinations()
        {
            var uri = new Uri(_httpClient.BaseAddress, $"destinations");
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US");
            request.Headers.TryAddWithoutValidation("Accept", "application/json;version=2.0");

            OnGetDestinationsRequest(request);

            await AuthorizeRequest(request);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            OnGetDestinationsResponse(response);

            return await response.Content.ReadFromJsonAsync<Tripbuk.Server.Models.Viator.DestinationsResponse>();
        }
        partial void OnGetDestinationsRequest(HttpRequestMessage request);
        partial void OnGetDestinationsResponse(HttpResponseMessage response);
    }
}