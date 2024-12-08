
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

namespace ERP.Client
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


        public async System.Threading.Tasks.Task ExportMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/masters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/masters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/masters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/masters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMasters(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Master>> GetMasters(Query query)
        {
            return await GetMasters(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Master>> GetMasters(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Masters");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMasters(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Master>>(response);
        }

        partial void OnCreateMaster(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.Master> CreateMaster(ERP.Server.Models.Postgres.Master master = default(ERP.Server.Models.Postgres.Master))
        {
            var uri = new Uri(baseUri, $"Masters");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(master), Encoding.UTF8, "application/json");

            OnCreateMaster(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.Master>(response);
        }

        partial void OnDeleteMaster(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMaster(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Masters({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMasterById(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.Master> GetMasterById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Masters({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMasterById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.Master>(response);
        }

        partial void OnUpdateMaster(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMaster(int id = default(int), ERP.Server.Models.Postgres.Master master = default(ERP.Server.Models.Postgres.Master))
        {
            var uri = new Uri(baseUri, $"Masters({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", master.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(master), Encoding.UTF8, "application/json");

            OnUpdateMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStdNarrationMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/stdnarrationmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/stdnarrationmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStdNarrationMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/stdnarrationmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/stdnarrationmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStdNarrationMasters(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.StdNarrationMaster>> GetStdNarrationMasters(Query query)
        {
            return await GetStdNarrationMasters(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.StdNarrationMaster>> GetStdNarrationMasters(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"StdNarrationMasters");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStdNarrationMasters(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.StdNarrationMaster>>(response);
        }

        partial void OnCreateStdNarrationMaster(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.StdNarrationMaster> CreateStdNarrationMaster(ERP.Server.Models.Postgres.StdNarrationMaster stdNarrationMaster = default(ERP.Server.Models.Postgres.StdNarrationMaster))
        {
            var uri = new Uri(baseUri, $"StdNarrationMasters");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(stdNarrationMaster), Encoding.UTF8, "application/json");

            OnCreateStdNarrationMaster(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.StdNarrationMaster>(response);
        }

        partial void OnDeleteStdNarrationMaster(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStdNarrationMaster(int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"StdNarrationMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStdNarrationMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStdNarrationMasterByMasterId(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.StdNarrationMaster> GetStdNarrationMasterByMasterId(string expand = default(string), int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"StdNarrationMasters({masterId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStdNarrationMasterByMasterId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.StdNarrationMaster>(response);
        }

        partial void OnUpdateStdNarrationMaster(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStdNarrationMaster(int masterId = default(int), ERP.Server.Models.Postgres.StdNarrationMaster stdNarrationMaster = default(ERP.Server.Models.Postgres.StdNarrationMaster))
        {
            var uri = new Uri(baseUri, $"StdNarrationMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", stdNarrationMaster.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(stdNarrationMaster), Encoding.UTF8, "application/json");

            OnUpdateStdNarrationMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportItemMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportItemMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetItemMasters(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.ItemMaster>> GetItemMasters(Query query)
        {
            return await GetItemMasters(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.ItemMaster>> GetItemMasters(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ItemMasters");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetItemMasters(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.ItemMaster>>(response);
        }

        partial void OnCreateItemMaster(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.ItemMaster> CreateItemMaster(ERP.Server.Models.Postgres.ItemMaster itemMaster = default(ERP.Server.Models.Postgres.ItemMaster))
        {
            var uri = new Uri(baseUri, $"ItemMasters");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(itemMaster), Encoding.UTF8, "application/json");

            OnCreateItemMaster(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.ItemMaster>(response);
        }

        partial void OnDeleteItemMaster(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteItemMaster(int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"ItemMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteItemMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetItemMasterByMasterId(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.ItemMaster> GetItemMasterByMasterId(string expand = default(string), int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"ItemMasters({masterId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetItemMasterByMasterId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.ItemMaster>(response);
        }

        partial void OnUpdateItemMaster(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateItemMaster(int masterId = default(int), ERP.Server.Models.Postgres.ItemMaster itemMaster = default(ERP.Server.Models.Postgres.ItemMaster))
        {
            var uri = new Uri(baseUri, $"ItemMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", itemMaster.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(itemMaster), Encoding.UTF8, "application/json");

            OnUpdateItemMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportItemGroupMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemgroupmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemgroupmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportItemGroupMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemgroupmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemgroupmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetItemGroupMasters(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.ItemGroupMaster>> GetItemGroupMasters(Query query)
        {
            return await GetItemGroupMasters(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.ItemGroupMaster>> GetItemGroupMasters(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ItemGroupMasters");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetItemGroupMasters(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.ItemGroupMaster>>(response);
        }

        partial void OnCreateItemGroupMaster(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.ItemGroupMaster> CreateItemGroupMaster(ERP.Server.Models.Postgres.ItemGroupMaster itemGroupMaster = default(ERP.Server.Models.Postgres.ItemGroupMaster))
        {
            var uri = new Uri(baseUri, $"ItemGroupMasters");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(itemGroupMaster), Encoding.UTF8, "application/json");

            OnCreateItemGroupMaster(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.ItemGroupMaster>(response);
        }

        partial void OnDeleteItemGroupMaster(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteItemGroupMaster(int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"ItemGroupMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteItemGroupMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetItemGroupMasterByMasterId(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.ItemGroupMaster> GetItemGroupMasterByMasterId(string expand = default(string), int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"ItemGroupMasters({masterId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetItemGroupMasterByMasterId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.ItemGroupMaster>(response);
        }

        partial void OnUpdateItemGroupMaster(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateItemGroupMaster(int masterId = default(int), ERP.Server.Models.Postgres.ItemGroupMaster itemGroupMaster = default(ERP.Server.Models.Postgres.ItemGroupMaster))
        {
            var uri = new Uri(baseUri, $"ItemGroupMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", itemGroupMaster.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(itemGroupMaster), Encoding.UTF8, "application/json");

            OnUpdateItemGroupMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAccountMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAccountMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAccountMasters(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.AccountMaster>> GetAccountMasters(Query query)
        {
            return await GetAccountMasters(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.AccountMaster>> GetAccountMasters(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AccountMasters");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAccountMasters(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.AccountMaster>>(response);
        }

        partial void OnCreateAccountMaster(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.AccountMaster> CreateAccountMaster(ERP.Server.Models.Postgres.AccountMaster accountMaster = default(ERP.Server.Models.Postgres.AccountMaster))
        {
            var uri = new Uri(baseUri, $"AccountMasters");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(accountMaster), Encoding.UTF8, "application/json");

            OnCreateAccountMaster(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.AccountMaster>(response);
        }

        partial void OnDeleteAccountMaster(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAccountMaster(int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"AccountMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAccountMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAccountMasterByMasterId(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.AccountMaster> GetAccountMasterByMasterId(string expand = default(string), int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"AccountMasters({masterId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAccountMasterByMasterId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.AccountMaster>(response);
        }

        partial void OnUpdateAccountMaster(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAccountMaster(int masterId = default(int), ERP.Server.Models.Postgres.AccountMaster accountMaster = default(ERP.Server.Models.Postgres.AccountMaster))
        {
            var uri = new Uri(baseUri, $"AccountMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", accountMaster.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(accountMaster), Encoding.UTF8, "application/json");

            OnUpdateAccountMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAccountGroupMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountgroupmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountgroupmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAccountGroupMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountgroupmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountgroupmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAccountGroupMasters(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.AccountGroupMaster>> GetAccountGroupMasters(Query query)
        {
            return await GetAccountGroupMasters(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.AccountGroupMaster>> GetAccountGroupMasters(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AccountGroupMasters");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAccountGroupMasters(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.AccountGroupMaster>>(response);
        }

        partial void OnCreateAccountGroupMaster(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.AccountGroupMaster> CreateAccountGroupMaster(ERP.Server.Models.Postgres.AccountGroupMaster accountGroupMaster = default(ERP.Server.Models.Postgres.AccountGroupMaster))
        {
            var uri = new Uri(baseUri, $"AccountGroupMasters");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(accountGroupMaster), Encoding.UTF8, "application/json");

            OnCreateAccountGroupMaster(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.AccountGroupMaster>(response);
        }

        partial void OnDeleteAccountGroupMaster(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAccountGroupMaster(int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"AccountGroupMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAccountGroupMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAccountGroupMasterByMasterId(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.AccountGroupMaster> GetAccountGroupMasterByMasterId(string expand = default(string), int masterId = default(int))
        {
            var uri = new Uri(baseUri, $"AccountGroupMasters({masterId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAccountGroupMasterByMasterId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.AccountGroupMaster>(response);
        }

        partial void OnUpdateAccountGroupMaster(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAccountGroupMaster(int masterId = default(int), ERP.Server.Models.Postgres.AccountGroupMaster accountGroupMaster = default(ERP.Server.Models.Postgres.AccountGroupMaster))
        {
            var uri = new Uri(baseUri, $"AccountGroupMasters({masterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", accountGroupMaster.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(accountGroupMaster), Encoding.UTF8, "application/json");

            OnUpdateAccountGroupMaster(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}