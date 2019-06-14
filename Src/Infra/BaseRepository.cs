using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infra
{
    public class BaseRepository<T> where T : IEntity
    {
        private readonly IConnectionMultiplexer _redisConnection;

        /// <summary>
        /// The Namespace is the first part of any key created by this Repository, e.g. "location" or "employee"
        /// </summary>
        private readonly string _namespace;

        public BaseRepository(IConnectionMultiplexer redis, string nameSpace)
        {
            _redisConnection = redis;
            _namespace = nameSpace;
        }

        public async Task<T> GetById(Guid id, CancellationToken tc = default)
        {
            return await Get<T>(id.ToString(), tc);
        }

        public async Task<TD> Get<TD>(string keySuffix, CancellationToken tc = default)
        {
            var key = MakeKey(keySuffix);
            var database = _redisConnection.GetDatabase();
            var serializedObject = await database.StringGetAsync(key);
            return serializedObject.IsNullOrEmpty ? 
                default : 
                JsonConvert.DeserializeObject<TD>(serializedObject.ToString());
        }

        public async Task<List<T>> GetMultiple(Guid[] ids, CancellationToken tc = default)
        {
            var database = _redisConnection.GetDatabase();
            var keys = new List<RedisKey>();
            foreach (var id in ids)
            {
                keys.Add(MakeKey(id));
            }
            var serializedItems = await database.StringGetAsync(keys.ToArray(), CommandFlags.None);
            return serializedItems.Select(item => JsonConvert.DeserializeObject<T>(item.ToString())).ToList();
        }

        public async Task<bool> Exists(Guid id, CancellationToken tc = default)
        {
            return await Exists(id.ToString(), tc);
        }

        public async Task<bool> Exists(string keySuffix, CancellationToken tc = default)
        {
            var key = MakeKey(keySuffix);
            var database = _redisConnection.GetDatabase();
            var x = await database. KeyExistsAsync(key);
            return x;
        }

        public async Task Save(Guid id, object entity, CancellationToken tc = default)
        {
            await Save(id.ToString(), entity, tc);
        }

        public async Task Save(string keySuffix, object entity, CancellationToken tc = default)
        {
            var key = MakeKey(keySuffix);
            var database = _redisConnection.GetDatabase();
            await database.StringSetAsync(MakeKey(key), JsonConvert.SerializeObject(entity));
        }

        private string MakeKey(Guid id)
        {
            return MakeKey(id.ToString());
        }

        private string MakeKey(string keySuffix)
        {
            if (!keySuffix.StartsWith(_namespace + ":"))
            {
                return _namespace + ":" + keySuffix;
            }
            else return keySuffix; //Key is already prefixed with namespace
        }

        public async Task Save(T item, CancellationToken tc = default)
        {
            await Save(item.Id, item, tc);

            await MergeIntoAllCollection(item, tc);
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken tc = default)
        {
            return await Get<List<T>>("all", tc) ?? new List<T>();
        }

        private async Task MergeIntoAllCollection(T employee, CancellationToken tc = default)
        {
            var allEmployees = new List<T>();
            if (await Exists("all", tc))
            {
                allEmployees = await Get<List<T>>("all", tc);
            }

            //If the district already exists in the ALL collection, remove that entry
            if (allEmployees.Any(x => x.Id == employee.Id))
            {
                allEmployees.Remove(allEmployees.First(x => x.Id == employee.Id));
            }

            //Add the modified district to the ALL collection
            allEmployees.Add(employee);

            await Save("all", allEmployees, tc);
        }
    }
}