import React from 'react';

const AgeOption = ({ label, value }) => (
    <option value={value}>{label}</option>
);

const AgeSelect = (props) => (
    <select className="form-select" value={props.currentValue} onChange={props.onChange}>
        {options.map((option) => (
            <AgeOption
                key={option.value}
                label={option.label}
                value={option.value}
            />
        ))}
    </select>
);

const options = [
    { value: "Toddler", label: "0-2 years old" },
    { value: "Preschool", label: "3-5 years old" },
    { value: "Gradeschool", label: "6-12 years old" },
    { value: "MiddleSchool", label: "12-14 years old" },
    { value: "Teens", label: "14-18 years old" },
    { value: "Twenties", label: "19-30 years old" },
    { value: "Thirties", label: "31-40 years old" },
    { value: "Forties", label: "41-50 years old" },
    { value: "Fifties", label: "51-60 years old" },
    { value: "Sixties", label: "61-70 years old" },
    { value: "Seventies", label: "70+ years old" },
];

export default AgeSelect;
