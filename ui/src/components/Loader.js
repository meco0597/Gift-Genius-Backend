import React, { useState, useEffect } from 'react';
import { Card, CardContent, Typography, LinearProgress } from '@mui/material';

const Loader = () => {
  const [progress, setProgress] = useState(0);
  const [headerIndex, setHeaderIndex] = useState(0);
  const headers = [
    "Asking the AI model for the best gift ideas...",
    "Finding unique and thoughtful gift suggestions...",
    "Unleashing the power of AI to curate amazing gifts...",
  ];

  useEffect(() => {
    const interval = setInterval(() => {
      setProgress((prevProgress) => prevProgress + Math.random() * 10);
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

  return (
    <div className="loader-container">
      <Card sx={{ backgroundColor: 'rgba(250, 250, 250,  0.5)', minWidth: '70vw', maxWidth: '90vw', borderColor: 'rgb(100, 100, 100)' }}>
        <CardContent>
          <Typography sx={{ animation: '$fade-in 1s linear infinite' }} variant="h7" color={'rgb(80, 80, 80)'} component="div" gutterBottom>
            {headers[headerIndex]}
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
                    backgroundImage: `linear-gradient(90deg, rgba(148,98,250,1) ${100 - progress}%, rgba(0,77,252,1) 68%)`
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
