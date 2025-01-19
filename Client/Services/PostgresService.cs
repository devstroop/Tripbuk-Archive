
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


        public async System.Threading.Tasks.Task ExportAccountGroupsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountgroups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountgroups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAccountGroupsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountgroups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountgroups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAccountGroups(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.AccountGroup>> GetAccountGroups(Query query)
        {
            return await GetAccountGroups(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.AccountGroup>> GetAccountGroups(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AccountGroups");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAccountGroups(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.AccountGroup>>(response);
        }

        partial void OnCreateAccountGroup(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.AccountGroup> CreateAccountGroup(ERP.Server.Models.Postgres.AccountGroup accountGroup = default(ERP.Server.Models.Postgres.AccountGroup))
        {
            var uri = new Uri(baseUri, $"AccountGroups");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(accountGroup), Encoding.UTF8, "application/json");

            OnCreateAccountGroup(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.AccountGroup>(response);
        }

        partial void OnDeleteAccountGroup(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAccountGroup(int id = default(int))
        {
            var uri = new Uri(baseUri, $"AccountGroups({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAccountGroup(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAccountGroupById(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.AccountGroup> GetAccountGroupById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"AccountGroups({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAccountGroupById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.AccountGroup>(response);
        }

        partial void OnUpdateAccountGroup(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAccountGroup(int id = default(int), ERP.Server.Models.Postgres.AccountGroup accountGroup = default(ERP.Server.Models.Postgres.AccountGroup))
        {
            var uri = new Uri(baseUri, $"AccountGroups({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", accountGroup.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(accountGroup), Encoding.UTF8, "application/json");

            OnUpdateAccountGroup(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAccountsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accounts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accounts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAccountsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accounts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accounts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAccounts(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Account>> GetAccounts(Query query)
        {
            return await GetAccounts(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Account>> GetAccounts(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Accounts");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAccounts(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Account>>(response);
        }

        partial void OnCreateAccount(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.Account> CreateAccount(ERP.Server.Models.Postgres.Account account = default(ERP.Server.Models.Postgres.Account))
        {
            var uri = new Uri(baseUri, $"Accounts");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(account), Encoding.UTF8, "application/json");

            OnCreateAccount(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.Account>(response);
        }

        partial void OnDeleteAccount(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAccount(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Accounts({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAccount(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAccountById(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.Account> GetAccountById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Accounts({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAccountById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.Account>(response);
        }

        partial void OnUpdateAccount(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAccount(int id = default(int), ERP.Server.Models.Postgres.Account account = default(ERP.Server.Models.Postgres.Account))
        {
            var uri = new Uri(baseUri, $"Accounts({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", account.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(account), Encoding.UTF8, "application/json");

            OnUpdateAccount(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportItemGroupsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemgroups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemgroups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportItemGroupsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemgroups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemgroups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetItemGroups(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.ItemGroup>> GetItemGroups(Query query)
        {
            return await GetItemGroups(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.ItemGroup>> GetItemGroups(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ItemGroups");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetItemGroups(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.ItemGroup>>(response);
        }

        partial void OnCreateItemGroup(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.ItemGroup> CreateItemGroup(ERP.Server.Models.Postgres.ItemGroup itemGroup = default(ERP.Server.Models.Postgres.ItemGroup))
        {
            var uri = new Uri(baseUri, $"ItemGroups");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(itemGroup), Encoding.UTF8, "application/json");

            OnCreateItemGroup(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.ItemGroup>(response);
        }

        partial void OnDeleteItemGroup(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteItemGroup(int id = default(int))
        {
            var uri = new Uri(baseUri, $"ItemGroups({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteItemGroup(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetItemGroupById(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.ItemGroup> GetItemGroupById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"ItemGroups({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetItemGroupById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.ItemGroup>(response);
        }

        partial void OnUpdateItemGroup(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateItemGroup(int id = default(int), ERP.Server.Models.Postgres.ItemGroup itemGroup = default(ERP.Server.Models.Postgres.ItemGroup))
        {
            var uri = new Uri(baseUri, $"ItemGroups({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", itemGroup.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(itemGroup), Encoding.UTF8, "application/json");

            OnUpdateItemGroup(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportItemsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/items/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/items/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportItemsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/items/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/items/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetItems(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Item>> GetItems(Query query)
        {
            return await GetItems(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Item>> GetItems(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Items");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetItems(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Item>>(response);
        }

        partial void OnCreateItem(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.Item> CreateItem(ERP.Server.Models.Postgres.Item item = default(ERP.Server.Models.Postgres.Item))
        {
            var uri = new Uri(baseUri, $"Items");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(item), Encoding.UTF8, "application/json");

            OnCreateItem(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.Item>(response);
        }

        partial void OnDeleteItem(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteItem(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Items({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteItem(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetItemById(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.Item> GetItemById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Items({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetItemById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.Item>(response);
        }

        partial void OnUpdateItem(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateItem(int id = default(int), ERP.Server.Models.Postgres.Item item = default(ERP.Server.Models.Postgres.Item))
        {
            var uri = new Uri(baseUri, $"Items({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", item.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(item), Encoding.UTF8, "application/json");

            OnUpdateItem(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStandardNarrationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/standardnarrations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/standardnarrations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStandardNarrationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/standardnarrations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/standardnarrations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStandardNarrations(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.StandardNarration>> GetStandardNarrations(Query query)
        {
            return await GetStandardNarrations(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.StandardNarration>> GetStandardNarrations(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"StandardNarrations");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStandardNarrations(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.StandardNarration>>(response);
        }

        partial void OnCreateStandardNarration(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.StandardNarration> CreateStandardNarration(ERP.Server.Models.Postgres.StandardNarration standardNarration = default(ERP.Server.Models.Postgres.StandardNarration))
        {
            var uri = new Uri(baseUri, $"StandardNarrations");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(standardNarration), Encoding.UTF8, "application/json");

            OnCreateStandardNarration(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.StandardNarration>(response);
        }

        partial void OnDeleteStandardNarration(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStandardNarration(int id = default(int))
        {
            var uri = new Uri(baseUri, $"StandardNarrations({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStandardNarration(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStandardNarrationById(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.StandardNarration> GetStandardNarrationById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"StandardNarrations({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStandardNarrationById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.StandardNarration>(response);
        }

        partial void OnUpdateStandardNarration(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStandardNarration(int id = default(int), ERP.Server.Models.Postgres.StandardNarration standardNarration = default(ERP.Server.Models.Postgres.StandardNarration))
        {
            var uri = new Uri(baseUri, $"StandardNarrations({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", standardNarration.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(standardNarration), Encoding.UTF8, "application/json");

            OnUpdateStandardNarration(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportUnitConversionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/unitconversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/unitconversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportUnitConversionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/unitconversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/unitconversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetUnitConversions(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.UnitConversion>> GetUnitConversions(Query query)
        {
            return await GetUnitConversions(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.UnitConversion>> GetUnitConversions(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"UnitConversions");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUnitConversions(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.UnitConversion>>(response);
        }

        partial void OnCreateUnitConversion(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.UnitConversion> CreateUnitConversion(ERP.Server.Models.Postgres.UnitConversion unitConversion = default(ERP.Server.Models.Postgres.UnitConversion))
        {
            var uri = new Uri(baseUri, $"UnitConversions");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(unitConversion), Encoding.UTF8, "application/json");

            OnCreateUnitConversion(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.UnitConversion>(response);
        }

        partial void OnDeleteUnitConversion(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteUnitConversion(int id = default(int))
        {
            var uri = new Uri(baseUri, $"UnitConversions({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteUnitConversion(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetUnitConversionById(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.UnitConversion> GetUnitConversionById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"UnitConversions({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUnitConversionById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.UnitConversion>(response);
        }

        partial void OnUpdateUnitConversion(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateUnitConversion(int id = default(int), ERP.Server.Models.Postgres.UnitConversion unitConversion = default(ERP.Server.Models.Postgres.UnitConversion))
        {
            var uri = new Uri(baseUri, $"UnitConversions({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", unitConversion.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(unitConversion), Encoding.UTF8, "application/json");

            OnUpdateUnitConversion(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportUnitsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/units/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/units/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportUnitsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/units/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/units/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetUnits(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Unit>> GetUnits(Query query)
        {
            return await GetUnits(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Unit>> GetUnits(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Units");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUnits(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.Unit>>(response);
        }

        partial void OnCreateUnit(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.Unit> CreateUnit(ERP.Server.Models.Postgres.Unit _unit = default(ERP.Server.Models.Postgres.Unit))
        {
            var uri = new Uri(baseUri, $"Units");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(_unit), Encoding.UTF8, "application/json");

            OnCreateUnit(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.Unit>(response);
        }

        partial void OnDeleteUnit(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteUnit(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Units({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteUnit(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetUnitById(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.Unit> GetUnitById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Units({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUnitById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.Unit>(response);
        }

        partial void OnUpdateUnit(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateUnit(int id = default(int), ERP.Server.Models.Postgres.Unit _unit = default(ERP.Server.Models.Postgres.Unit))
        {
            var uri = new Uri(baseUri, $"Units({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", _unit.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(_unit), Encoding.UTF8, "application/json");

            OnUpdateUnit(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSmtpConfigsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/smtpconfigs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/smtpconfigs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSmtpConfigsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/smtpconfigs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/smtpconfigs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSmtpConfigs(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.SmtpConfig>> GetSmtpConfigs(Query query)
        {
            return await GetSmtpConfigs(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.SmtpConfig>> GetSmtpConfigs(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"SmtpConfigs");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSmtpConfigs(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ERP.Server.Models.Postgres.SmtpConfig>>(response);
        }

        partial void OnCreateSmtpConfig(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.SmtpConfig> CreateSmtpConfig(ERP.Server.Models.Postgres.SmtpConfig smtpConfig = default(ERP.Server.Models.Postgres.SmtpConfig))
        {
            var uri = new Uri(baseUri, $"SmtpConfigs");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(smtpConfig), Encoding.UTF8, "application/json");

            OnCreateSmtpConfig(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.SmtpConfig>(response);
        }

        partial void OnDeleteSmtpConfig(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSmtpConfig(Guid id = default(Guid))
        {
            var uri = new Uri(baseUri, $"SmtpConfigs({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSmtpConfig(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSmtpConfigById(HttpRequestMessage requestMessage);

        public async Task<ERP.Server.Models.Postgres.SmtpConfig> GetSmtpConfigById(string expand = default(string), Guid id = default(Guid))
        {
            var uri = new Uri(baseUri, $"SmtpConfigs({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSmtpConfigById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ERP.Server.Models.Postgres.SmtpConfig>(response);
        }

        partial void OnUpdateSmtpConfig(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSmtpConfig(Guid id = default(Guid), ERP.Server.Models.Postgres.SmtpConfig smtpConfig = default(ERP.Server.Models.Postgres.SmtpConfig))
        {
            var uri = new Uri(baseUri, $"SmtpConfigs({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", smtpConfig.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(smtpConfig), Encoding.UTF8, "application/json");

            OnUpdateSmtpConfig(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}