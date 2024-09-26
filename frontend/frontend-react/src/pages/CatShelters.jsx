import { useEffect, useRef, useState } from "react";
import CatShelterTable from "../components/CatShelterTable";
import { useNavigate } from "react-router-dom";
import Button from "../components/Button";
import { deleteCatShelter, getCatShelters } from "../services/CatShelterService";
import { HttpStatusCode } from "axios";
import Spinner from "../components/Spinner";

export default function CatShelters() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [deleteId, setDeleteId] = useState(null);
  const [catShelters, setCatShelters] = useState([]);
  const [name, setName] = useState("");
  const pageNumberRef = useRef(1);
  const isNewSearchRef = useRef(true);

  useEffect(() => {
    if (!isLoading) {
      return;
    }
    const loadCatShelters = async () => {
      const response = await getCatShelters({ catShelterName: name, pageSize: 10, pageNumber: pageNumberRef.current });
      if (response.status !== HttpStatusCode.Ok) {
        alert("Failed to retrieve cat shelters.");
        setIsLoading(false);
        return;
      }
      const newCatShelters = response.data.map((cs) => {
        return {
          id: cs.id,
          name: cs.name,
          location: cs.location,
          establishedAt: cs.establishedAt
        };
      })
      setCatShelters((prev) => {
        return isNewSearchRef.current ? newCatShelters : [...prev, ...newCatShelters];
      })
      setIsLoading(false);
    }
    loadCatShelters();
  }, [isLoading])

  useEffect(() => {
    if (!deleteId) {
      return;
    }
    const remove = async () => {
      const response = await deleteCatShelter(deleteId)
      if (response.status !== HttpStatusCode.NoContent) {
        alert("Failed to delete cat shelter.");
        return;
      }
      setIsLoading(true);
    }
    remove();
  }, [deleteId])

  return (
    <div id="catShelters">
      <Button className="themed-button" text="Add Cat shelter" onClick={() => navigate("/cat_shelters/add")} />
      <h2 className="cat-shelter-table-title">Cat Shelters</h2>
      <div className="search-input">
        <input type="text" placeholder="Enter cat shelter's name" value={name} onChange={(e) => setName(e.target.value)} />
        <Button onClick={() => { pageNumberRef.current = 1; isNewSearchRef.current = true; setIsLoading(true); }} text="Search" />
      </div>
      {
        !isLoading ?
          <CatShelterTable catShelters={catShelters}
            deleteCatShelter={(id) => {
              if (window.confirm("Are you sure you want to delete this cat shelter?")) {
                setDeleteId(id);
              }
            }}
          /> :
          <Spinner />
      }
      <br />
      <Button className="themed-button" text="Load more" onClick={() => {
        pageNumberRef.current = pageNumberRef.current + 1;
        isNewSearchRef.current = false;
        setIsLoading(true);
      }} />
    </div>
  )
}
