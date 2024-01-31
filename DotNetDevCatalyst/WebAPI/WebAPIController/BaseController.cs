
using DevCatalyst.Data.Models;
using DevCatalyst.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using DevCatalyst.WebAPI.Models;
using DevCatalyst.WebAPI.Services;



namespace DevCatalyst.WebAPI.WebAPIController
{

    public class BaseController<T, M, R> : ControllerBase where T : BaseModel where M : GridModel where R : BaseRepository<T>
    {

        protected BaseService<T, M, R> _Service;

        protected string Name { get; set; }
        public BaseController(BaseService<T, M, R> service, string name)
        {
            _Service = service;
            Name = name;    
        }
        


        [HttpGet("GetCount")]
        public ActionResult<Response> GetCount()
        {
            try
            {
                var models = _Service.GetCount();
                return new Response(200,"Success",models);
            }
            catch (Exception e)
            {
                return new Response(500, e.Message, null);
            }

        }

        [HttpPost("GetCount")]
        public ActionResult<Response> GetCount(M model)
        {
            try
            {
                var models = _Service.GetCount(model);
                return new Response(200, "Success", models);
            }
            catch (Exception e)
            {
                return new Response(500, e.Message, null);
            }

        }


        [HttpPost("GetAll")]
        public ActionResult<Response> GetAll(M model)
        {
            try
            {
                var models = _Service.GetAll(model);
                return new Response(200, "Success", models.ToList());
            }
            catch (Exception e)
            {
                return new Response(500, e.Message, null);
            }

        }

        [HttpGet("GetAll")]
        public ActionResult<Response> GetAll()
        {
            try
            {
                var models = _Service.GetAll();
                return new Response(200, "Success", models.ToList());
            }
            catch (Exception e)
            {
                return new Response(500, e.Message, null);
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Response> Get(Guid id)
        {
            try
            {
                var model = _Service.GetById(id);
                if (model == null)
                    return new Response(404, "Not Found", null);
                return new Response(200, "Success", model);
            }
            catch (Exception e)
            {
                return new Response(500, e.Message, null);
            }

}

        [HttpPost]
        public ActionResult<Response> Add(T model)
        {
            try
            {
                return  new Response(200, "Success", _Service.Add(model)); 
            }
            catch (Exception e)
            {
                return new Response(500, e.Message, null);
            }
        }

        [HttpPut("{id}")]
        public Response Edit(int id, T model)
        {

            try
            {
                var existingModel = _Service.GetById(id);
                if (existingModel == null) 
                { 
                    return new Response(404, "Not Found", null);
                }
                else
                {
                    model.ID = existingModel.ID;
                    model.RECORD_ID = existingModel.RECORD_ID;
                    return new Response(200, "Success", _Service.Edit(model));
                }

            }
            catch (Exception e)
            {
                return new Response(500, e.Message,null);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Response> Delete(Guid id)
        {
            try
            {
                return new Response(404, "Success", _Service.Delete(id));
            }
            catch (Exception e)
            {
                return new Response(500, e.Message,null);
            }
        }




    }
}
