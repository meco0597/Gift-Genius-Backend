import Link from "next/link";
import styles from "../styles/Home.module.css";

const Header = ({ loggedInUser }) => {
  const hrefLinks = [
    { href: "/", text: "Find Gifts" },

    { href: "/about", text: "About" },

    { href: "/feedback", text: "Give us Feedback" },
  ];

  return (
    <header className={styles.header}>
      <div className={styles.menu}>
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
          <div className={styles.navbar_links}>
            <Link href="/" title="Givr" className={styles.navbar_brand}>
              Givr
            </Link>
            {hrefLinks.map(({ href, text }) => (
              <Link className="nav-link" href={href} key={href} title={text}>
                {text}
              </Link>
            ))}
          </div>
        </nav>
      </div>
    </header>
  );
};

export default Header;
