import { height, width } from '@mui/system';
import * as React from 'react';
import StarRatings from 'react-star-ratings';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import IosShareRoundedIcon from '@mui/icons-material/IosShareRounded';
import Link from 'next/link';


const AmazonProductContent = ({ suggestion }) => {

    return (
        <div style={{
            display: 'flex',
            flexDirection: 'column',
            height: '100%',
            paddingLeft: '8px',
            width: '100%'
        }}>
            <div style={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'flex-start',
                height: '100%',
            }}>
                <Link style={{
                    fontSize: '11px'
                }} href={suggestion.link} target="_blank" >
                    {truncate(suggestion.title)}
                </Link>

                <div style={{
                    display: 'flex',
                    flexDirection: 'row',
                    paddingTop: '4px'
                }}>
                    <div style={{
                        display: 'flex',
                        fontSize: '10px',
                        color: 'gray',
                        alignItems: 'end'
                    }}>
                        <p style={{ fontSize: '10px', color: 'black' }}>{Math.round(suggestion.rating * 10) / 10}</p>

                        <StarRatings
                            rating={suggestion.rating}
                            starRatedColor="#f0a741"
                            numberOfStars={5}
                            starDimension='12px'
                            starSpacing="0px"
                            name='rating'
                        />
                        <p style={{ fontSize: '10px' }}>({suggestion.numOfReviews})</p>
                    </div>
                </div>
            </div >
            <div style={{
                display: 'flex',
                flexDirection: 'row',
                paddingTop: '4px',
                width: '100%',
                justifyContent: 'flex-end',
                gap: '2px',
                maxHeight: '50px',
            }}>
                <Button sx={{ flexGrow: '2', maxWidth: '200px', borderRadius: '10px', fontSize: "10px" }} size='small' href={suggestion.link} target="_blank" variant="contained">View</Button>
                <Button sx={{ borderRadius: '25px', textAlign: 'center', minWidth: '10px', maxWidth: '60px', color: "gray", fontSize: "10px" }} size='small' endIcon={<IosShareRoundedIcon />}></Button>            </div>
        </div>
    )
}

const truncate = (input) =>
    input?.length > 100 ? `${input.substring(0, 100)}...` : input;

export default AmazonProductContent;
