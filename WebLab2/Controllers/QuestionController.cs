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
    public class QuestionController : ControllerBase
    {
        private readonly TestBaseDbContext _context;
        public QuestionController(TestBaseDbContext context)
        {
            _context = context;
        }
        // GET: api/question/list
        /// <summary>
        /// API для получения листа вопросов
        /// </summary>
        /// <returns>Лист Question</returns>
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Question>>> GetAllQuestions()
        {
            return await _context.Questions.ToListAsync();
        }

        // GET api/question/<id>
        /// <summary>
        /// API для получения вопроса по ключу
        /// </summary>
        /// <param name="id">Ключ для доступа к вопросу</param>
        /// <returns>Question</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return question;
        }

        // POST api/question
        /// <summary>
        /// API для добавления вопроса
        /// </summary>
        /// <param name="question">Модель Question для добавления в БД</param>
        /// <returns>Созданный Question</returns>
        [HttpPost]
        [Authorize(Roles = "admin,moderator")]
        public async Task<ActionResult<Question>> PostQuestion([FromBody] Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        // PUT api/question/<id>
        /// <summary>
        /// API для изменения вопроса
        /// </summary>
        /// <param name="id">Ключ для доступа к вопросу</param>
        /// <param name="question">Модель Question для изменения в БД</param>
        /// <returns>Ошибку, в случае отсутствия вопроса в БД</returns>
        [HttpPut("id={id}")]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.Id)
            {
                return BadRequest();
            }
            _context.Entry(question).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }

        // DELETE api/question/id=<id>
        /// <summary>
        /// API для удаления вопроса
        /// </summary>
        /// <param name="id">Ключ для доступа к вопросу</param>
        /// <returns>Ошибку, в случае отсутствия вопроса в БД</returns>
        [HttpDelete("id={id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
