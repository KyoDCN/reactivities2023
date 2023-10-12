import { observer } from 'mobx-react-lite';
import React, { useEffect, useState } from 'react';
import { Button, Grid, Header, Image } from 'semantic-ui-react';
import PhotoWidgetDropzone from './PhotoWidgetDropzone';
import {Cropper} from 'react-cropper'
import PhotoWidgetCropper from './PhotoWidgetCropper';

interface Props {
    loading: boolean;
    uploadPhoto: (file: Blob) => void
}

export default observer(function PhotoUploadWidget({loading, uploadPhoto}: Props) {
    const [files, setFiles] = useState<any>([])
    const [cropper, setCropper] = useState<Cropper>();

    function onCrop() {
        if(cropper) {
            cropper.getCroppedCanvas().toBlob(blob => {
                if(!blob) return;
                uploadPhoto(blob)
            });
        }
    }

    useEffect(() => {
        return () => {
            // Disposes of the file object after being used, so it doesn't stay bloated in memory
            files.forEach((file: any) => URL.revokeObjectURL(file.preview))
        }
    }, [files])

    if(!files) return <></>

    return (
        <Grid>
            <Grid.Column width={4}>
                <Header sub color='teal' content="Step 1 - Add Photo" />
                <PhotoWidgetDropzone setFiles={setFiles} />
            </Grid.Column>
            <Grid.Column width={1}/>
            <Grid.Column width={4}>
                <Header sub color='teal' content="Step 2 - Resize Image" />
                {files.length > 0 && 
                    <PhotoWidgetCropper setCropper={setCropper} imagePreview={files[0].preview} />
                }
            </Grid.Column>
            <Grid.Column width={1}/>
            <Grid.Column width={4}>
                <Header sub color='teal' content="Step 3 - Preview & Upload" />
                {files.length > 0 && <>
                    <div className='img-preview' style={{minHeight: 200, overflow: 'hidden', marginBottom: '12px'}} />
                    <Button.Group widths={2}>
                        <Button onClick={onCrop} positive icon='check' loading={loading} />
                        <Button onClick={() => setFiles([])} icon='close' disabled={loading}/>
                    </Button.Group>
                </>}
            </Grid.Column>
        </Grid>
    )
});