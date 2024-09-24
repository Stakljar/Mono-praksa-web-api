import './App.css';
import Cats from './pages/Cats';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Intro from './pages/Intro';
import CatShelters from './pages/CatShelters';
import Error from './pages/Error';

function App() {
  return (
    <div className="App">
      <nav class="navbar">
        <div class="navbar-logo">
          <a href="/cat_shelters">Cat Shelters</a>
        </div>
        <ul className="navbar-links">
          <li><a href="/">Intro</a></li>
          <li><a href="/cat_shelters">Cat shelters</a></li>
          <li><a href="/cats">Cats</a></li>
        </ul>
      </nav>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Intro />} />
          <Route path="/cat_shelters" element={<CatShelters />} />
          <Route path="/cats" element={<Cats />} />
          <Route path="*" element={<Error />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
