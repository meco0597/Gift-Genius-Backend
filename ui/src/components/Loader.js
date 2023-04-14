import Link from "next/link";
import { Jelly } from '@uiball/loaders'
import styles from "../styles/Home.module.css";
import { useEffect } from "react";

const Loader = () => {
  useEffect(() => {
    window.scrollTo(0, 0)
  }, [])

  return (
    <div className={styles.loader}>
      <Jelly
        size={50}
        speed={1.2}
        color="#3756A1"
      />
    </div>
  );
};

export default Loader;
