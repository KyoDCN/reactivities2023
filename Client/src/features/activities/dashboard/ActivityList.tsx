import React, { Fragment } from 'react'
import { Header } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store';
import { observer } from 'mobx-react-lite';
import ActivityListItem from './ActivityListItem';

interface Props {
}

function ActivityList({}: Props) {
    const {activityStore} = useStore();
    const {groupedActivities} = activityStore;

    return (
        <>
            {groupedActivities.map(([groupKey, activitiesValue]) => (
                <Fragment key={groupKey}>
                    <Header sub color='teal'>
                        {groupKey}
                    </Header>
                    {activitiesValue.map(activity => (
                        <ActivityListItem activity={activity} key={activity.id} />
                    ))}
                </Fragment>
            ))}
        </>
        
    )
}

export default observer(ActivityList)