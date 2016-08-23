﻿using SoftwareTechnologiesTeamProject.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SoftwareTechnologiesTeamProject.Controllers
{
    using Extensions;
    using Microsoft.AspNet.Identity;
    using ViewModels;

    public class MatchesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Matches
        public ActionResult Index()
        {
            var matches = db.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .OrderByDescending(m => m.DateTime)
                .ToList();

            return View(matches.ToList());
        }

        // GET: Matches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Votes)
                .FirstOrDefault(m => m.Id == id);

            if (match == null)
            {
                return HttpNotFound();
            }

            MatchDetailsViewModel viewModel = new MatchDetailsViewModel();
            viewModel.Match = match;

            viewModel.HomeTeamHistory = db.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => (m.HomeTeam.Name == match.HomeTeam.Name || m.AwayTeam.Name == match.HomeTeam.Name) && m.IsResultUpdated && m.DateTime < match.DateTime)
                .OrderByDescending(m => m.DateTime)
                .Take(10)
                .ToList();

            viewModel.AwayTeamHistory = db.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => (m.HomeTeam.Name == match.AwayTeam.Name || m.AwayTeam.Name == match.AwayTeam.Name) && m.IsResultUpdated && m.DateTime < match.DateTime)
                .OrderByDescending(m => m.DateTime)
                .Take(10)
                .ToList();

            viewModel.LeagueTeams = db.Teams
                .Where(t => t.LeagueId == match.LeagueId)
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsFor - t.GoalsAgainst)
                .ThenByDescending(t => t.GoalsFor)
                .ToList();

            viewModel.LeagueName = db.Leagues.Find(match.LeagueId).Name;

            return View(viewModel);
        }

        public ActionResult AddVote(int? id, string voteType)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var match = db.Matches.Find(id);

            if (match.HasUserVoted(userId))
            {
                this.AddNotification("You have already voted for this game !", NotificationType.ERROR);
                return RedirectToAction("Details", new { id = match.Id });
            }

            var vote = new Vote()
            {
                MatchId = match.Id,
                Match = match,
                VoteType = voteType,
                Voter = user,
                VoterId = userId
            };

            match.IncreaseVoteCount(voteType);
            match.Votes.Add(vote);
            db.Entry(match).State = EntityState.Modified;

            user.MatchHistory.Add(match);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = match.Id });
        }


        public ActionResult UpdateResult(int? id)
        {
            Match match = db.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .FirstOrDefault(m => m.Id == id);

            return View(match);
        }

        [HttpPost]
        public ActionResult UpdateResult([Bind(Include = "Id,HomeTeamGoals,AwayTeamGoals,HomeTeamId,AwayTeamId")]Match match)
        {
            var currentMatch = db.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Votes.Select(v => v.Voter))
                .FirstOrDefault(m => m.Id == match.Id);

            if (currentMatch == null)
            {
                return HttpNotFound();
            }

            currentMatch.HomeTeamGoals = match.HomeTeamGoals;
            currentMatch.AwayTeamGoals = match.AwayTeamGoals;
            string winner = currentMatch.GetWinningSide();

            currentMatch.UpdateTeams(winner);
            currentMatch.IsResultUpdated = true;

            db.Entry(currentMatch.HomeTeam).State = EntityState.Modified;
            db.Entry(currentMatch.AwayTeam).State = EntityState.Modified;

            foreach (var vote in currentMatch.Votes.Where(v => v.VoteType == winner))
            {
                var user = vote.Voter;
                user.Balance += 50;
                db.Entry(user).State = EntityState.Modified;
            }

            db.Entry(currentMatch).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", "Matches", new { id = match.Id });
        }

        // GET: Matches/Create
        public ActionResult Create()
        {
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.LeagueId = new SelectList(db.Leagues, "Id", "Name");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LeagueId,HomeTeamId,AwayTeamId,DateTime,HomeCoefficient,DrawCoefficient,AwayCoefficient")] Match match)
        {
            if (ModelState.IsValid)
            {


                match.HomeTeam = db.Teams.Find(match.HomeTeamId);
                match.AwayTeam = db.Teams.Find(match.AwayTeamId);
                match.League = db.Leagues.Find(match.LeagueId);
                match.LeagueName = db.Leagues.Find(match.LeagueId).Name;

                //Add this match to home/away teams match history
                //var homeTeam = db.Teams.Find(match.HomeTeamId);
                //homeTeam.Matches.Add(match);
                //var awayTeam = db.Teams.Find(match.AwayTeamId);
                //awayTeam.Matches.Add(match);

                db.Matches.Add(match);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name", match.AwayTeamId);
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name", match.HomeTeamId);
            ViewBag.LeagueId = new SelectList(db.Leagues, "Id", "Name", match.LeagueId);
            return View(match);
        }

        // GET: Matches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.Matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }


            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name", match.AwayTeamId);
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name", match.HomeTeamId);
            ViewBag.LeagueId = new SelectList(db.Leagues, "Id", "Name", match.LeagueId);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LeagueId,HomeTeamId,AwayTeamId,DateTime,Result,HomeTeamGoals,AwayTeamGoals,TotalVotesCount,HomeVotesCount,DrawVotesCount,AwayVotesCount,HomeCoefficient,DrawCoefficient,AwayCoefficient")] Match match)
        {
            if (ModelState.IsValid)
            {
                db.Entry(match).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name", match.AwayTeamId);
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name", match.HomeTeamId);
            ViewBag.LeagueId = new SelectList(db.Leagues, "Id", "Name", match.LeagueId);
            return View(match);
        }

        // GET: Matches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.Matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Match match = db.Matches.Find(id);
            db.Matches.Remove(match);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
