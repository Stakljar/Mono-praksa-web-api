import './App.css';
import Cats from './pages/Cats';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Intro from './pages/Intro';
import CatShelters from './pages/CatShelters';
import Error from './pages/Error';
import CatInfo from './pages/CatInfo';
import CatShelterInfo from './pages/CatShelterInfo';
import Main from './pages/Main';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Main />}>
            <Route index element={<Intro />} />
            <Route path="/cat_shelters" element={<CatShelters />} />
            <Route path="/cat_shelters/:id" element={<CatShelterInfo />} />
            <Route path="/cats" element={<Cats />} />
            <Route path="/cats/:id" element={<CatInfo />} />
            <Route path="*" element={<Error />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
