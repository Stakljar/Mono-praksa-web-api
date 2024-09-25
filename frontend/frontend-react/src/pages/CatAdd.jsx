import { useEffect, useState } from "react";
import { addCat } from "../services/CatService";
import { HttpStatusCode } from "axios";
import { useNavigate } from "react-router-dom";
import { getCatSheltersWithoutCats } from "../services/CatShelterService";
import Spinner from "../components/Spinner";

export default function CatAdd() {
  const navigate = useNavigate()
  const [shelters, setShelters] = useState([])
  const [cat, setCat] = useState({ name: "", age: "", color: "", arrivalDate: "", shelterId: "" })
  const [isLoading, setIsLoading] = useState(false)

  useEffect(() => {
    const getShelters = async () => {
      const response = await getCatSheltersWithoutCats()
      if (response?.status === HttpStatusCode.Ok) {
        setShelters(response.data.map((shelter) => { return { id: shelter.id, name: shelter.name } }))
      }
      else {
        alert("Failed to retrieve shelters")
      }
    }
    getShelters()
  }, [])

  useEffect(() => {
    if (!isLoading) {
      return
    }
    const add = async () => {
      const response = await addCat({...cat, shelterId: cat.shelterId || null, arrivalDate: cat.arrivalDate || null })
      if (response?.status === HttpStatusCode.Created) {
        navigate(-1)
      }
      else {
        alert("Failed to insert cat")
        setIsLoading(false)
      }
    }
    add()
  }, [isLoading])

  const handleChange = (e) => {
    setCat((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <div>
      <form onSubmit={(e) => {
        e.preventDefault();
        if ((!cat.shelterId && cat.arrivalDate) || (cat.shelterId && !cat.arrivalDate)) {
          alert('When assigning shelter both arrival date and shelter must be selected.');
          return;
        }
        setIsLoading(true)
      }}>
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

        <label htmlFor="catShelterNames">Cat's Shelter Name:</label><br />
        <select id="catShelterNames" name="shelterId" value={cat.shelterId} onChange={handleChange}>
          <option value="">-- Select Shelter --</option>
          {
            shelters.map((shelter) => {
              return <option key={shelter.id} value={shelter.id}>{shelter.name}</option>
            })
          }
        </select><br /><br />

        <label htmlFor="catArrivalDate">Cat's Shelter Arrival Date:</label><br />
        <input
          type="date"
          id="catArrivalDate"
          name="arrivalDate"
          value={cat.arrivalDate}
          onChange={handleChange}
          placeholder="Enter cat's shelter arrival date"
        /><br /><br />

        <input type="submit" className="themed-button" value="Add Cat" />
        { isLoading && <Spinner />}
      </form>
    </div>
  );
}
