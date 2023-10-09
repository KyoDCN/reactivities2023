import React, { useEffect, useState } from 'react'
import { Segment, List, Label, Item, Image } from 'semantic-ui-react'
import { Link } from 'react-router-dom'
import { observer } from 'mobx-react-lite'
import { Activity } from '../../../app/models/activity';
import { runInAction } from 'mobx';
import { Profile } from '../../../app/models/profile';

interface Props {
    activity: Activity;
}

export default observer(function ActivityDetailedSidebar ({activity: {attendees, host}}: Props) {
    if(!attendees) return <></>;
    if(!host) return <></>;

    return (
        <>
            <Segment
                textAlign='center'
                style={{ border: 'none' }}
                attached='top'
                secondary
                inverted
                color='teal'
            >
                {attendees.length} {attendees.length === 1 ? "Person" : "People"} going
            </Segment>
            <Segment attached>
                <List relaxed divided>
                    <Label style={{ position: 'absolute', left: 'auto', right: '-69px' }} color='orange' ribbon='right'>
                        Host
                    </Label>
                    <Item key={host.username} style={{ position: 'relative' }}>                            
                        <Image size='tiny' src={host.image || '/assets/user.png'} />
                        <Item.Content verticalAlign='middle'>
                            <Item.Header as='h3'>
                                <Link to={`/profiles/${host.username}`}>{host.displayName}</Link>
                            </Item.Header>
                            <Item.Extra style={{ color: 'orange' }}>Following</Item.Extra>
                        </Item.Content>
                    </Item>
                    {attendees.map(attendee => (
                        attendee.username !== host.username &&
                        <Item key={attendee.username} style={{ position: 'relative' }}>                            
                            <Image size='tiny' src={attendee.image || '/assets/user.png'} />
                            <Item.Content verticalAlign='middle'>
                                <Item.Header as='h3'>
                                    <Link to={`/profiles/${attendee.username}`}>{attendee.displayName}</Link>
                                </Item.Header>
                                <Item.Extra style={{ color: 'orange' }}>Following</Item.Extra>
                            </Item.Content>
                        </Item>
                    ))}
                </List>
            </Segment>
        </>
    )
})