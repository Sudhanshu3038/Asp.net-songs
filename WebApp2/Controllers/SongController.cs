using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApp2.Data1;
using WebApp2.Model;

// For more information on enabling Web API for empty projects,visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public SongController(ApplicationDbContext context)
        {
            _context = context;
           
            
        }

        [HttpGet]
        public async Task<ActionResult>Get(int page = 1, int pageSize = 10)
        {
            return Ok(await _context.Songs.Where(s=>!s.IsRowDeleted).Include(s => s.Artist)
            .Include(s => s.Category)
            .Skip((page - 1) * pageSize)
            .Take(pageSize).AsNoTracking().ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var songFromDb = await _context.Songs.Where(s => !s.IsRowDeleted).Include(s => s.Artist)
            .Include(s => s.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (songFromDb == null)
            {
                return NotFound();
            }
            return Ok(songFromDb); ;
        }
        [HttpGet("SearchByName/{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var song = await _context.Songs.Where(s => !s.IsRowDeleted && s.SongName.Contains(name)).ToListAsync();

            if (song == null)
                return NotFound();
            else
                return Ok(song);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Song song)
        {
            var songFromDb = await _context.Songs.AddAsync(song);
            if (songFromDb == null)
            {
                return NotFound();
            }
            else
            {
                _context.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = song.Id }, song);
                
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Song song)
        {
            var songFromDb = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);
            if (songFromDb == null)
            {
                return NotFound();
            }
            else
            {
                songFromDb.SongName = song.SongName;
                songFromDb.FilePath = song.FilePath;
                _context.Songs.Update(songFromDb);
                await _context.SaveChangesAsync();
                _context.Entry(song).State = EntityState.Modified;
                return Ok("Song Updated");
            }
        }
        [HttpPut("ByName/{Name}")]
        public async Task<IActionResult> PutSongByTitle(string name , Song updatedSong)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.SongName == name);
            if (song == null)
            {
                return NotFound();
            }

            
            song.SongName = updatedSong.SongName;
            song.FilePath = updatedSong.FilePath;
            song.ArtistId = updatedSong.ArtistId;
            song.CategoryId = updatedSong.CategoryId;
            song.ModifiedBy = "system";
            song.ModifiedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(int id, IFormFile file)
        {
            var songFromDb = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);

            if (songFromDb == null)
                return NotFound();

            else if (file == null || file.Length == 0)
                return BadRequest("No File Selected");

            else
            {
                var basePath = "Music";
                var uploadsFolderPath = Path.Combine(basePath, "uploads");

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                songFromDb.FilePath = filePath;
                _context.Songs.Update(songFromDb);
                await _context.SaveChangesAsync();

                return Ok(new { filePath });
            }
        }


        [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var songFromDb = await _context.Songs.FirstOrDefaultAsync(x=>x.ArtistId==id);
                if (songFromDb == null)
                {
                    return NotFound();
                }
                else
                {
                   songFromDb.IsRowDeleted = true; 
                    _context.Songs.Update(songFromDb);
                    await _context.SaveChangesAsync();
                     return Ok("Song Deleted");
                }

            }
        
    }
    }
