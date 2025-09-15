import { useState, useEffect } from 'react';


function SurveyScreen({ onAnswersSubmitted, api, devServerUrl }) {
  const [content, setContent] = useState({});


  useEffect(() => {
    async function fetchSurvey() {
      const response = await api(`${devServerUrl}/api/survey`);
      const data = await response.json();

      setContent(data);
    }

    fetchSurvey();
  }, []);


  //async function handleLoadSurvey() {
  //  const response = await api(`${devServerUrl}/api/survey`);
  //  const data = await response.json();

  //  setContent(data);
  //}

  async function handleFormSubmit(e) {
    e.preventDefault();

    const form = e.target;
    const payload = [];

    // Group checkboxes by name so we can collect all selected values
    const checkboxesByName = {};

    Array.from(form.elements)
      .filter(el => el.name)
      .forEach(el => {
        const questionId = el.name.substring(2);

        if (el.type === "checkbox") {
          if (!checkboxesByName[questionId]) {
            checkboxesByName[questionId] = [];
          }
          if (el.checked) {
            checkboxesByName[questionId].push(el.value);
          }

        } else if (el.type === 'text') {
          payload.push({ question_id: questionId, value: el.value, type: el.type });
        } else if (el.type === 'number') {
          payload.push({ question_id: questionId, value: (el.value === '' ? null : el.value), type: el.type });
        } else if (el.type === 'date') {
          payload.push({ question_id: questionId, value: el.value, type: el.type });
        } else if (el.type === 'radio') {
          if (el.checked) {
            payload.push({ question_id: questionId, value: el.value, type: el.type });
          }
        } else if (el.type === 'checkbox') {
          if (el.checked) {
            payload.push({ question_id: questionId, value: el.value, type: el.type });
          }
        }
      });

    // Add checkbox groups to payload as "choices"
    for (const [questionId, choices] of Object.entries(checkboxesByName)) {
      payload.push({
        question_id: questionId,
        choices: choices,
        type: "checkbox"
      });
    }


    const response = await fetch(`${devServerUrl}/api/submit-answers`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    });

    if (response.status == 201 || response.status == 204) {
      onAnswersSubmitted();
    }
  } 


  let form;
  if (Object.keys(content).length > 0) {
    const html = (content.questions || []).map((q, i) => {
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
        questionItem = (q.Choices || []).map((c, i) => {
          return (
            <div key={i}>
              <label>
                <input type="radio" name={`q_${q.Id}`} value={c.Id} />
                  {c.Label}
              </label><br />
            </div>
          );
        });
      }
      if (q.Type === 'multi') {
        questionItem = (q.Choices || []).map((c, i) => {
          return (
            <div key={i}>
              <label key={i}>
                <input type="checkbox" name={`q_${q.Id}`} value={c.Id} />
                  {c.Label}
              </label><br />
            </div>
          );
        });
      }


      return (
        <fieldset key={i}>
          <legend>{q.Position}. {q.Text}</legend>

          <div>
            {questionItem}
          </div>
        </fieldset>
      );
    });

    form =
      <form id="encuesta" method="post" action={`${devServerUrl}/api/submit-answers`} onSubmit={handleFormSubmit}>
        <h1>{content['survey']['Title']}</h1>
        <p>{content['survey']['Description']}</p>
        {html}
        <button>Enviar respuesta</button>
      </form>
      ;
  }


  return (
    <div>
      {/*<p>The survey should be shown here</p>*/}
      {/*<button onClick={handleLoadSurvey}>Load survey</button>*/}
      {form}
    </div>
  );
}

export default SurveyScreen;