using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using KlantBaseWebDemo.Data;

namespace KlantBaseWebDemo
{
    public partial class KlantBaseService
    {
        KlantBaseContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly KlantBaseContext context;
        private readonly NavigationManager navigationManager;

        public KlantBaseService(KlantBaseContext context, NavigationManager navigationManager)
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


        public async Task ExportAdresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/adres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/adres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/adres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/adres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdresRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Adre> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.Adre>> GetAdres(Query query = null)
        {
            var items = Context.Adres.AsQueryable();


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

            OnAdresRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<KlantBaseWebDemo.Models.KlantBase.Adre>> GetAdressen()
        {
            return await Context.Adres.ToListAsync(); // Eenvoudige versie zonder filters
        }

        partial void OnAdreGet(KlantBaseWebDemo.Models.KlantBase.Adre item);
        partial void OnGetAdreById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Adre> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.Adre> GetAdreById(int id)
        {
            var items = Context.Adres
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetAdreById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdreGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdreCreated(KlantBaseWebDemo.Models.KlantBase.Adre item);
        partial void OnAfterAdreCreated(KlantBaseWebDemo.Models.KlantBase.Adre item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Adre> CreateAdre(KlantBaseWebDemo.Models.KlantBase.Adre adre)
        {
            OnAdreCreated(adre);

            var existingItem = Context.Adres
                              .Where(i => i.Id == adre.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Adres.Add(adre);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adre).State = EntityState.Detached;
                throw;
            }

            OnAfterAdreCreated(adre);

            return adre;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.Adre> CancelAdreChanges(KlantBaseWebDemo.Models.KlantBase.Adre item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdreUpdated(KlantBaseWebDemo.Models.KlantBase.Adre item);
        partial void OnAfterAdreUpdated(KlantBaseWebDemo.Models.KlantBase.Adre item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Adre> UpdateAdre(int id, KlantBaseWebDemo.Models.KlantBase.Adre adre)
        {
            OnAdreUpdated(adre);

            var itemToUpdate = Context.Adres
                              .Where(i => i.Id == adre.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adre);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdreUpdated(adre);

            return adre;
        }

        partial void OnAdreDeleted(KlantBaseWebDemo.Models.KlantBase.Adre item);
        partial void OnAfterAdreDeleted(KlantBaseWebDemo.Models.KlantBase.Adre item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Adre> DeleteAdre(int id)
        {
            var itemToDelete = Context.Adres
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdreDeleted(itemToDelete);


            Context.Adres.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdreDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportContactpersonensToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/contactpersonens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/contactpersonens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportContactpersonensToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/contactpersonens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/contactpersonens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnContactpersonensRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Contactpersonen> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.Contactpersonen>> GetContactpersonens(Query query = null)
        {
            var items = Context.Contactpersonens.AsQueryable();


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

            OnContactpersonensRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnContactpersonenGet(KlantBaseWebDemo.Models.KlantBase.Contactpersonen item);
        partial void OnGetContactpersonenByContactPersId(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Contactpersonen> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.Contactpersonen> GetContactpersonenByContactPersId(int contactpersid)
        {
            var items = Context.Contactpersonens
                              .AsNoTracking()
                              .Where(i => i.ContactPersId == contactpersid);

 
            OnGetContactpersonenByContactPersId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnContactpersonenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnContactpersonenCreated(KlantBaseWebDemo.Models.KlantBase.Contactpersonen item);
        partial void OnAfterContactpersonenCreated(KlantBaseWebDemo.Models.KlantBase.Contactpersonen item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Contactpersonen> CreateContactpersonen(KlantBaseWebDemo.Models.KlantBase.Contactpersonen contactpersonen)
        {
            OnContactpersonenCreated(contactpersonen);

            var existingItem = Context.Contactpersonens
                              .Where(i => i.ContactPersId == contactpersonen.ContactPersId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Contactpersonens.Add(contactpersonen);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(contactpersonen).State = EntityState.Detached;
                throw;
            }

            OnAfterContactpersonenCreated(contactpersonen);

            return contactpersonen;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.Contactpersonen> CancelContactpersonenChanges(KlantBaseWebDemo.Models.KlantBase.Contactpersonen item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnContactpersonenUpdated(KlantBaseWebDemo.Models.KlantBase.Contactpersonen item);
        partial void OnAfterContactpersonenUpdated(KlantBaseWebDemo.Models.KlantBase.Contactpersonen item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Contactpersonen> UpdateContactpersonen(int contactpersid, KlantBaseWebDemo.Models.KlantBase.Contactpersonen contactpersonen)
        {
            OnContactpersonenUpdated(contactpersonen);

            var itemToUpdate = Context.Contactpersonens
                              .Where(i => i.ContactPersId == contactpersonen.ContactPersId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(contactpersonen);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterContactpersonenUpdated(contactpersonen);

            return contactpersonen;
        }

        partial void OnContactpersonenDeleted(KlantBaseWebDemo.Models.KlantBase.Contactpersonen item);
        partial void OnAfterContactpersonenDeleted(KlantBaseWebDemo.Models.KlantBase.Contactpersonen item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Contactpersonen> DeleteContactpersonen(int contactpersid)
        {
            var itemToDelete = Context.Contactpersonens
                              .Where(i => i.ContactPersId == contactpersid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnContactpersonenDeleted(itemToDelete);


            Context.Contactpersonens.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterContactpersonenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCorrespondentiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/correspondenties/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/correspondenties/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCorrespondentiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/correspondenties/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/correspondenties/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCorrespondentiesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Correspondentie> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.Correspondentie>> GetCorrespondenties(Query query = null)
        {
            var items = Context.Correspondenties.AsQueryable();


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

            OnCorrespondentiesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCorrespondentieGet(KlantBaseWebDemo.Models.KlantBase.Correspondentie item);
        partial void OnGetCorrespondentieById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Correspondentie> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.Correspondentie> GetCorrespondentieById(int id)
        {
            var items = Context.Correspondenties
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetCorrespondentieById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCorrespondentieGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCorrespondentieCreated(KlantBaseWebDemo.Models.KlantBase.Correspondentie item);
        partial void OnAfterCorrespondentieCreated(KlantBaseWebDemo.Models.KlantBase.Correspondentie item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Correspondentie> CreateCorrespondentie(KlantBaseWebDemo.Models.KlantBase.Correspondentie correspondentie)
        {
            OnCorrespondentieCreated(correspondentie);

            var existingItem = Context.Correspondenties
                              .Where(i => i.Id == correspondentie.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Correspondenties.Add(correspondentie);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(correspondentie).State = EntityState.Detached;
                throw;
            }

            OnAfterCorrespondentieCreated(correspondentie);

            return correspondentie;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.Correspondentie> CancelCorrespondentieChanges(KlantBaseWebDemo.Models.KlantBase.Correspondentie item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCorrespondentieUpdated(KlantBaseWebDemo.Models.KlantBase.Correspondentie item);
        partial void OnAfterCorrespondentieUpdated(KlantBaseWebDemo.Models.KlantBase.Correspondentie item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Correspondentie> UpdateCorrespondentie(int id, KlantBaseWebDemo.Models.KlantBase.Correspondentie correspondentie)
        {
            OnCorrespondentieUpdated(correspondentie);

            var itemToUpdate = Context.Correspondenties
                              .Where(i => i.Id == correspondentie.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(correspondentie);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCorrespondentieUpdated(correspondentie);

            return correspondentie;
        }

        partial void OnCorrespondentieDeleted(KlantBaseWebDemo.Models.KlantBase.Correspondentie item);
        partial void OnAfterCorrespondentieDeleted(KlantBaseWebDemo.Models.KlantBase.Correspondentie item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Correspondentie> DeleteCorrespondentie(int id)
        {
            var itemToDelete = Context.Correspondenties
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCorrespondentieDeleted(itemToDelete);


            Context.Correspondenties.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCorrespondentieDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblMaandensToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblmaandens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblmaandens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblMaandensToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblmaandens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblmaandens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblMaandensRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblMaanden> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblMaanden>> GetStblMaandens(Query query = null)
        {
            var items = Context.StblMaandens.AsQueryable();


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

            OnStblMaandensRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportStblSelectiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblselecties/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblselecties/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblSelectiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblselecties/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblselecties/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblSelectiesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblSelectie> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblSelectie>> GetStblSelecties(Query query = null)
        {
            var items = Context.StblSelecties.AsQueryable();


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

            OnStblSelectiesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblSelectieGet(KlantBaseWebDemo.Models.KlantBase.StblSelectie item);
        partial void OnGetStblSelectieBySelId(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblSelectie> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectie> GetStblSelectieBySelId(int selid)
        {
            var items = Context.StblSelecties
                              .AsNoTracking()
                              .Where(i => i.SelId == selid);

 
            OnGetStblSelectieBySelId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblSelectieGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblSelectieCreated(KlantBaseWebDemo.Models.KlantBase.StblSelectie item);
        partial void OnAfterStblSelectieCreated(KlantBaseWebDemo.Models.KlantBase.StblSelectie item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectie> CreateStblSelectie(KlantBaseWebDemo.Models.KlantBase.StblSelectie stblselectie)
        {
            OnStblSelectieCreated(stblselectie);

            var existingItem = Context.StblSelecties
                              .Where(i => i.SelId == stblselectie.SelId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblSelecties.Add(stblselectie);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblselectie).State = EntityState.Detached;
                throw;
            }

            OnAfterStblSelectieCreated(stblselectie);

            return stblselectie;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectie> CancelStblSelectieChanges(KlantBaseWebDemo.Models.KlantBase.StblSelectie item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblSelectieUpdated(KlantBaseWebDemo.Models.KlantBase.StblSelectie item);
        partial void OnAfterStblSelectieUpdated(KlantBaseWebDemo.Models.KlantBase.StblSelectie item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectie> UpdateStblSelectie(int selid, KlantBaseWebDemo.Models.KlantBase.StblSelectie stblselectie)
        {
            OnStblSelectieUpdated(stblselectie);

            var itemToUpdate = Context.StblSelecties
                              .Where(i => i.SelId == stblselectie.SelId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblselectie);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblSelectieUpdated(stblselectie);

            return stblselectie;
        }

        partial void OnStblSelectieDeleted(KlantBaseWebDemo.Models.KlantBase.StblSelectie item);
        partial void OnAfterStblSelectieDeleted(KlantBaseWebDemo.Models.KlantBase.StblSelectie item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectie> DeleteStblSelectie(int selid)
        {
            var itemToDelete = Context.StblSelecties
                              .Where(i => i.SelId == selid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblSelectieDeleted(itemToDelete);


            Context.StblSelecties.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblSelectieDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblActieSoortsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblactiesoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblactiesoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblActieSoortsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblactiesoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblactiesoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblActieSoortsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblActieSoort> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblActieSoort>> GetStblActieSoorts(Query query = null)
        {
            var items = Context.StblActieSoorts.AsQueryable();


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

            OnStblActieSoortsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblActieSoortGet(KlantBaseWebDemo.Models.KlantBase.StblActieSoort item);
        partial void OnGetStblActieSoortById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblActieSoort> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblActieSoort> GetStblActieSoortById(int id)
        {
            var items = Context.StblActieSoorts
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblActieSoortById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblActieSoortGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblActieSoortCreated(KlantBaseWebDemo.Models.KlantBase.StblActieSoort item);
        partial void OnAfterStblActieSoortCreated(KlantBaseWebDemo.Models.KlantBase.StblActieSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblActieSoort> CreateStblActieSoort(KlantBaseWebDemo.Models.KlantBase.StblActieSoort stblactiesoort)
        {
            OnStblActieSoortCreated(stblactiesoort);

            var existingItem = Context.StblActieSoorts
                              .Where(i => i.Id == stblactiesoort.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblActieSoorts.Add(stblactiesoort);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblactiesoort).State = EntityState.Detached;
                throw;
            }

            OnAfterStblActieSoortCreated(stblactiesoort);

            return stblactiesoort;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblActieSoort> CancelStblActieSoortChanges(KlantBaseWebDemo.Models.KlantBase.StblActieSoort item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblActieSoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblActieSoort item);
        partial void OnAfterStblActieSoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblActieSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblActieSoort> UpdateStblActieSoort(int id, KlantBaseWebDemo.Models.KlantBase.StblActieSoort stblactiesoort)
        {
            OnStblActieSoortUpdated(stblactiesoort);

            var itemToUpdate = Context.StblActieSoorts
                              .Where(i => i.Id == stblactiesoort.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblactiesoort);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblActieSoortUpdated(stblactiesoort);

            return stblactiesoort;
        }

        partial void OnStblActieSoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblActieSoort item);
        partial void OnAfterStblActieSoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblActieSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblActieSoort> DeleteStblActieSoort(int id)
        {
            var itemToDelete = Context.StblActieSoorts
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblActieSoortDeleted(itemToDelete);


            Context.StblActieSoorts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblActieSoortDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblAfwerkingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblafwerkings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblafwerkings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblAfwerkingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblafwerkings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblafwerkings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblAfwerkingsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblAfwerking> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblAfwerking>> GetStblAfwerkings(Query query = null)
        {
            var items = Context.StblAfwerkings.AsQueryable();


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

            OnStblAfwerkingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblAfwerkingGet(KlantBaseWebDemo.Models.KlantBase.StblAfwerking item);
        partial void OnGetStblAfwerkingById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblAfwerking> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblAfwerking> GetStblAfwerkingById(int id)
        {
            var items = Context.StblAfwerkings
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblAfwerkingById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblAfwerkingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblAfwerkingCreated(KlantBaseWebDemo.Models.KlantBase.StblAfwerking item);
        partial void OnAfterStblAfwerkingCreated(KlantBaseWebDemo.Models.KlantBase.StblAfwerking item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblAfwerking> CreateStblAfwerking(KlantBaseWebDemo.Models.KlantBase.StblAfwerking stblafwerking)
        {
            OnStblAfwerkingCreated(stblafwerking);

            var existingItem = Context.StblAfwerkings
                              .Where(i => i.Id == stblafwerking.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblAfwerkings.Add(stblafwerking);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblafwerking).State = EntityState.Detached;
                throw;
            }

            OnAfterStblAfwerkingCreated(stblafwerking);

            return stblafwerking;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblAfwerking> CancelStblAfwerkingChanges(KlantBaseWebDemo.Models.KlantBase.StblAfwerking item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblAfwerkingUpdated(KlantBaseWebDemo.Models.KlantBase.StblAfwerking item);
        partial void OnAfterStblAfwerkingUpdated(KlantBaseWebDemo.Models.KlantBase.StblAfwerking item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblAfwerking> UpdateStblAfwerking(int id, KlantBaseWebDemo.Models.KlantBase.StblAfwerking stblafwerking)
        {
            OnStblAfwerkingUpdated(stblafwerking);

            var itemToUpdate = Context.StblAfwerkings
                              .Where(i => i.Id == stblafwerking.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblafwerking);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblAfwerkingUpdated(stblafwerking);

            return stblafwerking;
        }

        partial void OnStblAfwerkingDeleted(KlantBaseWebDemo.Models.KlantBase.StblAfwerking item);
        partial void OnAfterStblAfwerkingDeleted(KlantBaseWebDemo.Models.KlantBase.StblAfwerking item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblAfwerking> DeleteStblAfwerking(int id)
        {
            var itemToDelete = Context.StblAfwerkings
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblAfwerkingDeleted(itemToDelete);


            Context.StblAfwerkings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblAfwerkingDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblBelStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblbelstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblbelstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblBelStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblbelstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblbelstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblBelStatusesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblBelStatus> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblBelStatus>> GetStblBelStatuses(Query query = null)
        {
            var items = Context.StblBelStatuses.AsQueryable();


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

            OnStblBelStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblBelStatusGet(KlantBaseWebDemo.Models.KlantBase.StblBelStatus item);
        partial void OnGetStblBelStatusById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblBelStatus> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBelStatus> GetStblBelStatusById(int id)
        {
            var items = Context.StblBelStatuses
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblBelStatusById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblBelStatusGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblBelStatusCreated(KlantBaseWebDemo.Models.KlantBase.StblBelStatus item);
        partial void OnAfterStblBelStatusCreated(KlantBaseWebDemo.Models.KlantBase.StblBelStatus item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBelStatus> CreateStblBelStatus(KlantBaseWebDemo.Models.KlantBase.StblBelStatus stblbelstatus)
        {
            OnStblBelStatusCreated(stblbelstatus);

            var existingItem = Context.StblBelStatuses
                              .Where(i => i.Id == stblbelstatus.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblBelStatuses.Add(stblbelstatus);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblbelstatus).State = EntityState.Detached;
                throw;
            }

            OnAfterStblBelStatusCreated(stblbelstatus);

            return stblbelstatus;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBelStatus> CancelStblBelStatusChanges(KlantBaseWebDemo.Models.KlantBase.StblBelStatus item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblBelStatusUpdated(KlantBaseWebDemo.Models.KlantBase.StblBelStatus item);
        partial void OnAfterStblBelStatusUpdated(KlantBaseWebDemo.Models.KlantBase.StblBelStatus item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBelStatus> UpdateStblBelStatus(int id, KlantBaseWebDemo.Models.KlantBase.StblBelStatus stblbelstatus)
        {
            OnStblBelStatusUpdated(stblbelstatus);

            var itemToUpdate = Context.StblBelStatuses
                              .Where(i => i.Id == stblbelstatus.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblbelstatus);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblBelStatusUpdated(stblbelstatus);

            return stblbelstatus;
        }

        partial void OnStblBelStatusDeleted(KlantBaseWebDemo.Models.KlantBase.StblBelStatus item);
        partial void OnAfterStblBelStatusDeleted(KlantBaseWebDemo.Models.KlantBase.StblBelStatus item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBelStatus> DeleteStblBelStatus(int id)
        {
            var itemToDelete = Context.StblBelStatuses
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblBelStatusDeleted(itemToDelete);


            Context.StblBelStatuses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblBelStatusDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblBooksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblbooks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblbooks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblBooksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblbooks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblbooks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblBooksRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblBook> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblBook>> GetStblBooks(Query query = null)
        {
            var items = Context.StblBooks.AsQueryable();


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

            OnStblBooksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblBookGet(KlantBaseWebDemo.Models.KlantBase.StblBook item);
        partial void OnGetStblBookById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblBook> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBook> GetStblBookById(int id)
        {
            var items = Context.StblBooks
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblBookById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblBookGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblBookCreated(KlantBaseWebDemo.Models.KlantBase.StblBook item);
        partial void OnAfterStblBookCreated(KlantBaseWebDemo.Models.KlantBase.StblBook item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBook> CreateStblBook(KlantBaseWebDemo.Models.KlantBase.StblBook stblbook)
        {
            OnStblBookCreated(stblbook);

            var existingItem = Context.StblBooks
                              .Where(i => i.Id == stblbook.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblBooks.Add(stblbook);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblbook).State = EntityState.Detached;
                throw;
            }

            OnAfterStblBookCreated(stblbook);

            return stblbook;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBook> CancelStblBookChanges(KlantBaseWebDemo.Models.KlantBase.StblBook item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblBookUpdated(KlantBaseWebDemo.Models.KlantBase.StblBook item);
        partial void OnAfterStblBookUpdated(KlantBaseWebDemo.Models.KlantBase.StblBook item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBook> UpdateStblBook(int id, KlantBaseWebDemo.Models.KlantBase.StblBook stblbook)
        {
            OnStblBookUpdated(stblbook);

            var itemToUpdate = Context.StblBooks
                              .Where(i => i.Id == stblbook.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblbook);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblBookUpdated(stblbook);

            return stblbook;
        }

        partial void OnStblBookDeleted(KlantBaseWebDemo.Models.KlantBase.StblBook item);
        partial void OnAfterStblBookDeleted(KlantBaseWebDemo.Models.KlantBase.StblBook item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblBook> DeleteStblBook(int id)
        {
            var itemToDelete = Context.StblBooks
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblBookDeleted(itemToDelete);


            Context.StblBooks.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblBookDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblCorrespondentieFieldsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblcorrespondentiefields/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblcorrespondentiefields/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblCorrespondentieFieldsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblcorrespondentiefields/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblcorrespondentiefields/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblCorrespondentieFieldsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField>> GetStblCorrespondentieFields(Query query = null)
        {
            var items = Context.StblCorrespondentieFields.AsQueryable();


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

            OnStblCorrespondentieFieldsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblCorrespondentieFieldGet(KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField item);
        partial void OnGetStblCorrespondentieFieldById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField> GetStblCorrespondentieFieldById(int id)
        {
            var items = Context.StblCorrespondentieFields
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblCorrespondentieFieldById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblCorrespondentieFieldGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblCorrespondentieFieldCreated(KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField item);
        partial void OnAfterStblCorrespondentieFieldCreated(KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField> CreateStblCorrespondentieField(KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField stblcorrespondentiefield)
        {
            OnStblCorrespondentieFieldCreated(stblcorrespondentiefield);

            var existingItem = Context.StblCorrespondentieFields
                              .Where(i => i.Id == stblcorrespondentiefield.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblCorrespondentieFields.Add(stblcorrespondentiefield);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblcorrespondentiefield).State = EntityState.Detached;
                throw;
            }

            OnAfterStblCorrespondentieFieldCreated(stblcorrespondentiefield);

            return stblcorrespondentiefield;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField> CancelStblCorrespondentieFieldChanges(KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblCorrespondentieFieldUpdated(KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField item);
        partial void OnAfterStblCorrespondentieFieldUpdated(KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField> UpdateStblCorrespondentieField(int id, KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField stblcorrespondentiefield)
        {
            OnStblCorrespondentieFieldUpdated(stblcorrespondentiefield);

            var itemToUpdate = Context.StblCorrespondentieFields
                              .Where(i => i.Id == stblcorrespondentiefield.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblcorrespondentiefield);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblCorrespondentieFieldUpdated(stblcorrespondentiefield);

            return stblcorrespondentiefield;
        }

        partial void OnStblCorrespondentieFieldDeleted(KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField item);
        partial void OnAfterStblCorrespondentieFieldDeleted(KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblCorrespondentieField> DeleteStblCorrespondentieField(int id)
        {
            var itemToDelete = Context.StblCorrespondentieFields
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblCorrespondentieFieldDeleted(itemToDelete);


            Context.StblCorrespondentieFields.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblCorrespondentieFieldDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblDocumentSoortsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stbldocumentsoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stbldocumentsoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblDocumentSoortsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stbldocumentsoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stbldocumentsoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblDocumentSoortsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort>> GetStblDocumentSoorts(Query query = null)
        {
            var items = Context.StblDocumentSoorts.AsQueryable();


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

            OnStblDocumentSoortsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblDocumentSoortGet(KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort item);
        partial void OnGetStblDocumentSoortById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort> GetStblDocumentSoortById(int id)
        {
            var items = Context.StblDocumentSoorts
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblDocumentSoortById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblDocumentSoortGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblDocumentSoortCreated(KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort item);
        partial void OnAfterStblDocumentSoortCreated(KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort> CreateStblDocumentSoort(KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort stbldocumentsoort)
        {
            OnStblDocumentSoortCreated(stbldocumentsoort);

            var existingItem = Context.StblDocumentSoorts
                              .Where(i => i.Id == stbldocumentsoort.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblDocumentSoorts.Add(stbldocumentsoort);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stbldocumentsoort).State = EntityState.Detached;
                throw;
            }

            OnAfterStblDocumentSoortCreated(stbldocumentsoort);

            return stbldocumentsoort;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort> CancelStblDocumentSoortChanges(KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblDocumentSoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort item);
        partial void OnAfterStblDocumentSoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort> UpdateStblDocumentSoort(int id, KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort stbldocumentsoort)
        {
            OnStblDocumentSoortUpdated(stbldocumentsoort);

            var itemToUpdate = Context.StblDocumentSoorts
                              .Where(i => i.Id == stbldocumentsoort.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stbldocumentsoort);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblDocumentSoortUpdated(stbldocumentsoort);

            return stbldocumentsoort;
        }

        partial void OnStblDocumentSoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort item);
        partial void OnAfterStblDocumentSoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblDocumentSoort> DeleteStblDocumentSoort(int id)
        {
            var itemToDelete = Context.StblDocumentSoorts
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblDocumentSoortDeleted(itemToDelete);


            Context.StblDocumentSoorts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblDocumentSoortDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblFabrikantsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblfabrikants/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblfabrikants/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblFabrikantsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblfabrikants/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblfabrikants/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblFabrikantsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblFabrikant> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblFabrikant>> GetStblFabrikants(Query query = null)
        {
            var items = Context.StblFabrikants.AsQueryable();


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

            OnStblFabrikantsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblFabrikantGet(KlantBaseWebDemo.Models.KlantBase.StblFabrikant item);
        partial void OnGetStblFabrikantById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblFabrikant> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFabrikant> GetStblFabrikantById(int id)
        {
            var items = Context.StblFabrikants
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblFabrikantById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblFabrikantGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblFabrikantCreated(KlantBaseWebDemo.Models.KlantBase.StblFabrikant item);
        partial void OnAfterStblFabrikantCreated(KlantBaseWebDemo.Models.KlantBase.StblFabrikant item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFabrikant> CreateStblFabrikant(KlantBaseWebDemo.Models.KlantBase.StblFabrikant stblfabrikant)
        {
            OnStblFabrikantCreated(stblfabrikant);

            var existingItem = Context.StblFabrikants
                              .Where(i => i.Id == stblfabrikant.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblFabrikants.Add(stblfabrikant);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblfabrikant).State = EntityState.Detached;
                throw;
            }

            OnAfterStblFabrikantCreated(stblfabrikant);

            return stblfabrikant;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFabrikant> CancelStblFabrikantChanges(KlantBaseWebDemo.Models.KlantBase.StblFabrikant item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblFabrikantUpdated(KlantBaseWebDemo.Models.KlantBase.StblFabrikant item);
        partial void OnAfterStblFabrikantUpdated(KlantBaseWebDemo.Models.KlantBase.StblFabrikant item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFabrikant> UpdateStblFabrikant(int id, KlantBaseWebDemo.Models.KlantBase.StblFabrikant stblfabrikant)
        {
            OnStblFabrikantUpdated(stblfabrikant);

            var itemToUpdate = Context.StblFabrikants
                              .Where(i => i.Id == stblfabrikant.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblfabrikant);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblFabrikantUpdated(stblfabrikant);

            return stblfabrikant;
        }

        partial void OnStblFabrikantDeleted(KlantBaseWebDemo.Models.KlantBase.StblFabrikant item);
        partial void OnAfterStblFabrikantDeleted(KlantBaseWebDemo.Models.KlantBase.StblFabrikant item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFabrikant> DeleteStblFabrikant(int id)
        {
            var itemToDelete = Context.StblFabrikants
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblFabrikantDeleted(itemToDelete);


            Context.StblFabrikants.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblFabrikantDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblFactureermethodesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblfactureermethodes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblfactureermethodes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblFactureermethodesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblfactureermethodes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblfactureermethodes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblFactureermethodesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblFactureermethode> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblFactureermethode>> GetStblFactureermethodes(Query query = null)
        {
            var items = Context.StblFactureermethodes.AsQueryable();


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

            OnStblFactureermethodesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblFactureermethodeGet(KlantBaseWebDemo.Models.KlantBase.StblFactureermethode item);
        partial void OnGetStblFactureermethodeById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblFactureermethode> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactureermethode> GetStblFactureermethodeById(int id)
        {
            var items = Context.StblFactureermethodes
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblFactureermethodeById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblFactureermethodeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblFactureermethodeCreated(KlantBaseWebDemo.Models.KlantBase.StblFactureermethode item);
        partial void OnAfterStblFactureermethodeCreated(KlantBaseWebDemo.Models.KlantBase.StblFactureermethode item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactureermethode> CreateStblFactureermethode(KlantBaseWebDemo.Models.KlantBase.StblFactureermethode stblfactureermethode)
        {
            OnStblFactureermethodeCreated(stblfactureermethode);

            var existingItem = Context.StblFactureermethodes
                              .Where(i => i.Id == stblfactureermethode.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblFactureermethodes.Add(stblfactureermethode);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblfactureermethode).State = EntityState.Detached;
                throw;
            }

            OnAfterStblFactureermethodeCreated(stblfactureermethode);

            return stblfactureermethode;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactureermethode> CancelStblFactureermethodeChanges(KlantBaseWebDemo.Models.KlantBase.StblFactureermethode item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblFactureermethodeUpdated(KlantBaseWebDemo.Models.KlantBase.StblFactureermethode item);
        partial void OnAfterStblFactureermethodeUpdated(KlantBaseWebDemo.Models.KlantBase.StblFactureermethode item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactureermethode> UpdateStblFactureermethode(int id, KlantBaseWebDemo.Models.KlantBase.StblFactureermethode stblfactureermethode)
        {
            OnStblFactureermethodeUpdated(stblfactureermethode);

            var itemToUpdate = Context.StblFactureermethodes
                              .Where(i => i.Id == stblfactureermethode.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblfactureermethode);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblFactureermethodeUpdated(stblfactureermethode);

            return stblfactureermethode;
        }

        partial void OnStblFactureermethodeDeleted(KlantBaseWebDemo.Models.KlantBase.StblFactureermethode item);
        partial void OnAfterStblFactureermethodeDeleted(KlantBaseWebDemo.Models.KlantBase.StblFactureermethode item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactureermethode> DeleteStblFactureermethode(int id)
        {
            var itemToDelete = Context.StblFactureermethodes
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblFactureermethodeDeleted(itemToDelete);


            Context.StblFactureermethodes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblFactureermethodeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblFactuurKortingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblfactuurkortings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblfactuurkortings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblFactuurKortingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblfactuurkortings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblfactuurkortings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblFactuurKortingsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting>> GetStblFactuurKortings(Query query = null)
        {
            var items = Context.StblFactuurKortings.AsQueryable();


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

            OnStblFactuurKortingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblFactuurKortingGet(KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting item);
        partial void OnGetStblFactuurKortingById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting> GetStblFactuurKortingById(int id)
        {
            var items = Context.StblFactuurKortings
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblFactuurKortingById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblFactuurKortingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblFactuurKortingCreated(KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting item);
        partial void OnAfterStblFactuurKortingCreated(KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting> CreateStblFactuurKorting(KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting stblfactuurkorting)
        {
            OnStblFactuurKortingCreated(stblfactuurkorting);

            var existingItem = Context.StblFactuurKortings
                              .Where(i => i.Id == stblfactuurkorting.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblFactuurKortings.Add(stblfactuurkorting);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblfactuurkorting).State = EntityState.Detached;
                throw;
            }

            OnAfterStblFactuurKortingCreated(stblfactuurkorting);

            return stblfactuurkorting;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting> CancelStblFactuurKortingChanges(KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblFactuurKortingUpdated(KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting item);
        partial void OnAfterStblFactuurKortingUpdated(KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting> UpdateStblFactuurKorting(int id, KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting stblfactuurkorting)
        {
            OnStblFactuurKortingUpdated(stblfactuurkorting);

            var itemToUpdate = Context.StblFactuurKortings
                              .Where(i => i.Id == stblfactuurkorting.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblfactuurkorting);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblFactuurKortingUpdated(stblfactuurkorting);

            return stblfactuurkorting;
        }

        partial void OnStblFactuurKortingDeleted(KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting item);
        partial void OnAfterStblFactuurKortingDeleted(KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblFactuurKorting> DeleteStblFactuurKorting(int id)
        {
            var itemToDelete = Context.StblFactuurKortings
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblFactuurKortingDeleted(itemToDelete);


            Context.StblFactuurKortings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblFactuurKortingDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblGlobalsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblglobals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblglobals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblGlobalsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblglobals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblglobals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblGlobalsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblGlobal> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblGlobal>> GetStblGlobals(Query query = null)
        {
            var items = Context.StblGlobals.AsQueryable();


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

            OnStblGlobalsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblGlobalGet(KlantBaseWebDemo.Models.KlantBase.StblGlobal item);
        partial void OnGetStblGlobalById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblGlobal> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblGlobal> GetStblGlobalById(int id)
        {
            var items = Context.StblGlobals
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblGlobalById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblGlobalGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblGlobalCreated(KlantBaseWebDemo.Models.KlantBase.StblGlobal item);
        partial void OnAfterStblGlobalCreated(KlantBaseWebDemo.Models.KlantBase.StblGlobal item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblGlobal> CreateStblGlobal(KlantBaseWebDemo.Models.KlantBase.StblGlobal stblglobal)
        {
            OnStblGlobalCreated(stblglobal);

            var existingItem = Context.StblGlobals
                              .Where(i => i.Id == stblglobal.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblGlobals.Add(stblglobal);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblglobal).State = EntityState.Detached;
                throw;
            }

            OnAfterStblGlobalCreated(stblglobal);

            return stblglobal;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblGlobal> CancelStblGlobalChanges(KlantBaseWebDemo.Models.KlantBase.StblGlobal item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblGlobalUpdated(KlantBaseWebDemo.Models.KlantBase.StblGlobal item);
        partial void OnAfterStblGlobalUpdated(KlantBaseWebDemo.Models.KlantBase.StblGlobal item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblGlobal> UpdateStblGlobal(int id, KlantBaseWebDemo.Models.KlantBase.StblGlobal stblglobal)
        {
            OnStblGlobalUpdated(stblglobal);

            var itemToUpdate = Context.StblGlobals
                              .Where(i => i.Id == stblglobal.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblglobal);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblGlobalUpdated(stblglobal);

            return stblglobal;
        }

        partial void OnStblGlobalDeleted(KlantBaseWebDemo.Models.KlantBase.StblGlobal item);
        partial void OnAfterStblGlobalDeleted(KlantBaseWebDemo.Models.KlantBase.StblGlobal item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblGlobal> DeleteStblGlobal(int id)
        {
            var itemToDelete = Context.StblGlobals
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblGlobalDeleted(itemToDelete);


            Context.StblGlobals.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblGlobalDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblMeldsoortsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblmeldsoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblmeldsoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblMeldsoortsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblmeldsoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblmeldsoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblMeldsoortsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblMeldsoort> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblMeldsoort>> GetStblMeldsoorts(Query query = null)
        {
            var items = Context.StblMeldsoorts.AsQueryable();


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

            OnStblMeldsoortsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblMeldsoortGet(KlantBaseWebDemo.Models.KlantBase.StblMeldsoort item);
        partial void OnGetStblMeldsoortById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblMeldsoort> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblMeldsoort> GetStblMeldsoortById(int id)
        {
            var items = Context.StblMeldsoorts
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblMeldsoortById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblMeldsoortGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblMeldsoortCreated(KlantBaseWebDemo.Models.KlantBase.StblMeldsoort item);
        partial void OnAfterStblMeldsoortCreated(KlantBaseWebDemo.Models.KlantBase.StblMeldsoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblMeldsoort> CreateStblMeldsoort(KlantBaseWebDemo.Models.KlantBase.StblMeldsoort stblmeldsoort)
        {
            OnStblMeldsoortCreated(stblmeldsoort);

            var existingItem = Context.StblMeldsoorts
                              .Where(i => i.Id == stblmeldsoort.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblMeldsoorts.Add(stblmeldsoort);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblmeldsoort).State = EntityState.Detached;
                throw;
            }

            OnAfterStblMeldsoortCreated(stblmeldsoort);

            return stblmeldsoort;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblMeldsoort> CancelStblMeldsoortChanges(KlantBaseWebDemo.Models.KlantBase.StblMeldsoort item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblMeldsoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblMeldsoort item);
        partial void OnAfterStblMeldsoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblMeldsoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblMeldsoort> UpdateStblMeldsoort(int id, KlantBaseWebDemo.Models.KlantBase.StblMeldsoort stblmeldsoort)
        {
            OnStblMeldsoortUpdated(stblmeldsoort);

            var itemToUpdate = Context.StblMeldsoorts
                              .Where(i => i.Id == stblmeldsoort.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblmeldsoort);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblMeldsoortUpdated(stblmeldsoort);

            return stblmeldsoort;
        }

        partial void OnStblMeldsoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblMeldsoort item);
        partial void OnAfterStblMeldsoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblMeldsoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblMeldsoort> DeleteStblMeldsoort(int id)
        {
            var itemToDelete = Context.StblMeldsoorts
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblMeldsoortDeleted(itemToDelete);


            Context.StblMeldsoorts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblMeldsoortDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblOpdrachtCategoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblopdrachtcategories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblopdrachtcategories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblOpdrachtCategoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblopdrachtcategories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblopdrachtcategories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblOpdrachtCategoriesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie>> GetStblOpdrachtCategories(Query query = null)
        {
            var items = Context.StblOpdrachtCategories.AsQueryable();


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

            OnStblOpdrachtCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblOpdrachtCategorieGet(KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie item);
        partial void OnGetStblOpdrachtCategorieById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie> GetStblOpdrachtCategorieById(int id)
        {
            var items = Context.StblOpdrachtCategories
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblOpdrachtCategorieById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblOpdrachtCategorieGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblOpdrachtCategorieCreated(KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie item);
        partial void OnAfterStblOpdrachtCategorieCreated(KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie> CreateStblOpdrachtCategorie(KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie stblopdrachtcategorie)
        {
            OnStblOpdrachtCategorieCreated(stblopdrachtcategorie);

            var existingItem = Context.StblOpdrachtCategories
                              .Where(i => i.Id == stblopdrachtcategorie.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblOpdrachtCategories.Add(stblopdrachtcategorie);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblopdrachtcategorie).State = EntityState.Detached;
                throw;
            }

            OnAfterStblOpdrachtCategorieCreated(stblopdrachtcategorie);

            return stblopdrachtcategorie;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie> CancelStblOpdrachtCategorieChanges(KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblOpdrachtCategorieUpdated(KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie item);
        partial void OnAfterStblOpdrachtCategorieUpdated(KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie> UpdateStblOpdrachtCategorie(int id, KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie stblopdrachtcategorie)
        {
            OnStblOpdrachtCategorieUpdated(stblopdrachtcategorie);

            var itemToUpdate = Context.StblOpdrachtCategories
                              .Where(i => i.Id == stblopdrachtcategorie.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblopdrachtcategorie);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblOpdrachtCategorieUpdated(stblopdrachtcategorie);

            return stblopdrachtcategorie;
        }

        partial void OnStblOpdrachtCategorieDeleted(KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie item);
        partial void OnAfterStblOpdrachtCategorieDeleted(KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblOpdrachtCategorie> DeleteStblOpdrachtCategorie(int id)
        {
            var itemToDelete = Context.StblOpdrachtCategories
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblOpdrachtCategorieDeleted(itemToDelete);


            Context.StblOpdrachtCategories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblOpdrachtCategorieDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblPrioritiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblpriorities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblpriorities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblPrioritiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblpriorities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblpriorities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblPrioritiesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblPriority> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblPriority>> GetStblPriorities(Query query = null)
        {
            var items = Context.StblPriorities.AsQueryable();


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

            OnStblPrioritiesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblPriorityGet(KlantBaseWebDemo.Models.KlantBase.StblPriority item);
        partial void OnGetStblPriorityById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblPriority> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblPriority> GetStblPriorityById(int id)
        {
            var items = Context.StblPriorities
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblPriorityById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblPriorityGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblPriorityCreated(KlantBaseWebDemo.Models.KlantBase.StblPriority item);
        partial void OnAfterStblPriorityCreated(KlantBaseWebDemo.Models.KlantBase.StblPriority item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblPriority> CreateStblPriority(KlantBaseWebDemo.Models.KlantBase.StblPriority stblpriority)
        {
            OnStblPriorityCreated(stblpriority);

            var existingItem = Context.StblPriorities
                              .Where(i => i.Id == stblpriority.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblPriorities.Add(stblpriority);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblpriority).State = EntityState.Detached;
                throw;
            }

            OnAfterStblPriorityCreated(stblpriority);

            return stblpriority;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblPriority> CancelStblPriorityChanges(KlantBaseWebDemo.Models.KlantBase.StblPriority item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblPriorityUpdated(KlantBaseWebDemo.Models.KlantBase.StblPriority item);
        partial void OnAfterStblPriorityUpdated(KlantBaseWebDemo.Models.KlantBase.StblPriority item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblPriority> UpdateStblPriority(int id, KlantBaseWebDemo.Models.KlantBase.StblPriority stblpriority)
        {
            OnStblPriorityUpdated(stblpriority);

            var itemToUpdate = Context.StblPriorities
                              .Where(i => i.Id == stblpriority.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblpriority);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblPriorityUpdated(stblpriority);

            return stblpriority;
        }

        partial void OnStblPriorityDeleted(KlantBaseWebDemo.Models.KlantBase.StblPriority item);
        partial void OnAfterStblPriorityDeleted(KlantBaseWebDemo.Models.KlantBase.StblPriority item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblPriority> DeleteStblPriority(int id)
        {
            var itemToDelete = Context.StblPriorities
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblPriorityDeleted(itemToDelete);


            Context.StblPriorities.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblPriorityDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblRelatieSoortsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblrelatiesoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblrelatiesoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblRelatieSoortsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblrelatiesoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblrelatiesoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblRelatieSoortsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort>> GetStblRelatieSoorts(Query query = null)
        {
            var items = Context.StblRelatieSoorts.AsQueryable();


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

            OnStblRelatieSoortsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblRelatieSoortGet(KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort item);
        partial void OnGetStblRelatieSoortByOmschrijving(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort> GetStblRelatieSoortByOmschrijving(string omschrijving)
        {
            var items = Context.StblRelatieSoorts
                              .AsNoTracking()
                              .Where(i => i.Omschrijving == omschrijving);

 
            OnGetStblRelatieSoortByOmschrijving(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblRelatieSoortGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblRelatieSoortCreated(KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort item);
        partial void OnAfterStblRelatieSoortCreated(KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort> CreateStblRelatieSoort(KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort stblrelatiesoort)
        {
            OnStblRelatieSoortCreated(stblrelatiesoort);

            var existingItem = Context.StblRelatieSoorts
                              .Where(i => i.Omschrijving == stblrelatiesoort.Omschrijving)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblRelatieSoorts.Add(stblrelatiesoort);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblrelatiesoort).State = EntityState.Detached;
                throw;
            }

            OnAfterStblRelatieSoortCreated(stblrelatiesoort);

            return stblrelatiesoort;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort> CancelStblRelatieSoortChanges(KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblRelatieSoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort item);
        partial void OnAfterStblRelatieSoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort> UpdateStblRelatieSoort(string omschrijving, KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort stblrelatiesoort)
        {
            OnStblRelatieSoortUpdated(stblrelatiesoort);

            var itemToUpdate = Context.StblRelatieSoorts
                              .Where(i => i.Omschrijving == stblrelatiesoort.Omschrijving)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblrelatiesoort);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblRelatieSoortUpdated(stblrelatiesoort);

            return stblrelatiesoort;
        }

        partial void OnStblRelatieSoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort item);
        partial void OnAfterStblRelatieSoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblRelatieSoort> DeleteStblRelatieSoort(string omschrijving)
        {
            var itemToDelete = Context.StblRelatieSoorts
                              .Where(i => i.Omschrijving == omschrijving)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblRelatieSoortDeleted(itemToDelete);


            Context.StblRelatieSoorts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblRelatieSoortDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblReplaceFieldsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblreplacefields/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblreplacefields/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblReplaceFieldsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblreplacefields/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblreplacefields/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblReplaceFieldsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblReplaceField> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblReplaceField>> GetStblReplaceFields(Query query = null)
        {
            var items = Context.StblReplaceFields.AsQueryable();


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

            OnStblReplaceFieldsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblReplaceFieldGet(KlantBaseWebDemo.Models.KlantBase.StblReplaceField item);
        partial void OnGetStblReplaceFieldById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblReplaceField> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblReplaceField> GetStblReplaceFieldById(int id)
        {
            var items = Context.StblReplaceFields
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblReplaceFieldById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblReplaceFieldGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblReplaceFieldCreated(KlantBaseWebDemo.Models.KlantBase.StblReplaceField item);
        partial void OnAfterStblReplaceFieldCreated(KlantBaseWebDemo.Models.KlantBase.StblReplaceField item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblReplaceField> CreateStblReplaceField(KlantBaseWebDemo.Models.KlantBase.StblReplaceField stblreplacefield)
        {
            OnStblReplaceFieldCreated(stblreplacefield);

            var existingItem = Context.StblReplaceFields
                              .Where(i => i.Id == stblreplacefield.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblReplaceFields.Add(stblreplacefield);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblreplacefield).State = EntityState.Detached;
                throw;
            }

            OnAfterStblReplaceFieldCreated(stblreplacefield);

            return stblreplacefield;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblReplaceField> CancelStblReplaceFieldChanges(KlantBaseWebDemo.Models.KlantBase.StblReplaceField item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblReplaceFieldUpdated(KlantBaseWebDemo.Models.KlantBase.StblReplaceField item);
        partial void OnAfterStblReplaceFieldUpdated(KlantBaseWebDemo.Models.KlantBase.StblReplaceField item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblReplaceField> UpdateStblReplaceField(int id, KlantBaseWebDemo.Models.KlantBase.StblReplaceField stblreplacefield)
        {
            OnStblReplaceFieldUpdated(stblreplacefield);

            var itemToUpdate = Context.StblReplaceFields
                              .Where(i => i.Id == stblreplacefield.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblreplacefield);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblReplaceFieldUpdated(stblreplacefield);

            return stblreplacefield;
        }

        partial void OnStblReplaceFieldDeleted(KlantBaseWebDemo.Models.KlantBase.StblReplaceField item);
        partial void OnAfterStblReplaceFieldDeleted(KlantBaseWebDemo.Models.KlantBase.StblReplaceField item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblReplaceField> DeleteStblReplaceField(int id)
        {
            var itemToDelete = Context.StblReplaceFields
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblReplaceFieldDeleted(itemToDelete);


            Context.StblReplaceFields.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblReplaceFieldDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblReplaceFieldsNewsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblreplacefieldsnews/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblreplacefieldsnews/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblReplaceFieldsNewsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblreplacefieldsnews/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblreplacefieldsnews/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblReplaceFieldsNewsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblReplaceFieldsNew> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblReplaceFieldsNew>> GetStblReplaceFieldsNews(Query query = null)
        {
            var items = Context.StblReplaceFieldsNews.AsQueryable();


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

            OnStblReplaceFieldsNewsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportStblSelectieCodesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblselectiecodes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblselectiecodes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblSelectieCodesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblselectiecodes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblselectiecodes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblSelectieCodesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblSelectieCode> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblSelectieCode>> GetStblSelectieCodes(Query query = null)
        {
            var items = Context.StblSelectieCodes.AsQueryable();


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

            OnStblSelectieCodesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblSelectieCodeGet(KlantBaseWebDemo.Models.KlantBase.StblSelectieCode item);
        partial void OnGetStblSelectieCodeById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblSelectieCode> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectieCode> GetStblSelectieCodeById(int id)
        {
            var items = Context.StblSelectieCodes
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblSelectieCodeById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblSelectieCodeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblSelectieCodeCreated(KlantBaseWebDemo.Models.KlantBase.StblSelectieCode item);
        partial void OnAfterStblSelectieCodeCreated(KlantBaseWebDemo.Models.KlantBase.StblSelectieCode item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectieCode> CreateStblSelectieCode(KlantBaseWebDemo.Models.KlantBase.StblSelectieCode stblselectiecode)
        {
            OnStblSelectieCodeCreated(stblselectiecode);

            var existingItem = Context.StblSelectieCodes
                              .Where(i => i.Id == stblselectiecode.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblSelectieCodes.Add(stblselectiecode);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblselectiecode).State = EntityState.Detached;
                throw;
            }

            OnAfterStblSelectieCodeCreated(stblselectiecode);

            return stblselectiecode;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectieCode> CancelStblSelectieCodeChanges(KlantBaseWebDemo.Models.KlantBase.StblSelectieCode item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblSelectieCodeUpdated(KlantBaseWebDemo.Models.KlantBase.StblSelectieCode item);
        partial void OnAfterStblSelectieCodeUpdated(KlantBaseWebDemo.Models.KlantBase.StblSelectieCode item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectieCode> UpdateStblSelectieCode(int id, KlantBaseWebDemo.Models.KlantBase.StblSelectieCode stblselectiecode)
        {
            OnStblSelectieCodeUpdated(stblselectiecode);

            var itemToUpdate = Context.StblSelectieCodes
                              .Where(i => i.Id == stblselectiecode.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblselectiecode);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblSelectieCodeUpdated(stblselectiecode);

            return stblselectiecode;
        }

        partial void OnStblSelectieCodeDeleted(KlantBaseWebDemo.Models.KlantBase.StblSelectieCode item);
        partial void OnAfterStblSelectieCodeDeleted(KlantBaseWebDemo.Models.KlantBase.StblSelectieCode item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSelectieCode> DeleteStblSelectieCode(int id)
        {
            var itemToDelete = Context.StblSelectieCodes
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblSelectieCodeDeleted(itemToDelete);


            Context.StblSelectieCodes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblSelectieCodeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblStatusesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblStatus> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblStatus>> GetStblStatuses(Query query = null)
        {
            var items = Context.StblStatuses.AsQueryable();


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

            OnStblStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblStatusGet(KlantBaseWebDemo.Models.KlantBase.StblStatus item);
        partial void OnGetStblStatusById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblStatus> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblStatus> GetStblStatusById(int id)
        {
            var items = Context.StblStatuses
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblStatusById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblStatusGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblStatusCreated(KlantBaseWebDemo.Models.KlantBase.StblStatus item);
        partial void OnAfterStblStatusCreated(KlantBaseWebDemo.Models.KlantBase.StblStatus item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblStatus> CreateStblStatus(KlantBaseWebDemo.Models.KlantBase.StblStatus stblstatus)
        {
            OnStblStatusCreated(stblstatus);

            var existingItem = Context.StblStatuses
                              .Where(i => i.Id == stblstatus.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblStatuses.Add(stblstatus);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblstatus).State = EntityState.Detached;
                throw;
            }

            OnAfterStblStatusCreated(stblstatus);

            return stblstatus;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblStatus> CancelStblStatusChanges(KlantBaseWebDemo.Models.KlantBase.StblStatus item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblStatusUpdated(KlantBaseWebDemo.Models.KlantBase.StblStatus item);
        partial void OnAfterStblStatusUpdated(KlantBaseWebDemo.Models.KlantBase.StblStatus item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblStatus> UpdateStblStatus(int id, KlantBaseWebDemo.Models.KlantBase.StblStatus stblstatus)
        {
            OnStblStatusUpdated(stblstatus);

            var itemToUpdate = Context.StblStatuses
                              .Where(i => i.Id == stblstatus.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblstatus);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblStatusUpdated(stblstatus);

            return stblstatus;
        }

        partial void OnStblStatusDeleted(KlantBaseWebDemo.Models.KlantBase.StblStatus item);
        partial void OnAfterStblStatusDeleted(KlantBaseWebDemo.Models.KlantBase.StblStatus item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblStatus> DeleteStblStatus(int id)
        {
            var itemToDelete = Context.StblStatuses
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblStatusDeleted(itemToDelete);


            Context.StblStatuses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblStatusDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblSysteemsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblsysteems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblsysteems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblSysteemsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stblsysteems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stblsysteems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblSysteemsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblSysteem> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblSysteem>> GetStblSysteems(Query query = null)
        {
            var items = Context.StblSysteems.AsQueryable();


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

            OnStblSysteemsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblSysteemGet(KlantBaseWebDemo.Models.KlantBase.StblSysteem item);
        partial void OnGetStblSysteemById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblSysteem> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSysteem> GetStblSysteemById(int id)
        {
            var items = Context.StblSysteems
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblSysteemById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblSysteemGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblSysteemCreated(KlantBaseWebDemo.Models.KlantBase.StblSysteem item);
        partial void OnAfterStblSysteemCreated(KlantBaseWebDemo.Models.KlantBase.StblSysteem item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSysteem> CreateStblSysteem(KlantBaseWebDemo.Models.KlantBase.StblSysteem stblsysteem)
        {
            OnStblSysteemCreated(stblsysteem);

            var existingItem = Context.StblSysteems
                              .Where(i => i.Id == stblsysteem.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblSysteems.Add(stblsysteem);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stblsysteem).State = EntityState.Detached;
                throw;
            }

            OnAfterStblSysteemCreated(stblsysteem);

            return stblsysteem;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSysteem> CancelStblSysteemChanges(KlantBaseWebDemo.Models.KlantBase.StblSysteem item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblSysteemUpdated(KlantBaseWebDemo.Models.KlantBase.StblSysteem item);
        partial void OnAfterStblSysteemUpdated(KlantBaseWebDemo.Models.KlantBase.StblSysteem item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSysteem> UpdateStblSysteem(int id, KlantBaseWebDemo.Models.KlantBase.StblSysteem stblsysteem)
        {
            OnStblSysteemUpdated(stblsysteem);

            var itemToUpdate = Context.StblSysteems
                              .Where(i => i.Id == stblsysteem.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stblsysteem);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblSysteemUpdated(stblsysteem);

            return stblsysteem;
        }

        partial void OnStblSysteemDeleted(KlantBaseWebDemo.Models.KlantBase.StblSysteem item);
        partial void OnAfterStblSysteemDeleted(KlantBaseWebDemo.Models.KlantBase.StblSysteem item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblSysteem> DeleteStblSysteem(int id)
        {
            var itemToDelete = Context.StblSysteems
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblSysteemDeleted(itemToDelete);


            Context.StblSysteems.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblSysteemDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStblTrainingsSoortsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stbltrainingssoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stbltrainingssoorts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStblTrainingsSoortsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stbltrainingssoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stbltrainingssoorts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStblTrainingsSoortsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort>> GetStblTrainingsSoorts(Query query = null)
        {
            var items = Context.StblTrainingsSoorts.AsQueryable();


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

            OnStblTrainingsSoortsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStblTrainingsSoortGet(KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort item);
        partial void OnGetStblTrainingsSoortById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort> GetStblTrainingsSoortById(int id)
        {
            var items = Context.StblTrainingsSoorts
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStblTrainingsSoortById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStblTrainingsSoortGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStblTrainingsSoortCreated(KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort item);
        partial void OnAfterStblTrainingsSoortCreated(KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort> CreateStblTrainingsSoort(KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort stbltrainingssoort)
        {
            OnStblTrainingsSoortCreated(stbltrainingssoort);

            var existingItem = Context.StblTrainingsSoorts
                              .Where(i => i.Id == stbltrainingssoort.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StblTrainingsSoorts.Add(stbltrainingssoort);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stbltrainingssoort).State = EntityState.Detached;
                throw;
            }

            OnAfterStblTrainingsSoortCreated(stbltrainingssoort);

            return stbltrainingssoort;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort> CancelStblTrainingsSoortChanges(KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStblTrainingsSoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort item);
        partial void OnAfterStblTrainingsSoortUpdated(KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort> UpdateStblTrainingsSoort(int id, KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort stbltrainingssoort)
        {
            OnStblTrainingsSoortUpdated(stbltrainingssoort);

            var itemToUpdate = Context.StblTrainingsSoorts
                              .Where(i => i.Id == stbltrainingssoort.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stbltrainingssoort);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStblTrainingsSoortUpdated(stbltrainingssoort);

            return stbltrainingssoort;
        }

        partial void OnStblTrainingsSoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort item);
        partial void OnAfterStblTrainingsSoortDeleted(KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.StblTrainingsSoort> DeleteStblTrainingsSoort(int id)
        {
            var itemToDelete = Context.StblTrainingsSoorts
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStblTrainingsSoortDeleted(itemToDelete);


            Context.StblTrainingsSoorts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStblTrainingsSoortDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStucadoorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stucadoors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stucadoors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStucadoorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/stucadoors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/stucadoors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStucadoorsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Stucadoor> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.Stucadoor>> GetStucadoors(Query query = null)
        {
            var items = Context.Stucadoors.AsQueryable();


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

            OnStucadoorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStucadoorGet(KlantBaseWebDemo.Models.KlantBase.Stucadoor item);
        partial void OnGetStucadoorById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Stucadoor> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.Stucadoor> GetStucadoorById(int id)
        {
            var items = Context.Stucadoors
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetStucadoorById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStucadoorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStucadoorCreated(KlantBaseWebDemo.Models.KlantBase.Stucadoor item);
        partial void OnAfterStucadoorCreated(KlantBaseWebDemo.Models.KlantBase.Stucadoor item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Stucadoor> CreateStucadoor(KlantBaseWebDemo.Models.KlantBase.Stucadoor stucadoor)
        {
            OnStucadoorCreated(stucadoor);

            var existingItem = Context.Stucadoors
                              .Where(i => i.Id == stucadoor.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Stucadoors.Add(stucadoor);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stucadoor).State = EntityState.Detached;
                throw;
            }

            OnAfterStucadoorCreated(stucadoor);

            return stucadoor;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.Stucadoor> CancelStucadoorChanges(KlantBaseWebDemo.Models.KlantBase.Stucadoor item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStucadoorUpdated(KlantBaseWebDemo.Models.KlantBase.Stucadoor item);
        partial void OnAfterStucadoorUpdated(KlantBaseWebDemo.Models.KlantBase.Stucadoor item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Stucadoor> UpdateStucadoor(int id, KlantBaseWebDemo.Models.KlantBase.Stucadoor stucadoor)
        {
            OnStucadoorUpdated(stucadoor);

            var itemToUpdate = Context.Stucadoors
                              .Where(i => i.Id == stucadoor.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stucadoor);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStucadoorUpdated(stucadoor);

            return stucadoor;
        }

        partial void OnStucadoorDeleted(KlantBaseWebDemo.Models.KlantBase.Stucadoor item);
        partial void OnAfterStucadoorDeleted(KlantBaseWebDemo.Models.KlantBase.Stucadoor item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Stucadoor> DeleteStucadoor(int id)
        {
            var itemToDelete = Context.Stucadoors
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStucadoorDeleted(itemToDelete);


            Context.Stucadoors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStucadoorDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblFactRegelsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblfactregels/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblfactregels/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblFactRegelsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblfactregels/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblfactregels/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblFactRegelsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblFactRegel> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblFactRegel>> GetTblFactRegels(Query query = null)
        {
            var items = Context.TblFactRegels.AsQueryable();

            items = items.Include(i => i.TblProjectOnderdelen);

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

            OnTblFactRegelsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblFactRegelGet(KlantBaseWebDemo.Models.KlantBase.TblFactRegel item);
        partial void OnGetTblFactRegelByFldFdid(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblFactRegel> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFactRegel> GetTblFactRegelByFldFdid(int fldfdid)
        {
            var items = Context.TblFactRegels
                              .AsNoTracking()
                              .Where(i => i.FldFdid == fldfdid);

            items = items.Include(i => i.TblProjectOnderdelen);
 
            OnGetTblFactRegelByFldFdid(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblFactRegelGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblFactRegelCreated(KlantBaseWebDemo.Models.KlantBase.TblFactRegel item);
        partial void OnAfterTblFactRegelCreated(KlantBaseWebDemo.Models.KlantBase.TblFactRegel item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFactRegel> CreateTblFactRegel(KlantBaseWebDemo.Models.KlantBase.TblFactRegel tblfactregel)
        {
            OnTblFactRegelCreated(tblfactregel);

            var existingItem = Context.TblFactRegels
                              .Where(i => i.FldFdid == tblfactregel.FldFdid)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblFactRegels.Add(tblfactregel);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblfactregel).State = EntityState.Detached;
                throw;
            }

            OnAfterTblFactRegelCreated(tblfactregel);

            return tblfactregel;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFactRegel> CancelTblFactRegelChanges(KlantBaseWebDemo.Models.KlantBase.TblFactRegel item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblFactRegelUpdated(KlantBaseWebDemo.Models.KlantBase.TblFactRegel item);
        partial void OnAfterTblFactRegelUpdated(KlantBaseWebDemo.Models.KlantBase.TblFactRegel item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFactRegel> UpdateTblFactRegel(int fldfdid, KlantBaseWebDemo.Models.KlantBase.TblFactRegel tblfactregel)
        {
            OnTblFactRegelUpdated(tblfactregel);

            var itemToUpdate = Context.TblFactRegels
                              .Where(i => i.FldFdid == tblfactregel.FldFdid)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblfactregel);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblFactRegelUpdated(tblfactregel);

            return tblfactregel;
        }

        partial void OnTblFactRegelDeleted(KlantBaseWebDemo.Models.KlantBase.TblFactRegel item);
        partial void OnAfterTblFactRegelDeleted(KlantBaseWebDemo.Models.KlantBase.TblFactRegel item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFactRegel> DeleteTblFactRegel(int fldfdid)
        {
            var itemToDelete = Context.TblFactRegels
                              .Where(i => i.FldFdid == fldfdid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblFactRegelDeleted(itemToDelete);


            Context.TblFactRegels.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblFactRegelDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblFaktuursToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblfaktuurs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblfaktuurs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblFaktuursToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblfaktuurs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblfaktuurs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblFaktuursRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblFaktuur> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblFaktuur>> GetTblFaktuurs(Query query = null)
        {
            var items = Context.TblFaktuurs.AsQueryable();


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

            OnTblFaktuursRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblFaktuurGet(KlantBaseWebDemo.Models.KlantBase.TblFaktuur item);
        partial void OnGetTblFaktuurById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblFaktuur> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFaktuur> GetTblFaktuurById(int id)
        {
            var items = Context.TblFaktuurs
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetTblFaktuurById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblFaktuurGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblFaktuurCreated(KlantBaseWebDemo.Models.KlantBase.TblFaktuur item);
        partial void OnAfterTblFaktuurCreated(KlantBaseWebDemo.Models.KlantBase.TblFaktuur item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFaktuur> CreateTblFaktuur(KlantBaseWebDemo.Models.KlantBase.TblFaktuur tblfaktuur)
        {
            OnTblFaktuurCreated(tblfaktuur);

            var existingItem = Context.TblFaktuurs
                              .Where(i => i.Id == tblfaktuur.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblFaktuurs.Add(tblfaktuur);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblfaktuur).State = EntityState.Detached;
                throw;
            }

            OnAfterTblFaktuurCreated(tblfaktuur);

            return tblfaktuur;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFaktuur> CancelTblFaktuurChanges(KlantBaseWebDemo.Models.KlantBase.TblFaktuur item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblFaktuurUpdated(KlantBaseWebDemo.Models.KlantBase.TblFaktuur item);
        partial void OnAfterTblFaktuurUpdated(KlantBaseWebDemo.Models.KlantBase.TblFaktuur item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFaktuur> UpdateTblFaktuur(int id, KlantBaseWebDemo.Models.KlantBase.TblFaktuur tblfaktuur)
        {
            OnTblFaktuurUpdated(tblfaktuur);

            var itemToUpdate = Context.TblFaktuurs
                              .Where(i => i.Id == tblfaktuur.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblfaktuur);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblFaktuurUpdated(tblfaktuur);

            return tblfaktuur;
        }

        partial void OnTblFaktuurDeleted(KlantBaseWebDemo.Models.KlantBase.TblFaktuur item);
        partial void OnAfterTblFaktuurDeleted(KlantBaseWebDemo.Models.KlantBase.TblFaktuur item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblFaktuur> DeleteTblFaktuur(int id)
        {
            var itemToDelete = Context.TblFaktuurs
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblFaktuurDeleted(itemToDelete);


            Context.TblFaktuurs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblFaktuurDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblGlobalsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblglobals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblglobals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblGlobalsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblglobals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblglobals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblGlobalsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblGlobal> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblGlobal>> GetTblGlobals(Query query = null)
        {
            var items = Context.TblGlobals.AsQueryable();


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

            OnTblGlobalsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblGlobalGet(KlantBaseWebDemo.Models.KlantBase.TblGlobal item);
        partial void OnGetTblGlobalById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblGlobal> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblGlobal> GetTblGlobalById(int id)
        {
            var items = Context.TblGlobals
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetTblGlobalById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblGlobalGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblGlobalCreated(KlantBaseWebDemo.Models.KlantBase.TblGlobal item);
        partial void OnAfterTblGlobalCreated(KlantBaseWebDemo.Models.KlantBase.TblGlobal item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblGlobal> CreateTblGlobal(KlantBaseWebDemo.Models.KlantBase.TblGlobal tblglobal)
        {
            OnTblGlobalCreated(tblglobal);

            var existingItem = Context.TblGlobals
                              .Where(i => i.Id == tblglobal.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblGlobals.Add(tblglobal);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblglobal).State = EntityState.Detached;
                throw;
            }

            OnAfterTblGlobalCreated(tblglobal);

            return tblglobal;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblGlobal> CancelTblGlobalChanges(KlantBaseWebDemo.Models.KlantBase.TblGlobal item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblGlobalUpdated(KlantBaseWebDemo.Models.KlantBase.TblGlobal item);
        partial void OnAfterTblGlobalUpdated(KlantBaseWebDemo.Models.KlantBase.TblGlobal item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblGlobal> UpdateTblGlobal(int id, KlantBaseWebDemo.Models.KlantBase.TblGlobal tblglobal)
        {
            OnTblGlobalUpdated(tblglobal);

            var itemToUpdate = Context.TblGlobals
                              .Where(i => i.Id == tblglobal.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblglobal);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblGlobalUpdated(tblglobal);

            return tblglobal;
        }

        partial void OnTblGlobalDeleted(KlantBaseWebDemo.Models.KlantBase.TblGlobal item);
        partial void OnAfterTblGlobalDeleted(KlantBaseWebDemo.Models.KlantBase.TblGlobal item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblGlobal> DeleteTblGlobal(int id)
        {
            var itemToDelete = Context.TblGlobals
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblGlobalDeleted(itemToDelete);


            Context.TblGlobals.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblGlobalDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblKeuringsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblkeurings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblkeurings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblKeuringsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblkeurings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblkeurings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblKeuringsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblKeuring> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblKeuring>> GetTblKeurings(Query query = null)
        {
            var items = Context.TblKeurings.AsQueryable();


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

            OnTblKeuringsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblKeuringGet(KlantBaseWebDemo.Models.KlantBase.TblKeuring item);
        partial void OnGetTblKeuringById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblKeuring> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblKeuring> GetTblKeuringById(int id)
        {
            var items = Context.TblKeurings
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetTblKeuringById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblKeuringGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblKeuringCreated(KlantBaseWebDemo.Models.KlantBase.TblKeuring item);
        partial void OnAfterTblKeuringCreated(KlantBaseWebDemo.Models.KlantBase.TblKeuring item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblKeuring> CreateTblKeuring(KlantBaseWebDemo.Models.KlantBase.TblKeuring tblkeuring)
        {
            OnTblKeuringCreated(tblkeuring);

            var existingItem = Context.TblKeurings
                              .Where(i => i.Id == tblkeuring.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblKeurings.Add(tblkeuring);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblkeuring).State = EntityState.Detached;
                throw;
            }

            OnAfterTblKeuringCreated(tblkeuring);

            return tblkeuring;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblKeuring> CancelTblKeuringChanges(KlantBaseWebDemo.Models.KlantBase.TblKeuring item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblKeuringUpdated(KlantBaseWebDemo.Models.KlantBase.TblKeuring item);
        partial void OnAfterTblKeuringUpdated(KlantBaseWebDemo.Models.KlantBase.TblKeuring item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblKeuring> UpdateTblKeuring(int id, KlantBaseWebDemo.Models.KlantBase.TblKeuring tblkeuring)
        {
            OnTblKeuringUpdated(tblkeuring);

            var itemToUpdate = Context.TblKeurings
                              .Where(i => i.Id == tblkeuring.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblkeuring);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblKeuringUpdated(tblkeuring);

            return tblkeuring;
        }

        partial void OnTblKeuringDeleted(KlantBaseWebDemo.Models.KlantBase.TblKeuring item);
        partial void OnAfterTblKeuringDeleted(KlantBaseWebDemo.Models.KlantBase.TblKeuring item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblKeuring> DeleteTblKeuring(int id)
        {
            var itemToDelete = Context.TblKeurings
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblKeuringDeleted(itemToDelete);


            Context.TblKeurings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblKeuringDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblMemosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblmemos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblmemos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblMemosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblmemos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblmemos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblMemosRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblMemo> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblMemo>> GetTblMemos(Query query = null)
        {
            var items = Context.TblMemos.AsQueryable();


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

            OnTblMemosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblMemoGet(KlantBaseWebDemo.Models.KlantBase.TblMemo item);
        partial void OnGetTblMemoByFldMid(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblMemo> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblMemo> GetTblMemoByFldMid(int fldmid)
        {
            var items = Context.TblMemos
                              .AsNoTracking()
                              .Where(i => i.FldMid == fldmid);

 
            OnGetTblMemoByFldMid(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblMemoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblMemoCreated(KlantBaseWebDemo.Models.KlantBase.TblMemo item);
        partial void OnAfterTblMemoCreated(KlantBaseWebDemo.Models.KlantBase.TblMemo item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblMemo> CreateTblMemo(KlantBaseWebDemo.Models.KlantBase.TblMemo tblmemo)
        {
            OnTblMemoCreated(tblmemo);

            var existingItem = Context.TblMemos
                              .Where(i => i.FldMid == tblmemo.FldMid)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblMemos.Add(tblmemo);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblmemo).State = EntityState.Detached;
                throw;
            }

            OnAfterTblMemoCreated(tblmemo);

            return tblmemo;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblMemo> CancelTblMemoChanges(KlantBaseWebDemo.Models.KlantBase.TblMemo item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblMemoUpdated(KlantBaseWebDemo.Models.KlantBase.TblMemo item);
        partial void OnAfterTblMemoUpdated(KlantBaseWebDemo.Models.KlantBase.TblMemo item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblMemo> UpdateTblMemo(int fldmid, KlantBaseWebDemo.Models.KlantBase.TblMemo tblmemo)
        {
            OnTblMemoUpdated(tblmemo);

            var itemToUpdate = Context.TblMemos
                              .Where(i => i.FldMid == tblmemo.FldMid)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblmemo);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblMemoUpdated(tblmemo);

            return tblmemo;
        }

        partial void OnTblMemoDeleted(KlantBaseWebDemo.Models.KlantBase.TblMemo item);
        partial void OnAfterTblMemoDeleted(KlantBaseWebDemo.Models.KlantBase.TblMemo item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblMemo> DeleteTblMemo(int fldmid)
        {
            var itemToDelete = Context.TblMemos
                              .Where(i => i.FldMid == fldmid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblMemoDeleted(itemToDelete);


            Context.TblMemos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblMemoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblProjectensToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblprojectens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblprojectens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblProjectensToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblprojectens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblprojectens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblProjectensRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjecten> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjecten>> GetTblProjectens(Query query = null)
        {
            var items = Context.TblProjectens.AsQueryable();


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

            OnTblProjectensRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblProjectenGet(KlantBaseWebDemo.Models.KlantBase.TblProjecten item);
        partial void OnGetTblProjectenById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjecten> items);


        // Aangepaste GetTblProjectenById met includeOnderdelen parameter
        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjecten> GetTblProjectenById(int id, bool includeOnderdelen = true)
        {
            var query = Context.TblProjectens
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            if (includeOnderdelen)
            {
                query = query.Include(p => p.TblProjectOnderdelens);
            }

            OnGetTblProjectenById(ref query);

            var itemToReturn = await query.FirstOrDefaultAsync();

            if (itemToReturn != null)
            {
                OnTblProjectenGet(itemToReturn);
            }

            return itemToReturn;
        }

        partial void OnTblProjectenCreated(KlantBaseWebDemo.Models.KlantBase.TblProjecten item);
        partial void OnAfterTblProjectenCreated(KlantBaseWebDemo.Models.KlantBase.TblProjecten item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjecten> CreateTblProjecten(KlantBaseWebDemo.Models.KlantBase.TblProjecten tblprojecten)
        {
            OnTblProjectenCreated(tblprojecten);

            var existingItem = Context.TblProjectens
                              .Where(i => i.Id == tblprojecten.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblProjectens.Add(tblprojecten);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblprojecten).State = EntityState.Detached;
                throw;
            }

            OnAfterTblProjectenCreated(tblprojecten);

            return tblprojecten;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjecten> CancelTblProjectenChanges(KlantBaseWebDemo.Models.KlantBase.TblProjecten item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblProjectenUpdated(KlantBaseWebDemo.Models.KlantBase.TblProjecten item);
        partial void OnAfterTblProjectenUpdated(KlantBaseWebDemo.Models.KlantBase.TblProjecten item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjecten> UpdateTblProjecten(int id, KlantBaseWebDemo.Models.KlantBase.TblProjecten tblprojecten)
        {
            OnTblProjectenUpdated(tblprojecten);

            var itemToUpdate = Context.TblProjectens
                              .Where(i => i.Id == tblprojecten.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblprojecten);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblProjectenUpdated(tblprojecten);

            return tblprojecten;
        }

        partial void OnTblProjectenDeleted(KlantBaseWebDemo.Models.KlantBase.TblProjecten item);
        partial void OnAfterTblProjectenDeleted(KlantBaseWebDemo.Models.KlantBase.TblProjecten item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjecten> DeleteTblProjecten(int id)
        {
            var itemToDelete = Context.TblProjectens
                              .Where(i => i.Id == id)
                              .Include(i => i.TblProjectOnderdelens)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblProjectenDeleted(itemToDelete);


            Context.TblProjectens.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblProjectenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblProjectM2jaarsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblprojectm2jaars/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblprojectm2jaars/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblProjectM2jaarsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblprojectm2jaars/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblprojectm2jaars/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblProjectM2jaarsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar>> GetTblProjectM2jaars(Query query = null)
        {
            var items = Context.TblProjectM2jaars.AsQueryable();


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

            OnTblProjectM2jaarsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblProjectM2jaarGet(KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar item);
        partial void OnGetTblProjectM2jaarById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar> GetTblProjectM2jaarById(int id)
        {
            var items = Context.TblProjectM2jaars
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetTblProjectM2jaarById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblProjectM2jaarGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblProjectM2jaarCreated(KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar item);
        partial void OnAfterTblProjectM2jaarCreated(KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar> CreateTblProjectM2jaar(KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar tblprojectm2jaar)
        {
            OnTblProjectM2jaarCreated(tblprojectm2jaar);

            var existingItem = Context.TblProjectM2jaars
                              .Where(i => i.Id == tblprojectm2jaar.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblProjectM2jaars.Add(tblprojectm2jaar);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblprojectm2jaar).State = EntityState.Detached;
                throw;
            }

            OnAfterTblProjectM2jaarCreated(tblprojectm2jaar);

            return tblprojectm2jaar;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar> CancelTblProjectM2jaarChanges(KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblProjectM2jaarUpdated(KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar item);
        partial void OnAfterTblProjectM2jaarUpdated(KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar> UpdateTblProjectM2jaar(int id, KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar tblprojectm2jaar)
        {
            OnTblProjectM2jaarUpdated(tblprojectm2jaar);

            var itemToUpdate = Context.TblProjectM2jaars
                              .Where(i => i.Id == tblprojectm2jaar.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblprojectm2jaar);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblProjectM2jaarUpdated(tblprojectm2jaar);

            return tblprojectm2jaar;
        }

        partial void OnTblProjectM2jaarDeleted(KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar item);
        partial void OnAfterTblProjectM2jaarDeleted(KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectM2jaar> DeleteTblProjectM2jaar(int id)
        {
            var itemToDelete = Context.TblProjectM2jaars
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblProjectM2jaarDeleted(itemToDelete);


            Context.TblProjectM2jaars.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblProjectM2jaarDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblProjectNrsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblprojectnrs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblprojectnrs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblProjectNrsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblprojectnrs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblprojectnrs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblProjectNrsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjectNr> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjectNr>> GetTblProjectNrs(Query query = null)
        {
            var items = Context.TblProjectNrs.AsQueryable();


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

            OnTblProjectNrsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblProjectNrGet(KlantBaseWebDemo.Models.KlantBase.TblProjectNr item);
        partial void OnGetTblProjectNrById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjectNr> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectNr> GetTblProjectNrById(int id)
        {
            var items = Context.TblProjectNrs
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetTblProjectNrById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblProjectNrGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblProjectNrCreated(KlantBaseWebDemo.Models.KlantBase.TblProjectNr item);
        partial void OnAfterTblProjectNrCreated(KlantBaseWebDemo.Models.KlantBase.TblProjectNr item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectNr> CreateTblProjectNr(KlantBaseWebDemo.Models.KlantBase.TblProjectNr tblprojectnr)
        {
            OnTblProjectNrCreated(tblprojectnr);

            var existingItem = Context.TblProjectNrs
                              .Where(i => i.Id == tblprojectnr.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblProjectNrs.Add(tblprojectnr);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblprojectnr).State = EntityState.Detached;
                throw;
            }

            OnAfterTblProjectNrCreated(tblprojectnr);

            return tblprojectnr;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectNr> CancelTblProjectNrChanges(KlantBaseWebDemo.Models.KlantBase.TblProjectNr item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblProjectNrUpdated(KlantBaseWebDemo.Models.KlantBase.TblProjectNr item);
        partial void OnAfterTblProjectNrUpdated(KlantBaseWebDemo.Models.KlantBase.TblProjectNr item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectNr> UpdateTblProjectNr(int id, KlantBaseWebDemo.Models.KlantBase.TblProjectNr tblprojectnr)
        {
            OnTblProjectNrUpdated(tblprojectnr);

            var itemToUpdate = Context.TblProjectNrs
                              .Where(i => i.Id == tblprojectnr.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblprojectnr);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblProjectNrUpdated(tblprojectnr);

            return tblprojectnr;
        }

        partial void OnTblProjectNrDeleted(KlantBaseWebDemo.Models.KlantBase.TblProjectNr item);
        partial void OnAfterTblProjectNrDeleted(KlantBaseWebDemo.Models.KlantBase.TblProjectNr item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectNr> DeleteTblProjectNr(int id)
        {
            var itemToDelete = Context.TblProjectNrs
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblProjectNrDeleted(itemToDelete);


            Context.TblProjectNrs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblProjectNrDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblProjectOnderdelensToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblprojectonderdelens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblprojectonderdelens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblProjectOnderdelensToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblprojectonderdelens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblprojectonderdelens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblProjectOnderdelensRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>> GetTblProjectOnderdelens(Query query = null)
        {
            var items = Context.TblProjectOnderdelens.AsQueryable();

            items = items.Include(i => i.TblProjecten);

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

            OnTblProjectOnderdelensRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblProjectOnderdelenGet(KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen item);
        partial void OnGetTblProjectOnderdelenById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen> GetTblProjectOnderdelenById(int id)
        {
            var items = Context.TblProjectOnderdelens
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.TblProjecten);
 
            OnGetTblProjectOnderdelenById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblProjectOnderdelenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        // Nieuwe methode met filters voor Opdrachten en Onderhoudscontracten
        public async Task<IEnumerable<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen>> GetTblProjectOnderdelensByProjectId(int projectId, int? soort = null, int? excludeSoort = null)
        {
            var query = Context.TblProjectOnderdelens
                              .AsNoTracking()
                              .Where(o => o.FldProjectId == projectId);

            if (soort.HasValue)
            {
                query = query.Where(o => o.FldSoort == soort.Value);
            }
            else if (excludeSoort.HasValue)
            {
                query = query.Where(o => o.FldSoort != excludeSoort.Value);
            }

            return await query.ToListAsync();
        }

        partial void OnTblProjectOnderdelenCreated(KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen item);
        partial void OnAfterTblProjectOnderdelenCreated(KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen> CreateTblProjectOnderdelen(KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen tblprojectonderdelen)
        {
            OnTblProjectOnderdelenCreated(tblprojectonderdelen);

            var existingItem = Context.TblProjectOnderdelens
                              .Where(i => i.Id == tblprojectonderdelen.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblProjectOnderdelens.Add(tblprojectonderdelen);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblprojectonderdelen).State = EntityState.Detached;
                throw;
            }

            OnAfterTblProjectOnderdelenCreated(tblprojectonderdelen);

            return tblprojectonderdelen;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen> CancelTblProjectOnderdelenChanges(KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblProjectOnderdelenUpdated(KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen item);
        partial void OnAfterTblProjectOnderdelenUpdated(KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen> UpdateTblProjectOnderdelen(int id, KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen tblprojectonderdelen)
        {
            OnTblProjectOnderdelenUpdated(tblprojectonderdelen);

            var itemToUpdate = Context.TblProjectOnderdelens
                              .Where(i => i.Id == tblprojectonderdelen.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblprojectonderdelen);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblProjectOnderdelenUpdated(tblprojectonderdelen);

            return tblprojectonderdelen;
        }

        partial void OnTblProjectOnderdelenDeleted(KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen item);
        partial void OnAfterTblProjectOnderdelenDeleted(KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblProjectOnderdelen> DeleteTblProjectOnderdelen(int id)
        {
            var itemToDelete = Context.TblProjectOnderdelens
                              .Where(i => i.Id == id)
                              .Include(i => i.TblFactRegels)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblProjectOnderdelenDeleted(itemToDelete);


            Context.TblProjectOnderdelens.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblProjectOnderdelenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblRelatiePartijensToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblrelatiepartijens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblrelatiepartijens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblRelatiePartijensToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblrelatiepartijens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblrelatiepartijens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblRelatiePartijensRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen>> GetTblRelatiePartijens(Query query = null)
        {
            var items = Context.TblRelatiePartijens.AsQueryable();


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

            OnTblRelatiePartijensRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblRelatiePartijenGet(KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen item);
        partial void OnGetTblRelatiePartijenById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen> GetTblRelatiePartijenById(int id)
        {
            var items = Context.TblRelatiePartijens
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetTblRelatiePartijenById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblRelatiePartijenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblRelatiePartijenCreated(KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen item);
        partial void OnAfterTblRelatiePartijenCreated(KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen> CreateTblRelatiePartijen(KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen tblrelatiepartijen)
        {
            OnTblRelatiePartijenCreated(tblrelatiepartijen);

            var existingItem = Context.TblRelatiePartijens
                              .Where(i => i.Id == tblrelatiepartijen.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblRelatiePartijens.Add(tblrelatiepartijen);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblrelatiepartijen).State = EntityState.Detached;
                throw;
            }

            OnAfterTblRelatiePartijenCreated(tblrelatiepartijen);

            return tblrelatiepartijen;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen> CancelTblRelatiePartijenChanges(KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblRelatiePartijenUpdated(KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen item);
        partial void OnAfterTblRelatiePartijenUpdated(KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen> UpdateTblRelatiePartijen(int id, KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen tblrelatiepartijen)
        {
            OnTblRelatiePartijenUpdated(tblrelatiepartijen);

            var itemToUpdate = Context.TblRelatiePartijens
                              .Where(i => i.Id == tblrelatiepartijen.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblrelatiepartijen);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblRelatiePartijenUpdated(tblrelatiepartijen);

            return tblrelatiepartijen;
        }

        partial void OnTblRelatiePartijenDeleted(KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen item);
        partial void OnAfterTblRelatiePartijenDeleted(KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblRelatiePartijen> DeleteTblRelatiePartijen(int id)
        {
            var itemToDelete = Context.TblRelatiePartijens
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblRelatiePartijenDeleted(itemToDelete);


            Context.TblRelatiePartijens.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblRelatiePartijenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblSettingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblsettings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblsettings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblSettingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblsettings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblsettings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblSettingsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblSetting> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblSetting>> GetTblSettings(Query query = null)
        {
            var items = Context.TblSettings.AsQueryable();


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

            OnTblSettingsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportTblSoortProjectsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblsoortprojects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblsoortprojects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblSoortProjectsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblsoortprojects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblsoortprojects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblSoortProjectsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblSoortProject> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblSoortProject>> GetTblSoortProjects(Query query = null)
        {
            var items = Context.TblSoortProjects.AsQueryable();


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

            OnTblSoortProjectsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblSoortProjectGet(KlantBaseWebDemo.Models.KlantBase.TblSoortProject item);
        partial void OnGetTblSoortProjectById(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblSoortProject> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblSoortProject> GetTblSoortProjectById(int id)
        {
            var items = Context.TblSoortProjects
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetTblSoortProjectById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblSoortProjectGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblSoortProjectCreated(KlantBaseWebDemo.Models.KlantBase.TblSoortProject item);
        partial void OnAfterTblSoortProjectCreated(KlantBaseWebDemo.Models.KlantBase.TblSoortProject item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblSoortProject> CreateTblSoortProject(KlantBaseWebDemo.Models.KlantBase.TblSoortProject tblsoortproject)
        {
            OnTblSoortProjectCreated(tblsoortproject);

            var existingItem = Context.TblSoortProjects
                              .Where(i => i.Id == tblsoortproject.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblSoortProjects.Add(tblsoortproject);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblsoortproject).State = EntityState.Detached;
                throw;
            }

            OnAfterTblSoortProjectCreated(tblsoortproject);

            return tblsoortproject;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblSoortProject> CancelTblSoortProjectChanges(KlantBaseWebDemo.Models.KlantBase.TblSoortProject item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblSoortProjectUpdated(KlantBaseWebDemo.Models.KlantBase.TblSoortProject item);
        partial void OnAfterTblSoortProjectUpdated(KlantBaseWebDemo.Models.KlantBase.TblSoortProject item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblSoortProject> UpdateTblSoortProject(int id, KlantBaseWebDemo.Models.KlantBase.TblSoortProject tblsoortproject)
        {
            OnTblSoortProjectUpdated(tblsoortproject);

            var itemToUpdate = Context.TblSoortProjects
                              .Where(i => i.Id == tblsoortproject.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblsoortproject);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblSoortProjectUpdated(tblsoortproject);

            return tblsoortproject;
        }

        partial void OnTblSoortProjectDeleted(KlantBaseWebDemo.Models.KlantBase.TblSoortProject item);
        partial void OnAfterTblSoortProjectDeleted(KlantBaseWebDemo.Models.KlantBase.TblSoortProject item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblSoortProject> DeleteTblSoortProject(int id)
        {
            var itemToDelete = Context.TblSoortProjects
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblSoortProjectDeleted(itemToDelete);


            Context.TblSoortProjects.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblSoortProjectDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblStandaardDocsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblstandaarddocs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblstandaarddocs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblStandaardDocsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/tblstandaarddocs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/tblstandaarddocs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblStandaardDocsRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc>> GetTblStandaardDocs(Query query = null)
        {
            var items = Context.TblStandaardDocs.AsQueryable();


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

            OnTblStandaardDocsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblStandaardDocGet(KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc item);
        partial void OnGetTblStandaardDocByDocId(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc> GetTblStandaardDocByDocId(int docid)
        {
            var items = Context.TblStandaardDocs
                              .AsNoTracking()
                              .Where(i => i.DocId == docid);

 
            OnGetTblStandaardDocByDocId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTblStandaardDocGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblStandaardDocCreated(KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc item);
        partial void OnAfterTblStandaardDocCreated(KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc> CreateTblStandaardDoc(KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc tblstandaarddoc)
        {
            OnTblStandaardDocCreated(tblstandaarddoc);

            var existingItem = Context.TblStandaardDocs
                              .Where(i => i.DocId == tblstandaarddoc.DocId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblStandaardDocs.Add(tblstandaarddoc);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblstandaarddoc).State = EntityState.Detached;
                throw;
            }

            OnAfterTblStandaardDocCreated(tblstandaarddoc);

            return tblstandaarddoc;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc> CancelTblStandaardDocChanges(KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblStandaardDocUpdated(KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc item);
        partial void OnAfterTblStandaardDocUpdated(KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc> UpdateTblStandaardDoc(int docid, KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc tblstandaarddoc)
        {
            OnTblStandaardDocUpdated(tblstandaarddoc);

            var itemToUpdate = Context.TblStandaardDocs
                              .Where(i => i.DocId == tblstandaarddoc.DocId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblstandaarddoc);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblStandaardDocUpdated(tblstandaarddoc);

            return tblstandaarddoc;
        }

        partial void OnTblStandaardDocDeleted(KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc item);
        partial void OnAfterTblStandaardDocDeleted(KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.TblStandaardDoc> DeleteTblStandaardDoc(int docid)
        {
            var itemToDelete = Context.TblStandaardDocs
                              .Where(i => i.DocId == docid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblStandaardDocDeleted(itemToDelete);


            Context.TblStandaardDocs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblStandaardDocDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportWerknemersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/werknemers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/werknemers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportWerknemersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/werknemers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/werknemers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnWerknemersRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Werknemer> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.Werknemer>> GetWerknemers(Query query = null)
        {
            var items = Context.Werknemers.AsQueryable();


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

            OnWerknemersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnWerknemerGet(KlantBaseWebDemo.Models.KlantBase.Werknemer item);
        partial void OnGetWerknemerByWerknId(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.Werknemer> items);


        public async Task<KlantBaseWebDemo.Models.KlantBase.Werknemer> GetWerknemerByWerknId(int werknid)
        {
            var items = Context.Werknemers
                              .AsNoTracking()
                              .Where(i => i.WerknId == werknid);

 
            OnGetWerknemerByWerknId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnWerknemerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnWerknemerCreated(KlantBaseWebDemo.Models.KlantBase.Werknemer item);
        partial void OnAfterWerknemerCreated(KlantBaseWebDemo.Models.KlantBase.Werknemer item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Werknemer> CreateWerknemer(KlantBaseWebDemo.Models.KlantBase.Werknemer werknemer)
        {
            OnWerknemerCreated(werknemer);

            var existingItem = Context.Werknemers
                              .Where(i => i.WerknId == werknemer.WerknId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Werknemers.Add(werknemer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(werknemer).State = EntityState.Detached;
                throw;
            }

            OnAfterWerknemerCreated(werknemer);

            return werknemer;
        }

        public async Task<KlantBaseWebDemo.Models.KlantBase.Werknemer> CancelWerknemerChanges(KlantBaseWebDemo.Models.KlantBase.Werknemer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnWerknemerUpdated(KlantBaseWebDemo.Models.KlantBase.Werknemer item);
        partial void OnAfterWerknemerUpdated(KlantBaseWebDemo.Models.KlantBase.Werknemer item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Werknemer> UpdateWerknemer(int werknid, KlantBaseWebDemo.Models.KlantBase.Werknemer werknemer)
        {
            OnWerknemerUpdated(werknemer);

            var itemToUpdate = Context.Werknemers
                              .Where(i => i.WerknId == werknemer.WerknId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(werknemer);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterWerknemerUpdated(werknemer);

            return werknemer;
        }

        partial void OnWerknemerDeleted(KlantBaseWebDemo.Models.KlantBase.Werknemer item);
        partial void OnAfterWerknemerDeleted(KlantBaseWebDemo.Models.KlantBase.Werknemer item);

        public async Task<KlantBaseWebDemo.Models.KlantBase.Werknemer> DeleteWerknemer(int werknid)
        {
            var itemToDelete = Context.Werknemers
                              .Where(i => i.WerknId == werknid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnWerknemerDeleted(itemToDelete);


            Context.Werknemers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterWerknemerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportVwAankomendeInspectiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/vwaankomendeinspecties/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/vwaankomendeinspecties/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportVwAankomendeInspectiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/vwaankomendeinspecties/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/vwaankomendeinspecties/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnVwAankomendeInspectiesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.VwAankomendeInspecty> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.VwAankomendeInspecty>> GetVwAankomendeInspecties(Query query = null)
        {
            var items = Context.VwAankomendeInspecties.AsQueryable();


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

            OnVwAankomendeInspectiesRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportVwKiwainspectiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/vwkiwainspecties/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/vwkiwainspecties/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportVwKiwainspectiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/klantbase/vwkiwainspecties/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/klantbase/vwkiwainspecties/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnVwKiwainspectiesRead(ref IQueryable<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty> items);

        public async Task<IQueryable<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty>> GetVwKiwainspecties(Query query = null)
        {
            var items = Context.VwKiwainspecties.AsQueryable();


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

            OnVwKiwainspectiesRead(ref items);

            return await Task.FromResult(items);
        }
    }
}