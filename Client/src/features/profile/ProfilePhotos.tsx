import { observer } from 'mobx-react-lite';
import React, { useState, SyntheticEvent } from 'react';
import { Card, Header, Tab, Image, Grid, Button } from 'semantic-ui-react';
import { Photo, Profile } from '../../app/models/profile';
import { useStore } from '../../app/stores/store';
import PhotoUploadWidget from '../../app/common/imageUpload/PhotoUploadWidget';

interface Props {
    profile: Profile;
}

export default observer(function ProfilePhotos({profile}: Props) {
    const {profileStore: {isCurrentUser, uploadPhoto, isUploading, isLoading, setMainPhoto, deletePhoto}} = useStore();
    const [addPhotoMode, setAddPhotoMode] = useState(false);
    const [target, setTarget] = useState('');

    function handlePhotoUpload(file: Blob) {
        uploadPhoto(file).then(() => setAddPhotoMode(false));
    }

    function handleSetMainPhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>) {
        setTarget(e.currentTarget.name);
        setMainPhoto(photo);
    }

    function handleDeletePhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>) {
        setTarget(e.currentTarget.name);
        deletePhoto(photo);
    }

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header icon='image' content='Photos' floated='left' />
                    {isCurrentUser && 
                        <Button 
                            floated='right' 
                            basic 
                            content={addPhotoMode ? 'Cancel' : 'Add Photo'}
                            onClick={() => setAddPhotoMode(!addPhotoMode)}
                        />
                    }
                </Grid.Column>
                <Grid.Column width={16}>
                    {addPhotoMode 
                      ? (
                        <PhotoUploadWidget uploadPhoto={handlePhotoUpload} loading={isUploading} />
                    ) : (
                        <Card.Group itemsPerRow={5}>
                            {profile.photos?.map(photo => (
                                <Card key={photo.id}>
                                    <Image src={photo.url} />
                                    {isCurrentUser && (
                                        <Button.Group fluid widths={2}>
                                            <Button 
                                                basic
                                                color='green'
                                                content='Main'
                                                name={'setMain' + photo.id}
                                                disabled={photo.isMain || isLoading}
                                                loading={target === 'setMain' + photo.id && isLoading}
                                                onClick={(e) => handleSetMainPhoto(photo, e)}
                                            />
                                            <Button 
                                                basic
                                                color='red'
                                                icon='trash'
                                                name={'setDelete' + photo.id}
                                                disabled={isLoading}
                                                loading={target === 'setDelete'+ photo.id && isLoading}
                                                onClick={(e) => handleDeletePhoto(photo, e)}
                                            />
                                        </Button.Group>
                                    )}
                                </Card>
                            ))}
                        </Card.Group> 
                    )}
                </Grid.Column>
            </Grid>
           
        </Tab.Pane>
    )
})