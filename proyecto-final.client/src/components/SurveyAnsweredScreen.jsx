function SurveyAnsweredScreen({ onReturnToMainMenu }) {
  return (
    <div>
      <p>Gracias por responder esta encuesta!</p>
      <button onClick={onReturnToMainMenu}>Regresar al menu principal</button>
    </div>
  );
}


export default SurveyAnsweredScreen;