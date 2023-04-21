import "bootstrap/dist/css/bootstrap.css";
import "../styles/globals.css";
import '../styles/scss/global.scss'
import styles from "../styles/Home.module.css";
import Header from "../components/Header";
import Loader from "../components/Loader";
import { useEffect, useState } from 'react'
import Router from "next/router";
import { Jelly } from '@uiball/loaders'

import Head from "next/head";

const MyApp = ({ Component, pageProps }) => {
  const [loading, setLoading] = useState(false);
  useEffect(() => {
    const start = () => {
      setLoading(true);
    };
    const end = () => {
      setLoading(false);
    };
    Router.events.on("routeChangeStart", start);
    Router.events.on("routeChangeComplete", end);
    Router.events.on("routeChangeError", end);
    return () => {
      Router.events.off("routeChangeStart", start);
      Router.events.off("routeChangeComplete", end);
      Router.events.off("routeChangeError", end);
    };
  }, []);

  return (
    <div>
      <Head>
        <title>Givr - AI Powered Gift Idea Generator</title>
        <meta
          name="description"
          content="Discuss and share thoughts or ideas"
        />
        <link rel="icon" href="/GivrLogoSmall.png" />
      </Head>
      <Header />

      {loading ? (
        <Loader />
      ) : (
        <Component {...pageProps} />
      )}

      <footer className={styles.footer}>
        <a
          href={"/affiliate-disclosure"}
          rel="noopener noreferrer"
        >
          Affiliate Disclosure{" "}
          <span className={styles.logo}>
          </span>
        </a>
      </footer>
    </div>
  );
};

export default MyApp;
