import styles from "../styles/Home.module.css";
import { useState } from "react";
import Router from "next/router";

const Home = () => {
  const [associatedRelationship, setAssociatedRelationship] = useState("Friend");
  const [prounoun, setProunoun] = useState("his");
  const [associatedInterests, setAssociatedInterests] = useState("camping,golfing");
  const [associatedAge, setAssociatedAge] = useState("twenties");
  const [maxPrice, setMaxPrice] = useState(50);

  const handleSubmit = async (event) => {
    event.preventDefault();
    await Router.push(`/giftsuggestions?associatedRelationship=${associatedRelationship}&pronoun=${prounoun}&associatedAge=${associatedAge}&associatedInterests=${associatedInterests}&maxPrice=${maxPrice}`);
  };

  return (
    <div className={styles.container}>
      <main className={styles.main}>
        <form className="form-inline" onSubmit={handleSubmit}>
          <div className="input-group form-group mb-3">
            <label>Relationship</label>
            <select className="form-select" onSelect={e => setAssociatedRelationship(e.target.value)} aria-label="Default select example">
              <option selected>Friend</option>
              <option value="Sister">Sister</option>
              <option value="Brother">Brother</option>
              <option value="Mother">Mother</option>
              <option value="Father">Father</option>
              <option value="Girlfriend">Girlfriend</option>
              <option value="Boyfriend">Boyfriend</option>
              <option value="Wife">Wife</option>
              <option value="Husband">Husband</option>
              <option value="Grandma">Grandma</option>
              <option value="Daughter">Daughter</option>
              <option value="Son">Son</option>
              <option value="Cousin">Cousin</option>
              <option value="Aunt">Aunt</option>
              <option value="Uncle">Uncle</option>
            </select>

            <label>Age</label>
            <select className="form-select" onSelect={e => setAssociatedAge(e.target.value)} aria-label="Default select example">
              <option selected>Twenties</option>
              <option value="Toddler">Toddler</option>
              <option value="Preschool">Preschool</option>
              <option value="Gradeschool">Gradeschool</option>
              <option value="MiddleSchool">MiddleSchool</option>
              <option value="Teens">Teens</option>
              <option value="Forties">Forties</option>
              <option value="Fifties">Fifties</option>
              <option value="Sixties">Sixties</option>
              <option value="Seventies">Seventies</option>
              <option value="Eighties">Eighties</option>
              <option value="Nineties">Nineties</option>
            </select>

            <label>Pronoun</label>
            <select className="form-select" onSelect={e => setProunoun(e.target.value)} aria-label="Default select example">
              <option selected>His</option>
              <option value="Her">Her</option>
              <option value="Their">Their</option>
            </select>

            <label htmlFor="interests">Interests separated by comma</label>
            <textarea className="form-control" placeholder="camping,golfing" onChange={e => setAssociatedInterests(e.target.value)} id="interests" rows="2"></textarea>

            <label htmlFor="maxPrice" className="form-label">Max Price: ${maxPrice}</label>
            <input type="range" className="form-range" onChange={e => setMaxPrice(e.target.value)} min="10" value={maxPrice} max="500" step="5" id="customRange3"></input>

            <div className="input-group-append">
              <button className="btn btn-primary" type="submit">
                Find Gifts!
              </button>
            </div>
          </div>
        </form>
      </main>
    </div >
  );
};

export default Home;
