import { observer } from 'mobx-react-lite';
import React from 'react';
import { Image, List, Popup } from 'semantic-ui-react';
import { Profile } from '../../../app/models/profile';
import { Link } from 'react-router-dom';
import ProfileCard from '../../profile/ProfileCard';

interface Props {
    attendees: Profile[];
}

export default observer(function ActivityListItemAttendee({attendees}: Props) {
    return (
        <List horizontal>
            {attendees.map((attendee, i) => 
                <Popup
                    key={i}
                    hoverable
                    trigger={
                        <List.Item as={Link} to={`/profiles/${attendee.username}`}>
                            <Image size="mini" circular src={attendee.image || "/assets/user.png"}/>
                        </List.Item>
                    }
                >
                    <Popup.Content>
                        <ProfileCard profile={attendee} />
                    </Popup.Content>
                </Popup>
            )}
        </List>
    )
})