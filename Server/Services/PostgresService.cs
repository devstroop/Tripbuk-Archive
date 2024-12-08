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

using ERP.Server.Data;

namespace ERP.Server
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


        public async Task ExportMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/masters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/masters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/masters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/masters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMastersRead(ref IQueryable<ERP.Server.Models.Postgres.Master> items);

        public async Task<IQueryable<ERP.Server.Models.Postgres.Master>> GetMasters(Query query = null)
        {
            var items = Context.Masters.AsQueryable();


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

            OnMastersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMasterGet(ERP.Server.Models.Postgres.Master item);
        partial void OnGetMasterById(ref IQueryable<ERP.Server.Models.Postgres.Master> items);


        public async Task<ERP.Server.Models.Postgres.Master> GetMasterById(int id)
        {
            var items = Context.Masters
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetMasterById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMasterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMasterCreated(ERP.Server.Models.Postgres.Master item);
        partial void OnAfterMasterCreated(ERP.Server.Models.Postgres.Master item);

        public async Task<ERP.Server.Models.Postgres.Master> CreateMaster(ERP.Server.Models.Postgres.Master master)
        {
            OnMasterCreated(master);

            var existingItem = Context.Masters
                              .Where(i => i.Id == master.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Masters.Add(master);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(master).State = EntityState.Detached;
                throw;
            }

            OnAfterMasterCreated(master);

            return master;
        }

        public async Task<ERP.Server.Models.Postgres.Master> CancelMasterChanges(ERP.Server.Models.Postgres.Master item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMasterUpdated(ERP.Server.Models.Postgres.Master item);
        partial void OnAfterMasterUpdated(ERP.Server.Models.Postgres.Master item);

        public async Task<ERP.Server.Models.Postgres.Master> UpdateMaster(int id, ERP.Server.Models.Postgres.Master master)
        {
            OnMasterUpdated(master);

            var itemToUpdate = Context.Masters
                              .Where(i => i.Id == master.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(master);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMasterUpdated(master);

            return master;
        }

        partial void OnMasterDeleted(ERP.Server.Models.Postgres.Master item);
        partial void OnAfterMasterDeleted(ERP.Server.Models.Postgres.Master item);

        public async Task<ERP.Server.Models.Postgres.Master> DeleteMaster(int id)
        {
            var itemToDelete = Context.Masters
                              .Where(i => i.Id == id)
                              .Include(i => i.StdNarrationMasters)
                              .Include(i => i.ItemGroupMasters)
                              .Include(i => i.ItemMasters)
                              .Include(i => i.AccountGroupMasters)
                              .Include(i => i.AccountMasters)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMasterDeleted(itemToDelete);


            Context.Masters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMasterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStdNarrationMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/stdnarrationmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/stdnarrationmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStdNarrationMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/stdnarrationmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/stdnarrationmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStdNarrationMastersRead(ref IQueryable<ERP.Server.Models.Postgres.StdNarrationMaster> items);

        public async Task<IQueryable<ERP.Server.Models.Postgres.StdNarrationMaster>> GetStdNarrationMasters(Query query = null)
        {
            var items = Context.StdNarrationMasters.AsQueryable();

            items = items.Include(i => i.Master);

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

            OnStdNarrationMastersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStdNarrationMasterGet(ERP.Server.Models.Postgres.StdNarrationMaster item);
        partial void OnGetStdNarrationMasterByMasterId(ref IQueryable<ERP.Server.Models.Postgres.StdNarrationMaster> items);


        public async Task<ERP.Server.Models.Postgres.StdNarrationMaster> GetStdNarrationMasterByMasterId(int masterid)
        {
            var items = Context.StdNarrationMasters
                              .AsNoTracking()
                              .Where(i => i.MasterId == masterid);

            items = items.Include(i => i.Master);
 
            OnGetStdNarrationMasterByMasterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStdNarrationMasterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStdNarrationMasterCreated(ERP.Server.Models.Postgres.StdNarrationMaster item);
        partial void OnAfterStdNarrationMasterCreated(ERP.Server.Models.Postgres.StdNarrationMaster item);

        public async Task<ERP.Server.Models.Postgres.StdNarrationMaster> CreateStdNarrationMaster(ERP.Server.Models.Postgres.StdNarrationMaster stdnarrationmaster)
        {
            OnStdNarrationMasterCreated(stdnarrationmaster);

            var existingItem = Context.StdNarrationMasters
                              .Where(i => i.MasterId == stdnarrationmaster.MasterId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StdNarrationMasters.Add(stdnarrationmaster);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stdnarrationmaster).State = EntityState.Detached;
                throw;
            }

            OnAfterStdNarrationMasterCreated(stdnarrationmaster);

            return stdnarrationmaster;
        }

        public async Task<ERP.Server.Models.Postgres.StdNarrationMaster> CancelStdNarrationMasterChanges(ERP.Server.Models.Postgres.StdNarrationMaster item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStdNarrationMasterUpdated(ERP.Server.Models.Postgres.StdNarrationMaster item);
        partial void OnAfterStdNarrationMasterUpdated(ERP.Server.Models.Postgres.StdNarrationMaster item);

        public async Task<ERP.Server.Models.Postgres.StdNarrationMaster> UpdateStdNarrationMaster(int masterid, ERP.Server.Models.Postgres.StdNarrationMaster stdnarrationmaster)
        {
            OnStdNarrationMasterUpdated(stdnarrationmaster);

            var itemToUpdate = Context.StdNarrationMasters
                              .Where(i => i.MasterId == stdnarrationmaster.MasterId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stdnarrationmaster);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStdNarrationMasterUpdated(stdnarrationmaster);

            return stdnarrationmaster;
        }

        partial void OnStdNarrationMasterDeleted(ERP.Server.Models.Postgres.StdNarrationMaster item);
        partial void OnAfterStdNarrationMasterDeleted(ERP.Server.Models.Postgres.StdNarrationMaster item);

        public async Task<ERP.Server.Models.Postgres.StdNarrationMaster> DeleteStdNarrationMaster(int masterid)
        {
            var itemToDelete = Context.StdNarrationMasters
                              .Where(i => i.MasterId == masterid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStdNarrationMasterDeleted(itemToDelete);


            Context.StdNarrationMasters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStdNarrationMasterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportItemMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportItemMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnItemMastersRead(ref IQueryable<ERP.Server.Models.Postgres.ItemMaster> items);

        public async Task<IQueryable<ERP.Server.Models.Postgres.ItemMaster>> GetItemMasters(Query query = null)
        {
            var items = Context.ItemMasters.AsQueryable();

            items = items.Include(i => i.ItemGroupMaster);
            items = items.Include(i => i.Master);

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

            OnItemMastersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnItemMasterGet(ERP.Server.Models.Postgres.ItemMaster item);
        partial void OnGetItemMasterByMasterId(ref IQueryable<ERP.Server.Models.Postgres.ItemMaster> items);


        public async Task<ERP.Server.Models.Postgres.ItemMaster> GetItemMasterByMasterId(int masterid)
        {
            var items = Context.ItemMasters
                              .AsNoTracking()
                              .Where(i => i.MasterId == masterid);

            items = items.Include(i => i.ItemGroupMaster);
            items = items.Include(i => i.Master);
 
            OnGetItemMasterByMasterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnItemMasterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnItemMasterCreated(ERP.Server.Models.Postgres.ItemMaster item);
        partial void OnAfterItemMasterCreated(ERP.Server.Models.Postgres.ItemMaster item);

        public async Task<ERP.Server.Models.Postgres.ItemMaster> CreateItemMaster(ERP.Server.Models.Postgres.ItemMaster itemmaster)
        {
            OnItemMasterCreated(itemmaster);

            var existingItem = Context.ItemMasters
                              .Where(i => i.MasterId == itemmaster.MasterId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ItemMasters.Add(itemmaster);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemmaster).State = EntityState.Detached;
                throw;
            }

            OnAfterItemMasterCreated(itemmaster);

            return itemmaster;
        }

        public async Task<ERP.Server.Models.Postgres.ItemMaster> CancelItemMasterChanges(ERP.Server.Models.Postgres.ItemMaster item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnItemMasterUpdated(ERP.Server.Models.Postgres.ItemMaster item);
        partial void OnAfterItemMasterUpdated(ERP.Server.Models.Postgres.ItemMaster item);

        public async Task<ERP.Server.Models.Postgres.ItemMaster> UpdateItemMaster(int masterid, ERP.Server.Models.Postgres.ItemMaster itemmaster)
        {
            OnItemMasterUpdated(itemmaster);

            var itemToUpdate = Context.ItemMasters
                              .Where(i => i.MasterId == itemmaster.MasterId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(itemmaster);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterItemMasterUpdated(itemmaster);

            return itemmaster;
        }

        partial void OnItemMasterDeleted(ERP.Server.Models.Postgres.ItemMaster item);
        partial void OnAfterItemMasterDeleted(ERP.Server.Models.Postgres.ItemMaster item);

        public async Task<ERP.Server.Models.Postgres.ItemMaster> DeleteItemMaster(int masterid)
        {
            var itemToDelete = Context.ItemMasters
                              .Where(i => i.MasterId == masterid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnItemMasterDeleted(itemToDelete);


            Context.ItemMasters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterItemMasterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportItemGroupMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemgroupmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemgroupmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportItemGroupMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/itemgroupmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/itemgroupmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnItemGroupMastersRead(ref IQueryable<ERP.Server.Models.Postgres.ItemGroupMaster> items);

        public async Task<IQueryable<ERP.Server.Models.Postgres.ItemGroupMaster>> GetItemGroupMasters(Query query = null)
        {
            var items = Context.ItemGroupMasters.AsQueryable();

            items = items.Include(i => i.Master);
            items = items.Include(i => i.ItemGroupMaster1);

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

            OnItemGroupMastersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnItemGroupMasterGet(ERP.Server.Models.Postgres.ItemGroupMaster item);
        partial void OnGetItemGroupMasterByMasterId(ref IQueryable<ERP.Server.Models.Postgres.ItemGroupMaster> items);


        public async Task<ERP.Server.Models.Postgres.ItemGroupMaster> GetItemGroupMasterByMasterId(int masterid)
        {
            var items = Context.ItemGroupMasters
                              .AsNoTracking()
                              .Where(i => i.MasterId == masterid);

            items = items.Include(i => i.Master);
            items = items.Include(i => i.ItemGroupMaster1);
 
            OnGetItemGroupMasterByMasterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnItemGroupMasterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnItemGroupMasterCreated(ERP.Server.Models.Postgres.ItemGroupMaster item);
        partial void OnAfterItemGroupMasterCreated(ERP.Server.Models.Postgres.ItemGroupMaster item);

        public async Task<ERP.Server.Models.Postgres.ItemGroupMaster> CreateItemGroupMaster(ERP.Server.Models.Postgres.ItemGroupMaster itemgroupmaster)
        {
            OnItemGroupMasterCreated(itemgroupmaster);

            var existingItem = Context.ItemGroupMasters
                              .Where(i => i.MasterId == itemgroupmaster.MasterId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ItemGroupMasters.Add(itemgroupmaster);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemgroupmaster).State = EntityState.Detached;
                throw;
            }

            OnAfterItemGroupMasterCreated(itemgroupmaster);

            return itemgroupmaster;
        }

        public async Task<ERP.Server.Models.Postgres.ItemGroupMaster> CancelItemGroupMasterChanges(ERP.Server.Models.Postgres.ItemGroupMaster item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnItemGroupMasterUpdated(ERP.Server.Models.Postgres.ItemGroupMaster item);
        partial void OnAfterItemGroupMasterUpdated(ERP.Server.Models.Postgres.ItemGroupMaster item);

        public async Task<ERP.Server.Models.Postgres.ItemGroupMaster> UpdateItemGroupMaster(int masterid, ERP.Server.Models.Postgres.ItemGroupMaster itemgroupmaster)
        {
            OnItemGroupMasterUpdated(itemgroupmaster);

            var itemToUpdate = Context.ItemGroupMasters
                              .Where(i => i.MasterId == itemgroupmaster.MasterId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(itemgroupmaster);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterItemGroupMasterUpdated(itemgroupmaster);

            return itemgroupmaster;
        }

        partial void OnItemGroupMasterDeleted(ERP.Server.Models.Postgres.ItemGroupMaster item);
        partial void OnAfterItemGroupMasterDeleted(ERP.Server.Models.Postgres.ItemGroupMaster item);

        public async Task<ERP.Server.Models.Postgres.ItemGroupMaster> DeleteItemGroupMaster(int masterid)
        {
            var itemToDelete = Context.ItemGroupMasters
                              .Where(i => i.MasterId == masterid)
                              .Include(i => i.ItemGroupMasters1)
                              .Include(i => i.ItemMasters)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnItemGroupMasterDeleted(itemToDelete);


            Context.ItemGroupMasters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterItemGroupMasterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAccountMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAccountMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAccountMastersRead(ref IQueryable<ERP.Server.Models.Postgres.AccountMaster> items);

        public async Task<IQueryable<ERP.Server.Models.Postgres.AccountMaster>> GetAccountMasters(Query query = null)
        {
            var items = Context.AccountMasters.AsQueryable();

            items = items.Include(i => i.AccountGroupMaster);
            items = items.Include(i => i.Master);

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

            OnAccountMastersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAccountMasterGet(ERP.Server.Models.Postgres.AccountMaster item);
        partial void OnGetAccountMasterByMasterId(ref IQueryable<ERP.Server.Models.Postgres.AccountMaster> items);


        public async Task<ERP.Server.Models.Postgres.AccountMaster> GetAccountMasterByMasterId(int masterid)
        {
            var items = Context.AccountMasters
                              .AsNoTracking()
                              .Where(i => i.MasterId == masterid);

            items = items.Include(i => i.AccountGroupMaster);
            items = items.Include(i => i.Master);
 
            OnGetAccountMasterByMasterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAccountMasterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAccountMasterCreated(ERP.Server.Models.Postgres.AccountMaster item);
        partial void OnAfterAccountMasterCreated(ERP.Server.Models.Postgres.AccountMaster item);

        public async Task<ERP.Server.Models.Postgres.AccountMaster> CreateAccountMaster(ERP.Server.Models.Postgres.AccountMaster accountmaster)
        {
            OnAccountMasterCreated(accountmaster);

            var existingItem = Context.AccountMasters
                              .Where(i => i.MasterId == accountmaster.MasterId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AccountMasters.Add(accountmaster);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(accountmaster).State = EntityState.Detached;
                throw;
            }

            OnAfterAccountMasterCreated(accountmaster);

            return accountmaster;
        }

        public async Task<ERP.Server.Models.Postgres.AccountMaster> CancelAccountMasterChanges(ERP.Server.Models.Postgres.AccountMaster item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAccountMasterUpdated(ERP.Server.Models.Postgres.AccountMaster item);
        partial void OnAfterAccountMasterUpdated(ERP.Server.Models.Postgres.AccountMaster item);

        public async Task<ERP.Server.Models.Postgres.AccountMaster> UpdateAccountMaster(int masterid, ERP.Server.Models.Postgres.AccountMaster accountmaster)
        {
            OnAccountMasterUpdated(accountmaster);

            var itemToUpdate = Context.AccountMasters
                              .Where(i => i.MasterId == accountmaster.MasterId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(accountmaster);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAccountMasterUpdated(accountmaster);

            return accountmaster;
        }

        partial void OnAccountMasterDeleted(ERP.Server.Models.Postgres.AccountMaster item);
        partial void OnAfterAccountMasterDeleted(ERP.Server.Models.Postgres.AccountMaster item);

        public async Task<ERP.Server.Models.Postgres.AccountMaster> DeleteAccountMaster(int masterid)
        {
            var itemToDelete = Context.AccountMasters
                              .Where(i => i.MasterId == masterid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAccountMasterDeleted(itemToDelete);


            Context.AccountMasters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAccountMasterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAccountGroupMastersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountgroupmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountgroupmasters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAccountGroupMastersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/accountgroupmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/accountgroupmasters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAccountGroupMastersRead(ref IQueryable<ERP.Server.Models.Postgres.AccountGroupMaster> items);

        public async Task<IQueryable<ERP.Server.Models.Postgres.AccountGroupMaster>> GetAccountGroupMasters(Query query = null)
        {
            var items = Context.AccountGroupMasters.AsQueryable();

            items = items.Include(i => i.Master);
            items = items.Include(i => i.AccountGroupMaster1);

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

            OnAccountGroupMastersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAccountGroupMasterGet(ERP.Server.Models.Postgres.AccountGroupMaster item);
        partial void OnGetAccountGroupMasterByMasterId(ref IQueryable<ERP.Server.Models.Postgres.AccountGroupMaster> items);


        public async Task<ERP.Server.Models.Postgres.AccountGroupMaster> GetAccountGroupMasterByMasterId(int masterid)
        {
            var items = Context.AccountGroupMasters
                              .AsNoTracking()
                              .Where(i => i.MasterId == masterid);

            items = items.Include(i => i.Master);
            items = items.Include(i => i.AccountGroupMaster1);
 
            OnGetAccountGroupMasterByMasterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAccountGroupMasterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAccountGroupMasterCreated(ERP.Server.Models.Postgres.AccountGroupMaster item);
        partial void OnAfterAccountGroupMasterCreated(ERP.Server.Models.Postgres.AccountGroupMaster item);

        public async Task<ERP.Server.Models.Postgres.AccountGroupMaster> CreateAccountGroupMaster(ERP.Server.Models.Postgres.AccountGroupMaster accountgroupmaster)
        {
            OnAccountGroupMasterCreated(accountgroupmaster);

            var existingItem = Context.AccountGroupMasters
                              .Where(i => i.MasterId == accountgroupmaster.MasterId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AccountGroupMasters.Add(accountgroupmaster);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(accountgroupmaster).State = EntityState.Detached;
                throw;
            }

            OnAfterAccountGroupMasterCreated(accountgroupmaster);

            return accountgroupmaster;
        }

        public async Task<ERP.Server.Models.Postgres.AccountGroupMaster> CancelAccountGroupMasterChanges(ERP.Server.Models.Postgres.AccountGroupMaster item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAccountGroupMasterUpdated(ERP.Server.Models.Postgres.AccountGroupMaster item);
        partial void OnAfterAccountGroupMasterUpdated(ERP.Server.Models.Postgres.AccountGroupMaster item);

        public async Task<ERP.Server.Models.Postgres.AccountGroupMaster> UpdateAccountGroupMaster(int masterid, ERP.Server.Models.Postgres.AccountGroupMaster accountgroupmaster)
        {
            OnAccountGroupMasterUpdated(accountgroupmaster);

            var itemToUpdate = Context.AccountGroupMasters
                              .Where(i => i.MasterId == accountgroupmaster.MasterId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(accountgroupmaster);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAccountGroupMasterUpdated(accountgroupmaster);

            return accountgroupmaster;
        }

        partial void OnAccountGroupMasterDeleted(ERP.Server.Models.Postgres.AccountGroupMaster item);
        partial void OnAfterAccountGroupMasterDeleted(ERP.Server.Models.Postgres.AccountGroupMaster item);

        public async Task<ERP.Server.Models.Postgres.AccountGroupMaster> DeleteAccountGroupMaster(int masterid)
        {
            var itemToDelete = Context.AccountGroupMasters
                              .Where(i => i.MasterId == masterid)
                              .Include(i => i.AccountGroupMasters1)
                              .Include(i => i.AccountMasters)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAccountGroupMasterDeleted(itemToDelete);


            Context.AccountGroupMasters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAccountGroupMasterDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}