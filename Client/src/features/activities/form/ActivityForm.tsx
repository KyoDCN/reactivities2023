import React, { useEffect, useState } from 'react'
import { Button, Header, Segment } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store';
import { observer } from 'mobx-react-lite';
import { Link, useNavigate, useParams } from 'react-router-dom';
import LoadingComponent from '../../../app/layouts/LoadingComponent';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import MyTextInput from '../../../app/common/form/MyTextInput';
import MyTextArea from '../../../app/common/form/MyTextArea';
import MySelectInput from '../../../app/common/form/MySelectInput';
import { categoryOptions } from '../../../app/common/options/categoryOptions';
import MyDateInput from '../../../app/common/form/MyDateInput';
import { ActivityFormValues } from '../../../app/models/activity';
import { v4 as uuid } from 'uuid';

interface Props {
}

function ActivityForm({ }: Props) {
	const { activityStore } = useStore();
	const {
		createActivity,
		updateActivity,
		loading,
		loadActivity,
		loadingInitial
	} = activityStore;

	const { id } = useParams();
	const navigate = useNavigate();

	const activityObj = new ActivityFormValues;

	const [activity, setActivity] = useState(activityObj);

	const validationSchema = Yup.object({
		title: Yup.string().required("The activity title is required."),
		description: Yup.string().required("The activity description is required."),
		category: Yup.string().required(),
		date: Yup.date().required(),
		venue: Yup.string().required(),
		city: Yup.string().required()
	})

	useEffect(() => {
		if (id) {
			loadActivity(id).then(activity => {
				if (activity) setActivity(new ActivityFormValues(activity));
			})
		}
	}, [id, loadActivity])

	async function handleFormSubmit(activity: ActivityFormValues) {
		if (!activity.id) {
			activity.id = uuid();
			await createActivity(activity);
		} else {
			await updateActivity(activity);
		}

		navigate(`/activities/${activity.id}`)
	}

	if (loadingInitial) return <LoadingComponent content='Loading  ...' />;

	return (
		<Segment clearing>
			<Header content="Activity Details" sub color="teal" />
			<Formik
				validationSchema={validationSchema}
				enableReinitialize
				initialValues={activity}
				onSubmit={activity => handleFormSubmit(new ActivityFormValues(activity))}
			>
				{({ handleSubmit, isValid, isSubmitting, dirty }) => (
					<Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
						<MyTextInput name="title" placeholder="Title" />
						<MyTextArea placeholder='Description' name='description' rows={3} />
						<MySelectInput options={categoryOptions} placeholder='Category' name='category' />
						<MyDateInput
							placeholderText='Date'
							name='date'
							showTimeSelect
							timeCaption='time'
							dateFormat={"MMMM d, yyyy h:mm aa"}
						/>
						<Header content="Location Details" sub color="teal" />
						<MyTextInput placeholder='City' name='city' />
						<MyTextInput placeholder='Venue' name='venue' />
						<Button
							disabled={isSubmitting || !dirty || !isValid}
							loading={loading}
							floated='right'
							positive type='submit'
							content='Submit'
						/>
						<Button as={Link} to='/activities' floated='right' positive type='button' content='Cancel' />
					</Form>
				)}
			</Formik>
		</Segment>
	)
}

export default observer(ActivityForm)