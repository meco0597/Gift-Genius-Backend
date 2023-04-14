import * as React from 'react';
import { useState } from 'react';
import Autocomplete, {
    createFilterOptions
} from '@mui/material/Autocomplete'
import TextField from '@mui/material/TextField';
import { initialListOfInterests } from '../data/interestsDropdownList';
import { minWidth } from '@mui/system';

const filter = createFilterOptions();

const MultiselectWithCreate = ({ setAssociatedInterests }) => {
    const [value, setValue] = useState([]);
    const [interests, setInterests] = useState(initialListOfInterests);

    function handleCreate(name) {
        let newOption = { name, id: interests.length + 1 }

        // select new option
        setValue([...value, newOption]);

        // add new option to our dataset
        setInterests(data => [newOption, ...data])
    }



    return (
        <Autocomplete
            style={{ minWidth: '100%' }}
            multiple
            onChange={(event, value) => {
                setAssociatedInterests(value.join(','));
            }}
            id="size-medium-outlined-multi"
            size="medium"
            freeSolo={true}
            options={initialListOfInterests}
            noOptionsText="Hit Enter to Add Interest"
            autoSelect={true}
            filterOptions={(options, params) => {
                const filtered = filter(options, params);

                if (params.inputValue !== "") {
                    filtered.push(`${params.inputValue}`);
                }

                return filtered;
            }}
            renderInput={(params) => (
                <TextField {...params} />
            )}
        />
    )
}

export default MultiselectWithCreate;

