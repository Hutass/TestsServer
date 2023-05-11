using WebLab2.Models;

namespace WebLab2.Data
{
    public class TestBaseDbContextSeed
    {
        /// <summary>
        /// Заполняет базу данных начальными значениями, если они в ней отсутствуют
        /// </summary>
        /// <param name="context">контекст базы данных</param>
        /// <returns></returns>
        public static async Task SeedAsync(TestBaseDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();
                if (!context.Tests.Any())
                {
                    var tests = new Test[]
                {
                new Test{Name="Lab3-1"},
                new Test{Name="lab3-2"},
                new Test{Name="lab3-3"}
                };
                    foreach (Test test in tests)
                    {
                        context.Tests.Add(test);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.QuestionTypes.Any())
                {
                    var types = new QuestionType[]
                {
                new QuestionType{Name="Один"},
                new QuestionType{Name="Несколько"},
                new QuestionType{Name="Текст"}
                };
                    foreach (QuestionType type in types)
                    {
                        context.QuestionTypes.Add(type);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.Questions.Any())
                {
                    var questions = new Question[]
                {
                new Question{TestId=context.Tests.OrderBy(p=>p.Id).Last().Id, Text="lab3 question", TypeId=2}
                };
                    foreach (Question question in questions)
                    {
                        context.Questions.Add(question);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
