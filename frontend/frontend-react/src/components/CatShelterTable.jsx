import { useNavigate } from "react-router-dom";
import Button from "./Button";

export default function CatShelterTable({ catShelters, deleteCatShelter }) {
  const navigate = useNavigate();

  return (
    <table id="catShelterTable">
      <thead>
        <tr>
          <th>Name</th>
          <th>Location</th>
          <th>Establishment Date</th>
          <th>Edit</th>
          <th>Delete</th>
        </tr>
      </thead>
      <tbody>
        {catShelters?.length > 0 ? (
          catShelters.map((catShelter) => (
            <tr key={catShelter.id}>
              <td onClick={() => navigate("/cat_shelters/" + catShelter.id)}>{catShelter.name}</td>
              <td>{catShelter.location}</td>
              <td>{catShelter.establishedAt}</td>
              <td><Button className="edit-btn" onClick={() => navigate("/cat_shelters/update/" + catShelter.id)} text="Edit" /></td>
              <td><Button className="delete-btn" onClick={() => deleteCatShelter(catShelter.id)} text="Delete" /></td>
            </tr>
          ))
        ) : (
          <tr>
            <td>No cat shelters available</td>
          </tr>
        )}
      </tbody>
    </table>
  )
}
