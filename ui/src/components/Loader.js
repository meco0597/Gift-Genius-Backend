import React, { useState, useEffect } from 'react';
import { Card, CardContent, Typography, LinearProgress } from '@mui/material';

const Loader = () => {
  const [progress, setProgress] = useState(0);
  const [headerIndex, setHeaderIndex] = useState(0);
  const [showHighDemand, setShowHighDemand] = useState(false);
  const headers = [
    "Asking the AI model for the best gift ideas...",
    "Finding unique and thoughtful gift suggestions...",
    "Unleashing the power of AI to curate amazing gifts...",
    "Preparing a personalized list of gift recommendations...",
    "Unlocking the secrets of perfect gift-giving...",
    "Crafting handpicked gift suggestions just for you...",
    "Harnessing the magic of AI to discover the perfect presents...",
    "Delivering gift ideas tailored to your preferences...",
    "Exploring a world of creativity to inspire your gifting..."
  ];

  useEffect(() => {
    const interval = setInterval(() => {
      setProgress((prevProgress) => Math.min(prevProgress + Math.random() * 15, 90));
    }, 1000);

    return () => {
      clearInterval(interval);
    };
  }, []);

  useEffect(() => {
    const headerInterval = setInterval(() => {
      setHeaderIndex((prevIndex) => (prevIndex + 1) % headers.length);
    }, 3000);

    return () => {
      clearInterval(headerInterval);
    };
  }, []);

  useEffect(() => {
    const timeout = setTimeout(() => {
      // Replace this with the logic to handle what happens if the page takes longer than 10 seconds to load.
      // For demonstration purposes, I'm just logging a message here.
      setShowHighDemand(true);
      console.log("Page load is taking longer than 10 seconds!");
    }, 13000);

    return () => {
      clearTimeout(timeout);
    };
  }, []);

  return (
    <div className="loader-container">
      <Card sx={{ backgroundColor: 'rgba(255, 255, 255, 0.5)', minWidth: '70vw', maxWidth: '90vw', borderColor: 'rgb(100, 100, 100)' }}>
        <CardContent>
          <Typography sx={{ animation: '$fade-in 1s linear infinite' }} variant="h7" color={'rbg(60, 60, 60)'} component="div" gutterBottom>
            {showHighDemand ? 'We are experiencing high demand at this moment. Please bear with us, awesome gift suggestions are incoming!' : headers[headerIndex]}
          </Typography>
          <div style={{ display: 'flex', alignItems: 'center' }}>
            <div style={{ flex: 4 }}>
              <LinearProgress
                sx={{
                  height: 10,
                  borderRadius: 5,
                  marginTop: 2,
                  '& .MuiLinearProgress-bar': {
                    borderRadius: 5,
                    backgroundImage: `linear-gradient(90deg, rgba(148,98,250,1) ${Math.min(100 - progress, 90)}%, rgba(0,77,252,1) 68%)`
                  }
                }}
                variant="determinate"
                value={progress}
              />
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default Loader;
