using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Net;
using System.Net.Http.Headers;
using Devart.Common;
using Devart.Data.MySql.Entity;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final.Server.Controllers
{
	[ApiController]
	[Route("/api")]
	public class TestController : ControllerBase
	{
		public readonly ILogger<TestController> _logger;

		public readonly ProyectoEncuestaContext _context;


		public TestController(ILogger<TestController> logger)
		{
			_logger = logger;

			_context = new ProyectoEncuestaContext();
		}


		[Route("ping")]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok();
		}

		[Route("register")]
		[HttpPost]
		public IActionResult Register(object user)
		{
			// Parse the JSON
			string json = user.ToString();

			JObject jsonObj = JObject.Parse(json);

			// Add the respondent entity
			Respondent respondent = new Respondent();

			respondent.Name = jsonObj["name"].ToString().Trim();
			respondent.Email = jsonObj["email"].ToString().Trim();

			_context.Respondents.Add(respondent);
			_context.SaveChanges();

			
			// Get the last respondent ID
			var ID = _context.Respondents.Order().Last().Id;

			// Add the auth entity
			Auth auth = new Auth();

			auth.RespondentId = ID;
			auth.PasswordHash = jsonObj["passwordHash"].ToString().Trim();
			auth.Role = jsonObj["role"]?.ToString().Trim();

			_context.Auths.Add(auth);

			// Write changes to the DB
			_context.SaveChanges();

			return new CreatedResult();
		}

		[Route("login")]
		[HttpPost]
		public IActionResult LogIn(object user)
		{
			// Parse the JSON
			string json = user.ToString();

			JObject jsonObj = JObject.Parse(json);

			//var matchID = (from r in _context.Respondents
			//			where (r.Name == jsonObj["name"].ToString() && r.Email == jsonObj["email"].ToString())
			//			select r.Id).First();

			var q = _context.Respondents
						.Where(c => c.Name == jsonObj["name"].ToString().Trim() && c.Email == jsonObj["email"].ToString())
						.ToList();

			//System.FormattableString query = $"SELECT R.Id FROM Respondent as R WHERE R.Name == '{jsonObj["name"].ToString().Trim()}' AND R.Email == '{jsonObj["email"].ToString().Trim()}'";

			//var q = _context.Database.Execute(query);

			if (q.Count <= 0)
			{
				return BadRequest();
			}
			else
			{
				return Ok();
			}

			//if (ID != -1)
			//{
			//	return Ok();
			//}
			//{
			//	return BadRequest();
			//}

			//	return Ok();
		}

		[Route("survey")]
		[HttpGet]
		public IActionResult Survey()
		{
			var survey = _context.Surveys.FirstOrDefault();
			var questions = _context.Questions.ToList();
			var choices = _context.Choices.ToList();

			return Ok();
		}
	}
}
