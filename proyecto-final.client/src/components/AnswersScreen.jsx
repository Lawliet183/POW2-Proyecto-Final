import { useState, useEffect } from 'react';


function AnswersScreen({ onReturnToMainMenu, api, devServerUrl }) {
  const [data, setData] = useState([]);


  useEffect(() => {
    async function fetchAnswers() {
      const response = await api(`${devServerUrl}/api/get-answers`);
      const json = await response.json();

      setData(json);
    }

    fetchAnswers();
  }, []);


  const tableRows = data.map(survey => {
    const respondentData =
      <>
        <td>{survey.respondent_id || '?'}</td>
        <td>{survey.respondent_name || 'Anonimo'}</td>
        <td>{survey.respondent_email || 'Anonimo'}</td>
      </>
      ;

    const questions = survey.questions.slice();

    questions.sort((a, b) => a.question_id - b.question_id);

    const answers = questions.map(q => {
      if (q.type === "multi") {
        return (
          <td key={q.question_id}>{q.choices.join('; ')}</td>
        );
      } else {
        return (
          <td key={q.question_id}>{q.value}</td>
        );
      }
    });

    return (
      <tr key={survey.respondent_id}>
        {respondentData}
        {answers}
      </tr>
    )
  });


  const table =
    <table border="1" cellPadding="5" cellSpacing="0">
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Email</th>
          <th>Pregunta 1</th>
          <th>Pregunta 2</th>
          <th>Pregunta 3</th>
          <th>Pregunta 4</th>
          <th>Pregunta 5</th>
          <th>Pregunta 6</th>
          <th>Pregunta 7</th>
          <th>Pregunta 8</th>
          <th>Pregunta 9</th>
        </tr>
      </thead>
      <tbody>
        {tableRows}
      </tbody>
    </table>
    ;


  return (
    <div>
      {table}
      <button onClick={onReturnToMainMenu}>Regresar al menu principal</button>
    </div>
  );
}


export default AnswersScreen;