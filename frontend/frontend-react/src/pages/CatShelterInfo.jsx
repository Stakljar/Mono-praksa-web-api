import { useState } from "react";

export default function CatShelterInfo() {
  const [shelter, setShelter] = useState({
    name: "Happy Paws Shelter",
    location: "123 Main St, Paws City",
    establishedAt: "2010-06-15",
  });

  return (
    <div>
      <div className="shelter-info-container">
        <h1 className="shelter-name">{shelter.name}</h1>
        <div className="shelter-details">
          <h2>Shelter Information</h2>
          <div className="detail">
            <strong>Name:</strong> {shelter.name}
          </div>
          <div className="detail">
            <strong>Location:</strong> {shelter.location}
          </div>
          <div className="detail">
            <strong>Established:</strong> {shelter.establishedAt}
          </div>
        </div>
      </div>
    </div>
  );
}
