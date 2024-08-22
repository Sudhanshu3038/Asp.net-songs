using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp2.Data1;
using WebApp2.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<Artist>
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 25)
        {
            return Ok(await _context.Artists.Skip((page - 1) * pageSize)
            .Take(pageSize).AsNoTracking().ToArrayAsync()); 
        }

        // GET api/<Artist>/5
        [HttpGet("{id}")]
        public async Task<IActionResult>Get(int id)
        {
            var artistFromDb = await _context.Artists.AsNoTracking().FirstOrDefaultAsync(x => x.ArtistId == id);
            if(artistFromDb == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(artistFromDb);
            }
           
        }
        [HttpGet("Byname/{name}")]
        public async Task<ActionResult<Artist>> GetArtistByName(string name)
        {
            var artistFromDb = await _context.Artists.Where(a => a.ArtistName.Contains(name)).ToListAsync();

            if (artistFromDb == null)
            {
                return NotFound();
            }

            return Ok( artistFromDb);
        }

        // POST api/<Artist>
        [HttpPost]
        public async Task<IActionResult>Post([FromBody] Artist artist)
        {
         var artistFromDb = await _context.AddAsync(artist);
          if(artistFromDb ==null)  
            {
                return NotFound();
            }
            else
            {
                await _context.SaveChangesAsync();
                artist.CreatedBy = "system";
                artist.CreatedOn = DateTime.Now;
                return CreatedAtAction(nameof(Get), new { id = artist.ArtistId }, artist);
                
            }
        }

        // PUT api/<Artist>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Artist artist)
        {
            var artistFromDb = await _context.Artists.AsTracking().FirstOrDefaultAsync(x=>x.ArtistId ==id);
            if(artistFromDb ==null)
            {
                return NotFound();
            }
            else
            {
                artistFromDb.ArtistName = artist.ArtistName;
                _context.Artists.Update(artistFromDb);
                await _context.SaveChangesAsync();
                _context.Entry(artist).State = EntityState.Modified;
                return Ok("Artist Created");
            }
            
        }

        [HttpPut("ByName/{name}")]
        public async Task<IActionResult> PutArtistByName(string name, Artist updatedArtist)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.ArtistName == name);
            if (artist == null)
            {
                return NotFound();
            }            
            artist.ArtistName = updatedArtist.ArtistName;
            artist.ModifiedBy = "system"; 
            artist.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            _context.Entry(artist).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<Artist>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var artistFromDb = await _context.Artists.FirstOrDefaultAsync(x=>x.ArtistId ==id);
            if(artistFromDb ==null)
            {
                return NotFound();
            }
            else
            {
                _context.Artists.Remove(artistFromDb);
                await _context.SaveChangesAsync();
                return Ok("Artist deleted");

            }
        }
        
    }
}
