
using DevCatalyst.Data.Models;


namespace DevCatalyst.Data.Repositories
{
    public  interface IBaseRepository<T> where T : IBaseModel
    {
        protected static string? _ConnectionString;
        public static string? ConnectionString { get { return _ConnectionString; } }

        public string getIntegerID_Column();
        public string getGUIDID_Column();
        public string getID_Columns();
        public string getOrderBy_Columns();
        public string getKey_Column();
        public string getStatus_Column();
        public string getDescription_Column();

        public KeyColumnType getKey_Column_Type(); 
        public T? GetByID(dynamic id);

        public Guid? Add(T model);
        public Guid? Save(T model);

        public void Update(T model);

        public void Delete(dynamic id);

        public void Remove(dynamic id);


        public int GetAllCount(List<Status>? status = null, bool excluded = true);

        public  int GetWhereCount(string where, dynamic parameters, List<Status>? status = null, bool excluded = true);


        public  IEnumerable<T> GetAll(string orderBy = "", List<Status>? status = null, bool excluded = true);

        public  IEnumerable<T> GetWhere(string where, dynamic parameters, string orderBy = "", List<Status>? status = null, bool excluded = true);

        public  IEnumerable<T> GetChunk(string where, dynamic parameters, int size = 1000, int page = 1, string orderBy = "", List<Status>? status = null, bool excluded = true);

        public IEnumerable<T> GetAllChunk(int size = 1000, int page = 1, string orderBy = "", List<Status>? status = null, bool excluded = true);

        #region Dapper access

        public int Execute(string sql, dynamic? param) ;

        public int ExecuteScaler(string sql, dynamic? param );

 
        #endregion
    }
}

