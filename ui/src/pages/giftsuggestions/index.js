import { useEffect, useState } from 'react';
import Link from 'next/link';
import Image from 'next/image';
import PropTypes from 'prop-types';
import client from '../../helpers/axios-client';
import styles from '../../styles/Home.module.css';
import AmazonProductContent from '../../components/AmazonProductContent';
import LRUCache from 'lru-cache';
import { useExitIntent } from 'use-exit-intent'
import { Modal, Box, Typography, TextField, Button } from '@mui/material';


const cache = new LRUCache({
    max: 500,
    maxAge: 1000 * 60 * 2, // cache for 2 minutes
});

function shuffleArray(array) {
    const shuffledArray = [...array];
    for (let i = shuffledArray.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [shuffledArray[i], shuffledArray[j]] = [shuffledArray[j], shuffledArray[i]];
    }
    return shuffledArray;
}

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: '95%',
    maxWidth: 400,
    bgcolor: 'rgba(250, 250, 250, 0.95);',
    borderRadius: '25px',
    boxShadow: 24,
    p: 4,
};

function GiftSuggestions({ suggestions, recipientParameters }) {
    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);
    const [email, setEmail] = useState('');
    const [emailError, setEmailError] = useState('');
    const { registerHandler } = useExitIntent({
        "cookie": {
            "daysToExpire": 30,
            "key": "use-exit-intent"
        },
        "desktop": {
            "triggerOnIdle": true,
            "useBeforeUnload": true,
            "triggerOnMouseLeave": true,
            "delayInSecondsToTrigger": 3
        },
        "mobile": {
            "triggerOnIdle": true,
            "delayInSecondsToTrigger": 3
        }
    })

    const handleEmailChange = (event) => {
        setEmail(event.target.value);
    };

    const handleSubmit = () => {
        if (validateEmail(email)) {
            // Perform email submission logic here
            console.log('Email submitted:', email);
            // Reset the email input field
            setEmail('');
            // Close the modal
            handleClose();
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
            <Modal
                open={open}
                onClose={handleModalClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <Typography sx={{ textAlign: 'center' }} id="modal-modal-title" variant="h4" component="h2">
                        Don&apos;t lose your gift suggestions!
                    </Typography>
                    <TextField
                        label="Email"
                        value={email}
                        onChange={handleEmailChange}
                        fullWidth
                        sx={{ mt: 2 }}
                        error={Boolean(emailError)}
                        helperText={emailError}
                    />
                    <Button variant="contained" onClick={handleSubmit} sx={{ mt: 2 }}>
                        Submit
                    </Button>
                    <Button
                        variant="outlined"
                        onClick={handleClose}
                        sx={{ mt: 1 }}
                    >
                        Cancel
                    </Button>
                </Box>
            </Modal>

            <div style={{ display: 'flex', flexDirection: 'column', backgroundColor: 'rgba(250, 250, 250,  0.85)' }}>
                <div style={{ display: 'flex', flexDirection: 'column' }}>
                    <h1 style={{ marginTop: '20px', textAlign: 'center' }}>Your Gift Ideas!</h1>
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
