import { makeAutoObservable } from "mobx";
import { User, UserFormValues } from "../models/user";
import agent from "../api/agent";

class UserStore {
    user: User | null = null;

    constructor() {
        makeAutoObservable(this)
    }

    get isLoggedIn() {
        return !!this.user;
    }

    login = async (creds: UserFormValues) => {
        const user = await agent.Accounts.login(creds);
        console.log(user);
    }
}

export default UserStore;