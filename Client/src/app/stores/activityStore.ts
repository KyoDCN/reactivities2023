import { makeAutoObservable, runInAction } from "mobx";
import { Activity, ActivityFormValues } from "../models/activity";
import agent from "../api/agent";
import {v4 as uuid} from 'uuid';
import { format } from "date-fns";
import { store } from "./store";
import { Profile } from "../models/profile";

class ActivityStore {
    // activities: Activity[] = [];
    activityRegistry = new Map<string, Activity>();
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = true;

    constructor() {
        makeAutoObservable(this);
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort((a,b) => {
            if(!a.date || !b.date)
                return 0;

            return a.date.getTime() - b.date.getTime();
        });
    }

    get groupedActivities() {
        return Object.entries(
            this.activitiesByDate.reduce((activities, activity) => {
                if(activity.date) {
                    const date = format(activity.date, 'dd MMM yyyy')
                    activities[date] = activities[date] !== undefined ? [...activities[date], activity] : [activity];
                }
                
                return activities;
            }, {} as {[key: string]: Activity[]})
        )
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
            runInAction(() => this.selectedActivity = activity);
        } else {
            this.setLoadingInitial(true);
            try {
                activity = await agent.Activities.details(id);
                this.setActivity(activity);
                runInAction(() => this.selectedActivity = activity);
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
        const user = store.userStore.user;
        if(user) {
            const attendees = activity.attendees;

            if(attendees) {
                activity.isGoing = attendees.some((x) => x.username === user.username);
                activity.isHost = activity.hostUsername === user.username;
                var host = attendees.find(x => x.username === activity.hostUsername);

                if(host) activity.host = host;
            }
        }

        if(activity.date) 
            activity.date = new Date(activity.date);

        this.activityRegistry.set(activity.id, activity);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    selectActivity = (id: string) => {
        //this.selectedActivity = this.activities.find(x => x.id === id);
        this.selectedActivity = this.activityRegistry.get(id);
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

    createActivity = async (activity: ActivityFormValues) => {
        const user = store.userStore.user;
        if(!user) return;
        const attendee = new Profile(user);

        try {
            await agent.Activities.create(activity);
            const newActivity = new Activity(activity);
            newActivity.hostUsername = user.username;
            newActivity.attendees = [attendee];
            this.setActivity(newActivity);

            runInAction(() => {
                this.selectedActivity = newActivity;
            })
        } catch (error) {
            console.log(error);
        }
    }

    updateActivity = async (activity: ActivityFormValues) => {
        try {
            await agent.Activities.update(activity);
            runInAction(() => {
                if(activity.id) {
                    const updatedActivity = new Activity({...this.getActivity(activity.id), ...activity});
                    this.activityRegistry.set(updatedActivity.id, updatedActivity);
                    this.selectedActivity = updatedActivity;
                }
            })
        } catch (error) {
            console.log(error);
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

    updateAttendance = async () => {
        const user = store.userStore.user;
        this.loading = true;
        try {
            if(!this.selectedActivity) return;

            await agent.Activities.attend(this.selectedActivity.id)

            runInAction(() => {
                if(!user) return;
                if(!this.selectedActivity) return;
                if(!this.selectedActivity.attendees) return;

                // Cancel attendance
                if(this.selectedActivity.isGoing) {
                    this.selectedActivity.attendees = this.selectedActivity.attendees.filter(a => a.username !== user.username);
                    this.selectedActivity.isGoing = false;

                // Join attendance
                } else {
                    const attendee = new Profile(user);
                    this.selectedActivity.attendees.push(attendee);
                    this.selectedActivity.isGoing = true;
                }

                this.activityRegistry.set(this.selectedActivity.id, this.selectedActivity);
            })
        } catch (error) {
            console.log(error);
        } finally {
            runInAction(() => this.loading = false);
        }
    }

    cancelActivityToggle = async () => {
        if(!this.selectedActivity) return;

        this.loading = true;

        try {
            await agent.Activities.attend(this.selectedActivity.id);
            runInAction(() => {
                if(!this.selectedActivity) return;

                this.selectedActivity.isCancelled = !this.selectedActivity.isCancelled
                this.activityRegistry.set(this.selectedActivity.id, this.selectedActivity);
            })
        } catch (error) {
            console.log(error);
        } finally {
            runInAction(() => this.loading = false);
        }
    }
}

export default ActivityStore;