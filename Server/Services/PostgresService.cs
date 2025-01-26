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


        public async Task ExportDestinationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/destinations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/destinations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDestinationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/destinations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/destinations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDestinationsRead(ref IQueryable<Tripbuk.Server.Models.Postgres.Destination> items);

        public async Task<IQueryable<Tripbuk.Server.Models.Postgres.Destination>> GetDestinations(Query query = null)
        {
            var items = Context.Destinations.AsQueryable();

            items = items.Include(i => i.LocationCenter);
            items = items.Include(i => i.Destination1);

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

            OnDestinationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDestinationGet(Tripbuk.Server.Models.Postgres.Destination item);
        partial void OnGetDestinationById(ref IQueryable<Tripbuk.Server.Models.Postgres.Destination> items);


        public async Task<Tripbuk.Server.Models.Postgres.Destination> GetDestinationById(int id)
        {
            var items = Context.Destinations
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.LocationCenter);
            items = items.Include(i => i.Destination1);
 
            OnGetDestinationById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnDestinationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnDestinationCreated(Tripbuk.Server.Models.Postgres.Destination item);
        partial void OnAfterDestinationCreated(Tripbuk.Server.Models.Postgres.Destination item);

        public async Task<Tripbuk.Server.Models.Postgres.Destination> CreateDestination(Tripbuk.Server.Models.Postgres.Destination destination)
        {
            OnDestinationCreated(destination);

            var existingItem = Context.Destinations
                              .Where(i => i.Id == destination.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Destinations.Add(destination);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(destination).State = EntityState.Detached;
                throw;
            }

            OnAfterDestinationCreated(destination);

            return destination;
        }

        public async Task<Tripbuk.Server.Models.Postgres.Destination> CancelDestinationChanges(Tripbuk.Server.Models.Postgres.Destination item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDestinationUpdated(Tripbuk.Server.Models.Postgres.Destination item);
        partial void OnAfterDestinationUpdated(Tripbuk.Server.Models.Postgres.Destination item);

        public async Task<Tripbuk.Server.Models.Postgres.Destination> UpdateDestination(int id, Tripbuk.Server.Models.Postgres.Destination destination)
        {
            OnDestinationUpdated(destination);

            var itemToUpdate = Context.Destinations
                              .Where(i => i.Id == destination.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(destination);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterDestinationUpdated(destination);

            return destination;
        }

        partial void OnDestinationDeleted(Tripbuk.Server.Models.Postgres.Destination item);
        partial void OnAfterDestinationDeleted(Tripbuk.Server.Models.Postgres.Destination item);

        public async Task<Tripbuk.Server.Models.Postgres.Destination> DeleteDestination(int id)
        {
            var itemToDelete = Context.Destinations
                              .Where(i => i.Id == id)
                              .Include(i => i.Destinations1)
                              .Include(i => i.Places)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDestinationDeleted(itemToDelete);


            Context.Destinations.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDestinationDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLocationCentersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/locationcenters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/locationcenters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLocationCentersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/locationcenters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/locationcenters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLocationCentersRead(ref IQueryable<Tripbuk.Server.Models.Postgres.LocationCenter> items);

        public async Task<IQueryable<Tripbuk.Server.Models.Postgres.LocationCenter>> GetLocationCenters(Query query = null)
        {
            var items = Context.LocationCenters.AsQueryable();


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

            OnLocationCentersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLocationCenterGet(Tripbuk.Server.Models.Postgres.LocationCenter item);
        partial void OnGetLocationCenterById(ref IQueryable<Tripbuk.Server.Models.Postgres.LocationCenter> items);


        public async Task<Tripbuk.Server.Models.Postgres.LocationCenter> GetLocationCenterById(int id)
        {
            var items = Context.LocationCenters
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetLocationCenterById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLocationCenterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLocationCenterCreated(Tripbuk.Server.Models.Postgres.LocationCenter item);
        partial void OnAfterLocationCenterCreated(Tripbuk.Server.Models.Postgres.LocationCenter item);

        public async Task<Tripbuk.Server.Models.Postgres.LocationCenter> CreateLocationCenter(Tripbuk.Server.Models.Postgres.LocationCenter locationcenter)
        {
            OnLocationCenterCreated(locationcenter);

            var existingItem = Context.LocationCenters
                              .Where(i => i.Id == locationcenter.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.LocationCenters.Add(locationcenter);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(locationcenter).State = EntityState.Detached;
                throw;
            }

            OnAfterLocationCenterCreated(locationcenter);

            return locationcenter;
        }

        public async Task<Tripbuk.Server.Models.Postgres.LocationCenter> CancelLocationCenterChanges(Tripbuk.Server.Models.Postgres.LocationCenter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLocationCenterUpdated(Tripbuk.Server.Models.Postgres.LocationCenter item);
        partial void OnAfterLocationCenterUpdated(Tripbuk.Server.Models.Postgres.LocationCenter item);

        public async Task<Tripbuk.Server.Models.Postgres.LocationCenter> UpdateLocationCenter(int id, Tripbuk.Server.Models.Postgres.LocationCenter locationcenter)
        {
            OnLocationCenterUpdated(locationcenter);

            var itemToUpdate = Context.LocationCenters
                              .Where(i => i.Id == locationcenter.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(locationcenter);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLocationCenterUpdated(locationcenter);

            return locationcenter;
        }

        partial void OnLocationCenterDeleted(Tripbuk.Server.Models.Postgres.LocationCenter item);
        partial void OnAfterLocationCenterDeleted(Tripbuk.Server.Models.Postgres.LocationCenter item);

        public async Task<Tripbuk.Server.Models.Postgres.LocationCenter> DeleteLocationCenter(int id)
        {
            var itemToDelete = Context.LocationCenters
                              .Where(i => i.Id == id)
                              .Include(i => i.Destinations)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLocationCenterDeleted(itemToDelete);


            Context.LocationCenters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLocationCenterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportParentTagsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/parenttags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/parenttags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportParentTagsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/parenttags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/parenttags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnParentTagsRead(ref IQueryable<Tripbuk.Server.Models.Postgres.ParentTag> items);

        public async Task<IQueryable<Tripbuk.Server.Models.Postgres.ParentTag>> GetParentTags(Query query = null)
        {
            var items = Context.ParentTags.AsQueryable();


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

            OnParentTagsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportPlacesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/places/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/places/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPlacesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/places/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/places/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPlacesRead(ref IQueryable<Tripbuk.Server.Models.Postgres.Place> items);

        public async Task<IQueryable<Tripbuk.Server.Models.Postgres.Place>> GetPlaces(Query query = null)
        {
            var items = Context.Places.AsQueryable();

            items = items.Include(i => i.Destination);

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

            OnPlacesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPlaceGet(Tripbuk.Server.Models.Postgres.Place item);
        partial void OnGetPlaceById(ref IQueryable<Tripbuk.Server.Models.Postgres.Place> items);


        public async Task<Tripbuk.Server.Models.Postgres.Place> GetPlaceById(Guid id)
        {
            var items = Context.Places
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Destination);
 
            OnGetPlaceById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnPlaceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPlaceCreated(Tripbuk.Server.Models.Postgres.Place item);
        partial void OnAfterPlaceCreated(Tripbuk.Server.Models.Postgres.Place item);

        public async Task<Tripbuk.Server.Models.Postgres.Place> CreatePlace(Tripbuk.Server.Models.Postgres.Place place)
        {
            OnPlaceCreated(place);

            var existingItem = Context.Places
                              .Where(i => i.Id == place.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Places.Add(place);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(place).State = EntityState.Detached;
                throw;
            }

            OnAfterPlaceCreated(place);

            return place;
        }

        public async Task<Tripbuk.Server.Models.Postgres.Place> CancelPlaceChanges(Tripbuk.Server.Models.Postgres.Place item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPlaceUpdated(Tripbuk.Server.Models.Postgres.Place item);
        partial void OnAfterPlaceUpdated(Tripbuk.Server.Models.Postgres.Place item);

        public async Task<Tripbuk.Server.Models.Postgres.Place> UpdatePlace(Guid id, Tripbuk.Server.Models.Postgres.Place place)
        {
            OnPlaceUpdated(place);

            var itemToUpdate = Context.Places
                              .Where(i => i.Id == place.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(place);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPlaceUpdated(place);

            return place;
        }

        partial void OnPlaceDeleted(Tripbuk.Server.Models.Postgres.Place item);
        partial void OnAfterPlaceDeleted(Tripbuk.Server.Models.Postgres.Place item);

        public async Task<Tripbuk.Server.Models.Postgres.Place> DeletePlace(Guid id)
        {
            var itemToDelete = Context.Places
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPlaceDeleted(itemToDelete);


            Context.Places.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPlaceDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportPlaceTagsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/placetags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/placetags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPlaceTagsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/placetags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/placetags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPlaceTagsRead(ref IQueryable<Tripbuk.Server.Models.Postgres.PlaceTag> items);

        public async Task<IQueryable<Tripbuk.Server.Models.Postgres.PlaceTag>> GetPlaceTags(Query query = null)
        {
            var items = Context.PlaceTags.AsQueryable();


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

            OnPlaceTagsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportTagsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/tags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/tags/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTagsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/postgres/tags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/postgres/tags/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTagsRead(ref IQueryable<Tripbuk.Server.Models.Postgres.Tag> items);

        public async Task<IQueryable<Tripbuk.Server.Models.Postgres.Tag>> GetTags(Query query = null)
        {
            var items = Context.Tags.AsQueryable();


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

            OnTagsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTagGet(Tripbuk.Server.Models.Postgres.Tag item);
        partial void OnGetTagById(ref IQueryable<Tripbuk.Server.Models.Postgres.Tag> items);


        public async Task<Tripbuk.Server.Models.Postgres.Tag> GetTagById(int id)
        {
            var items = Context.Tags
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetTagById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTagGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTagCreated(Tripbuk.Server.Models.Postgres.Tag item);
        partial void OnAfterTagCreated(Tripbuk.Server.Models.Postgres.Tag item);

        public async Task<Tripbuk.Server.Models.Postgres.Tag> CreateTag(Tripbuk.Server.Models.Postgres.Tag tag)
        {
            OnTagCreated(tag);

            var existingItem = Context.Tags
                              .Where(i => i.Id == tag.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Tags.Add(tag);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tag).State = EntityState.Detached;
                throw;
            }

            OnAfterTagCreated(tag);

            return tag;
        }

        public async Task<Tripbuk.Server.Models.Postgres.Tag> CancelTagChanges(Tripbuk.Server.Models.Postgres.Tag item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTagUpdated(Tripbuk.Server.Models.Postgres.Tag item);
        partial void OnAfterTagUpdated(Tripbuk.Server.Models.Postgres.Tag item);

        public async Task<Tripbuk.Server.Models.Postgres.Tag> UpdateTag(int id, Tripbuk.Server.Models.Postgres.Tag tag)
        {
            OnTagUpdated(tag);

            var itemToUpdate = Context.Tags
                              .Where(i => i.Id == tag.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tag);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTagUpdated(tag);

            return tag;
        }

        partial void OnTagDeleted(Tripbuk.Server.Models.Postgres.Tag item);
        partial void OnAfterTagDeleted(Tripbuk.Server.Models.Postgres.Tag item);

        public async Task<Tripbuk.Server.Models.Postgres.Tag> DeleteTag(int id)
        {
            var itemToDelete = Context.Tags
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTagDeleted(itemToDelete);


            Context.Tags.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTagDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}