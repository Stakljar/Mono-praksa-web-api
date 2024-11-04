import { useEffect, useState } from "react";
import { getCat, updateCat } from "../services/CatService";
import { useNavigate, useParams } from "react-router-dom";
import { HttpStatusCode } from "axios";
import { getCatSheltersWithoutCats } from "../services/CatShelterService";
import Spinner from "../components/Spinner";

export default function CatUpdate() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const { id } = useParams();
  const [isSubmitLoading, setIsSubmitLoading] = useState(false);
  const [cat, setCat] = useState({ id: id, name: "", age: "", color: "", arrivalDate: "", shelterId: "" });
  const [shelters, setShelters] = useState([]);

  useEffect(() => {
    if (!isLoading) {
      return;
    }
    const loadCatAndShelters = async () => {
      const [sheltersResponse, catResponse] = await Promise.all([getCatSheltersWithoutCats(id), getCat(id)]);
      if (catResponse.status !== HttpStatusCode.Ok || sheltersResponse.status !== HttpStatusCode.Ok) {
        alert("Failed to retrieve data.");
        setIsLoading(false);
        return;
      }
      setShelters(sheltersResponse.data.map((shelter) => { return { id: shelter.id, name: shelter.name } }));
      setCat({
        id: catResponse.data.id,
        name: catResponse.data.name,
        age: catResponse.data.age,
        color: catResponse.data.color,
        arrivalDate: catResponse.data.arrivalDate || "",
        shelterId: catResponse.data.catShelterId || ""
      });
      setIsLoading(false);
    }
    loadCatAndShelters();
  }, [isLoading])

  useEffect(() => {
    if (!isSubmitLoading) {
      return;
    }
    const update = async () => {
      const response = await updateCat(id, { ...cat, shelterId: cat.shelterId || null, arrivalDate: cat.arrivalDate || null });
      if (response?.status === HttpStatusCode.NoContent) {
        navigate(-1);
      }
      else {
        alert("Failed to update cat data.");
        setIsSubmitLoading(false);
      }
    }
    update();
  }, [isSubmitLoading])

  const handleChange = (e) => {
    setCat((prev) => { return { ...prev, [e.target.name]: e.target.value } });
  }

  return (
    <div>
      <div>
        <form onSubmit={(e) => {
          e.preventDefault();
          if ((!cat.shelterId && cat.arrivalDate) || (cat.shelterId && !cat.arrivalDate)) {
            alert('When assigning shelter both arrival date and shelter must be selected.');
            return;
          }
          setIsSubmitLoading(true);
        }}>
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
            value={cat?.arrivalDate}
            onChange={handleChange}
          /><br /><br />

          <input type="submit" className="themed-button" value="Update Cat" />

          {isLoading && isSubmitLoading && <Spinner />}
        </form>
      </div>
    </div>
  );
}
