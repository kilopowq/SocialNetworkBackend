using System.Collections.Generic;

namespace DAL
{
        public interface IRepository<T>
         where T : class
        {
            IEnumerable<T> GetList(); 
            T Get(int id); 
            void Add(T item); 
            void Update(T item);
            void Delete(int id);
            void Save();
            void Close();
        }
}
