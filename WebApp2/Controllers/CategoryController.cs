using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp2.Data1;
using WebApp2.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 25)
        {
            return Ok(await _context.Categorys.Skip((page - 1) * pageSize)
            .Take(pageSize).AsNoTracking().ToArrayAsync());
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _context.Categorys.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(category);
            }
        }

       


        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            var categoryFromDb = await _context.AddAsync(category);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            else
            {
                await _context.SaveChangesAsync();
                return Ok("Category Added");
            }
        }

        // PUT api/<CategoryController>/5

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            var categoryFromDb = await _context.Categorys.FirstOrDefaultAsync(x => x.CategoryId == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            else
            {
                categoryFromDb.CategoryName = category.CategoryName;
                _context.Categorys.Update(category);
                _context.Entry(category).State = EntityState.Modified;
                return Ok("Category Updated");
            }
        }

        [HttpPut("ByName/{name}")]
        public async Task<IActionResult> PutCategoryByName(string name, Category updatedCategory)
        {
            var category = await _context.Categorys.FirstOrDefaultAsync(c => c.CategoryName == name);
            if (category == null)
            {
                return NotFound();
            }

            // Update fields
            category.CategoryName = updatedCategory.CategoryName;
            category.ModifiedBy = "system"; 
            category.ModifiedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryFromDb = await _context.Categorys.FirstOrDefaultAsync(x => x.CategoryId == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            else
            {
                _context.Categorys.Remove(categoryFromDb);
                await _context.SaveChangesAsync();
                return Ok("Category deleted");

            }
        }
        
    }
}
