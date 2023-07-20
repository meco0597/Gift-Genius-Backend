import "bootstrap/dist/css/bootstrap.css";
import "../styles/globals.css";
import '../styles/scss/global.scss'
import styles from "../styles/Home.module.css";
import Header from "../components/Header";
import Loader from "../components/Loader";
import { useEffect, useState } from 'react'
import Router from "next/router";
import CssBaseline from '@mui/material/CssBaseline';

import Head from "next/head";
import Link from "next/link";
import Script from 'next/script'

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
        <meta name="viewport" content="initial-scale=1, width=device-width" />


      </Head>
      <Header />

      {/* Google Analytics Tags */}
      <Script strategy="afterInteractive" src="https://www.googletagmanager.com/gtag/js?id=G-Z7QMWJWMD9" />
      <Script
        id='google-analytics'
        strategy="afterInteractive"
        dangerouslySetInnerHTML={{
          __html: `
            window.dataLayer = window.dataLayer || [];
            function gtag(){dataLayer.push(arguments);}
            gtag('js', new Date());
            gtag('config', 'G-Z7QMWJWMD9', {
              page_path: window.location.pathname,
            });
          `,
        }} />

      <CssBaseline />
      {loading ? (
        <Loader />
      ) : (
        <div className={styles.wrapper}>
          <Component {...pageProps} />
        </div>
      )}

      <footer className={styles.footer}>
        <div className="container-fluid py-3">
          <div className="row">
            <div className="col-md-6">
              <p className="text-muted mb-0">© Givr 2023. All rights reserved.</p>
            </div>
            <div className="col-md-6">
              <Link href="/affiliate-disclosure" className="text-muted">
                Affiliate Disclosure
              </Link>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default MyApp;
