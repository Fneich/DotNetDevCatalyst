
using DevCatalyst.Data.Models;
using DevCatalyst.WebAPI.Models;

namespace DevCatalyst.WebAPI.Client
{
    public interface IAPIClient
    {

        public void setToken(string token);

        public  Task<Response> GetCount(string method = "", GridModel? model = null);

        public  Task<Response> GetAll(string method = "", GridModel? model = null);



        public Task<Response> Get(dynamic id,string method= "");
        public  Task<Response> Add(dynamic model, string method = "");


        public  Task<Response> Edit(dynamic id, dynamic model, string method = "");

        public  Task<Response> Delete(dynamic id, string method = "");


    }
}
