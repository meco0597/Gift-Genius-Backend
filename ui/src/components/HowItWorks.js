import * as React from 'react';

import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Container from '@mui/material/Container';
import PersonSearchOutlinedIcon from '@mui/icons-material/PersonSearchOutlined';
import CardGiftcardOutlinedIcon from '@mui/icons-material/CardGiftcardOutlined';
import MemoryOutlinedIcon from '@mui/icons-material/MemoryOutlined';

const item = {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    px: 5,
};

const number = {
    fontSize: 24,
    fontFamily: 'default',
    color: 'black',
    fontWeight: 'medium',
};

const image = {
    height: 55,
    my: 4,
};

function HowItWorks() {
    return (
        <Box
            className='how-it-works'
            component="section"
        >
            <Container
                sx={{
                    mt: 10,
                    mb: 15,
                    position: 'relative',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <h1 className='how-it-works-bold' sx={{ mb: 14 }}>
                    How it works
                </h1>
                <div>
                    <Grid container spacing={5}>
                        <Grid item xs={12} md={4}>
                            <Box sx={item}>
                                <Box sx={image}><PersonSearchOutlinedIcon fontSize='large' /></Box>
                                <h4 className='how-it-works-bold'>Explore Personalized Gift Ideas</h4>
                                <h5>Our app simplifies the gift-giving process by allowing you to provide basic information and our advanced algorithm generates a curated list of gift suggestions tailored specifically to the recipient.</h5>
                            </Box>
                        </Grid>
                        <Grid item xs={12} md={4}>
                            <Box sx={item}>
                                <Box sx={image}><MemoryOutlinedIcon fontSize='large' /></Box>
                                <h4 className='how-it-works-bold'>Harnessing the Power of AI</h4>
                                <h5>We leverage the capabilities of cutting-edge AI technology, to provide intelligent and personalized gift recommendations.</h5>
                            </Box>
                        </Grid>
                        <Grid item xs={12} md={4}>
                            <Box sx={item}>
                                <Box sx={image}><CardGiftcardOutlinedIcon fontSize='large' /></Box>
                                <h4 className='how-it-works-bold'>Effortless Gift Inspiration</h4>
                                <h5>With just a few clicks, our app provides you with relevant gift ideas. You can easily explore different options, regenerate suggestions, and find the perfect gift without the hassle of endless searching.</h5>
                            </Box>
                        </Grid>
                    </Grid>
                </div>
            </Container>
        </Box>
    );
}

export default HowItWorks;