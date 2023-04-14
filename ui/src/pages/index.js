import styles from "../styles/Home.module.css";
import { useState } from "react";
import Router from "next/router";
import MultiselectWithCreate from "../components/MultiselectWithCreate";

const Home = () => {
  const [associatedRelationship, setAssociatedRelationship] = useState("Friend");
  const [prounoun, setProunoun] = useState("his");
  const [associatedInterests, setAssociatedInterests] = useState("camping,golfing,chicago bears");
  const [associatedAge, setAssociatedAge] = useState("twenties");
  const [maxPrice, setMaxPrice] = useState(50);

  const handleSubmit = async (event) => {
    event.preventDefault();
    await Router.push(`/giftsuggestions?associatedRelationship=${associatedRelationship}&pronoun=${prounoun}&associatedAge=${associatedAge}&associatedInterests=${associatedInterests}&maxPrice=${maxPrice}`);
  };

  return (
    <>
      <div className="home-banner" style={{ textAlign: 'center', marginTop: '50px' }}>
        <h1>Give The Perfect Gift 🎁</h1>
        <h5>AI Powered Gift Suggestions That Simplify The Joy of Gift Giving.</h5>
      </div>
      <div className={styles.container}>
        <main className={styles.main}>

          <form className="form-inline" style={{ maxWidth: '700px' }} onSubmit={handleSubmit}>
            <div className="input-group form-group mb-3">
              <h1></h1>
              <label>I&apos;m looking for a gift for...</label>
              <label>Relationship:</label>
              <select className="form-select" onChange={e => setAssociatedRelationship(e.target.value)} aria-label="Default select example">
                <option value="Friend">Friend</option>
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

              <label>Pronoun:</label>
              <select className="form-select" onChange={e => setProunoun(e.target.value)} aria-label="Default select example">
                <option value="His">His</option>
                <option value="Her">Her</option>
                <option value="Their">Their</option>
              </select>

              <label>Age:</label>
              <select className="form-select" value={"Twenties"} onChange={e => setAssociatedAge(e.target.value)} aria-label="Default select example">
                <option value="Toddler">Toddler</option>
                <option value="Preschool">Preschool</option>
                <option value="Gradeschool">Gradeschool</option>
                <option value="MiddleSchool">MiddleSchool</option>
                <option value="Teens">Teens</option>
                <option value="Twenties">Twenties</option>
                <option value="Thirties">Thirties</option>
                <option value="Forties">Forties</option>
                <option value="Fifties">Fifties</option>
                <option value="Sixties">Sixties</option>
                <option value="Seventies">Seventies</option>
                <option value="Eighties">Eighties</option>
                <option value="Nineties">Nineties</option>
              </select>

              <label htmlFor="interests">Interests:</label>
              <MultiselectWithCreate setAssociatedInterests={setAssociatedInterests} />

              <label htmlFor="maxPrice" className="form-label">Max Price: ${maxPrice}</label>
              <input type="range" className="form-range" onChange={e => setMaxPrice(e.target.value)} min="10" value={maxPrice} max="500" step="5" id="customRange3"></input>

              <div style={{ alignContent: 'center' }} className="input-group-append">
                <button className="btn btn-primary" type="submit">
                  Find Gifts!
                </button>
              </div>
            </div>
          </form>
        </main>
      </div >
    </>
  );
};

export default Home;
