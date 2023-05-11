using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebLab2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLab2.Controllers
{

    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class QuestionTypeController : ControllerBase
    {
        private readonly TestBaseDbContext _context;
        public QuestionTypeController(TestBaseDbContext context)
        {
            _context = context;
        }
        // GET: api/questiontype/list
        /// <summary>
        /// API для получения листа типов вопросов
        /// </summary>
        /// <returns>Лист QuestionType</returns>
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<QuestionType>>> GetAllQuestionTypes()
        {
            return await _context.QuestionTypes.ToListAsync(); //Include(p => p.Questions).ToListAsync();
        }

        // GET api/questiontype/<id>
        /// <summary>
        /// API для получения типа вопроса по ключу
        /// </summary>
        /// <param name="id">Ключ для доступа к типу вопроса</param>
        /// <returns>QuestionType</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionType>> GetQuestionType(int id)
        {
            var questionType = await _context.QuestionTypes.FindAsync(id);
            if (questionType == null)
            {
                return NotFound();
            }
            return questionType;
        }

        // POST api/questiontype
        /// <summary>
        /// API для добавления типа вопроса
        /// </summary>
        /// <param name="questionType">Модель QuestionType для добавления в БД</param>
        /// <returns>Созданный QuestionType</returns>
        [HttpPost]
        [Authorize(Roles = "admin,moderator")]
        public async Task<ActionResult<QuestionType>> PostQuestionType([FromBody] QuestionType questionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.QuestionTypes.Add(questionType);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetQuestionType", new { id = questionType.Id }, questionType);
        }

        // PUT api/questiontype/<id>
        /// <summary>
        /// API для изменения типа вопроса
        /// </summary>
        /// <param name="id">Ключ для доступа к типу вопросу</param>
        /// <param name="questionType">Модель QuestionType для изменения в БД</param>
        /// <returns>Ошибку, в случае отсутствия типа вопроса в БД</returns>
        [HttpPut("id={id}")]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> PutQuestionType(int id, QuestionType questionType)
        {
            if (id != questionType.Id)
            {
                return BadRequest();
            }
            _context.Entry(questionType).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionTypeExists(id))
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

        private bool QuestionTypeExists(int id)
        {
            return _context.QuestionTypes.Any(e => e.Id == id);
        }

        // DELETE api/questiontype/id=<id>
        /// <summary>
        /// API для удаления типа вопроса
        /// </summary>
        /// <param name="id">Ключ для доступа к типу вопроса</param>
        /// <returns>Ошибку, в случае отсутствия типа вопроса в БД</returns>
        [HttpDelete("id={id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteQuestionType(int id)
        {
            var questionType = await _context.QuestionTypes.FindAsync(id);
            if (questionType == null)
            {
                return NotFound();
            }
            _context.QuestionTypes.Remove(questionType);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
