import styles from "../../styles/AffiliateDisclosure.module.css";

export default function AffiliateDisclosure() {
  return (
    <>
      <main className={styles.container}>
        <h1>Affiliate Disclosure</h1>
        <p className={styles.paragraph}>Our website provides links to gift suggestions on Amazon.com. As an Amazon Associate, we earn from qualifying purchases made through these links. This means that we may earn a commission when you make a purchase through one of our affiliate links. However, this does not affect the price you pay for the product. We only recommend products that we believe will be of value to our readers, and we appreciate your support in helping us to continue providing high-quality gift suggestions. Thank you for visiting our website and for considering our recommendations.</p>
      </main>
    </>
  );
};