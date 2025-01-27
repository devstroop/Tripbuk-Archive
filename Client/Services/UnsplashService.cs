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
    public partial class UnsplashService(NavigationManager navigationManager, HttpClient httpClient)
    {
        /// <summary>
        /// Search for random photos on Unsplash
        /// </summary>
        /// <returns></returns>
        public async Task<Tripbuk.Server.Models.Unsplash.UnsplashPhoto> GetRandomPhoto()
        {
            var uri = new Uri($"{navigationManager.BaseUri}proxy/unsplash/photos/random");
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            OnRandomPhotoRequest(request);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            OnRandomPhotoResponse(response);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return await response.Content.ReadFromJsonAsync<Tripbuk.Server.Models.Unsplash.UnsplashPhoto>();
        }
        partial void OnRandomPhotoRequest(HttpRequestMessage request);
        partial void OnRandomPhotoResponse(HttpResponseMessage response);
    }
}