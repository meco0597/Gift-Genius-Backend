import React from 'react';
import { Container, Typography, Button, Grid, Link } from '@mui/material';
import EmailIcon from '@mui/icons-material/Email';
import FacebookIcon from '@mui/icons-material/Facebook';
import TwitterIcon from '@mui/icons-material/Twitter';
import InstagramIcon from '@mui/icons-material/Instagram';

const ContactUsPage = () => {
    const containerStyle = {
        marginTop: '1.5rem',
        marginBottom: '1.5rem',
        textAlign: 'center',
    };

    const iconStyle = {
        marginRight: '0.5rem',
    };

    return (
        <Container maxWidth="sm" style={containerStyle}>
            <Typography variant="h4" component="h1" gutterBottom>
                Contact Us
            </Typography>
            <Typography variant="body1" gutterBottom>
                For gift sponsoring opportunities, please reach out to us at:
            </Typography>
            <Typography variant="body1" gutterBottom>
                <Link href="mailto:info@givr.ai" color="inherit" underline="none">
                    <EmailIcon style={iconStyle} />
                    info@givr.ai
                </Link>
            </Typography>
            <Typography variant="body1" gutterBottom>
                Connect with us on social media:
            </Typography>
            <Grid container justifyContent="center" spacing={2}>
                <Grid item>
                    <Button
                        component={Link}
                        href="#"
                        target="_blank"
                        rel="noopener"
                        startIcon={<FacebookIcon />}
                    >
                        Facebook
                    </Button>
                </Grid>
                <Grid item>
                    <Button
                        component={Link}
                        href="#"
                        target="_blank"
                        rel="noopener"
                        startIcon={<TwitterIcon />}
                    >
                        Twitter
                    </Button>
                </Grid>
                <Grid item>
                    <Button
                        component={Link}
                        href="https://www.instagram.com/givr.ai/"
                        target="_blank"
                        rel="noopener"
                        startIcon={<InstagramIcon />}
                    >
                        Instagram
                    </Button>
                </Grid>
            </Grid>
        </Container>
    );
};

export default ContactUsPage;
