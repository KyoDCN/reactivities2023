import { makeAutoObservable, runInAction } from "mobx";
import { Photo, Profile } from "../models/profile";
import agent from "../api/agent";
import { store } from "./store";

export default class ProfileStore {
    profile: Profile | null = null;
    loadingProfile: boolean = false;
    isUploading: boolean = false;
    isLoading: boolean = false;

    constructor() {
        makeAutoObservable(this);
    }

    get isCurrentUser() {
        if(store.userStore.user && this.profile) {
            return store.userStore.user.username === this.profile.username;
        }
        return false;
    }

    loadProfile = async (username: string) => {
        this.loadingProfile = true;
        try {
            const profile = await agent.Profiles.get(username);
            runInAction(() => {
                this.profile = profile;
            })
        } catch(error) {
            console.log(error);
        } finally {
            runInAction(() => this.loadingProfile = false);
        }
    }

    uploadPhoto = async (file: Blob) => {
        this.isUploading = true;

        try {
            const response = await agent.Profiles.uploadPhoto(file);
            const photo = response.data;
            runInAction(() => {
                if(!this.profile) return;
                if(!this.profile.photos) return;

                this.profile.photos.push(photo);
                if(photo.isMain && store.userStore.user) {
                    store.userStore.setImage(photo.url);
                    this.profile.image = photo.url;
                }
            })
        } catch (error) {
            console.log(error)
        } finally {
            runInAction(() => this.isUploading = false);
        }
    }

    setMainPhoto = async (photo: Photo) => {
        this.isLoading = true;
        
        try {
            await agent.Profiles.setMainPhoto(photo.id);
            store.userStore.setImage(photo.url);
            runInAction(() => {
                if(this.profile && this.profile.photos) {
                    let foundPhoto = this.profile.photos.find(p => p.isMain);
                    if(foundPhoto) foundPhoto.isMain = false;
                    foundPhoto = this.profile.photos.find(p => p.id === photo.id);
                    if(foundPhoto) foundPhoto.isMain = true;
                    this.profile.image = photo.url;
                }
            })
        } catch (error) {
            console.log(error);
        } finally {
            runInAction(() => this.isLoading = false);
        }
    }

    deletePhoto = async (photo: Photo) => {
        this.isLoading = true;
        
        try {
            await agent.Profiles.deletePhoto(photo.id);

            if(this.profile) {
                this.profile.photos = this.profile?.photos?.filter(p => !p.isMain);

                if(photo.isMain) {
                    if(!this.profile.photos) return;

                    let foundPhoto = this.profile.photos[0]
                    await agent.Profiles.setMainPhoto(foundPhoto.id);
                    foundPhoto.isMain = true;
                    this.profile.image = foundPhoto.url;
                    store.userStore.setImage(foundPhoto.url)
                }
            }
        } catch (error) {
            console.log(error);
        } finally {
            runInAction(() => this.isLoading = false);
        }
    }
}