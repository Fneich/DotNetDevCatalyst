
using System.Text.Json.Serialization;


namespace DevCatalyst.WebAPI.Models
{
    public class Response
    {
        [JsonPropertyName("data")]
        public dynamic? Data { get; set; } = null;

        [JsonPropertyName("code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } =string.Empty;

        public Response(int code,string message,dynamic? data) 
        {  
            StatusCode = code;
            Message = message;
            Data = data;
        }



    }
}
