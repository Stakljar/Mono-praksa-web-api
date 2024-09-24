import './App.css';
import CatTable from './components/CatTable';
import AddCatForm from './components/AddCatForm';
import UpdateCatModal from './components/UpdateCatModal';
import { useState } from 'react';

function App() {
  const [cats, setCats] = useState([
    { id: 1, name: 'Havoc', age: 3, color: 'black' },
    { id: 2, name: 'Zelg', age: 1, color: 'black' },
  ]);

  const [catToEdit, setCatToEdit] = useState(null);

  return (
    <div className="App">
      <AddCatForm addCat={(newCat) => {
        setCats([...cats, newCat]);
      }} />

      <h1 className="cat-table-title">Cats</h1>
      <CatTable cats={cats}
        editCat={(cat) => {
          setCatToEdit(cat);
        }}
        deleteCat={(cat) => {
          if (window.confirm("Are you sure you want to delete this cat from the list?")) {
            setCats((prevCats) => prevCats.filter(c => c.id !== cat.id));
          }
        }}
      />

      {
        catToEdit ?
          <UpdateCatModal
            isOpen={catToEdit}
            catToEdit={catToEdit}
            onClose={() => {
              setCatToEdit(null);
            }}
            updateCat={(updatedCat) => {
              setCats(cats.map((cat) => (cat.id === updatedCat.id ? updatedCat : cat)));
            }}
          /> : ""
      }
    </div>
  );
}

export default App;
