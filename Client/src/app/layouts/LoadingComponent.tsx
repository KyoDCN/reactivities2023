import React from 'react';
import { Dimmer, Loader } from 'semantic-ui-react';

interface Props {
    inverted?: boolean;
    content?: string;
}

export default function LoadingComponent({inverted = true, content = 'Loading...'}: Props) {
    return (
        <Dimmer style={{marginTop: 55}} active={true} inverted={inverted} >
            <Loader content={content} />
        </Dimmer>
    )
}