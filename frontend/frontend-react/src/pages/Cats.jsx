import { useEffect, useRef, useState } from "react";
import CatTable from "../components/CatTable";
import Button from "../components/Button";
import { useNavigate } from "react-router-dom";
import { deleteCat, getCats } from "../services/CatService";
import Spinner from "../components/Spinner";
import { HttpStatusCode } from "axios";

export default function Cats() {
  const navigate = useNavigate();
  const [cats, setCats] = useState([]);
  const [name, setName] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [deleteId, setDeleteId] = useState(null);
  const pageNumberRef = useRef(1);
  const isNewSearchRef = useRef(true);

  useEffect(() => {
    if (!isLoading) {
      return
    }
    const loadCats = async () => {
      const response = await getCats({ name: name, pageSize: 10, pageNumber: pageNumberRef.current })
      if (response.status !== HttpStatusCode.Ok) {
        alert("Failed to retrieve cats.");
        setIsLoading(false);
        return;
      }
      const newCats = response.data.map((cat) => {
        return {
          id: cat.id,
          name: cat.name,
          age: cat.age,
          color: cat.color,
          shelterName: cat.catShelterName,
          arrivalDate: cat.arrivalDate
        }
      })
      setCats((prev) => {
        return isNewSearchRef.current ? newCats : [...prev, ...newCats]
      })
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
        alert("Failed to delete cat.");
        return;
      }
      setIsLoading(true)
    }
    remove()
  }, [deleteId])

  return (
    <div id="cats">
      <Button className="themed-button" text="Add Cat" onClick={() => navigate("/cats/add")} />
      <h2 className="cat-table-title">Cats</h2>
      <div className="search-input">
        <input type="text" placeholder="Enter cat's name" value={name} onChange={(e) => setName(e.target.value) } />
        <Button onClick={() => { pageNumberRef.current = 1; isNewSearchRef.current = true; setIsLoading(true); }} text="Search" />
      </div>
      {
        !isLoading ?
          <CatTable cats={cats}
            deleteCat={(id) => {
              if (window.confirm("Are you sure you want to delete this cat?")) {
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
