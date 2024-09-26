import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getCatShelter } from "../services/CatShelterService";
import Spinner from "../components/Spinner";
import { HttpStatusCode } from "axios";

export default function CatShelterInfo() {
  const [isLoading, setIsLoading] = useState(true);
  const { id } = useParams();
  const [shelter, setShelter] = useState({ id: id, name: "", location: "", establishedAt: "" });

  useEffect(() => {
    if (!isLoading) {
      return;
    }
    const loadShelter = async () => {
      const response = await getCatShelter(id);
      if (response.status !== HttpStatusCode.Ok) {
        alert("Failed to retrieve cat shelter data.");
        setIsLoading(false);
        return;
      }
      setIsLoading(false);
      setShelter({
        id: response.data.id,
        name: response.data.name,
        location: response.data.location,
        establishedAt: response.data.establishedAt
      });
    }
    loadShelter();
  }, [isLoading])

  return (
    <div>
      <div className="shelter-info-container">
        {
          !isLoading ?
            <>
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
            </> :
            <Spinner />
        }
      </div>
    </div>
  );
}
