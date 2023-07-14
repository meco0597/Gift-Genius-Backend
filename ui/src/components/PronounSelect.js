import React from 'react';
import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';

const PronounSelect = (props) => (
    <ToggleButtonGroup
        sx={{ width: '100%', height: '50px', backgroundColor: 'rgba(255, 255, 255,   0.50)' }}
        value={props.currentValue} onChange={props.onChange}
        color='primary'
        exclusive
        aria-label="pronoun selection"
    >
        {options.map((option) => (
            <ToggleButton key={option.value} value={option.value} aria-label={option.value}>
                {option.label}
            </ToggleButton>))}
    </ToggleButtonGroup>
);

const options = [
    { label: 'Male', value: 'he' },
    { label: 'Female', value: 'she' },
    { label: 'Other', value: 'they' },
];

export default PronounSelect;
