
using System.Dynamic;
using System.Text.Json.Serialization;


namespace DevCatalyst.Data.Models
{
    public class GridModel
    {
        public static GridModel DefaultGridModel = new GridModel();

        protected GridModel()
        {
            Order = "";
            Size = 10;
            Page = 1;
        }

        [JsonConstructor]
        public GridModel(SearchWhereStatment SearchStatment, string Order="", int Size = 10, int Page = 1)
        {
            ParamsValues = new ExpandoObject();
            this.Order = Order;
            this.SearchStatment = SearchStatment;
            this.Size = Size;
            this.Page = Page;
            this.Query = "";
            if (SearchStatment != null)
            {
                SearchWhereClause? searchWhereClause = SearchStatment.SearchWhereClause(ParamsValues);
                Query = searchWhereClause == null ? string.Empty: searchWhereClause.Clause;
            }

        }


        public  SearchWhereStatment? SearchStatment { get; set; } 

    
        public string Query { get; set; } = string.Empty;

   
        public string Order { get; set; } = string.Empty;


        public int Page { get; set; } 

        public int Size { get; set; }

        [JsonIgnore]
        public ExpandoObject ParamsValues { get; set; }= new ExpandoObject();

    }


    public class SearchWhereClause {

        private string clause = string.Empty;

        public string Clause =>  clause;

        public SearchWhereClause Push()
        {
            clause += " ( ";
            return this;
        }

        public SearchWhereClause Pop()
        {
            clause += " ) ";
            return this;
        }

        public SearchWhereClause And()
        {
            clause += " And ";
            return this;
        }

        public SearchWhereClause Or()
        {
            clause += " Or ";
            return this;
        }



        public SearchWhereClause Statment(string col,string op,string val, ExpandoObject ParamsValues)
        {
            clause += $" {col} {op} @{col} ";
            var dynamicID = ParamsValues as IDictionary<string, Object>;
            dynamicID.Add(col, val);
            return this;
        }

        public SearchWhereClause Concat(SearchWhereClause _clause)
        {
            clause += $"  {_clause.Clause} ";
            return this;
        }

    }



    public class SearchWhereStatment
    {
        public string? Column { get; set; }
        public string? Operator { get; set; }
        public string? Value { get; set; }
        public SearchWhereStatment? FirstStatment { get; set; }
        public Linkage? AndOr { get; set; }
        public SearchWhereStatment? SecondStatment { get; set; }


        public SearchWhereClause? SearchWhereClause(ExpandoObject ParamsValues)
        { 
           
                if (FirstStatment == null && Column != null && Operator != null && Value != null)
                {
                    return new SearchWhereClause().Push().Statment(Column,Operator, Value, ParamsValues).Pop();
                }
               
                else if(FirstStatment != null && SecondStatment != null && AndOr != null)
                {
                    SearchWhereClause? _FirstStatmentSearchWhereClause = FirstStatment.SearchWhereClause(ParamsValues);
                    SearchWhereClause? _SecondStatmentSearchWhereClause = SecondStatment.SearchWhereClause(ParamsValues);
                if (_FirstStatmentSearchWhereClause != null && _SecondStatmentSearchWhereClause != null && AndOr == Linkage.AND)
                    {
                        return _FirstStatmentSearchWhereClause.And().Concat(_SecondStatmentSearchWhereClause);
                    }
                    else if (_FirstStatmentSearchWhereClause != null && _SecondStatmentSearchWhereClause != null && AndOr == Linkage.OR)
                    {
                        return _FirstStatmentSearchWhereClause.Or().Concat(_SecondStatmentSearchWhereClause);
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            
        }

        [JsonConstructor]
        public SearchWhereStatment(string? Column, string? Operator, string? Value, SearchWhereStatment? FirstStatment, Linkage? AndOr, SearchWhereStatment? SecondStatment) //, ExpandoObject _paramsValues)
        {
            this.Column = Column;
            this.Operator = Operator;
            this.Value = Value;
            this.FirstStatment = FirstStatment;
            this.AndOr = AndOr;
            this.SecondStatment = SecondStatment;
        }

        //[JsonConstructor]
        //public SearchWhereStatment(SearchWhereStatment FirstStatment, Linkage AndOr, SearchWhereStatment SecondStatment)//, ExpandoObject _paramsValues)
        //{
        //    this.Column = null;
        //    this.Operator = null;
        //    this.Value = null;
        //    this.FirstStatment = FirstStatment;
        //    this.AndOr = AndOr;
        //    this.SecondStatment = SecondStatment;
        //}


    }

    public enum Linkage {AND=1, OR=2}



}
