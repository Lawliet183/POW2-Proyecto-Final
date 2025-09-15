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
using Microsoft.AspNetCore.WebUtilities;

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
		public string Survey()
		{
			Survey survey = _context.Surveys.AsNoTracking().FirstOrDefault();
			//string surveyJson = JsonConvert.SerializeObject(survey);


			List<Question> questions = _context.Questions.AsNoTracking().ToList();
			//string questionsJson = JsonConvert.SerializeObject(questions);
			JArray questionsArray = JArray.FromObject(questions);

			List<Choice> choices = _context.Choices.AsNoTracking().ToList();
			//string choicesJson = JsonConvert.SerializeObject(choices);
			JArray choicesArray = JArray.FromObject(choices);

			//( (JObject) questionsArray[0] )["Choices"] = JArray.FromObject(choicesArray.Where(x => x["QuestionId"] == questionsArray[0]["Id"]));

			foreach (JToken question in questionsArray)
			{
				List<JToken> actualChoices = choicesArray.Where(x => x["QuestionId"].Value<int>() == question["Id"].Value<int>()).ToList();
				question["Choices"] = JArray.FromObject(actualChoices);
			}

			//JObject finalObject = new JObject([surveyJson, questionsJson, choicesJson]);

			//JObject finalObject = new JObject([survey, questions, choices]);

			//string finalJson = JsonConvert.SerializeObject(finalObject);



			JObject finalObject = new JObject
			{
				["survey"] = JObject.FromObject(survey),
				["questions"] = questionsArray
				//["choices"] = choicesArray
			};

			//JArray x = new JArray(finalObject);

			//return new JArray(finalObject).ToString();

			//return JsonConvert.SerializeObject(survey);

			//return Ok();

			//return "";

			return finalObject.ToString();
		}

		[Route("submit-answers")]
		[HttpPost]
		public IActionResult SubmitAnswers(object form)
		{
			string json = form.ToString();

			JArray jsonArray = JArray.Parse(json);

			//foreach (var item in jsonArray)
			//{
			//	Console.WriteLine(item);
			//}
			Submission submission = new Submission()
			{
				SurveyId = 1
			};

			_context.Submissions.Add(submission);
			_context.SaveChanges();

			//var cho = _context.Choices.ToList();
			//var test = _context.Answers.ToList();

			//List<Answer> answers = new List<Answer>();
			//int lastQuestionID = -1;
			foreach (JToken choice in jsonArray)
			{
				//int questionId = choice["question_id"].Value<int>();

				//if (lastQuestionID == questionId)
				//{
				//	continue;
				//}
				//else
				//{
				//	lastQuestionID = questionId;
				//}

				Answer answer = new Answer();

				answer.QuestionId = choice["question_id"].Value<int>();
				answer.SubmissionId = _context.Submissions.Where(s => s.SurveyId == 1).Order().LastOrDefault().Id;

				switch (choice["type"].ToString())
				{
					case "text":
						answer.AnswerText = choice["value"].ToString();
						break;
					case "number":
						answer.AnswerNumber = choice["value"].Value<float>();
						break;
					case "date":
						answer.AnswerDate = choice["value"].Value<DateTime>();
						break;
					case "radio":
						answer.SelectedChoiceId = choice["value"].Value<int>();
						break;
					case "checkbox":
						//answer.Choices = _context.Choices.Where(c => c.QuestionId == questionId).ToList();

						//_context.SaveChanges();

						JArray choicesArray = JArray.FromObject(choice["choices"]);

						List<Choice> choicesList = new List<Choice>();
						foreach (JToken selectedChoice in choicesArray)
						{
							choicesList.Add(_context.Choices.Where(c => c.Id == selectedChoice.Value<int>()).FirstOrDefault());
						}

						answer.Choices = choicesList;

						//List<Choice> choicesList = new List<Choice>();
						//foreach (JToken selectedChoice in choicesArray)
						//{
						//	choicesList.Add(new Choice()
						//	{

						//	})
						//}

						break;
					default:
						break;
				}

				_context.Answers.Add(answer);
			}

			_context.SaveChanges();

			return Ok();
		}
	}
}
