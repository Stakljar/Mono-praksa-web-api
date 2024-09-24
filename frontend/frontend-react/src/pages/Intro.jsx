import { useNavigate } from "react-router-dom";

export default function Intro() {
    const navigate = useNavigate()
    return (
        <div className="intro-container">
          <div className="welcome-section">
            <h1 className="intro-title">Welcome to the Cat Shelter System</h1>
            <p className="intro-description">
              A safe haven for cats and a system to manage shelters efficiently.
            </p>
            <button className="themed-button" onClick={() => navigate("/cat_shelters")}>
              Explore Shelters
            </button>
          </div>
        </div>
      );
}