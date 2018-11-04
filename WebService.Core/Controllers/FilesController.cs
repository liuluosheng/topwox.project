using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Topwox.WebService.Core.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private MongoClient _client;
        public FilesController(MongoClient client)
        {
            _client = client;
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post()
        {
            var database = _client.GetDatabase("UploadDB");
            GridFSBucket g = new GridFSBucket(database);
            List<string> result = new List<string>();
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                var bytes = new byte[file.Length];
                file.OpenReadStream().Read(bytes, 0, bytes.Length);
                result.Add($"api/files/{g.UploadFromBytes(file.FileName, bytes)}?name={file.FileName}");
            }
            return Ok( new { result= string.Join(",", result)});
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="id">MongoDB的ObjectId</param>
        /// <param name="name">下载的文件名</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(string id, string name)
        {
            name = HttpUtility.UrlEncode(name);
            bool checkId = ObjectId.TryParse(id, out ObjectId objectId);
            var database = _client.GetDatabase("UploadDB");
            GridFSBucket g = new GridFSBucket(database);
            var obj = checkId ? g.DownloadAsBytes(objectId) : g.DownloadAsBytesByName(name);
            if (obj != null)
            {
                return File(obj, "application/octet-stream", name);
            }
            return BadRequest("Not Find!");
        }
    }
}