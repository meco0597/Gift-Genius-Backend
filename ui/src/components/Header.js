import Link from "next/link";
import styles from "../styles/Home.module.css";
import Image from "next/image";
import "react-widgets/styles.css";

const Header = ({ loggedInUser }) => {
  const hrefLinks = [
    { href: "/affiliate-disclosure", text: "Affiliate Disclosure" },
  ];

  return (
    <header className={styles.header}>
      <div className={styles.menu}>
        <nav className="navbar navbar-expand-lg">
          <div className={styles.navbar_links}>
            <Link href="/" title="Givr" className={styles.navbar_brand}>
              <Image
                className="my- w-full"
                src="/GivrLogo.png"
                alt="Givr Logo"
                width={73}
                height={47}
                priority
              />
            </Link>
            <Link style={{ color: "white", padding: 0, maxWidth: '110px' }} className="nav-link" href="/" key="/" title="Find gifts">
              <button className="btn btn-primary" href="/" type="submit">
                Find Gifts
              </button>
            </Link>

            {hrefLinks.map(({ href, text }) => (
              <Link style={{ fontSize: '14px', color: 'rgb(100, 100, 100)' }} className="nav-link" href={href} key={href} title={text}>
                {text}
              </Link>
            ))}
          </div>
        </nav>
      </div >
    </header >
  );
};

export default Header;
