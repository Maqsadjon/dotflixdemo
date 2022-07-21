using DotFlixDemo.Data;
using DotFlixDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.IO;

namespace DotFlixDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public MovieController(MovieDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }
        [HttpPost]
        public async Task<IActionResult> SetImage(long movieId, IFormFile file)
        {
            var movie = await _dbContext.Movies
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == movieId);



            string fileExt = Path.GetExtension(file.FileName);
            string filename = "Images/" + Guid.NewGuid() + fileExt;
            string path = Path.Combine(_env.WebRootPath, $"{filename}");

            if (fileExt == ".png" || fileExt == ".jpg")
            {
                FileStream fileStream = System.IO.File.Open(path, FileMode.Create);

                await file.OpenReadStream().CopyToAsync(fileStream);

                if(!string.IsNullOrEmpty(movie.Image))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, movie.Image));
                }

                fileStream.Flush();
                fileStream.Close();
                movie.Image = filename;


                await _dbContext.SaveChangesAsync();
                return Ok(movie);
            }
            else
            {
                return BadRequest("Ishkalchilar hormanglar");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(long movieId)
        {
            var movie = _dbContext.Movies
                .Include(p=>p.Author)
                .FirstOrDefault(p => p.Id == movieId);

            if (movie is not null)
            {
                string path = Path.Combine(_env.WebRootPath, movie.Image);
                byte[] file = await System.IO.File.ReadAllBytesAsync(path);
                return File(file, "octet/stream", Path.GetFileName(path));
            }
            else
            {
                return NotFound("Rasim topilmadi uzr");
            }
        }



        [HttpGet]
        public async  Task<IActionResult > GetMovies()
        {   
            var movies = await _dbContext.Movies.Include(p => p.Author).ToListAsync();
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] Movie movie)
        {
            var entry = await _dbContext.Movies.AddAsync(movie);
            await _dbContext.SaveChangesAsync();
            return Ok(entry.Entity);
        }
    }
}
