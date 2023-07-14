import React from 'react';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import Select from '@mui/material/Select';
import FormControl from '@mui/material/FormControl';
import Box from '@mui/material/Box';

const RelationshipSelect = (props) => (
    <Box sx={{ minWidth: '100%', mt: 2 }}>
        <FormControl fullWidth>
            <InputLabel id="relationship-select-label">Relationship</InputLabel>
            <Select
                sx={{ backgroundColor: 'rgba(255, 255, 255,   0.50)' }}
                labelId="relationship-select-label"
                id="demo-simple-select"
                value={props.currentValue}
                label="Relationship"
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
    { label: 'Friend', value: 'friend' },
    { label: 'Romantic Partner', value: 'partner' },
    { label: 'Sibling', value: 'sibling' },
    { label: 'Parent', value: 'parent' },
    { label: 'Child', value: 'child' },
    { label: 'Grandparent', value: 'grandparent' },
    { label: 'Relative', value: 'relative' },
    { label: 'Colleague', value: 'colleague' },
    { label: 'Acquaintance', value: 'acquaintance' },
];

export default RelationshipSelect;
