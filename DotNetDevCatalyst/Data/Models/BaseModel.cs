

namespace DevCatalyst.Data.Models
{
    public class BaseModel : IBaseModel
    {

       // public static string BaseColumns = $"{IAbstractModel.IntegerID_Column},{IAbstractModel.GUIDID_Column},{IAbstractModel.Status_Column},{IAbstractModel.Description_Column}";
     
        // All the below fields are not necessary used

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
    }



}
