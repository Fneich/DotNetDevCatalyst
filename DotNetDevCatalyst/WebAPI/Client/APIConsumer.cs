

using DevCatalyst.WebAPI.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;


namespace DevCatalyst.WebAPI.Client
{
 

    public class APIConsumer<T,U>
    {

        private string _BaseAddress { get; set; }
       
        private string _Token { get; set; }

        private readonly HttpClient _http ;

        public  APIConsumer(string url,string token="")
        {
            _BaseAddress = url;
            _Token = token;
            _http = new HttpClient();
            _http.BaseAddress = new Uri(_BaseAddress);
            if (_Token != "")
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Token);
            }
            
        }

        public void setToken(string token)
        {
            _Token = token;
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Token);
        }

        public async Task<Response> Get(string requestUrl, U? content)
        {
            try
            {


                HttpResponseMessage responseTask;
                if (content==null)
                {

                    responseTask = await _http.GetAsync(requestUrl);
                }
                else
                {
                    string serializeObject = JsonConvert.SerializeObject(content);
                   responseTask = await _http.PostAsync(requestUrl , new StringContent(serializeObject, Encoding.UTF8, "application/json"));
                }
                
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsStringAsync();
                    T deserializeObject = JsonConvert.DeserializeObject<T>(readTask.Result);
                    return new Response((int)responseTask.StatusCode,"", deserializeObject);
                }
                else
                {
                    return new Response((int)responseTask.StatusCode, "",null);
                }
        }
            catch (Exception)
            {
                return new Response((int)HttpStatusCode.InternalServerError, "Internal Error", null);
            }

}

        public async Task<Response> GetWithoutModel(string requestUrl)
        {
            try
            {

                    HttpResponseMessage responseTask;             
                    responseTask = await _http.GetAsync(requestUrl);


                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsStringAsync();
                    T deserializeObject = JsonConvert.DeserializeObject<T>(readTask.Result);
                    return new Response((int)responseTask.StatusCode,"", deserializeObject);
                }
                else
                {
                    return new Response((int)responseTask.StatusCode, "",null);
                }
            }
            catch (Exception)
            {
                return new Response((int)HttpStatusCode.InternalServerError, "Internal Error", null);
            }

        }

        public async Task<Response> GetWithoutModelAndResponse(string requestUrl)
        {
            try
            {

                HttpResponseMessage responseTask;
                responseTask = await _http.GetAsync(requestUrl);


                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsStringAsync();
                    return new Response((int)responseTask.StatusCode, "", readTask.Result);
                }
                else
                {
                    return new Response((int)responseTask.StatusCode,"", null);
                }
            }
            catch (Exception)
            {
                return new Response((int)HttpStatusCode.InternalServerError, "Internal Error", null);
            }

        }


        public async Task<Response> Post(string requestUrl, U? content) 
        {
            try
            {

                string serializeObject = JsonConvert.SerializeObject(content);
                var responseTask = await _http.PostAsync(requestUrl, new StringContent(serializeObject, Encoding.UTF8, "application/json"));
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsStringAsync();
                    T deserializeObject = JsonConvert.DeserializeObject<T>(readTask.Result);
                    return new Response((int)responseTask.StatusCode,"", deserializeObject);
                }
                else
                {
                    return new Response((int)responseTask.StatusCode,"", null);
                }
            }
            catch (Exception)
            {
                return new Response((int)HttpStatusCode.InternalServerError, "Internal Error", null);
            }

        }

        public async Task<Response> Put(string requestUrl, T? content)
        {

            try
            {

                string serializeObject = JsonConvert.SerializeObject(content);
                var responseTask = await _http.PutAsync(requestUrl, new StringContent(serializeObject, Encoding.UTF8, "application/json"));
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsStringAsync();
                    T deserializeObject = JsonConvert.DeserializeObject<T>(readTask.Result);
                    return new Response((int)responseTask.StatusCode, "", deserializeObject);
                }
                else
                {
                    return new Response((int)responseTask.StatusCode, "", null);
                }
            }
            catch (Exception)
            {
                return new Response((int)HttpStatusCode.InternalServerError, "Internal Error", null);
            }

        }

        public async Task<Response> Delete(string requestUrl)
        {
            try
            {
                var responseTask = await _http.DeleteAsync(requestUrl);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsStringAsync();
                    T deserializeObject = JsonConvert.DeserializeObject<T>(readTask.Result);
                    return new Response((int)responseTask.StatusCode,"", deserializeObject);
                }
                else
                {
                    return new Response((int)responseTask.StatusCode,"", null);
                }
            }
            catch (Exception)
            {
                return new Response((int)HttpStatusCode.InternalServerError, "Internal Error", null);
            }

        }

        internal Task<Response> Get(string name, object gridModel, object gridmodel)
        {
            throw new NotImplementedException();
        }
    }
}
