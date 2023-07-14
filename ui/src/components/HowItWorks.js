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
    px: 2,
};

const number = {
    fontSize: 24,
    fontFamily: 'default',
    color: 'black',
    fontWeight: 'medium',
};

const image = {
    fontSize: 48,
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
                    mt: 5,
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
                                <Box sx={image}>💡</Box>
                                <h4 className='how-it-works-bold'>Gift Ideas, Made Easy</h4>
                                <h5>Finding gifts is hard. With just a few inputs, we use AI to curate incredibly unique gifts to help quickly and easily find the perfect gift for any occasion.</h5>
                            </Box>
                        </Grid>
                        <Grid item xs={12} md={4}>
                            <Box sx={item}>
                                <Box sx={image}>🧠</Box>
                                <h4 className='how-it-works-bold'>Harnessing the Power of AI</h4>
                                <h5>We use intelligent AI to give you gift giving super powers! Each recommendation is specifically generated to find you the most thoughtful gifts.</h5>
                            </Box>
                        </Grid>
                        <Grid item xs={12} md={4}>
                            <Box sx={item}>
                                <Box sx={image}>⏰</Box>
                                <h4 className='how-it-works-bold'>Save Time For What Matters</h4>
                                <h5>We make it easy to quickly search, find and order the perfect gift for anyone in your life, so you get back to doing what you love.</h5>
                            </Box>
                        </Grid>
                    </Grid>
                </div>
            </Container>
        </Box>
    );
}

export default HowItWorks;