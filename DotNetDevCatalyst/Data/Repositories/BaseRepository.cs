
using Dapper;
using DevCatalyst.Data.Models;
using DevCatalyst.Data.Extensions;
using DevCatalyst.Data.Database;
using System.Dynamic;
using Microsoft.IdentityModel.Tokens;

namespace DevCatalyst.Data.Repositories
{



    public class BaseRepository<T> : IBaseRepository<T> where T : IBaseModel
    {


        public string BaseColumns
        {
            get
            {
                string baseColumns = $"{IntegerID_Column},{GUIDID_Column},{Status_Column},{Description_Column}";
                return Editor_Tracking_Columns == null ? baseColumns : $"{baseColumns},{Editor_Tracking_Columns}";
            }
        }

        public string IntegerID_Column { get; set; } 
        public string GUIDID_Column { get; set; } 
        public string ID_Columns => $"{IntegerID_Column},{GUIDID_Column}";
        public string OrderBy_Columns { get; set; }
        public string Key_Column { get; set; } 
        public KeyColumnType Key_Column_Type { get; set; }
        public string Status_Column { get; set; }
        public string Description_Column { get; set; }

        public string? Creator_Column { get; set; } = null;

        public string? Creation_Date_Column { get; set; } = null;

        public string? Last_Editor_Column { get; set; } = null;

        public string? Last_Edit_Date_Column { get; set; } = null;

      //  public string Creation_Columns => $"{Creator_Column},{Creation_Date_Column}";

        public string? Creation_Columns
        {
            get { 
                if (Creator_Column == null && Creation_Date_Column == null) 
                {
                    return null;
                }
                else if (Creator_Column != null && Creation_Date_Column == null)
                {
                    return Creator_Column;
                }
                else if (Creator_Column == null && Creation_Date_Column != null)
                {
                    return Creation_Date_Column;
                }
                else 
                {
                    return $"{Creator_Column},{Creation_Date_Column}";
                }
            }
        }

        public string? Last_Edit_Columns
        {
            get
            {
                if (Last_Editor_Column == null && Last_Edit_Date_Column == null)
                {
                    return null;
                }
                else if (Last_Editor_Column != null && Last_Edit_Date_Column == null)
                {
                    return Last_Editor_Column;
                }
                else if (Last_Editor_Column == null && Last_Edit_Date_Column != null)
                {
                    return Last_Edit_Date_Column;
                }
                else
                {
                    return $"{Last_Editor_Column},{Last_Edit_Date_Column}";
                }
            }
        }


        //public string Last_Edit_Columns => $"{Last_Editor_Column},{Last_Edit_Date_Column}";


        public string? Editor_Tracking_Columns
        {
            get
            {
                if (Creation_Columns == null && Last_Edit_Columns == null)
                {
                    return null;
                }
                else if (Creation_Columns != null && Last_Edit_Columns == null)
                {
                    return Creation_Columns;
                }
                else if (Creation_Columns == null && Last_Edit_Columns != null)
                {
                    return Last_Edit_Columns;
                }
                else
                {
                    return $"{Creation_Columns},{Last_Edit_Columns}";
                }
            }
        }

       // public string Editor_Tracking_Columns => $"{Creation_Columns},{Last_Edit_Columns}";

        protected string SqlSelect = string.Empty;
        protected string SqlInsert = string.Empty;
        protected string SqlUpdate = string.Empty;
        protected string SqlDelete = string.Empty;
        protected string TableName = string.Empty;
        protected string Columns = string.Empty;


        public string getIntegerID_Column() { return IntegerID_Column; }
        public string getGUIDID_Column() { return GUIDID_Column; }
        public string getID_Columns() { return ID_Columns; }
        public string getOrderBy_Columns() { return OrderBy_Columns; }
        public string getKey_Column() { return Key_Column; }

        public KeyColumnType getKey_Column_Type() { return Key_Column_Type; }

        public string? getCreator_Column() { return Creator_Column; }

        public string? getCreation_Date_Column() { return Creation_Date_Column; }

        public string? getLast_Editor_Column() { return Last_Editor_Column; } 

        public string? getLast_Edit_Date_Column() { return Last_Edit_Date_Column; } 

        public string getStatus_Column() { return Status_Column; }

        public string getDescription_Column() { return Description_Column; }


        public BaseRepository(AbstractColumns abstractColumns, KeyColumnType key_Column_Type)
        {
            IntegerID_Column = abstractColumns.IntegerID_Column;
            GUIDID_Column = abstractColumns.GUIDID_Column;
            OrderBy_Columns = abstractColumns.OrderBy_Columns;
            Key_Column = abstractColumns.Key_Column;
            Creator_Column = abstractColumns.Creator_Column;
            Creation_Date_Column = abstractColumns.Creation_Date_Column;
            Last_Editor_Column = abstractColumns.Last_Editor_Column;
            Last_Edit_Date_Column = abstractColumns.Last_Edit_Date_Column;
            Status_Column = abstractColumns.Status_Column;
            Description_Column = abstractColumns.Description_Column;
            Key_Column_Type = key_Column_Type;
        }


        protected void Init(string connectionString, string table, string columns)
        {

            if (IBaseRepository<T>._ConnectionString == null || !IBaseRepository<T>._ConnectionString.IsNotNullOrEmpty())
            {
                IBaseRepository<T>._ConnectionString = connectionString;
            }

            //if (string.IsNullOrEmpty(excludeUpdate))
            //    excludeUpdate = "," + excludeUpdate;
            TableName = table;
            Columns = $"{BaseColumns},{columns}";
            SqlSelect = $"SELECT {Columns.AddBraces()} FROM [{TableName}]";
            SqlInsert = Columns.GenerateInsertQuery(TableName, IntegerID_Column);
            SqlUpdate = Columns.GenerateUpdateQuery(TableName,Key_Column, ID_Columns);
            SqlDelete = $"UPDATE {TableName} SET {Status_Column}={(int)Status.DELETED} WHERE {Key_Column}=@{Key_Column}; ";

        }


        public T? GetByID(dynamic id)
        {
            var dynamicID = new ExpandoObject() as IDictionary<string, Object>;
            dynamicID.Add(Key_Column, id);
            return Query<T>($"{SqlSelect} WHERE {Key_Column} = @{Key_Column} ", dynamicID).FirstOrDefault();
        }

        protected void Insert(T model)
        {
            //if (SqlInsert != null && SqlInsert.IsNotNullOrEmpty())
                Execute(SqlInsert, model);
        }

        public void Update(T model)
        {
            //if (SqlUpdate != null && SqlUpdate.IsNotNullOrEmpty())   
            Execute(SqlUpdate, model);
        }

        public Guid? Add(T model)
        {
            model.STATUS = Status.NEW;
            model.RECORD_ID = Guid.NewGuid();
            model.CREATION_DATE = DateTime.Now;
            model.LAST_EDIT_DATE = DateTime.Now;
            Insert(model);
            return model.RECORD_ID;
        }

        public Guid? Save(T model)
        {
            model.STATUS = Status.UPDATED;
            model.LAST_EDIT_DATE = DateTime.Now;
            Update(model);
            return model.RECORD_ID;
        }

        public Guid? Disable(T model)
        {
            model.STATUS = Status.DISABLED;
            model.LAST_EDIT_DATE = DateTime.Now;
            Update(model);
            return model.RECORD_ID;
        }


        public void Delete(dynamic id)
        {
            var dynamicID = new ExpandoObject() as IDictionary<string, Object>;
            dynamicID.Add(Key_Column, id);
            if (SqlDelete != null && SqlDelete.IsNotNullOrEmpty())
                Execute(SqlDelete,  dynamicID );
        }



        public void Remove(dynamic id)
        {
            var dynamicID = new ExpandoObject() as IDictionary<string, Object>;
            dynamicID.Add(Key_Column, id);
            Execute($"DELETE FROM [{TableName}]  WHERE {Key_Column}=@{Key_Column}", dynamicID);
        }


        protected string StatusWhereQuery(string where, List<Status>? status = null, bool excluded = true)
        {
            status = status == null ? IBaseModel.DeletedStatus : status;
            if (where != string.Empty)
            {
                where = $"({where}) AND";
            }
            if (status != null && status.Count > 0)
            {
                string op = excluded ? "!=" : "=";
                string conc = excluded ? "AND" : "OR";
                foreach (var s in status)
                {
                    where += $" {Status_Column} {op} {(int)s} {conc}";
                }

            }
            if (where.EndsWith("AND"))
            {
                where = where.Substring(0, where.Length - 3);
            }
            if (where.EndsWith("OR"))
            {
                where = where.Substring(0, where.Length - 2);
            }
            return where;
        }


        public int GetCount(string where, dynamic? parameters)
        {
            where = where.IsNullOrEmpty() ? "" : $"WHERE {where}";
            return ExecuteScaler($"Select count(*) from [{TableName}]  {where}", parameters);
        }

        public int GetAllCount(List<Status>? status = null, bool excluded = true)
        {
            string where = StatusWhereQuery("", status, excluded);
            return GetCount(where, null);
        }

        public virtual int GetWhereCount(string where, dynamic parameters, List<Status>? status = null, bool excluded = true)
        {
            where = StatusWhereQuery(where, status, excluded);
            return GetCount(where, parameters);
        }


        public virtual IEnumerable<T> Get(string where, dynamic? parameters, string orderBy = "")
        {
            orderBy = orderBy.IsNullOrEmpty() ? $" {OrderBy_Columns}" : orderBy;
            where = where.IsNullOrEmpty() ? "" : $"WHERE {where}";
            var queryString = $"{SqlSelect} {where} ORDER BY {orderBy} ";
            return Query<T>(queryString, parameters);
        }


        public virtual IEnumerable<T> GetAll(string orderBy = "", List<Status>? status = null, bool excluded = true)
        {
            string where = StatusWhereQuery("", status, excluded);
            return Get(where, null, orderBy);
        }

        public virtual IEnumerable<T> GetWhere(string where, dynamic parameters, string orderBy = "", List<Status>? status = null, bool excluded = true)
        {
            where = StatusWhereQuery(where, status, excluded);
            return Get(where, parameters, orderBy);

        }


        public virtual IEnumerable<T> GetChunk(string where, dynamic parameters, int size = 1000, int page = 1, string orderBy = "", List<Status>? status = null, bool excluded = true)
        {
            where = StatusWhereQuery(where, status, excluded);
            var queryString = $"{SqlSelect} WHERE {where} ORDER BY {orderBy} OFFSET {(page - 1) * size} ROWS FETCH NEXT {size} ROWS ONLY";
            return Query<T>(queryString, parameters);
        }

        public virtual IEnumerable<T> GetAllChunk( int size = 1000, int page = 1, string orderBy = "", List<Status>? status = null, bool excluded = true)
        {
            var where = StatusWhereQuery("", status, excluded);
            var queryString = $"{SqlSelect} WHERE {where} ORDER BY {orderBy} OFFSET {(page - 1) * size} ROWS FETCH NEXT {size} ROWS ONLY";
            return Query<T>(queryString, null);
        }

        #region Dapper access

        public int Execute(string sql, dynamic? param = null)
        {
            return ConnectionManager.GetConnection(IBaseRepository<T>.ConnectionString).Execute(sql, param);
        }
        public int ExecuteScaler(string sql, dynamic? param = null)
        {
            return ConnectionManager.GetConnection(IBaseRepository<T>.ConnectionString).ExecuteScaler(sql, param);
        }

        private IEnumerable<T> Query<T1>(string queryString, dynamic? parameters=null)
        {
            var connection = ConnectionManager.GetConnection(IBaseRepository<T>.ConnectionString);
            return connection.Query<T1>(queryString, parameters);
        }



        #endregion




    }

    public class BaseColumns : AbstractColumns
    {

        public string CREATION_DATE_Column { get; set; }
        public string CREATOR_Column { get; set; }
        public string CREATOR_ID_Column { get; set; }
        public string CREATOR_RECORD_ID_Column { get; set; }
        public string LAST_EDIT_DATE_Column { get; set; }
        public string EDITOR_Column { get; set; }
        public string EDITOR_ID_Column { get; set; }
        public string EDITOR_RECORD_ID_Column { get; set; }


        public BaseColumns(string _IntegerID_Column, string _GUIDID_Column, string _OrderBy_Columns, string _Key_Column, string _Status_Column, string _Description_Column, string _CREATION_DATE_Column, string _CREATOR_Column, string _CREATOR_ID_Column, string _CREATOR_RECORD_ID_Column, string _LAST_EDIT_DATE_Column, string _EDITOR_Column, string _EDITOR_ID_Column, string _EDITOR_RECORD_ID_Column)
        : base(_IntegerID_Column, _GUIDID_Column, _OrderBy_Columns, _Key_Column, _Status_Column, _Description_Column)
        {
            CREATION_DATE_Column = _CREATION_DATE_Column;
            CREATOR_Column = _CREATOR_Column;
            CREATOR_ID_Column = _CREATOR_ID_Column;
            CREATOR_RECORD_ID_Column = _CREATOR_RECORD_ID_Column;
            LAST_EDIT_DATE_Column = _LAST_EDIT_DATE_Column;
            EDITOR_Column = _EDITOR_Column;
            EDITOR_ID_Column = _EDITOR_ID_Column;
            EDITOR_RECORD_ID_Column = _EDITOR_RECORD_ID_Column;
        }
    }

    public class AbstractColumns
    {

        public string IntegerID_Column { get; set; }
        public string GUIDID_Column { get; set; }
        public string OrderBy_Columns { get; set; }
        public string Key_Column { get; set; }
        public string Status_Column { get; set; }
        public string Description_Column { get; set; }

        public string? Creator_Column { get; set; } = null;

        public string? Creation_Date_Column { get; set; } = null;

        public string? Last_Editor_Column { get; set; } = null;

        public string? Last_Edit_Date_Column { get; set; } = null;


        public AbstractColumns(string _IntegerID_Column, string _GUIDID_Column, string _OrderBy_Columns, string _Key_Column, string _Status_Column, string _Description_Column,string? _Creator_Column=null, string? _Creation_Date_Column = null, string? _Last_Editor_Column = null, string? _Last_Edit_Date_Column = null)
        {
            IntegerID_Column = _IntegerID_Column;
            GUIDID_Column = _GUIDID_Column;
            OrderBy_Columns = _OrderBy_Columns;
            Key_Column = _Key_Column;
            Status_Column = _Status_Column;
            Description_Column = _Description_Column;
            Creator_Column = _Creator_Column;
            Creation_Date_Column= _Creation_Date_Column;
            Last_Editor_Column= _Last_Editor_Column;
            Last_Edit_Date_Column = _Last_Edit_Date_Column;
        }
    }

    public enum KeyColumnType 
    {
    Integer =1, Uniqueidentifier=2
    }


}
