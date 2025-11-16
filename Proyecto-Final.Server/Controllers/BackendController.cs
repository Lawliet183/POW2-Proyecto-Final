using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proyecto_Final.Server;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

namespace Proyecto_Final.Server.Controllers
{
	[ApiController]
	[Route("/api")]
	public class BackendController : ControllerBase
	{
		public readonly ILogger<BackendController> _logger;

		public readonly ProyectoEncuestaContext _context;


		public BackendController(ILogger<BackendController> logger, ProyectoEncuestaContext context)
		{
			_logger = logger;

			//_context = new ProyectoEncuestaContext();

			_context = context;

			_context.Database.CanConnect();
		}


		[Route("ping")]
		[HttpGet]
		public IActionResult Ping()
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
			int ID = _context.Respondents.Order().Last().Id;

			// Add the auth entity
			Auth auth = new Auth();

			auth.RespondentId = ID;
			auth.PasswordHash = jsonObj["passwordHash"].ToString().Trim();
			auth.Role = jsonObj["role"]?.ToString().Trim();

			_context.Auths.Add(auth);

			// Write changes to the DB
			_context.SaveChanges();

			return Created();
		}

		[Route("login")]
		[HttpPost]
		public string LogIn(object user)
		{
			// Parse the JSON
			string json = user.ToString();

			JObject jsonObj = JObject.Parse(json);

			//var matchID = (from r in _context.Respondents
			//			where (r.Name == jsonObj["name"].ToString() && r.Email == jsonObj["email"].ToString())
			//			select r.Id).First();

			Respondent respondent = _context.Respondents
												.Where(c => c.Name == jsonObj["name"].ToString().Trim() && c.Email == jsonObj["email"].ToString())
												.FirstOrDefault();

			Auth? auth = _context.Auths
							.Where(a => a.RespondentId == respondent.Id && a.PasswordHash == jsonObj["passwordHash"].ToString())
							.FirstOrDefault();

			//System.FormattableString query = $"SELECT R.Id FROM Respondent as R WHERE R.Name == '{jsonObj["name"].ToString().Trim()}' AND R.Email == '{jsonObj["email"].ToString().Trim()}'";

			//var q = _context.Database.Execute(query);

			JObject response = new JObject
			{
				["status"] = (auth is null) ? "error" : "ok",
				["user_id"] = (auth is null) ? "null" : auth.RespondentId,
				["role"] = (auth is null) ? "null" : auth.Role
			};

			return response.ToString();

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

			JObject jsonObj = JObject.Parse(json);

			Submission submission = new Submission()
			{
				SurveyId = 1,
				RespondentId = jsonObj["user_id"].Value<int>()
			};

			_context.Submissions.Add(submission);
			_context.SaveChanges();

			//JArray jsonArray = JArray.Parse(json);
			JArray jsonArray = JArray.FromObject(jsonObj["form"]);

			foreach (JToken choice in jsonArray)
			{
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
						JArray choicesArray = JArray.FromObject(choice["choices"]);

						List<Choice> choicesList = new List<Choice>();
						foreach (JToken selectedChoice in choicesArray)
						{
							choicesList.Add(_context.Choices.Where(c => c.Id == selectedChoice.Value<int>()).FirstOrDefault());
						}

						answer.Choices = choicesList;

						break;
					default:
						break;
				}

				_context.Answers.Add(answer);
			}

			_context.SaveChanges();

			return Created();
		}

		[Route("get-answers")]
		[HttpGet]
		public string GetAnswers()
		{

			var nonAnonymousRespondents = _context.Respondents.Join(
				_context.Submissions,
				r => r.Id,
				s => s.RespondentId,
				(r, s) => new { s.Id, s.RespondentId, r.Name, r.Email }
			).ToList();

			var allAnswersNoMulti = _context.Submissions.Join(
				_context.Answers,
				s => s.Id,
				a => a.SubmissionId,
				(s, a) => new { s, a }
			).Join(
				_context.Questions,
				temp => temp.a.QuestionId,
				q => q.Id,
				(temp, q) => new {
					temp.s.Id,
					temp.s.RespondentId,
					temp.a.QuestionId,
					temp.a.AnswerText,
					temp.a.AnswerNumber,
					temp.a.AnswerDate,
					temp.a.SelectedChoiceId,
					temp.a.Choices,
					q.Type
				}
			).ToList();

			var singleChoiceAnswers = _context.Answers.Join(
				_context.Choices,
				a => a.SelectedChoiceId,
				c => c.Id,
				(a, c) => new { a.SubmissionId, a.QuestionId, a.SelectedChoiceId, c.Label }
			).ToList();

			//var multipleChoiceAnswers = _context.

			//var questions = _context.Questions.ToList();
			//var answers = _context.Answers.ToList();

			var submissions = _context.Submissions.ToList();

			List<JObject> responseItems = new List<JObject>();
			foreach (var submission in submissions)
			{
				var questions = allAnswersNoMulti.FindAll(a => a.Id == submission.Id);

				List<JObject> questionItems = new List<JObject>();
				foreach (var question in questions)
				{
					JObject obj = new JObject
					{
						["question_id"] = question.QuestionId,
						["type"] = question.Type
					};

					JArray choicesArray = new JArray();
					if (question.Type == "multi")
					{
						var choicesList = question.Choices.ToList();

						foreach(var choice in choicesList)
						{
							choicesArray.Add(choice.Label);
						}
					}

					JProperty property = question.Type switch
					{
						"text" => new JProperty("value", question.AnswerText),
						"number" => new JProperty("value", question.AnswerNumber),
						"date" => new JProperty("value", question.AnswerDate),
						"single" => new JProperty("value", singleChoiceAnswers.Find(c => c.SelectedChoiceId == question.SelectedChoiceId)?.Label),
						"multi" => new JProperty("choices", choicesArray),
						_ => new JProperty("value", "")
					};

					obj.Add(property);

					questionItems.Add(obj);
				}

				JArray questionsArray = JArray.FromObject(questionItems);

				var respondent = nonAnonymousRespondents.Find(r => r.RespondentId == submission.RespondentId);

				responseItems.Add(new JObject
				{
					["submission_id"] = submission.Id,
					["respondent_id"] = submission.RespondentId,
					["respondent_name"] = respondent?.Name,
					["respondent_email"] = respondent?.Email,
					["questions"] = questionsArray
				});
			}

			return JsonConvert.SerializeObject(responseItems);
		}
	}
}
