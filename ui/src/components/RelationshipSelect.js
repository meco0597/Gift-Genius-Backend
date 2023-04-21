import React from 'react';

const RelationshipOption = ({ label, value }) => (
    <option value={value}>{label}</option>
);

const RelationshipSelect = (props) => (
    <select className="form-select" value={props.currentValue} onChange={props.onChange}>
        {options.map((option) => (
            <RelationshipOption
                key={option.value}
                label={option.label}
                value={option.value}
            />
        ))}
    </select>
);

const options = [
    { label: 'Friend', value: 'Friend' },
    { label: 'Sister', value: 'Sister' },
    { label: 'Brother', value: 'Brother' },
    { label: 'Mother', value: 'Mother' },
    { label: 'Father', value: 'Father' },
    { label: 'Girlfriend', value: 'Girlfriend' },
    { label: 'Boyfriend', value: 'Boyfriend' },
    { label: 'Wife', value: 'Wife' },
    { label: 'Husband', value: 'Husband' },
    { label: 'Grandma', value: 'Grandma' },
    { label: 'Daughter', value: 'Daughter' },
    { label: 'Son', value: 'Son' },
    { label: 'Cousin', value: 'Cousin' },
    { label: 'Aunt', value: 'Aunt' },
    { label: 'Uncle', value: 'Uncle' }
];

export default RelationshipSelect;
