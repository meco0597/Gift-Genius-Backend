import * as React from 'react';
import { useState, useEffect } from 'react';
import Autocomplete, {
    createFilterOptions
} from '@mui/material/Autocomplete'
import TextField from '@mui/material/TextField';
import { initialListOfInterests } from '../data/interestsDropdownList';
import { minWidth } from '@mui/system';

const filter = createFilterOptions();

const MultiselectWithCreate = ({ setAssociatedInterests, focus }) => {
    const [value, setValue] = useState([]);
    const [interests, setInterests] = useState(initialListOfInterests);
    let inputRef;

    useEffect(() => {
        if (focus) {
            inputRef.focus();
        }
    }, [focus]);

    function handleCreate(name) {
        let newOption = { name, id: interests.length + 1 }

        // select new option
        setValue([...value, newOption]);

        // add new option to our dataset
        setInterests(data => [newOption, ...data])
    }

    return (
        <Autocomplete
            multiple
            onChange={(event, value) => {
                handleCreate(value);
                setAssociatedInterests(value.join(','));
            }}
            id="tags-outlined"
            freeSolo={true}
            options={initialListOfInterests}
            noOptionsText="Hit Enter to Add Interest"
            autoSelect={true}
            autoHighlight={true}
            filterSelectedOptions
            fullWidth
            onInputChange={(event, newInputValue) => {
                if (newInputValue.endsWith(',')) {
                    event.target.blur()
                    event.target.focus()
                }
            }}
            filterOptions={(options, params) => {
                if (params.inputValue == "") {
                    return []
                }

                const filtered = filter(options, params);

                if (params.inputValue !== "") {
                    filtered.push(`${params.inputValue}`);
                }

                return filtered;
            }}
            renderInput={(params) => (
                <TextField {...params}
                    inputRef={input => {
                        inputRef = input;
                    }}
                    placeholder={value.length === 0 ? 'E.g. Cooking, Going on Hikes, Camping' : ''}
                />
            )}
        />
    )
}

export default MultiselectWithCreate;