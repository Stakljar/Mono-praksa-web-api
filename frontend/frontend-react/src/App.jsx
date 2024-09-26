import './App.css';
import Cats from './pages/Cats';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
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
  const router = createBrowserRouter([
    {
      path: "/",
      element: <Main />,
      children: [
        {
          index: true,
          element: <Intro />,
        },
        {
          path: "cat_shelters",
          element: <CatShelters />,
        },
        {
          path: "cat_shelters/:id",
          element: <CatShelterInfo />,
        },
        {
          path: "cat_shelters/add",
          element: <CatShelterAdd />,
        },
        {
          path: "cat_shelters/update/:id",
          element: <CatShelterUpdate />,
        },
        {
          path: "cats",
          element: <Cats />,
        },
        {
          path: "cats/:id",
          element: <CatInfo />,
        },
        {
          path: "cats/add",
          element: <CatAdd />,
        },
        {
          path: "cats/update/:id",
          element: <CatUpdate />,
        },
        {
          path: "*",
          element: <Error />,
        },
      ],
    },
  ]);

  return (
    <div className="App">
      <RouterProvider router={router} />
    </div>
  );
}

export default App;
