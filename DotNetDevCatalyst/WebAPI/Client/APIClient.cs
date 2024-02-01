



using DevCatalyst.Data.Models;
using DevCatalyst.WebAPI.Models;



namespace DevCatalyst.WebAPI.Client
{
  
    public class APIClient : IAPIClient
    {
    
        protected string URL ;
        protected string Token;
        protected string Controller;

        public APIClient(string _URL,string _Controller, string _Token=""){
            URL = _URL;
            Controller = _Controller;
            Token = _Token;
        }

        public void setToken(string token)
        {
            Token = token;
        }



        public async Task<Response> GetCount(string method= "",GridModel? model = null)
        {
            if (method == "")
                method= "GetCount";
            Response res = await Custom(RequestType.Get, $"{Controller}/{method}", model);
            return res;

        }

        public async Task<Response> GetAll(string method = "", GridModel? model=null)
        {
            if (method == "")
                method = "GetAll";
            Response res = await Custom(RequestType.Get, $"{Controller}/{method}", model);
            return res;

        }



        public async Task<Response> Get( dynamic id,string method = "")
        {

            Response res = await Custom(RequestType.Get, method == "" ? $"{Controller}/{id}" : $"{Controller}/{method}/{id}");
            return res;
        }
        public async Task<Response> Add(dynamic model, string method = "")
        {
            if (method == "")
                method = "Add";
            Response res = await Custom(RequestType.Post, $"{Controller}/{method}", model);
            return res;
        }


        public async Task<Response> Edit(dynamic id,dynamic model, string method = "")
        {

            if (method == "")
                method = "Edit";
            Response res = await Custom(RequestType.Put, $"{Controller}/{method}", model,id);
            return res;
        }

        public async Task<Response> Delete(dynamic id, string method = "")
        {

            if (method == "")
                method = "Delete";
            Response res = await Custom(RequestType.Delete, $"{Controller}/{method}",null,id);
            return res;

        }

        public async Task<Response> Custom(RequestType type, string method = "", GridModel? model = null, dynamic? id = null)
        {

            APIConsumer<dynamic, object> aPIConsumer = new APIConsumer<dynamic, object>(URL, Token);
            Response res = new Response(0,"",null);
            if (type == RequestType.Get)
            {
                res = await aPIConsumer.Get($"{Controller}/{method}", model);
            }
            else if(type == RequestType.Post)
            {
                res = await aPIConsumer.Post($"{Controller}/{method}", model);
            }
            else if (type == RequestType.Put)
            {
                res = await aPIConsumer.Put($"{Controller}/{method}", model);
            }
            else if (type == RequestType.Delete)
            {
                res = await aPIConsumer.Delete($"{Controller}/{method}/{id}");
            }

            return res;
        }



    }


    public enum RequestType 
    { 
        Get =1,
        Post =2,
        Put =3,
        Delete = 4
    }

}
