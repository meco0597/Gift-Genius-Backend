import "../styles/globals.css";
import styles from "../styles/Home.module.css";
import Header from "../components/Header";

import "bootstrap/dist/css/bootstrap.css";
import Head from "next/head";

const MyApp = ({ Component, pageProps }) => {
  return (
    <div>
      <Head>
        <title>Givr, your AI powered gift idea generator.</title>
        <meta
          name="description"
          content="Discuss and share thoughts or ideas"
        />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <Header />
      <Component {...pageProps} />

      <footer className={styles.footer}>
        <a
          href={"/"}
          target="_blank"
          rel="noopener noreferrer"
        >
          Powered by Givr{" "}
          <span className={styles.logo}>
          </span>
        </a>
      </footer>
    </div>
  );
};

export default MyApp;
