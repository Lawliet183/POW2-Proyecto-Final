import { useState } from 'react';


function SurveyScreen({ api, devServerUrl }) {
  const [content, setContent] = useState({});


  async function handleLoadSurvey() {
    const response = await api(`${devServerUrl}/api/survey`);
    const data = await response.json();
    setContent(data);

    console.log('Fetch successful');
  }

  async function handleFormSubmit(e) {
    e.preventDefault();

    const form = e.target;
    const formData = new FormData(form);
    const params = new URLSearchParams(formData);

    const response = await fetch(`${devServerUrl}/api/submit-answers`, {
      method: "POST",
      headers: { "Content-Type": "application/x-www-form-urlencoded" },
      body: params
    });

    if (response.status == 200) {
      console.log('sent successfully');
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