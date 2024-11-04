import { NavLink, Outlet } from "react-router-dom";

export default function Main() {
  return (
    <>
      <nav className="navbar">
        <div className="navbar-logo">
          <NavLink to="/cat_shelters">Cat Shelters</NavLink>
        </div>
        <ul className="navbar-links">
          <li><NavLink to="/">Intro</NavLink></li>
          <li><NavLink to="/cat_shelters">Cat shelters</NavLink></li>
          <li><NavLink to="/cats">Cats</NavLink></li>
        </ul>
      </nav>
      <Outlet />
    </>
  )
}
