using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JCore
{
    public class RepositoryFromList<T> : IRepository<T>
    {
        List<T> _items;
        readonly Func<IList<T>,T,string> _idFilter;

        public RepositoryFromList(IEnumerable<T> items) : this(items.ToList(), CreateIDFromIndex) { }
        public RepositoryFromList() : this(new List<T>(),CreateIDFromIndex) { }

        static string CreateIDFromIndex(IList<T> all,T current) => (all.IndexOf(current)+1).ToString();
        
        public RepositoryFromList(Func<IList<T>,T, string> filter) : this(new List<T>(), filter){}
        public RepositoryFromList(List<T> list,Func<IList<T>,T,string> idFilter)
        {
            _items = list;
            _idFilter = idFilter;
        }
        
        public void SetList(List<T> items) => _items = items;
        public void AddRange(params T[] items) => _items.AddRange(items);

        public static implicit operator RepositoryFromList<T>(List<T> items) => new RepositoryFromList<T>(items);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T GetById(string key) => 
            _items.FirstOrDefault(GetItemMatchingID(key));

        Func<T, bool> GetItemMatchingID(string key)
        {
            if (_idFilter == null) return x => true;
            return x => _idFilter.Invoke(_items,x).Equals(key);
        }
        
        IEnumerable<T> GetAll() => _items;

        public void Update(string key, T item) =>
            TryOperateOnValidId(key, index =>
            {
                _items[index] = item;
            });

        void TryOperateOnValidId(string key,Action<int> action)
        {
            if (_idFilter == null) return;
            var matching = ((IReadonlyRepository<T>)this).GetById(key);
            if (matching == null) return;
            var indexOfMatching = _items.IndexOf(matching);
            if(indexOfMatching < 0 || indexOfMatching > _items.Count) return;
            action?.Invoke(indexOfMatching);
        }

        public void Delete(string key) => TryOperateOnValidId(key, index => 
            _items.RemoveAt(index));

        public void Add(T item) => _items.Add(item);
        
        IEnumerable<T> IReadonlyRepository<T>.GetAll() => _items;
    }
}