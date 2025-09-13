import { useState } from 'react';


function SurveyScreen({ api, devServerUrl }) {
  const [content, setContent] = useState([])


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

  const survey = 
    <div>
      <h1>{content.survey.Title}</h1>
      <p>{content.Description}</p>
      <p>Status: {content.Status}</p>
    </div>
  ;


  return (
    <div>
      <p>The survey should be shown here</p>
      <button onClick={handleLoadSurvey}>Load survey</button>
      {survey}
    </div>
  );
}

export default SurveyScreen;