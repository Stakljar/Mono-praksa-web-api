import { Outlet } from "react-router-dom";

export default function Main() {
  return (
    <>
      <nav className="navbar">
        <div className="navbar-logo">
          <a href="/cat_shelters">Cat Shelters</a>
        </div>
        <ul className="navbar-links">
          <li><a href="/">Intro</a></li>
          <li><a href="/cat_shelters">Cat shelters</a></li>
          <li><a href="/cats">Cats</a></li>
        </ul>
      </nav>
      <Outlet />
    </>
  )
}