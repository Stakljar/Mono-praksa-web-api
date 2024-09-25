import './App.css';
import Cats from './pages/Cats';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Intro from './pages/Intro';
import CatShelters from './pages/CatShelters';
import Error from './pages/Error';
import CatInfo from './pages/CatInfo';
import CatShelterInfo from './pages/CatShelterInfo';
import Main from './pages/Main';
import CatShelterAdd from './pages/CatShelterAdd';
import CatAdd from './pages/CatAdd';
import CatShelterUpdate from './pages/CatShelterUpdate';
import CatUpdate from './pages/CatUpdate';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Main />}>
            <Route index element={<Intro />} />
            <Route path="/cat_shelters" element={<CatShelters />} />
            <Route path="/cat_shelters/:id" element={<CatShelterInfo />} />
            <Route path="/cat_shelters/add" element={<CatShelterAdd />} />
            <Route path="/cat_shelters/update/:id" element={<CatShelterUpdate />} />
            <Route path="/cats" element={<Cats />} />
            <Route path="/cats/:id" element={<CatInfo />} />
            <Route path="/cats/add" element={<CatAdd />} />
            <Route path="/cats/update/:id" element={<CatUpdate />} />
            <Route path="*" element={<Error />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
