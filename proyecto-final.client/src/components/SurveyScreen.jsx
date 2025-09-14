import { useState } from 'react';


function SurveyScreen({ api, devServerUrl }) {
  const [content, setContent] = useState({});


  async function handleLoadSurvey() {
    const response = await api(`${devServerUrl}/api/survey`);
    const data = await response.json();
    setContent(data);

    console.log('Fetch successful');
  }


  //const survey = content.map(x => {
  //  const surveyField = x.survey.map(y => {
  //    return (
  //      <div>
  //        <h1>{y.Title}</h1>
  //        <p>{y.Description}</p>
  //        <p>Status: {y.Status}</p>
  //      </div>
  //    );
  //  });

  //  return surveyField;
  //});

  //const survey =
  //  <div>
  //    <h1>{content && content['survey']['Title']}</h1>
  //    <p>{content && content['survey']['Description']}</p>
  //    <p>Status: {content && content['survey']['Status']}</p>
  //  </div>
  //;

  let survey;
  let questions;
  let choices;

  //Object.keys(content).forEach(value => {
  //  if (value == 'survey') {
  //    survey =
  //      <div>
  //        <h1>{content[value]['Title']}</h1>
  //        <p>{content[value]['Description']}</p>
  //      </div>
  //      ;
  //  } else if (value == 'questions') {
  //    questions =
        
  //  }
  //});


  let form;
  if (Object.keys(content).length > 0) {
    const html = (content.questions || []).map(q => {
      let questionItem;

      if (q.Type === 'text') {
        questionItem = <input name={`q_${q.Id}`} placeholder="Tu respuesta" />;
      }
      if (q.Type === 'number') {
        const min = q.Min_value ?? '';
        const max = q.Max_value ?? '';
        questionItem = <input type="number" name={`q_${q.Id}`} min={min} max={max} />;
      }
      if (q.Type === 'date') {
        questionItem = <input type="date" name={`q_${q.Id}`} />;
      }
      if (q.Type === 'single') {
        questionItem = (q.Choices || []).map(c => {
          return (
            <>
              <label>
                <input type="radio" name={`q_${q.Id}`} value={c.Id} />
                  {c.Label}
              </label><br />
            </>
          );
        });
      }
      if (q.Type === 'multi') {
        questionItem = (q.Choices || []).map(c => {
          return (
            <>
              <label>
                <input type="checkbox" name={`q_${q.Id}`} value={c.Id} />
                  {c.Label}
              </label><br />
            </>
          );
        });
      }


      return (
        <fieldset>
          <legend>{q.Position}. {q.Text}</legend>

          <div>
            {questionItem}
          </div>
        </fieldset>
      );
    });

    form =
      <form id="encuesta-ia" method="post" action="#">
        <h1>{content['survey']['Title']}</h1>
        <p>{content['survey']['Description']}</p>
        {html}
      </form>

    ////////////////////
    console.log(content);
  }


  return (
    <div>
      <p>The survey should be shown here</p>
      <button onClick={handleLoadSurvey}>Load survey</button>
      {form}
    </div>
  );
}

export default SurveyScreen;