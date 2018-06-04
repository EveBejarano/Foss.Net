using FunTourDataLayer;
using FunTourDataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FunTourDataLayer.Services
{
    public class Consumer<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        internal DbSet<TEntity> dbSet;
        internal IEntityToReload IEntity;

        public Consumer(ApplicationDbContext context, IEntityToReload IEntity)
        {
            this._context = context;
            this.dbSet = context.Set<TEntity>();
            this.IEntity = IEntity;
        }

        public async Task<bool> ReLoadEntities(string URL, string _parameters) 
        {

            try
            {
                var EntityList = await GetEntitiesFromAPIReturnListElements(URL, _parameters);

                var Result = SaveListEntitiesToDataBase(EntityList);

                return Result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        
        public async Task<IEnumerable<Object>> GetEntitiesFromAPIReturnListElements(string URL, string _parameters)
        {
            var Url = URL;

            string data;

            var _entity = IEntity.NewEntity(_parameters);

            string Json = JsonConvert.SerializeObject(_entity);
            var request = new StringContent(Json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.PostAsync(Url, request))
                {
                    using (HttpContent content = res.Content)
                    {
                        data = await content.ReadAsStringAsync();
                    }
                }
            }

            return IEntity.DesearializeJson(data);
        }

        public bool SaveListEntitiesToDataBase( IEnumerable<Object> EntityList)
        {
            if (EntityList.First().GetType() != IEntity.GetType())
            {
                return false;
            }

            try
            {
                _context.Database.ExecuteSqlCommand("TRUNCATE TABLE ", dbSet);

                foreach (TEntity item in EntityList)
                {
                    dbSet.Add(item);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
