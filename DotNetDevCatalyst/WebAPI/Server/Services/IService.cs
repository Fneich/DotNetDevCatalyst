

using DevCatalyst.Data.Models;


namespace DevCatalyst.WebAPI.Server.Services
{
    interface IService<T, M> where M : GridModel
    {

        public IEnumerable<T> Get(M gridmodel);

        public IEnumerable<T> GetAll(M? gridmodel);

        public long GetCount(M? gridmodel);

        public T GetById(dynamic id);


        public T Add(T model);


        public T? Edit( T model);



        public dynamic? Delete(dynamic id);



    }
}
