'use client'
import React, { useEffect, useState } from 'react';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import Select from '@mui/material/Select';
import FormControl from '@mui/material/FormControl';
import Box from '@mui/material/Box';

const AgeSelect = (props) => {
    // const [currentValue, setCurrentValue] = useState(props.currentValue);
    const options = getOptionsForRelationship(props.relationship);

    // // Check if currentValue is a valid option, and update it if not
    // useEffect(() => {
    //     console.log("current value " + currentValue)
    //     const isValidOption = options.some((option) => option.value === currentValue);
    //     if (!isValidOption) {
    //         console.log("options[0] value " + options[0].value)
    //         setCurrentValue(options[0].value);
    //         props.onChange(options[0].value);
    //     }
    // }, [options, currentValue, props.onChange, props.relationship]);

    // const handleOnChange = (event) => {
    //     const newValue = event.target.value;
    //     console.log("new value " + newValue)
    //     setCurrentValue(newValue);
    //     props.onChange(newValue);
    // };

    return (
        <Box sx={{ minWidth: '100%' }}>
            <FormControl fullWidth>
                <Select
                    style={{ backgroundColor: 'rgba(255, 255, 255, 0.50)' }}
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
};

const getOptionsForRelationship = (relationship) => {
    let options = [
        { value: "toddler", label: "0-2 years old" },
        { value: "preschool", label: "3-5 years old" },
        { value: "gradeschool", label: "6-12 years old" },
        { value: "middleSchool", label: "12-14 years old" },
        { value: "teens", label: "14-18 years old" },
        { value: "twenties", label: "19-30 years old" },
        { value: "thirties", label: "31-40 years old" },
        { value: "forties", label: "41-50 years old" },
        { value: "fifties", label: "51-60 years old" },
        { value: "sixties", label: "61-70 years old" },
        { value: "seventies", label: "70+ years old" },
    ];

    // if (relationship === "Parent" || relationship === "Grandparent" || relationship === "Colleague" || relationship === "Partner") {
    //     options = options.filter(option => option.value !== "Toddler" && option.value !== "Preschool" && option.value !== "MiddleSchool" && option.value !== "Gradeschool");
    // }

    // if (relationship === "Parent" || relationship === "Grandparent") {
    //     options = options.filter(option => option.value !== "Teens");
    // }

    // if (relationship === "Grandparent") {
    //     options = options.filter(option => option.value !== "Twenties" && option.value !== "Thirties" && option.value !== "Forties");
    // }

    return options;
};

export default AgeSelect;
