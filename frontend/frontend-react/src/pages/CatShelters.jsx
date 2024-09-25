import { useEffect, useState } from "react";
import CatShelterTable from "../components/CatShelterTable";
import { useNavigate } from "react-router-dom";
import Button from "../components/Button";
import { deleteCatShelter, getCatShelters } from "../services/CatShelterService";
import { HttpStatusCode } from "axios";
import Spinner from "../components/Spinner";

export default function CatShelters() {
  const [isLoading, setIsLoading] = useState(true)
  const [deleteId, setDeleteId] = useState(null)
  const navigate = useNavigate()
  const [catShelters, setCatShelters] = useState([]);

  useEffect(() => {
    if(!isLoading) {
      return;
    }
    const loadCatShelters = async () => {
      const response = await getCatShelters()
      if (response.status !== HttpStatusCode.Ok) {
        alert("Failed to retrieve cat shelters");
        setIsLoading(false);
        return;
      }
      setCatShelters(response.data.map((cs) => {
        return {
          id: cs.id,
          name: cs.name,
          location: cs.location,
          establishedAt: cs.establishedAt
        }
      }))
    }
    loadCatShelters()
  }, [isLoading])

  useEffect(() => {
    if (!deleteId) {
      return
    }
    const remove = async () => {
      const response = await deleteCatShelter(deleteId)
      if (response.status !== HttpStatusCode.NoContent) {
        alert("Failed to delete cat shelter");
        return;
      }
      setIsLoading(true)
    }
    remove()
  }, [deleteId])

  return (
    <div>
      <Button className="themed-button" text="Add Cat shelter" onClick={() => navigate("/cat_shelters/add")} />
      <h2 className="cat-shelter-table-title">Cat Shelters</h2>
      {
        isLoading ?
          <CatShelterTable catShelters={catShelters}
            deleteCatShelter={(id) => {
              if (window.confirm("Are you sure you want to delete this cat shelter from the list?")) {
                setDeleteId(id);
              }
            }}
          /> :
          <Spinner />
      }
    </div>
  )
}