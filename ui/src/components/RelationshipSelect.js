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
    { label: 'Friend', value: 'friend' },
    { label: 'Partner', value: 'partner' },
    { label: 'Sibling', value: 'sibling' },
    { label: 'Parent', value: 'parent' },
    { label: 'Child', value: 'child' },
    { label: 'Grandparent', value: 'grandparent' },
    { label: 'Relative', value: 'relative' },
    { label: 'Colleague', value: 'colleague' },
    { label: 'Acquaintance', value: 'acquaintance' },
];

export default RelationshipSelect;
