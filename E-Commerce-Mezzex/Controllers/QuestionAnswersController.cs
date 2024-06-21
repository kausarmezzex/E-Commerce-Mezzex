using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce_Mezzex.Models.Domain;

namespace E_Commerce_Mezzex.Controllers
{
    public class QuestionAnswersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionAnswersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QuestionAnswers
        public async Task<IActionResult> Index()
        {
            var questionAnswers = await _context.QuestionsAnswers
                .Include(qa => qa.Product)
                .ToListAsync();
            return View(questionAnswers);
        }

        // GET: QuestionAnswers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionsAnswers
                .Include(qa => qa.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (questionAnswer == null)
            {
                return NotFound();
            }

            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ProductId = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: QuestionAnswers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionDate,ProductId")] QuestionAnswer questionAnswer, string[] Questions, string[] Answers)
        {
            if (!ModelState.IsValid)
            {
                questionAnswer.QuestionDate = DateTime.Now;

                // Save each question and answer pair
                for (int i = 0; i < Questions.Length; i++)
                {
                    var qa = new QuestionAnswer
                    {
                        Question = Questions[i],
                        Answer = Answers[i],
                        QuestionDate = questionAnswer.QuestionDate,
                        ProductId = questionAnswer.ProductId // Assuming you want to associate with the same product for each pair
                    };
                    _context.Add(qa);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ProductId = new SelectList(await _context.Products.ToListAsync(), "Id", "Name", questionAnswer.ProductId);
            return View(questionAnswer);
        }


        // GET: QuestionAnswers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionsAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            ViewBag.ProductId = new SelectList(await _context.Products.ToListAsync(), "Id", "Name", questionAnswer.ProductId);
            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer,QuestionDate,ProductId")] QuestionAnswer questionAnswer)
        {
            if (id != questionAnswer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionAnswer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionAnswerExists(questionAnswer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ProductId = new SelectList(await _context.Products.ToListAsync(), "Id", "Name", questionAnswer.ProductId);
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionsAnswers
                .Include(qa => qa.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionAnswer = await _context.QuestionsAnswers.FindAsync(id);
            if (questionAnswer != null)
            {
                _context.QuestionsAnswers.Remove(questionAnswer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionAnswerExists(int id)
        {
            return _context.QuestionsAnswers.Any(e => e.Id == id);
        }
    }
}
