using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebLab2.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLab2.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestBaseDbContext _context;
        public TestController(TestBaseDbContext context)
        {
            _context = context;
        }
        // GET: api/test/list
        /// <summary>
        /// API для получения листа тестов
        /// </summary>
        /// <returns>Лист Test</returns>
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Test>>> GetAllTests()
        {
            return await _context.Tests.Include(p => p.Questions).ToListAsync();
        }

        // GET api/test/<id>
        /// <summary>
        /// API для получения теста по ключу
        /// </summary>
        /// <param name="id">Ключ для доступа к тесту</param>
        /// <returns>Test</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }
            return test;
        }

        // POST api/test
        /// <summary>
        /// API для добавления теста
        /// </summary>
        /// <param name="test">Модель Test для добавления в БД</param>
        /// <returns>Созданный Test</returns>
        [HttpPost]
        [Authorize(Roles ="admin,moderator")]
        public async Task<ActionResult<Test>> PostTest([FromBody] Test test)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Tests.Add(test);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTest", new { id = test.Id }, test);
        }

        // PUT api/test/<id>
        /// <summary>
        /// API для изменения теста
        /// </summary>
        /// <param name="id">Ключ для доступа к тесту</param>
        /// <param name="test">Модель Test для изменения в БД</param>
        /// <returns>Ошибку, в случае отсутствия вопроса в БД</returns>
        [HttpPut("id={id}")]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> PutTest(int id, Test test)
        {
            if (id != test.Id)
            {
                return BadRequest();
            }
            _context.Entry(test).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool TestExists(int id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }

        // DELETE api/test/id=<id>
        /// <summary>
        /// API для удаления теста
        /// </summary>
        /// <param name="id">Ключ для доступа к тесту</param>
        /// <returns>Ошибку, в случае отсутствия вопроса в БД</returns>
        [HttpDelete("id={id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
