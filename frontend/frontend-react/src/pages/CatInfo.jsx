import { useState } from "react";
import { useParams } from "react-router-dom";

export default function CatInfo() {
  const { id } = useParams();
  const [cat, setCat] = useState({ name: "Havoc", age: "3", color: "black", arrivalDate: "2023-02-02", shelterName: "name" })
  return (
    <div>
      <div className="cat-info-container">
        <h1 className="cat-name">{cat.name}</h1>
        <div className="cat-details">
          <h2>Cat Details</h2>
          <div className="detail">
            <strong>Name:</strong> {cat.name}
          </div>
          <div className="detail">
            <strong>Age:</strong> {cat.age} years old
          </div>
          <div className="detail">
            <strong>Color:</strong> {cat.color}
          </div>
          <div className="detail">
            <strong>Shelter:</strong> {cat.shelterName}
          </div>
          <div className="detail">
            <strong>Arrival Date:</strong> {cat.arrivalDate}
          </div>
        </div>
      </div>
    </div>
  );
}