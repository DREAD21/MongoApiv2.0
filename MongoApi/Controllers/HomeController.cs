using Microsoft.AspNetCore.Mvc;
using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        public HomeController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> Upload(IFormFile file, string key)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is not selected");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();

                var collection = _dbContext.GetCollection<FileModel>("files");
                var fileModel = new FileModel
                {
                    Key = key,
                    Content = content
                };

                await collection.InsertOneAsync(fileModel);
            }

            return Ok();
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Download(string key)
        {
            var collection = _dbContext.GetCollection<FileModel>("files");
            var filter = Builders<FileModel>.Filter.Eq("Key", key);
            var fileModel = await collection.Find(filter).FirstOrDefaultAsync();

            if (fileModel == null)
            {
                return NotFound();
            }

            return File(fileModel.Content, "application/pdf", key);
        }

    }
}
