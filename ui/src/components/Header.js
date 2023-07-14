import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import Link from '@mui/material/Link';
import Image from 'next/image';
import styles from "../styles/Home.module.css";

const hrefLinks = [
  { href: '/contact-us', text: 'Contact Us', newTab: false },
  { href: '/affiliate-disclosure', text: 'Affiliate Disclosure', newTab: false },
  { href: 'https://docs.google.com/forms/d/e/1FAIpQLSePpR0KPMKca-iKRyiJ_Y7-hS9ZG3GoxYCmIyT4mbIySmA2vw/viewform', text: 'Submit Feedback', newTab: true },
];

function ResponsiveAppBar() {
  const [anchorElNav, setAnchorElNav] = React.useState(null);

  const handleOpenNavMenu = (event) => {
    setAnchorElNav(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  return (
    <AppBar className={styles.header} sx={{ backgroundColor: 'rgba(250, 250, 250, 0.6)' }} elevation={0} position="static">
      <Container maxWidth="xl">
        <Toolbar className={styles.navbar_links} disableGutters>

          <Link href="/" title="Givr" sx={{ display: { xs: 'none', md: 'flex' } }}>
            <Image
              sx={{ display: { xs: 'none', md: 'flex' } }}
              className="my- w-full"
              src="/GivrLogo.png"
              alt="Givr Logo"
              width={73}
              height={47}
              priority
            />
          </Link>

          <Box sx={{ flexGrow: 1, maxWidth: 100, display: { xs: 'flex', md: 'none' } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="black"
            >
              <MenuIcon />
            </IconButton>

            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'left',
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: 'block', md: 'none' },
              }}
            >
              {hrefLinks.map((page) => (
                <MenuItem key={page.text} onClick={handleCloseNavMenu}>
                  <Link
                    href={page.href}
                    key={page.href}
                    title={page.text}
                    textDecoration="none"
                    target={page.newTab ? '_blank' : '_self'}
                    color="textPrimary"
                    style={{
                      fontSize: '14px',
                      color: 'black',
                      textDecoration: 'none',
                    }}
                    underline="none"
                  >
                    {page.text}
                  </Link>
                </MenuItem>
              ))}
            </Menu>

          </Box>
          <Link href="/" title="Givr" sx={{ justifyContent: 'center', flexGrow: 2, display: { xs: 'flex', md: 'none' } }}>
            <Image
              className="my- w-full"
              src="/GivrLogo.png"
              alt="Givr Logo"
              width={73}
              height={47}
              sx={{ display: { xs: 'flex', md: 'none' }, mr: 1 }}
              priority
            />
          </Link>
          <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
            {hrefLinks.map((page) => (
              <Link
                href={page.href}
                key={page.href}
                title={page.text}
                textDecoration="none"
                color="textPrimary"
                target={page.newTab ? '_blank' : '_self'}
                style={{
                  fontSize: '14px',
                  color: 'black',
                  textDecoration: 'none',
                  marginLeft: '1rem',
                }}
                underline="none"
              >
                {page.text}
              </Link>
            ))}
          </Box>

          <Box sx={{
            flexGrow: 0,
            textAlign: 'right',
            justifyContent: 'center'
          }}>
            <Button
              variant="contained"
              href='/'
              disableElevation
              sx={{
                backgroundColor: "#3756A1",
                textTransform: 'none',
                maxWidth: 100,
                textAlign: 'center',
              }}
            >
              Find Gifts
            </Button>
          </Box>

        </Toolbar>
      </Container>
    </AppBar>
  );
}
export default ResponsiveAppBar;