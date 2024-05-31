using HotPotProject.Models;

namespace HotPotProject.Interfaces
{
    public interface IRepository<K, S, T>
    {
        public Task<T> Add(T item);
        public Task<T> Update(T item);
        public Task<T> GetAsync(K key);
       // public Task<T[]> GetAsyncArray(K[] key);

        public Task<List<T>> GetAsync();
        public Task<T> Delete(K key);
        public Task<T> GetAsync(S key);
        
    }
}
