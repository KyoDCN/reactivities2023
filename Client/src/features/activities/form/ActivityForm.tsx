import React, { ChangeEvent, useEffect, useState } from 'react'
import { Button, Form, Segment } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store';
import { observer } from 'mobx-react-lite';
import { Link, useNavigate, useParams } from 'react-router-dom';
import LoadingComponent from '../../../app/layouts/LoadingComponent';
import {v4 as uuid} from 'uuid';

interface Props {
}

function ActivityForm({}: Props) {
    const {activityStore} = useStore();
    const {
        selectedActivity, 
        createActivity, 
        updateActivity, 
        loading: submitting, 
        loadActivity, 
        loadingInitial
    } = activityStore;

    const {id} = useParams();
    const navigate = useNavigate();

    const [activity, setActivity] = useState({
        id: '',
        title: '',
        category: '',
        description: '',
        date: '',
        city: '',
        venue: ''
    });

    useEffect(() => {
        if(id) {
            loadActivity(id)
                .then(activity => {
                    if(activity) setActivity(activity);
                })
        } 
    }, [id, loadActivity])

    async function handleSubmit() {
        if(!activity.id) {
            activity.id = uuid();
            await createActivity(activity);
        } else {
            await updateActivity(activity);
        }
        
        navigate(`/activities/${activity.id}`)
    }

    function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        const { name, value } = event.target;
        setActivity({...activity, [name]: value });
    }

    if(loadingInitial) return <LoadingComponent content='Loading  ...'/>;

    return (
        <Segment clearing>
            <Form onSubmit={handleSubmit} autoComplete='off'>
                <Form.Input placeholder='Title' value={activity.title} name='title' onChange={handleInputChange} />
                <Form.TextArea placeholder='Description' value={activity.description} name='description' onChange={handleInputChange}  />
                <Form.Input placeholder='Category' value={activity.category} name='category' onChange={handleInputChange}  />
                <Form.Input type='date' placeholder='Date' value={activity.date} name='date' onChange={handleInputChange}  />
                <Form.Input placeholder='City' value={activity.city} name='city' onChange={handleInputChange}  />
                <Form.Input placeholder='Venue' value={activity.venue} name='venue' onChange={handleInputChange}  />
                <Button loading={submitting} floated='right' positive type='submit' content='Submit'/>
                <Button as={Link} to='/activities' floated='right' positive type='button' content='Cancel'/>
            </Form>
        </Segment>
    )
}

export default observer(ActivityForm)