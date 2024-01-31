

namespace DevCatalyst.Data.Models
{
    public interface IBaseModel
    {

        //public static string IntegerID_Column = "ID";
        //public static string GUIDID_Column = "RECORD_ID";
        //public static string ID_Columns = $"{IntegerID_Column}";
        //public static string OrderBy_Columns = "ID";
        //public static string Key_Column = "RECORD_ID";
        //public static string Status_Column = "STATUS";
        //public static string Description_Column = "DESCRIPTION";


        #region IDs

        //integer id 
        public int? ID { get; set; }

        //unique identifier id
        public Guid? RECORD_ID { get; set; }

        #endregion

        #region Creation_Fields
        //date time when the record is insert
        public DateTime? CREATION_DATE { get; set; }

        //text represent the username of the creator of the record
        public string? CREATOR { get; set; }

        //integer id  of the creator of the record
        public int? CREATOR_ID { get; set; }

        //unique identifier id of the creator of the record
        public Guid? CREATOR_RECORD_ID { get; set; }
        #endregion

        #region Last_Edit_Fields

        //date time when the record is updated or edited or deleted
        public DateTime? LAST_EDIT_DATE { get; set; }

        //text represent the username of the editor of the record
        public string? EDITOR { get; set; }

        //integer id  of the editor of the record
        public int? EDITOR_ID { get; set; }

        //unique identifier id of the editor of the record
        public Guid? EDITOR_RECORD_ID { get; set; }

        #endregion

        public Status? STATUS { get; set; }

        public string? DESCRIPTION { get; set; }

        public static List<Status> AllStatus
        {
            get
            {
                return new List<Status>() { Status.NEW, Status.UPDATED,Status.DISABLED,Status.DELETED };
            }
        }

        public static List<Status> ActiveStatus {
            get {
                return new List<Status>(){ Status.NEW,Status.UPDATED}; 
            } 
        }

        public static List<Status> InActiveStatus
        {
            get
            {
                return new List<Status>() { Status.DISABLED, Status.DELETED };
            }
        }

        public static List<Status> DeletedStatus
        {
            get
            {
                return new List<Status>() {  Status.DELETED };
            }
        }

        public static List<Status> DisabledStatus
        {
            get
            {
                return new List<Status>() { Status.DELETED };
            }
        }

    }


    public enum Status
    {
        NOT_SET = 0,
        NEW = 1,
        UPDATED = 2,
        DISABLED = -1,
        DELETED = -99
    }

 
}
