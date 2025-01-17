import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import AgeSelect from "../components/AgeSelect";
import MultiselectWithCreate from "../components/MultiselectWithCreate";
import PronounSelect from "../components/PronounSelect";
import RelationshipSelect from "../components/RelationshipSelect";
import Slider from '@mui/material/Slider';
import styles from "../styles/Home.module.css";
import { getListItemSecondaryActionClassesUtilityClass } from "@mui/material";
import HowItWorks from "../components/HowItWorks";
import SingleSelect from "../components/OccasionSelect";
import OccasionSelect from "../components/OccasionSelect";

const Home = () => {
  const router = useRouter();
  const [associatedRelationship, setAssociatedRelationship] = useState("friend");
  const [prounoun, setProunoun] = useState("he");
  const [associatedInterests, setAssociatedInterests] = useState("");
  const [associatedAge, setAssociatedAge] = useState("twenties");
  const [occasion, setOccasion] = useState("birthday");
  const [maxPrice, setMaxPrice] = useState(50);
  const [interestsFocus, setInterestsFocus] = useState(false);

  useEffect(() => {
    if (router.query.associatedRelationship) {
      setAssociatedRelationship(router.query.associatedRelationship);
    }
    if (router.query.pronoun) {
      setProunoun(router.query.pronoun);
    }
    if (router.query.associatedAge) {
      setAssociatedAge(router.query.associatedAge);
    }
    if (router.query.occasion) {
      setOccasion(router.query.occasion);
    }
    if (router.query.maxPrice) {
      setMaxPrice(parseInt(router.query.maxPrice));
    }
    if (router.query.associatedRelationship &&
      router.query.pronoun &&
      router.query.associatedAge &&
      router.query.maxPrice) {
      setInterestsFocus(true);
    }
  }, [router.query]);

  const handleSubmit = async (event) => {
    event.preventDefault();
    await router.push({
      pathname: '/giftsuggestions',
      query: {
        associatedRelationship: associatedRelationship,
        pronoun: prounoun,
        associatedAge: associatedAge,
        associatedInterests: associatedInterests,
        occasion: occasion,
        maxPrice: maxPrice,
      },
    });
  };

  return (
    <main className={styles.main}>
      <div style={{ textAlign: 'center', marginTop: '50px', marginBottom: '8px' }}>
        <h1 style={{ fontWeight: '400' }}>Give The Perfect Gift 🎁</h1>
        <h4>AI Powered Gift Suggestions</h4>
        <h5>Thoughtful Gifts, Made Easy</h5>
      </div>
      <div className={styles.container}>
        <form className="form-inline" style={{ maxWidth: '700px' }} onSubmit={handleSubmit}>
          <div className="input-group form-group">
            <label style={{ fontSize: '20px' }}>Who is this Gift For?</label>
            <RelationshipSelect
              currentValue={associatedRelationship}
              onChange={(e) => setAssociatedRelationship(e.target.value)}
            />

            <label>Sex:</label>
            <PronounSelect currentValue={prounoun} onChange={(e) => setProunoun(e.target.value)} />

            <label>Age:</label>
            <AgeSelect
              currentValue={associatedAge}
              relationship={associatedRelationship}
              onChange={(e) => setAssociatedAge(e.target.value)}
            />

            <label htmlFor="interests">Interests:</label>
            <MultiselectWithCreate setAssociatedInterests={setAssociatedInterests} focus={interestsFocus} />

            <label style={{ fontSize: '20px', marginTop: '32px' }}>What is the Occasion?</label>
            <OccasionSelect
              currentValue={occasion}
              onChange={(e) => setOccasion(e.target.value)}
            />

            <label style={{ fontSize: '20px', marginTop: '32px' }} htmlFor="maxPrice" className="form-label">What is your budget?: ${maxPrice}</label>
            <Slider
              aria-label="max-price"
              defaultValue={maxPrice}
              value={maxPrice}
              valueLabelDisplay="auto"
              step={10}
              size='large'
              onChange={(_, value) => setMaxPrice(value)}
              min={10}
              max={500}
            />

            <div style={{ alignContent: 'center', marginTop: '20px' }} className="input-group-append">
              {associatedInterests.length === 0 ? (
                <button className="btn btn-primary btn-lg" type="submit" disabled>
                  Find Gifts!
                </button>
              ) : (
                <button className="btn btn-primary btn-lg" type="submit">
                  Find Gifts!
                </button>
              )}
            </div>
          </div>
        </form>
      </div>
      <HowItWorks />
    </main>
  );
};

export default Home;
