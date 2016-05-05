using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WordPuzzleScorer.Domain;
using WordPuzzleScorer.UI.Models;

namespace WordPuzzleScorer.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Score(AnswerViewModel answer)
        {
            ILineParser parser = new DefaultLineParser();
            IWordChecker wordChecker = new StubWordChecker();
            IWordScorer wordScorer = new DefaultWordScorer(wordChecker);

            IScorer scorer = new DefaultScorer(parser, wordScorer);

            var bonusTiles = new List<List<int>>();
            bonusTiles.Add(new List<int> { 1, 3 });
            bonusTiles.Add(new List<int> { 0 });
            bonusTiles.Add(new List<int> { 0 });

            var domainAnswer = new Answer();

            for (var i = 0; i < answer.AnswerLines.Count(); i++ )
            {
                domainAnswer.Lines.Add(new Line(answer.AnswerLines[i], bonusTiles[i]));
            }

            var score = scorer.Score(domainAnswer);

            var scoreViewModel = new ScoreViewModel();

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Score, ScoreViewModel>(); cfg.CreateMap<WordScore, WordScoreViewModel>(); });
            
            var mapper = config.CreateMapper();
            mapper.Map(score, scoreViewModel);

            return View(scoreViewModel);
        }
    }
}