import { User } from "./user";

export class UserParams{
        gender:string;
        minAge = 14
        maxAge = 70;
        pageNumber = 1;
        pageSize = 30;
        orderBy= "lastActive"
        constructor(user: User){
                this.gender = user.gender === 'female' ? 'male' : 'female';
        }
}