import { useState } from "react";
import AddCatShelterForm from "../components/AddCatShelterForm";
import CatShelterTable from "../components/CatShelterTable";
import UpdateCatShelterModal from "../components/UpdateCatShelterModal";

export default function CatShelters() {
  const [catShelters, setCatShelters] = useState([
    { id: 1, name: 'shelter', location: "location", establishedAt: '2022-02-02' },
    { id: 2, name: 'shelter', location: "location", establishedAt: '2022-02-02' },
  ]);

  const [catShelterToEdit, setCatShelterToEdit] = useState(null);

  return (
    <div>
      <AddCatShelterForm addCatShelter={(newCatShelter) => {
        setCatShelters([...catShelters, newCatShelter]);
      }} />

      <h1 className="cat-shelter-table-title">Cat Shelters</h1>
      <CatShelterTable catShelters={catShelters}
        editCatShelter={(catShelter) => {
          setCatShelterToEdit(catShelter);
        }}
        deleteCatShelter={(catShelter) => {
          if (window.confirm("Are you sure you want to delete this cat shelter from the list?")) {
            setCatShelters((prevCatShelters) => prevCatShelters.filter(cs => cs.id !== catShelter.id));
          }
        }}
      />

      {
        catShelterToEdit ?
          <UpdateCatShelterModal
            isOpen={catShelterToEdit}
            catShelterToEdit={catShelterToEdit}
            onClose={() => {
              setCatShelterToEdit(null);
            }}
            updateCatShelter={(updatedCatShelter) => {
              setCatShelters(catShelters.map((catShelter) => (catShelter.id === updatedCatShelter.id ? updatedCatShelter : catShelter)));
            }}
          /> : ""
      }
    </div>
  )
}