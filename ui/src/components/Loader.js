import Link from "next/link";
import { Jelly } from '@uiball/loaders'
import styles from "../styles/Home.module.css";

const Loader = () => {
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
