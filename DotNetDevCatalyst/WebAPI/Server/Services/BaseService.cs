

using DevCatalyst.Data.Models;
using DevCatalyst.Data.Repositories;



namespace DevCatalyst.WebAPI.Server.Services
{

    public class BaseService<T, M, R> : IService<T, M> where T : IBaseModel where M : GridModel where R : IBaseRepository<T>
    {
        private R _Repository;

        private  List<Status> statuses = new List<Status>();


        public BaseService(R repository, List<Status> statuses)
        {

            _Repository = repository;
            this.statuses = statuses;
        }


        public virtual T Add(T model)
        {
            _Repository.Add(model);
            return model;
        }


        public virtual dynamic? Delete(dynamic id)
        {
            BaseModel item = _Repository.GetByID(id);
            if (item == null)
                return null;
            _Repository.Delete(id);
            return id;
        }

        public virtual T? Edit(T model)
        {
            T? item = default ;
            if (model.ID != null && _Repository.getKey_Column_Type() == KeyColumnType.Integer)
            {
                item =  _Repository.GetByID(model.ID);
                if (item != null)
                {
                    _Repository.Save(model);
                    if (item.ID != null)
                    {
                        item = _Repository.GetByID(item.ID);
                    }
                }
            }
            else if(model.RECORD_ID != null && _Repository.getKey_Column_Type() == KeyColumnType.Uniqueidentifier)
            {
                item = _Repository.GetByID(model.RECORD_ID);
                if (item != null)
                {
                    _Repository.Save(model);
                    if (item.RECORD_ID != null)
                    {
                        item = _Repository.GetByID(item.RECORD_ID);
                    }
                }
            }

            return item;
        }


        public IEnumerable<T> GetAll(M? gridmodel = null)
        {
            if (gridmodel == null)
            {
                return _Repository.GetAll($"{_Repository.getOrderBy_Columns()}",statuses);
            }
            return _Repository.GetWhere(gridmodel.Query, gridmodel.ParamsValues, $"{_Repository.getOrderBy_Columns()}", statuses);
        }

        public IEnumerable<T> Get(M gridmodel)
        {
            if (gridmodel.Query == null)
            {
                return _Repository.GetAllChunk(gridmodel.Size,gridmodel.Page,$"{_Repository.getOrderBy_Columns()}", statuses);
            }
            else
            {
                return _Repository.GetChunk(gridmodel.Query, gridmodel.ParamsValues, gridmodel.Size, gridmodel.Page, $"{_Repository.getOrderBy_Columns()}", statuses);
            }

        }

        public long GetCount(M? gridmodel = null)
        {
            if (gridmodel == null)
            {
                return _Repository.GetAllCount(statuses);
            }
            else
            {
                return _Repository.GetWhereCount(gridmodel.Query,gridmodel.ParamsValues, statuses);
            }

        }

        public T GetById(dynamic id)
        {
            var Model = _Repository.GetByID(id);
            return Model;
        }



 
    }


}
