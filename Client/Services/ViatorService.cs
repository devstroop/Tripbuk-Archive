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
using Microsoft.JSInterop;
using Tripbuk.Server.Models;
using Tripbuk.Server.Models.Viator;

namespace Tripbuk.Client.Services
{
    public partial class ViatorService(NavigationManager navigationManager, HttpClient httpClient)
    {
        /// <summary>
        /// Set the culture for the request
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="culture"></param>
        private void SerCulture(HttpRequestMessage requestMessage, string culture = "en-US")
        {
            requestMessage.Headers.TryAddWithoutValidation("Accept-Language", culture);
        }
        
        /// <summary>
        /// Search for products by free text
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Tripbuk.Server.Models.Viator.SearchFreeTextResponse> SearchFreeText(SearchFreetextRequest data)
        {
            var uri = new Uri($"{navigationManager.BaseUri}proxy/viator/search/freetext");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            SerCulture(request);
            request.Content = JsonContent.Create<SearchFreetextRequest>(data);
            OnSearchFreeTextRequest(request);
            var response = await httpClient.SendAsync(request);
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
            var uri = new Uri($"{navigationManager.BaseUri}proxy/viator/products/search");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            SerCulture(request);
            request.Content = JsonContent.Create<ProductsSearchRequest>(data);
            OnProductSearchRequest(request);
            var response = await httpClient.SendAsync(request);
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
            var uri = new Uri($"{navigationManager.BaseUri}proxy/viator/product/tags");
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            SerCulture(request);
            OnGetTagsRequest(request);
            var response = await httpClient.SendAsync(request);
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
            var uri = new Uri($"{navigationManager.BaseUri}proxy/viator/destinations");
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            SerCulture(request);
            OnGetDestinationsRequest(request);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            OnGetDestinationsResponse(response);
            return await response.Content.ReadFromJsonAsync<Tripbuk.Server.Models.Viator.DestinationsResponse>();
        }
        partial void OnGetDestinationsRequest(HttpRequestMessage request);
        partial void OnGetDestinationsResponse(HttpResponseMessage response);
    }
}