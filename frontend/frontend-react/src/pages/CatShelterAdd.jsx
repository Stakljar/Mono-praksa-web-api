import { useEffect, useState } from "react";
import { addCatShelter } from "../services/CatShelterService";
import { AxiosHeaders, HttpStatusCode } from "axios";
import { useNavigate } from "react-router-dom";
import Spinner from "../components/Spinner";

export default function CatShelterAdd() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [catShelter, setCatShelter] = useState({ name: "", location: "", establishedAt: "" });

  useEffect(() => {
    if (!isLoading) {
      return
    }
    const add = async () => {
      const response = await addCatShelter(catShelter);
      if (response?.status === HttpStatusCode.Created) {
        navigate(-1);
      }
      else {
        alert("Failed to insert cat shelter.");
        setIsLoading(false);
      }
    }
    add();
  }, [isLoading])

  const handleChange = (e) => {
    setCatShelter((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <div>
      <form onSubmit={(e) => { e.preventDefault(); setIsLoading(true) }}>
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
        {isLoading && <Spinner />}
      </form>
    </div>
  );
}
