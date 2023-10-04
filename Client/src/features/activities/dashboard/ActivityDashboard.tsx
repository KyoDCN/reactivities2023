import { Grid } from "semantic-ui-react";
import ActivityList from "./ActivityList";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import LoadingComponent from "../../../app/layouts/LoadingComponent";
import ActivityFilters from "./ActivityFilters";

interface Props {
}

function ActivityDashboard({}: Props) {

  const {activityStore} = useStore();
  const {loadActivities, activityRegistry} = activityStore;

  useEffect(() => {
    if(activityRegistry.size === 0) loadActivities();
  }, [loadActivities])

  if(activityStore.loadingInitial) {
    return <LoadingComponent content='Loading ...' />
  }

  return (
    <Grid>
      <Grid.Column width='10'>
        <ActivityList />
      </Grid.Column>
      <Grid.Column width='6'>
        <ActivityFilters />
      </Grid.Column>
    </Grid>
  )
}

export default observer(ActivityDashboard)