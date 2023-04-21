import styles from "../styles/Home.module.css";
import { useState } from "react";
import Router from "next/router";
import MultiselectWithCreate from "../components/MultiselectWithCreate";
import RelationshipSelect from "../components/RelationshipSelect";
import AgeSelect from "../components/AgeSelect";
import PronounSelect from "../components/PronounSelect";

const Home = () => {
  const [associatedRelationship, setAssociatedRelationship] = useState("Friend");
  const [prounoun, setProunoun] = useState("His");
  const [associatedInterests, setAssociatedInterests] = useState("");
  const [associatedAge, setAssociatedAge] = useState("Twenties");
  const [maxPrice, setMaxPrice] = useState(50);

  const handleSubmit = async (event) => {
    event.preventDefault();
    await Router.push(`/giftsuggestions?associatedRelationship=${associatedRelationship}&pronoun=${prounoun}&associatedAge=${associatedAge}&associatedInterests=${associatedInterests}&maxPrice=${maxPrice}`);
  };

  return (
    <>
      <div className="home-banner" style={{ textAlign: 'center', marginTop: '50px', marginBottom: '30px' }}>
        <h1>Give The Perfect Gift 🎁</h1>
        <h5>AI Powered Gift Suggestions That Simplify The Joy of Gift Giving.</h5>
      </div>
      <div className={styles.container}>
        <main className={styles.main}>

          <form className="form-inline" style={{ maxWidth: '700px' }} onSubmit={handleSubmit}>
            <div className="input-group form-group mb-3">
              <label style={{ fontSize: '20px' }}>Who is this Gift For?</label>
              <RelationshipSelect currentValue={associatedRelationship} onChange={(e) => setAssociatedRelationship(e.target.value)} />

              <label>Pronoun:</label>
              <PronounSelect currentValue={prounoun} onChange={e => setProunoun(e.target.value)} />

              <label>Age:</label>
              <AgeSelect currentValue={associatedAge} onChange={e => setAssociatedAge(e.target.value)} />

              <label htmlFor="interests">Interests:</label>
              <MultiselectWithCreate setAssociatedInterests={setAssociatedInterests} />

              <label htmlFor="maxPrice" className="form-label">Max Price: ${maxPrice}</label>
              <input type="range" className="form-range" onChange={e => setMaxPrice(e.target.value)} min="10" value={maxPrice} max="500" step="5" id="customRange3"></input>

              <div style={{ alignContent: 'center' }} className="input-group-append">
                {associatedInterests.length == 0 ?
                  <button className="btn btn-primary btn-lg" type="submit" disabled >
                    Find Gifts!
                  </button>
                  :
                  <button className="btn btn-primary btn-lg" type="submit" >
                    Find Gifts!
                  </button>
                }
              </div>
            </div>
          </form>
        </main>
      </div >
    </>
  );
};


export default Home;
