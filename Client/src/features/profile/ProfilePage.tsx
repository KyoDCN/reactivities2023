import { observer } from "mobx-react-lite";
import ProfileHeader from "./ProfileHeader";
import { Grid } from "semantic-ui-react";
import ProfileContent from "./ProfileContent";
import { useParams } from "react-router-dom";
import { useStore } from "../../app/stores/store";
import { useEffect } from "react";
import LoadingComponent from "../../app/layouts/LoadingComponent";

export default observer(function ProfilePage() {
    const {username} = useParams<{username: string}>();
    const {profileStore} = useStore();
    const {loadingProfile, loadProfile, profile} = profileStore;

    useEffect(() => {
        if(!username) return;
        loadProfile(username);
    }, [loadProfile, username]);

    if(loadingProfile) return <LoadingComponent content="Loading Profile ..." />
    if(!profile) return <></>;

    return(
        <Grid>
            <Grid.Column width={16}>
                <ProfileHeader profile={profile}/>
                <ProfileContent profile={profile}/>
            </Grid.Column>
        </Grid>
    )
})