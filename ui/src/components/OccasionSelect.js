import React from 'react';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import Select from '@mui/material/Select';
import FormControl from '@mui/material/FormControl';
import Box from '@mui/material/Box';

const OccasionSelect = (props) => (
    <Box sx={{ minWidth: '100%', mt: 1 }}>
        <FormControl fullWidth>
            <Select
                sx={{ backgroundColor: 'rgba(255, 255, 255,   0.50)' }}
                labelId="occasion-select-label"
                id="demo-simple-select"
                value={props.currentValue}
                onChange={props.onChange}
            >
                {options.map((option) => (
                    <MenuItem key={option.value} value={option.value}>{option.label}</MenuItem>
                ))}
            </Select>
        </FormControl>
    </Box>
);

const options = [
    { label: 'Birthday', value: 'birthday' },
    { label: 'Anniversary', value: 'anniversary' },
    { label: 'Christmas', value: 'christmas' },
    { label: 'Valentine\'s Day', value: 'valentines day' },
    { label: 'Wedding', value: 'wedding' },
    { label: 'Graduation', value: 'graduation' },
    { label: 'Mother\'s Day', value: 'mothers day' },
    { label: 'Father\'s Day', value: 'fathers day' },
    { label: 'Baby Shower', value: 'baby shower' },
    { label: 'Housewarming', value: 'housewarming' },
    { label: 'Thanksgiving', value: 'thanksgiving' },
    { label: 'New Year', value: 'new year' },
    { label: 'Easter', value: 'easter' },
    { label: 'Halloween', value: 'halloween' },
    { label: 'Engagement', value: 'engagement' },
    { label: 'Retirement', value: 'retirement' },
    { label: 'Promotion', value: 'promotion' },
    { label: 'Get Well Soon', value: 'get well soon' },
    { label: 'Friendship', value: 'friendship' },
    { label: 'Just Because', value: 'just because' },
];

export default OccasionSelect;
