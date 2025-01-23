using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Tripbuk.Server.Data;

namespace Tripbuk.Server
{
    public partial class PostgresService
    {
        PostgresContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly PostgresContext context;
        private readonly NavigationManager navigationManager;

        public PostgresService(PostgresContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportAccountGroupsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountgroups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountgroups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAccountGroupsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountgroups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountgroups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAccountGroupsRead(ref IQueryable<Server.Models.Postgres.AccountGroup> items);

        public async Task<IQueryable<Server.Models.Postgres.AccountGroup>> GetAccountGroups(Query query = null)
        {
            var items = Context.AccountGroups.AsQueryable();

            items = items.Include(i => i.AccountGroup1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAccountGroupsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAccountGroupGet(Server.Models.Postgres.AccountGroup item);
        partial void OnGetAccountGroupById(ref IQueryable<Server.Models.Postgres.AccountGroup> items);


        public async Task<Server.Models.Postgres.AccountGroup> GetAccountGroupById(int id)
        {
            var items = Context.AccountGroups
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.AccountGroup1);
 
            OnGetAccountGroupById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAccountGroupGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAccountGroupCreated(Server.Models.Postgres.AccountGroup item);
        partial void OnAfterAccountGroupCreated(Server.Models.Postgres.AccountGroup item);

        public async Task<Server.Models.Postgres.AccountGroup> CreateAccountGroup(Server.Models.Postgres.AccountGroup accountgroup)
        {
            OnAccountGroupCreated(accountgroup);

            var existingItem = Context.AccountGroups
                              .Where(i => i.Id == accountgroup.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AccountGroups.Add(accountgroup);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(accountgroup).State = EntityState.Detached;
                throw;
            }

            OnAfterAccountGroupCreated(accountgroup);

            return accountgroup;
        }

        public async Task<Server.Models.Postgres.AccountGroup> CancelAccountGroupChanges(Server.Models.Postgres.AccountGroup item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAccountGroupUpdated(Server.Models.Postgres.AccountGroup item);
        partial void OnAfterAccountGroupUpdated(Server.Models.Postgres.AccountGroup item);

        public async Task<Server.Models.Postgres.AccountGroup> UpdateAccountGroup(int id, Server.Models.Postgres.AccountGroup accountgroup)
        {
            OnAccountGroupUpdated(accountgroup);

            var itemToUpdate = Context.AccountGroups
                              .Where(i => i.Id == accountgroup.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(accountgroup);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAccountGroupUpdated(accountgroup);

            return accountgroup;
        }

        partial void OnAccountGroupDeleted(Server.Models.Postgres.AccountGroup item);
        partial void OnAfterAccountGroupDeleted(Server.Models.Postgres.AccountGroup item);

        public async Task<Server.Models.Postgres.AccountGroup> DeleteAccountGroup(int id)
        {
            var itemToDelete = Context.AccountGroups
                              .Where(i => i.Id == id)
                              .Include(i => i.AccountGroups1)
                              .Include(i => i.Accounts)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAccountGroupDeleted(itemToDelete);


            Context.AccountGroups.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAccountGroupDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAccountsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accounts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accounts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAccountsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accounts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accounts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAccountsRead(ref IQueryable<Server.Models.Postgres.Account> items);

        public async Task<IQueryable<Server.Models.Postgres.Account>> GetAccounts(Query query = null)
        {
            var items = Context.Accounts.AsQueryable();

            items = items.Include(i => i.AccountGroup);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAccountsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAccountGet(Server.Models.Postgres.Account item);
        partial void OnGetAccountById(ref IQueryable<Server.Models.Postgres.Account> items);


        public async Task<Server.Models.Postgres.Account> GetAccountById(int id)
        {
            var items = Context.Accounts
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.AccountGroup);
 
            OnGetAccountById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAccountGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAccountCreated(Server.Models.Postgres.Account item);
        partial void OnAfterAccountCreated(Server.Models.Postgres.Account item);

        public async Task<Server.Models.Postgres.Account> CreateAccount(Server.Models.Postgres.Account account)
        {
            OnAccountCreated(account);

            var existingItem = Context.Accounts
                              .Where(i => i.Id == account.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Accounts.Add(account);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(account).State = EntityState.Detached;
                throw;
            }

            OnAfterAccountCreated(account);

            return account;
        }

        public async Task<Server.Models.Postgres.Account> CancelAccountChanges(Server.Models.Postgres.Account item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAccountUpdated(Server.Models.Postgres.Account item);
        partial void OnAfterAccountUpdated(Server.Models.Postgres.Account item);

        public async Task<Server.Models.Postgres.Account> UpdateAccount(int id, Server.Models.Postgres.Account account)
        {
            OnAccountUpdated(account);

            var itemToUpdate = Context.Accounts
                              .Where(i => i.Id == account.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(account);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAccountUpdated(account);

            return account;
        }

        partial void OnAccountDeleted(Server.Models.Postgres.Account item);
        partial void OnAfterAccountDeleted(Server.Models.Postgres.Account item);

        public async Task<Server.Models.Postgres.Account> DeleteAccount(int id)
        {
            var itemToDelete = Context.Accounts
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAccountDeleted(itemToDelete);


            Context.Accounts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAccountDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportItemGroupsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemgroups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemgroups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportItemGroupsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemgroups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemgroups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnItemGroupsRead(ref IQueryable<Server.Models.Postgres.ItemGroup> items);

        public async Task<IQueryable<Server.Models.Postgres.ItemGroup>> GetItemGroups(Query query = null)
        {
            var items = Context.ItemGroups.AsQueryable();

            items = items.Include(i => i.ItemGroup1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnItemGroupsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnItemGroupGet(Server.Models.Postgres.ItemGroup item);
        partial void OnGetItemGroupById(ref IQueryable<Server.Models.Postgres.ItemGroup> items);


        public async Task<Server.Models.Postgres.ItemGroup> GetItemGroupById(int id)
        {
            var items = Context.ItemGroups
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.ItemGroup1);
 
            OnGetItemGroupById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnItemGroupGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnItemGroupCreated(Server.Models.Postgres.ItemGroup item);
        partial void OnAfterItemGroupCreated(Server.Models.Postgres.ItemGroup item);

        public async Task<Server.Models.Postgres.ItemGroup> CreateItemGroup(Server.Models.Postgres.ItemGroup itemgroup)
        {
            OnItemGroupCreated(itemgroup);

            var existingItem = Context.ItemGroups
                              .Where(i => i.Id == itemgroup.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ItemGroups.Add(itemgroup);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemgroup).State = EntityState.Detached;
                throw;
            }

            OnAfterItemGroupCreated(itemgroup);

            return itemgroup;
        }

        public async Task<Server.Models.Postgres.ItemGroup> CancelItemGroupChanges(Server.Models.Postgres.ItemGroup item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnItemGroupUpdated(Server.Models.Postgres.ItemGroup item);
        partial void OnAfterItemGroupUpdated(Server.Models.Postgres.ItemGroup item);

        public async Task<Server.Models.Postgres.ItemGroup> UpdateItemGroup(int id, Server.Models.Postgres.ItemGroup itemgroup)
        {
            OnItemGroupUpdated(itemgroup);

            var itemToUpdate = Context.ItemGroups
                              .Where(i => i.Id == itemgroup.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(itemgroup);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterItemGroupUpdated(itemgroup);

            return itemgroup;
        }

        partial void OnItemGroupDeleted(Server.Models.Postgres.ItemGroup item);
        partial void OnAfterItemGroupDeleted(Server.Models.Postgres.ItemGroup item);

        public async Task<Server.Models.Postgres.ItemGroup> DeleteItemGroup(int id)
        {
            var itemToDelete = Context.ItemGroups
                              .Where(i => i.Id == id)
                              .Include(i => i.ItemGroups1)
                              .Include(i => i.Items)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnItemGroupDeleted(itemToDelete);


            Context.ItemGroups.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterItemGroupDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportItemsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/items/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/items/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportItemsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/items/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/items/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnItemsRead(ref IQueryable<Server.Models.Postgres.Item> items);

        public async Task<IQueryable<Server.Models.Postgres.Item>> GetItems(Query query = null)
        {
            var items = Context.Items.AsQueryable();

            items = items.Include(i => i.ItemGroup);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnItemsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnItemGet(Server.Models.Postgres.Item item);
        partial void OnGetItemById(ref IQueryable<Server.Models.Postgres.Item> items);


        public async Task<Server.Models.Postgres.Item> GetItemById(int id)
        {
            var items = Context.Items
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.ItemGroup);
 
            OnGetItemById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnItemGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnItemCreated(Server.Models.Postgres.Item item);
        partial void OnAfterItemCreated(Server.Models.Postgres.Item item);

        public async Task<Server.Models.Postgres.Item> CreateItem(Server.Models.Postgres.Item item)
        {
            OnItemCreated(item);

            var existingItem = Context.Items
                              .Where(i => i.Id == item.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Items.Add(item);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(item).State = EntityState.Detached;
                throw;
            }

            OnAfterItemCreated(item);

            return item;
        }

        public async Task<Server.Models.Postgres.Item> CancelItemChanges(Server.Models.Postgres.Item item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnItemUpdated(Server.Models.Postgres.Item item);
        partial void OnAfterItemUpdated(Server.Models.Postgres.Item item);

        public async Task<Server.Models.Postgres.Item> UpdateItem(int id, Server.Models.Postgres.Item item)
        {
            OnItemUpdated(item);

            var itemToUpdate = Context.Items
                              .Where(i => i.Id == item.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(item);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterItemUpdated(item);

            return item;
        }

        partial void OnItemDeleted(Server.Models.Postgres.Item item);
        partial void OnAfterItemDeleted(Server.Models.Postgres.Item item);

        public async Task<Server.Models.Postgres.Item> DeleteItem(int id)
        {
            var itemToDelete = Context.Items
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnItemDeleted(itemToDelete);


            Context.Items.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterItemDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStandardNarrationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/standardnarrations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/standardnarrations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStandardNarrationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/standardnarrations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/standardnarrations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStandardNarrationsRead(ref IQueryable<Server.Models.Postgres.StandardNarration> items);

        public async Task<IQueryable<Server.Models.Postgres.StandardNarration>> GetStandardNarrations(Query query = null)
        {
            var items = Context.StandardNarrations.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStandardNarrationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStandardNarrationGet(Server.Models.Postgres.StandardNarration item);
        partial void OnGetStandardNarrationById(ref IQueryable<Server.Models.Postgres.StandardNarration> items);


        public async Task<Server.Models.Postgres.StandardNarration> GetStandardNarrationById(int id)
        {
            var items = Context.StandardNarrations
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStandardNarrationById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStandardNarrationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStandardNarrationCreated(Server.Models.Postgres.StandardNarration item);
        partial void OnAfterStandardNarrationCreated(Server.Models.Postgres.StandardNarration item);

        public async Task<Server.Models.Postgres.StandardNarration> CreateStandardNarration(Server.Models.Postgres.StandardNarration standardnarration)
        {
            OnStandardNarrationCreated(standardnarration);

            var existingItem = Context.StandardNarrations
                              .Where(i => i.Id == standardnarration.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StandardNarrations.Add(standardnarration);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(standardnarration).State = EntityState.Detached;
                throw;
            }

            OnAfterStandardNarrationCreated(standardnarration);

            return standardnarration;
        }

        public async Task<Server.Models.Postgres.StandardNarration> CancelStandardNarrationChanges(Server.Models.Postgres.StandardNarration item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStandardNarrationUpdated(Server.Models.Postgres.StandardNarration item);
        partial void OnAfterStandardNarrationUpdated(Server.Models.Postgres.StandardNarration item);

        public async Task<Server.Models.Postgres.StandardNarration> UpdateStandardNarration(int id, Server.Models.Postgres.StandardNarration standardnarration)
        {
            OnStandardNarrationUpdated(standardnarration);

            var itemToUpdate = Context.StandardNarrations
                              .Where(i => i.Id == standardnarration.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(standardnarration);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStandardNarrationUpdated(standardnarration);

            return standardnarration;
        }

        partial void OnStandardNarrationDeleted(Server.Models.Postgres.StandardNarration item);
        partial void OnAfterStandardNarrationDeleted(Server.Models.Postgres.StandardNarration item);

        public async Task<Server.Models.Postgres.StandardNarration> DeleteStandardNarration(int id)
        {
            var itemToDelete = Context.StandardNarrations
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStandardNarrationDeleted(itemToDelete);


            Context.StandardNarrations.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStandardNarrationDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportUnitConversionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/unitconversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/unitconversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportUnitConversionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/unitconversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/unitconversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnUnitConversionsRead(ref IQueryable<Server.Models.Postgres.UnitConversion> items);

        public async Task<IQueryable<Server.Models.Postgres.UnitConversion>> GetUnitConversions(Query query = null)
        {
            var items = Context.UnitConversions.AsQueryable();

            items = items.Include(i => i.Unit);
            items = items.Include(i => i.Unit1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnUnitConversionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnUnitConversionGet(Server.Models.Postgres.UnitConversion item);
        partial void OnGetUnitConversionById(ref IQueryable<Server.Models.Postgres.UnitConversion> items);


        public async Task<Server.Models.Postgres.UnitConversion> GetUnitConversionById(int id)
        {
            var items = Context.UnitConversions
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Unit);
            items = items.Include(i => i.Unit1);
 
            OnGetUnitConversionById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnUnitConversionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnUnitConversionCreated(Server.Models.Postgres.UnitConversion item);
        partial void OnAfterUnitConversionCreated(Server.Models.Postgres.UnitConversion item);

        public async Task<Server.Models.Postgres.UnitConversion> CreateUnitConversion(Server.Models.Postgres.UnitConversion unitconversion)
        {
            OnUnitConversionCreated(unitconversion);

            var existingItem = Context.UnitConversions
                              .Where(i => i.Id == unitconversion.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.UnitConversions.Add(unitconversion);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(unitconversion).State = EntityState.Detached;
                throw;
            }

            OnAfterUnitConversionCreated(unitconversion);

            return unitconversion;
        }

        public async Task<Server.Models.Postgres.UnitConversion> CancelUnitConversionChanges(Server.Models.Postgres.UnitConversion item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnUnitConversionUpdated(Server.Models.Postgres.UnitConversion item);
        partial void OnAfterUnitConversionUpdated(Server.Models.Postgres.UnitConversion item);

        public async Task<Server.Models.Postgres.UnitConversion> UpdateUnitConversion(int id, Server.Models.Postgres.UnitConversion unitconversion)
        {
            OnUnitConversionUpdated(unitconversion);

            var itemToUpdate = Context.UnitConversions
                              .Where(i => i.Id == unitconversion.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(unitconversion);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterUnitConversionUpdated(unitconversion);

            return unitconversion;
        }

        partial void OnUnitConversionDeleted(Server.Models.Postgres.UnitConversion item);
        partial void OnAfterUnitConversionDeleted(Server.Models.Postgres.UnitConversion item);

        public async Task<Server.Models.Postgres.UnitConversion> DeleteUnitConversion(int id)
        {
            var itemToDelete = Context.UnitConversions
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnUnitConversionDeleted(itemToDelete);


            Context.UnitConversions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterUnitConversionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportUnitsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/units/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/units/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportUnitsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/units/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/units/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnUnitsRead(ref IQueryable<Server.Models.Postgres.Unit> items);

        public async Task<IQueryable<Server.Models.Postgres.Unit>> GetUnits(Query query = null)
        {
            var items = Context.Units.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnUnitsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnUnitGet(Server.Models.Postgres.Unit item);
        partial void OnGetUnitById(ref IQueryable<Server.Models.Postgres.Unit> items);


        public async Task<Server.Models.Postgres.Unit> GetUnitById(int id)
        {
            var items = Context.Units
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetUnitById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnUnitGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnUnitCreated(Server.Models.Postgres.Unit item);
        partial void OnAfterUnitCreated(Server.Models.Postgres.Unit item);

        public async Task<Server.Models.Postgres.Unit> CreateUnit(Server.Models.Postgres.Unit _unit)
        {
            OnUnitCreated(_unit);

            var existingItem = Context.Units
                              .Where(i => i.Id == _unit.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Units.Add(_unit);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(_unit).State = EntityState.Detached;
                throw;
            }

            OnAfterUnitCreated(_unit);

            return _unit;
        }

        public async Task<Server.Models.Postgres.Unit> CancelUnitChanges(Server.Models.Postgres.Unit item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnUnitUpdated(Server.Models.Postgres.Unit item);
        partial void OnAfterUnitUpdated(Server.Models.Postgres.Unit item);

        public async Task<Server.Models.Postgres.Unit> UpdateUnit(int id, Server.Models.Postgres.Unit _unit)
        {
            OnUnitUpdated(_unit);

            var itemToUpdate = Context.Units
                              .Where(i => i.Id == _unit.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(_unit);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterUnitUpdated(_unit);

            return _unit;
        }

        partial void OnUnitDeleted(Server.Models.Postgres.Unit item);
        partial void OnAfterUnitDeleted(Server.Models.Postgres.Unit item);

        public async Task<Server.Models.Postgres.Unit> DeleteUnit(int id)
        {
            var itemToDelete = Context.Units
                              .Where(i => i.Id == id)
                              .Include(i => i.UnitConversions)
                              .Include(i => i.UnitConversions1)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnUnitDeleted(itemToDelete);


            Context.Units.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterUnitDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSmtpConfigsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/smtpconfigs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/smtpconfigs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSmtpConfigsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/smtpconfigs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/smtpconfigs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSmtpConfigsRead(ref IQueryable<Server.Models.Postgres.SmtpConfig> items);

        public async Task<IQueryable<Server.Models.Postgres.SmtpConfig>> GetSmtpConfigs(Query query = null)
        {
            var items = Context.SmtpConfigs.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSmtpConfigsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSmtpConfigGet(Server.Models.Postgres.SmtpConfig item);
        partial void OnGetSmtpConfigById(ref IQueryable<Server.Models.Postgres.SmtpConfig> items);


        public async Task<Server.Models.Postgres.SmtpConfig> GetSmtpConfigById(Guid id)
        {
            var items = Context.SmtpConfigs
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetSmtpConfigById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSmtpConfigGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSmtpConfigCreated(Server.Models.Postgres.SmtpConfig item);
        partial void OnAfterSmtpConfigCreated(Server.Models.Postgres.SmtpConfig item);

        public async Task<Server.Models.Postgres.SmtpConfig> CreateSmtpConfig(Server.Models.Postgres.SmtpConfig smtpconfig)
        {
            OnSmtpConfigCreated(smtpconfig);

            var existingItem = Context.SmtpConfigs
                              .Where(i => i.Id == smtpconfig.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SmtpConfigs.Add(smtpconfig);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(smtpconfig).State = EntityState.Detached;
                throw;
            }

            OnAfterSmtpConfigCreated(smtpconfig);

            return smtpconfig;
        }

        public async Task<Server.Models.Postgres.SmtpConfig> CancelSmtpConfigChanges(Server.Models.Postgres.SmtpConfig item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSmtpConfigUpdated(Server.Models.Postgres.SmtpConfig item);
        partial void OnAfterSmtpConfigUpdated(Server.Models.Postgres.SmtpConfig item);

        public async Task<Server.Models.Postgres.SmtpConfig> UpdateSmtpConfig(Guid id, Server.Models.Postgres.SmtpConfig smtpconfig)
        {
            OnSmtpConfigUpdated(smtpconfig);

            var itemToUpdate = Context.SmtpConfigs
                              .Where(i => i.Id == smtpconfig.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(smtpconfig);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSmtpConfigUpdated(smtpconfig);

            return smtpconfig;
        }

        partial void OnSmtpConfigDeleted(Server.Models.Postgres.SmtpConfig item);
        partial void OnAfterSmtpConfigDeleted(Server.Models.Postgres.SmtpConfig item);

        public async Task<Server.Models.Postgres.SmtpConfig> DeleteSmtpConfig(Guid id)
        {
            var itemToDelete = Context.SmtpConfigs
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSmtpConfigDeleted(itemToDelete);


            Context.SmtpConfigs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSmtpConfigDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}