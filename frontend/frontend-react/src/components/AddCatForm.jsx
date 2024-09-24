import { useState } from "react";

export default function AddCatForm({ addCat }) {
  const [cat, setCat] = useState({ name: "", age: "", color: "" })

  const handleSubmit = (e) => {
    e.preventDefault();
    addCat(cat);
    setCat({ name: "", age: "", color: "" });
  };

  const handleChange = (e) => {
    setCat((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <form id="catForm" onSubmit={handleSubmit}>
      <label htmlFor="catName">Cat's Name:</label><br />
      <input
        type="text"
        id="catName"
        name="name"
        value={cat.name}
        onChange={handleChange}
        placeholder="Enter cat's name"
        required
      /><br /><br />

      <label htmlFor="catAge">Cat's Age:</label><br />
      <input
        type="number"
        id="catAge"
        name="age"
        value={cat.age}
        onChange={handleChange}
        min="0"
        max="35"
        placeholder="Enter cat's age"
        required
      /><br /><br />

      <label htmlFor="catColor">Cat's Color:</label><br />
      <input
        type="text"
        id="catColor"
        name="color"
        value={cat.color}
        onChange={handleChange}
        placeholder="Enter cat's color"
        required
      /><br /><br />

      <input type="submit" className="themed-button" value="Add Cat" />
    </form>
  );
}
