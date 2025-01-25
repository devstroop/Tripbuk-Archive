
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace Tripbuk.Client
{
    public partial class PostgresService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public PostgresService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/Postgres/");
        }


        public async System.Threading.Tasks.Task ExportTagsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/tags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/tags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTagsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/tags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/tags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTags(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.Tag>> GetTags(Query query)
        {
            return await GetTags(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.Tag>> GetTags(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Tags");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTags(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.Tag>>(response);
        }

        partial void OnCreateTag(HttpRequestMessage requestMessage);

        public async Task<Tripbuk.Server.Models.Postgres.Tag> CreateTag(Tripbuk.Server.Models.Postgres.Tag tag = default(Tripbuk.Server.Models.Postgres.Tag))
        {
            var uri = new Uri(baseUri, $"Tags");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");

            OnCreateTag(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Tripbuk.Server.Models.Postgres.Tag>(response);
        }

        partial void OnDeleteTag(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTag(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Tags({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTag(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTagById(HttpRequestMessage requestMessage);

        public async Task<Tripbuk.Server.Models.Postgres.Tag> GetTagById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Tags({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTagById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Tripbuk.Server.Models.Postgres.Tag>(response);
        }

        partial void OnUpdateTag(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTag(int id = default(int), Tripbuk.Server.Models.Postgres.Tag tag = default(Tripbuk.Server.Models.Postgres.Tag))
        {
            var uri = new Uri(baseUri, $"Tags({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");

            OnUpdateTag(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportPlaceTagsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/placetags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/placetags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPlaceTagsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/placetags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/placetags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetPlaceTags(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.PlaceTag>> GetPlaceTags(Query query)
        {
            return await GetPlaceTags(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.PlaceTag>> GetPlaceTags(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"PlaceTags");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPlaceTags(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.PlaceTag>>(response);
        }

        partial void OnCreatePlaceTag(HttpRequestMessage requestMessage);

        public async Task<Tripbuk.Server.Models.Postgres.PlaceTag> CreatePlaceTag(Tripbuk.Server.Models.Postgres.PlaceTag placeTag = default(Tripbuk.Server.Models.Postgres.PlaceTag))
        {
            var uri = new Uri(baseUri, $"PlaceTags");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(placeTag), Encoding.UTF8, "application/json");

            OnCreatePlaceTag(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Tripbuk.Server.Models.Postgres.PlaceTag>(response);
        }

        partial void OnDeletePlaceTag(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeletePlaceTag(Guid placeId = default(Guid), int tagId = default(int))
        {
            var uri = new Uri(baseUri, $"PlaceTags(PlaceId={placeId},TagId={tagId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePlaceTag(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetPlaceTagByPlaceIdAndTagId(HttpRequestMessage requestMessage);

        public async Task<Tripbuk.Server.Models.Postgres.PlaceTag> GetPlaceTagByPlaceIdAndTagId(string expand = default(string), Guid placeId = default(Guid), int tagId = default(int))
        {
            var uri = new Uri(baseUri, $"PlaceTags(PlaceId={placeId},TagId={tagId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPlaceTagByPlaceIdAndTagId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Tripbuk.Server.Models.Postgres.PlaceTag>(response);
        }

        partial void OnUpdatePlaceTag(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdatePlaceTag(Guid placeId = default(Guid), int tagId = default(int), Tripbuk.Server.Models.Postgres.PlaceTag placeTag = default(Tripbuk.Server.Models.Postgres.PlaceTag))
        {
            var uri = new Uri(baseUri, $"PlaceTags(PlaceId={placeId},TagId={tagId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(placeTag), Encoding.UTF8, "application/json");

            OnUpdatePlaceTag(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportPlacesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/places/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/places/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPlacesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/places/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/places/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetPlaces(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.Place>> GetPlaces(Query query)
        {
            return await GetPlaces(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.Place>> GetPlaces(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Places");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPlaces(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.Place>>(response);
        }

        partial void OnCreatePlace(HttpRequestMessage requestMessage);

        public async Task<Tripbuk.Server.Models.Postgres.Place> CreatePlace(Tripbuk.Server.Models.Postgres.Place place = default(Tripbuk.Server.Models.Postgres.Place))
        {
            var uri = new Uri(baseUri, $"Places");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(place), Encoding.UTF8, "application/json");

            OnCreatePlace(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Tripbuk.Server.Models.Postgres.Place>(response);
        }

        partial void OnDeletePlace(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeletePlace(Guid id = default(Guid))
        {
            var uri = new Uri(baseUri, $"Places({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePlace(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetPlaceById(HttpRequestMessage requestMessage);

        public async Task<Tripbuk.Server.Models.Postgres.Place> GetPlaceById(string expand = default(string), Guid id = default(Guid))
        {
            var uri = new Uri(baseUri, $"Places({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPlaceById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Tripbuk.Server.Models.Postgres.Place>(response);
        }

        partial void OnUpdatePlace(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdatePlace(Guid id = default(Guid), Tripbuk.Server.Models.Postgres.Place place = default(Tripbuk.Server.Models.Postgres.Place))
        {
            var uri = new Uri(baseUri, $"Places({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(place), Encoding.UTF8, "application/json");

            OnUpdatePlace(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportDestinationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/destinations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/destinations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportDestinationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/destinations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/destinations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetDestinations(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.Destination>> GetDestinations(Query query)
        {
            return await GetDestinations(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.Destination>> GetDestinations(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Destinations");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDestinations(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Tripbuk.Server.Models.Postgres.Destination>>(response);
        }

        partial void OnCreateDestination(HttpRequestMessage requestMessage);

        public async Task<Tripbuk.Server.Models.Postgres.Destination> CreateDestination(Tripbuk.Server.Models.Postgres.Destination destination = default(Tripbuk.Server.Models.Postgres.Destination))
        {
            var uri = new Uri(baseUri, $"Destinations");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(destination), Encoding.UTF8, "application/json");

            OnCreateDestination(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Tripbuk.Server.Models.Postgres.Destination>(response);
        }

        partial void OnDeleteDestination(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteDestination(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Destinations({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteDestination(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetDestinationById(HttpRequestMessage requestMessage);

        public async Task<Tripbuk.Server.Models.Postgres.Destination> GetDestinationById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Destinations({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDestinationById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Tripbuk.Server.Models.Postgres.Destination>(response);
        }

        partial void OnUpdateDestination(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateDestination(int id = default(int), Tripbuk.Server.Models.Postgres.Destination destination = default(Tripbuk.Server.Models.Postgres.Destination))
        {
            var uri = new Uri(baseUri, $"Destinations({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(destination), Encoding.UTF8, "application/json");

            OnUpdateDestination(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}