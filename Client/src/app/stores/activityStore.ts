import { makeAutoObservable, runInAction } from "mobx";
import { Activity } from "../models/activity";
import agent from "../api/agent";
import {v4 as uuid} from 'uuid';

class ActivityStore {
    // activities: Activity[] = [];
    activityRegistry = new Map<string, Activity>();
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;

    constructor() {
        makeAutoObservable(this);
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort((a,b) => Date.parse(a.date) - Date.parse(b.date));
    }

    loadActivities = async () => {
        this.setLoadingInitial(true);
        
        try {
            const res = await agent.Activities.list();
            res.forEach(activity => {
                this.setActivity(activity);
            })
            // res.map(x => x.date = x.date.split('T')[0]);
            // this.activities = res;
        } catch (error) {
            console.log(error);
        } finally {
            this.setLoadingInitial(false);
        }
    }

    loadActivity = async (id: string): Promise<Activity | undefined> => {
        let activity = this.getActivity(id);
        
        if(activity) {
            this.setSelectedActivity(activity);
        } else {
            this.setLoadingInitial(true);
            try {
                activity = await agent.Activities.details(id);
                this.setActivity(activity);
                this.setSelectedActivity(activity);
            } catch (error) {
                console.log(error);
            } finally {
                this.setLoadingInitial(false);
            }
        }

        return activity;
    }

    private getActivity = (id: string) => {
        return this.activityRegistry.get(id);
    }

    private setActivity = (activity: Activity) => {
        activity.date = activity.date.split('T')[0];
        this.activityRegistry.set(activity.id, activity);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    setSelectedActivity = (activity: Activity | undefined) => {
        this.selectedActivity = activity;
    }

    selectActivity = (id: string) => {
        //this.selectedActivity = this.activities.find(x => x.id === id);
        this.setSelectedActivity(this.activityRegistry.get(id));
    }

    cancelSelectedActivity = () => {
        this.selectedActivity = undefined;
    }

    openForm = (id?: string) => {
        id ? this.selectActivity(id) : this.cancelSelectedActivity();
        this.editMode = true;
    }

    closeForm = () => {
        this.editMode = false;
    }

    createActivity = async (activity: Activity) => {
        this.loading = true;
        activity.id = uuid();
        try {
            await agent.Activities.create(activity);
            runInAction(() => {
                //this.activities.push(activity);
                this.activityRegistry.set(activity.id, activity);
                this.setSelectedActivity(activity);
                this.editMode = false;
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }

    updateActivity = async (activity: Activity) => {
        this.loading = true;
        try {
            await agent.Activities.update(activity);
            runInAction(() => {
                // this.activities = [...this.activities.filter(x => x.id !== activity.id), activity];
                this.activityRegistry.set(activity.id, activity);
                this.setSelectedActivity(activity);
                this.editMode = false;
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })   
        }
    }

    deleteActivity = async (id: string) => {
        this.loading = true;
        try {
            await agent.Activities.delete(id);
            runInAction(() => {
                // this.activities = this.activities.filter(x => x.id !== id);
                this.activityRegistry.delete(id);
                if(this.selectedActivity?.id === id) {
                    this.cancelSelectedActivity();
                }
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }
}

export default ActivityStore;