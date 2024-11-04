import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getCatShelter } from "../services/CatShelterService";
import { updateCatShelter } from "../services/CatShelterService";
import { HttpStatusCode } from "axios";
import Spinner from "../components/Spinner";

export default function CatShelterUpdate() {
  const navigate = useNavigate()
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitLoading, setIsSubmitLoading] = useState(false)
  const { id } = useParams();
  const [catShelter, setCatShelter] = useState({
    id: id,
    name: "",
    location: "",
    establishedAt: ""
  })

  useEffect(() => {
    if (!isLoading) {
      return;
    }
    const loadShelter = async () => {
      const response = await getCatShelter(id)
      if (response.status !== HttpStatusCode.Ok) {
        alert("Failed to retrieve data.");
        setIsLoading(false);
        return;
      }
      setCatShelter({
        id: response.data.id,
        name: response.data.name,
        location: response.data.location,
        establishedAt: response.data.establishedAt
      })
      setIsLoading(false);
    }
    loadShelter()
  }, [isLoading])

  useEffect(() => {
    if (!isSubmitLoading) {
      return;
    }
    const updateShelter = async () => {
      const response = await updateCatShelter(id, catShelter)
      if (response.status !== HttpStatusCode.NoContent) {
        alert("Failed to update cat shelter data.");
        setIsSubmitLoading(false);
        return;
      }
      navigate(-1);
    }
    updateShelter()
  }, [isSubmitLoading])

  const handleChange = (e) => {
    setCatShelter((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <div>
      <div>
        <form onSubmit={(e) => { e.preventDefault(); setIsSubmitLoading(true) }}>
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

          <input type="submit" className="themed-button" value="Update Cat Shelter" />

          {isLoading && isSubmitLoading && <Spinner />}
        </form>
      </div>
    </div>
  );
}
