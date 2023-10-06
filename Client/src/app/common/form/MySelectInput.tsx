import { useField } from 'formik';
import React from 'react';
import { DropdownProps, Form, Label, Select } from 'semantic-ui-react';

interface Props {
    placeholder: string;
    name: string;
    options: {text: string, value: string}[];
    label?: string;
}

function MySelectInput(props: Props) {
    const [field, meta, helpers] = useField(props.name);

    return (
        // !! converts to boolean check
        <Form.Field error={meta.touched && !!meta.error}>
            <label>{props.label}</label>
            <Select 
                clearable 
                options={props.options}
                value={field.value || null}
                onChange={(_: React.SyntheticEvent<HTMLElement, Event>, d: DropdownProps) => helpers.setValue(d.value)}
                onBlur={() => props.placeholder}
                placeholder={props.placeholder}
            />
            {meta.touched && meta.error ? (
                <Label basic color="red">{meta.error}</Label>
            ) : null}
        </Form.Field>
    )
}

export default MySelectInput;