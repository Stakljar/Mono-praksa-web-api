import { useNavigate } from "react-router-dom";
import Button from "./Button";

export default function CatTable({ cats, deleteCat }) {
  const navigate = useNavigate();

  return (
    <table id="catTable">
      <thead>
        <tr>
          <th>Name</th>
          <th>Age</th>
          <th>Color</th>
          <th>Shelter Name</th>
          <th>Shelter Arrival Date</th>
          <th>Edit</th>
          <th>Delete</th>
        </tr>
      </thead>
      <tbody>
        {cats?.length > 0 ? (
          cats.map((cat) => (
            <tr key={cat.id} >
              <td onClick={() => navigate("/cats/" + cat.id)}>{cat.name}</td>
              <td>{cat.age}</td>
              <td>{cat.color}</td>
              <td>{cat.shelterName}</td>
              <td>{cat.arrivalDate}</td>
              <td><Button className="edit-btn" onClick={() => navigate("/cats/update/" + cat.id)} text="Edit" /></td>
              <td><Button className="delete-btn" onClick={() => deleteCat(cat.id)} text="Delete" /></td>
            </tr>
          ))
        ) : (
          <tr>
            <td>No cats available</td>
          </tr>
        )}
      </tbody>
    </table>
  )
}
