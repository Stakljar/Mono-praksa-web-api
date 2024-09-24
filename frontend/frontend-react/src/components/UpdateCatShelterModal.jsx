import { useState } from "react";
import Button from "./Button";

export default function UpdateCatShelterModal({ isOpen, catShelterToEdit, onClose, updateCatShelter }) {
  const [catShelter, setCatShelter] = useState(catShelterToEdit)

  const handleSubmit = (e) => {
    e.preventDefault();
    updateCatShelter(catShelter);
    onClose();
  };

  const handleChange = (e) => {
    setCatShelter((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  console.log(catShelterToEdit)
  if (!isOpen) return null;

  return (
    <div className="modal">
      <div className="modal-content">
        <form onSubmit={handleSubmit}>
          <label htmlFor="catShelterName">Cat Shelter's Name:</label><br />
          <input
            type="text"
            id="catShelterName"
            name="name"
            value={catShelter?.name}
            onChange={handleChange}
            required
          /><br /><br />

          <label htmlFor="catShelterLocation">Cat Shelter's Location:</label><br />
          <input
            type="text"
            id="catShelterLocation"
            name="location"
            value={catShelter?.location}
            onChange={handleChange}
            required
          /><br /><br />

          <label htmlFor="catShelterEstablishedAt">Cat Shelter's Establishment Date:</label><br />
          <input
            type="date"
            id="catShelterEstablishedAt"
            name="establishedAt"
            value={catShelter?.establishedAt}
            onChange={handleChange}
            required
          /><br /><br />

          <input type="submit" value="Update Cat Shelter" />
        </form>
        <Button onClick={onClose} text="Close" />
      </div>
    </div>
  );
}
