import { observer } from 'mobx-react-lite';
import React, {useCallback} from 'react';
import {useDropzone} from 'react-dropzone';
import { Header, Icon } from 'semantic-ui-react';

interface Props {
    setFiles: (files: any) => void;
}

export default observer(function PhotoWidgetDropzone({setFiles}: Props) {
    const dzStyles = {
        display: 'flex',
        flexDirection: 'column' as any,
        alignItems: 'center',
        justifyContent: 'center',
        border: 'dashed 3px #eee',
        borderColor: '#eee',
        borderRadius: '5px',
        textAlign: 'center' as any,
        height: 200
    }

    const dzActive = {
        borderColor: 'green'
    }

    const onDrop = useCallback((acceptedFiles: any[]) => {
        setFiles(acceptedFiles.map((file: any) => Object.assign(file, {
            preview: URL.createObjectURL(file)
        })))
    }, [setFiles])

    const {getRootProps, getInputProps, isDragActive} = useDropzone({onDrop})

    return (
        <div {...getRootProps()} style={isDragActive ? {...dzStyles, ...dzActive} : dzStyles}>
            <input {...getInputProps()} /> 
            <Icon name='upload' size='huge' />
            <Header content='Drop image here' />
        </div>
    )
})