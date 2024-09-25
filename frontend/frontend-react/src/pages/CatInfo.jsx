import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getCat } from "../services/CatService";
import Spinner from "../components/Spinner";
import { HttpStatusCode } from "axios";

export default function CatInfo() {
  const [isLoading, setIsLoading] = useState(true)
  const { id } = useParams();
  const [cat, setCat] = useState({ id: id, name: "", age: "", color: "", arrivalDate: "", shelterName: "" })

  useEffect(() => {
    if (!isLoading) {
      return;
    }
    const loadCat = async () => {
      const response = await getCat(id)
      if (response.status !== HttpStatusCode.Ok) {
        alert("Failed to retrieve cat data")
        setIsLoading(false);
        return;
      }
      setCat({
        id: response.data.id,
        name: response.data.name,
        age: response.data.age,
        color: response.data.color,
        arrivalDate: response.data.arrivalDate,
        shelterName: response.data.catShelterName
      })
      setIsLoading(false)
    }
    loadCat()
  }, [isLoading])

  return (
    <div>
      <div className="cat-info-container">
        {
          !isLoading ?
            <>
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
            </> :
            <Spinner />
        }
      </div>
    </div>
  );
}