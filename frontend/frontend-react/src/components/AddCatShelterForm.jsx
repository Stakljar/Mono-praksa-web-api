import { useState } from "react";

export default function AddCatShelterForm({ addCatShelter }) {
  const [catShelter, setCatShelter] = useState({ id: new Date().getMilliseconds(), name: "", location: "", establishedAt: "" })

  const handleSubmit = (e) => {
    e.preventDefault();
    addCatShelter(catShelter);
    setCatShelter({ name: "", location: "", establishedAt: "" });
  };

  const handleChange = (e) => {
    setCatShelter((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <form id="catShelterForm" onSubmit={handleSubmit}>
      <label htmlFor="catShelterName">Cat Shelter's Name:</label><br />
      <input
        type="text"
        id="catShelterName"
        name="name"
        value={catShelter.name}
        onChange={handleChange}
        placeholder="Enter cat shelter's name"
        required
      /><br /><br />

      <label htmlFor="catShelterLocation">Cat Shelter's Location:</label><br />
      <input
        type="text"
        id="catShelterLocation"
        name="location"
        value={catShelter.location}
        onChange={handleChange}
        placeholder="Enter cat shelter's location"
        required
      /><br /><br />

      <label htmlFor="catShelterEstablishedAt">Cat Shelter's Establishment Date:</label><br />
      <input
        type="date"
        id="catShelterEstablishedAt"
        name="establishedAt"
        value={catShelter.establishedAt}
        onChange={handleChange}
        placeholder="Enter cat shelter's date of establishment"
        required
      /><br /><br />

      <input type="submit" className="themed-button" value="Add Cat Shelter" />
    </form>
  );
}
