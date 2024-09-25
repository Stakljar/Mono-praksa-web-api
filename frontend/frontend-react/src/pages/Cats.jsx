import { useEffect, useState } from "react";
import CatTable from "../components/CatTable";
import Button from "../components/Button";
import { useNavigate } from "react-router-dom";
import { deleteCat, getCats } from "../services/CatService";
import Spinner from "../components/Spinner";
import { HttpStatusCode } from "axios";

export default function Cats() {
  const navigate = useNavigate()

  const [cats, setCats] = useState([]);
  const [isLoading, setIsLoading] = useState(true)
  const [deleteId, setDeleteId] = useState(null)

  useEffect(() => {
    if (!isLoading) {
      return
    }
    const loadCats = async () => {
      const response = await getCats()
      if (response.status !== HttpStatusCode.Ok) {
        alert("Failed to retrieve cats");
        setIsLoading(false);
        return;
      }
      setCats(response.data.map((cat) => {
        return {
          id: cat.id,
          name: cat.name,
          age: cat.age,
          color: cat.color,
          shelterName: cat.catShelterName,
          arrivalDate: cat.arrivalDate
        }
      }))
      setIsLoading(false)
    }
    loadCats()
  }, [isLoading])

  useEffect(() => {
    if (!deleteId) {
      return
    }
    const remove = async () => {
      const response = await deleteCat(deleteId)
      if (response.status !== HttpStatusCode.NoContent) {
        alert("Failed to delete cat");
        return;
      }
      setIsLoading(true)
    }
    remove()
  }, [deleteId])

  return (
    <div>
      <Button className="themed-button" text="Add Cat" onClick={() => navigate("/cats/add")} />
      <h2 className="cat-table-title">Cats</h2>
      {
        !isLoading ?
          <CatTable cats={cats}
            deleteCat={(id) => {
              if (window.confirm("Are you sure you want to delete this cat from the list?")) {
                setDeleteId(id);
              }
            }}
          /> :
          <Spinner />
      }
    </div>
  )
}