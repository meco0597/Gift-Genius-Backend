import { useEffect, useState } from 'react';
import Link from 'next/link';
import Image from 'next/image';
import PropTypes from 'prop-types';
import client from '../../helpers/axios-client';
import styles from '../../styles/Home.module.css';
import AmazonProductContent from '../../components/AmazonProductContent';
import LRUCache from 'lru-cache';
import { useExitIntent } from 'use-exit-intent'
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, Stack, TextField, Fab } from '@mui/material';
import EmailIcon from '@mui/icons-material/Email';
import CloseIcon from '@mui/icons-material/Close';
import axios from 'axios';

const cache = new LRUCache({
    max: 500,
    maxAge: 1000 * 60 * 4, // cache for 2 minutes
});

function shuffleArray(array) {
    const shuffledArray = [...array];
    for (let i = shuffledArray.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [shuffledArray[i], shuffledArray[j]] = [shuffledArray[j], shuffledArray[i]];
    }
    return shuffledArray;
}

function GiftSuggestions({ suggestions, recipientParameters }) {
    const [age, setAge] = useState(recipientParameters.associatedAge);
    const [relationship, setRelationship] = useState(recipientParameters.associatedRelationship);
    const [sex, setSex] = useState(recipientParameters.pronoun);
    const [interests, setInterests] = useState(recipientParameters.associatedInterests);
    const [maxPrice, setMaxPrice] = useState(recipientParameters.maxPrice);
    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);
    const [email, setEmail] = useState('');
    const [emailError, setEmailError] = useState('');
    const { registerHandler } = useExitIntent({
        "desktop": {
            "triggerOnIdle": false,
            "useBeforeUnload": true,
            "triggerOnMouseLeave": true
        },
        "mobile": {
            "triggerOnIdle": false
        }
    })

    const handleEmailChange = (event) => {
        setEmail(event.target.value);
    };

    const handleSubmit = async () => {
        if (validateEmail(email)) {
            try {
                console.log("email: ", email)
                const sendEmailResponse = await axios.post('/api/sendgrid', {
                    email: email,
                    suggestions: suggestions,
                });

                const updateContactsResponse = await axios.put('/api/sendgrid-update-contacts', {
                    contacts: [
                        {
                            email: email,
                            custom_fields: {
                                w1_T: relationship,
                                w2_T: age,
                                w3_T: sex,
                                w4_T: interests.join(','),
                                w5_T: maxPrice,
                            },
                        },
                    ],
                });

                // Reset the email input field
                setEmail('');
                // Close the modal
                handleClose();
            } catch (error) {
                console.error('Error submitting email:', error);
            }
        } else {
            setEmailError('Invalid email address');
        }
    };

    const validateEmail = (email) => {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    };

    const handleModalClose = () => {
        setEmail('');
        setEmailError('');
        handleClose();
    };

    registerHandler({
        id: 'openModal',
        handler: () => setOpen(true)
    })

    if (!suggestions || suggestions.length === 0) {
        return (
            <div style={{ display: 'flex', flexDirection: 'column' }}>
                <h1 style={{ marginTop: '20px', textAlign: 'center' }}>No gift suggestions found</h1>
                <Link href="/">
                    <p style={{ textAlign: 'center', color: '#0066cc' }}>Try Again</p>
                </Link>
            </div>
        );
    }

    return (
        <>
            <div className={styles.floatingActionButton}>
                <Fab color="secondary" variant='extended' aria-label="Add" onClick={handleOpen}>
                    <EmailIcon sx={{ mr: 1 }} />
                    Save These Results
                </Fab>
            </div>

            <Dialog
                open={open} onClose={handleModalClose} fullWidth maxWidth="sm">
                <DialogTitle sx={{ textAlign: 'center', display: 'flex', justifyContent: 'space-between' }} disableTypography className={styles.dialogTitle}>
                    <div style={{ marginLeft: '5%' }}>
                        <h2>Don&apos;t lose your gift suggestions!</h2>
                        <h5>Email yourself a copy</h5>
                    </div>
                    <IconButton onClick={handleModalClose} style={{ ml: 'auto', width: 40, height: 40 }}>
                        <CloseIcon color="primary" />
                    </IconButton>
                </DialogTitle>

                <DialogContent>
                    <Stack spacing={2} margin={1}>
                        <TextField
                            label="Email"
                            value={email}
                            onChange={handleEmailChange}
                            fullWidth
                            error={Boolean(emailError)}
                            helperText={emailError}
                        />
                        <Button variant="contained" onClick={handleSubmit} sx={{ mt: 2 }}>
                            Submit
                        </Button>
                    </Stack>
                </DialogContent>
            </Dialog>

            <div style={{ display: 'flex', flexDirection: 'column', backgroundColor: 'rgba(255, 255, 255, 0.85)' }}>
                <div style={{ display: 'flex', flexDirection: 'column' }}>

                    <div style={{ marginTop: '20px', textAlign: 'center', marginBottom: '8px' }}>
                        <h1>Your Gift Ideas!</h1>
                        <h3>Explore these curated gift ideas, generated specifically for you.</h3>
                        <Link href={`/?maxPrice=${recipientParameters.maxPrice}&associatedRelationship=${recipientParameters.associatedRelationship}&pronoun=${recipientParameters.pronoun}&associatedAge=${recipientParameters.associatedAge}`}>Search again with new interests</Link>
                    </div>

                    <div className={styles.suggestions}>
                        {suggestions.map((suggestion) => (
                            <div title={suggestion.title} key={suggestion.title} className={styles.card}>
                                <div style={{ width: '50%', height: '0', paddingBottom: '40%', position: 'relative' }}>
                                    <Image
                                        src={suggestion.thumbnailUrl}
                                        alt={suggestion.title}
                                        style={{ objectFit: 'contain', position: 'absolute', top: '0', left: '0', width: '100%', height: '100%' }}
                                        width={150}
                                        height={150}
                                        href={suggestion.link}
                                        target="_blank"
                                        priority
                                    />
                                </div>
                                <AmazonProductContent suggestion={suggestion} />
                            </div>
                        ))}
                    </div>

                </div >
                <div className={styles.tryAgainCard}>
                    <p style={{ fontSize: '16px' }}>Not what you&apos;re looking for?</p>
                    <Link
                        style={{ textDecoration: 'none' }}
                        href={`/?maxPrice=${recipientParameters.maxPrice}&associatedRelationship=${recipientParameters.associatedRelationship}&pronoun=${recipientParameters.pronoun}&associatedAge=${recipientParameters.associatedAge}`}>
                        <button className="btn btn-primary" style={{ padding: '10px 20px', fontSize: '14px', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Try Different Interests</button>
                    </Link>
                </div>
            </div >
        </>
    );
}

GiftSuggestions.propTypes = {
    suggestions: PropTypes.arrayOf(
        PropTypes.shape({
            title: PropTypes.string.isRequired,
            thumbnailUrl: PropTypes.string.isRequired,
            link: PropTypes.string.isRequired,
        })
    ).isRequired,
    recipientParameters: PropTypes.shape({
        maxPrice: PropTypes.string.isRequired,
        associatedAge: PropTypes.string.isRequired,
        associatedRelationship: PropTypes.string.isRequired,
        associatedInterests: PropTypes.string.isRequired,
        pronoun: PropTypes.string.isRequired,
    }).isRequired,
};

export async function getServerSideProps(context) {
    const { query, res } = context;
    const { associatedRelationship, pronoun, associatedAge, associatedInterests, maxPrice } = query;

    if (!associatedRelationship || !pronoun || !associatedAge || !associatedInterests || !maxPrice) {
        res.writeHead(301, { Location: '/' });
        res.end();
        return { props: {} };
    }

    // Check if the cache already contains the requested data   
    const cacheKey = JSON.stringify(query);
    const cachedResults = cache.get(cacheKey);
    if (cachedResults) {
        return { props: { suggestions: cachedResults } };
    }

    try {
        const response = await client.post(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/giftsuggestions/amazon/generate`, {
            associatedRelationship,
            pronoun,
            associatedAge,
            associatedInterests: associatedInterests.split(','),
            maxPrice,
        });

        var suggestions = shuffleArray(response.data);

        // Cache the response data
        cache.set(suggestions, response.data);

        return {
            props: {
                suggestions: suggestions || [],
                recipientParameters: {
                    maxPrice: maxPrice,
                    associatedAge: associatedAge,
                    associatedRelationship: associatedRelationship,
                    associatedInterests: associatedInterests.split(','),
                    pronoun: pronoun,
                }
            },
        };
    } catch (error) {
        console.error('Error fetching gift suggestions:', error);
        return {
            notFound: true
        }
    }
}

export default GiftSuggestions;
