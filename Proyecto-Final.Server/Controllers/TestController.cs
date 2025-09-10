using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Net;
using System.Net.Http.Headers;

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
		public IActionResult Post(object user)
		{
			//string json = httpRequest.QueryString.ToString();

			//string json = "";
			//user.Values().ToList().ForEach(s => json += s);

			string json = user.ToString();

			JObject jsonObj = JObject.Parse(json);

			//Console.WriteLine($"\n{jsonObj.ToString()}");

			//HttpContext.Response.Headers["Access-Control-Allow-Origin"] = "*";

			Respondent respondent = new Respondent();

			respondent.Name = jsonObj["name"].ToString();
			respondent.Email = jsonObj["email"].ToString();

			Auth auth = new Auth();

			auth.PasswordHash = jsonObj["passwordhash"].ToString();
			auth.Role = jsonObj["role"].ToString();

			_context.Respondents.Add(respondent);

			_context.Respondents.
			_context.Auths.Add(auth);
			

			_context.SaveChanges();

			//Console.WriteLine(name);

			return Ok();
		}
	}
}
