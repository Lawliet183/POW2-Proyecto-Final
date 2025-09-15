function AnswersScreen() {
  const table =
    <table border="1" cellpadding="5" cellspacing="0">
      <thead>
        <tr>
          <th>#</th>
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
        <tr>
          <td>1</td>
          <td>user1</td>
          <td>user1@example.com</td>
          <td>Answer 1</td>
          <td>Answer 2</td>
          <td>Answer 3</td>
          <td>Answer 4</td>
          <td>Answer 5</td>
          <td>Answer 6</td>
          <td>Answer 7</td>
          <td>Answer 8</td>
          <td>Answer 9</td>
        </tr>
        <tr>
          <td>2</td>
          <td>user2</td>
          <td>user2@example.com</td>
          <td>Answer 1</td>
          <td>Answer 2</td>
          <td>Answer 3</td>
          <td>Answer 4</td>
          <td>Answer 5</td>
          <td>Answer 6</td>
          <td>Answer 7</td>
          <td>Answer 8</td>
          <td>Answer 9</td>
        </tr>
      </tbody>
    </table>
    ;


  return (
    <div>
      {table}
    </div>
  );
}


export default AnswersScreen;