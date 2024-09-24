import Button from "./Button";

export default function CatShelterTable({ catShelters, editCatShelter, deleteCatShelter }) {
  return (
    <table id="catShelterTable">
      <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th>Location</th>
          <th>Established Date</th>
          <th>Edit</th>
          <th>Delete</th>
        </tr>
      </thead>
      <tbody>
        {catShelters?.length > 0 ? (
          catShelters.map((catShelter, index) => (
            <tr key={index}>
              <td>{catShelter.id}</td>
              <td>{catShelter.name}</td>
              <td>{catShelter.location}</td>
              <td>{catShelter.establishedAt}</td>
              <td><Button className="edit-btn" onClick={() => editCatShelter(catShelter)} text="Edit" /></td>
              <td><Button className="delete-btn" onClick={() => deleteCatShelter(catShelter)} text="Delete" /></td>
            </tr>
          ))
        ) : (
          <tr>
            <td>No Cat Shelters available</td>
          </tr>
        )}
      </tbody>
    </table>
  )
}
