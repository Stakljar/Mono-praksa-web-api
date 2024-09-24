import { useState } from "react";
import Button from "./Button";

export default function UpdateCatModal({ isOpen, catToEdit, onClose, updateCat }) {
  const [cat, setCat] = useState(catToEdit)

  const handleSubmit = (e) => {
    e.preventDefault();
    updateCat(cat);
    onClose();
  };

  const handleChange = (e) => {
    setCat((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  if (!isOpen) return null;

  return (
    <div className="modal">
      <div className="modal-content">
        <form onSubmit={handleSubmit}>
          <label htmlFor="catName">Cat's Name:</label><br />
          <input
            type="text"
            id="catName"
            name="name"
            value={cat?.name}
            onChange={handleChange}
            required
          /><br /><br />

          <label htmlFor="catAge">Cat's Age:</label><br />
          <input
            type="number"
            id="catAge"
            name="age"
            value={cat?.age}
            onChange={handleChange}
            min="0"
            max="35"
            required
          /><br /><br />

          <label htmlFor="catColor">Cat's Color:</label><br />
          <input
            type="text"
            id="catColor"
            name="color"
            value={cat?.color}
            onChange={handleChange}
            required
          /><br /><br />

          <label htmlFor="catShelterNames">Cat's Shelter Name:</label><br />
          <select id="catShelterNames" name="shelterName" value={cat.shelterName} onChange={handleChange}>
            <option value="" disabled>-- Select Shelter --</option>
            <option value="shelter">Shelter</option>
            <option value="shelter2">Shelter2</option>
          </select><br /><br />
          
          <label htmlFor="catArrivedAt">Cat's Shelter Arrival Date:</label><br />
          <input
            type="date"
            id="catArrivedAt"
            name="arrivedAt"
            value={cat.arrivedAt}
            onChange={handleChange}
          /><br /><br />

          <input type="submit" value="Update Cat" />
        </form>
        <Button onClick={onClose} text="Close" />
      </div>
    </div>
  );
}
