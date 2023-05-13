import { Address } from "./address";

export class User {
    firstName?: string
    lastName?: string
    address?: Address
    email?: string
    username?: string
    password?: string
    confirmPassword?: string
    role?: string

    constructor(){
        this.firstName = '';
        this.lastName = '';
        this.address = new Address();
        this.email = '';
        this.username = '';
        this.password = '';
        this.confirmPassword = '';
        this.role = '';
    }
}
