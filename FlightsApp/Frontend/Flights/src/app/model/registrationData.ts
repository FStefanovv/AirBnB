
export class RegistrationData {
    firstName?: string;
    lastName?: string;
    username?: string;
    eMail?: string;
    password?: string;
    confirmPassword?: string;

    constructor(){
        this.firstName = '';
        this.lastName = '';
        this.username = '';
        this.eMail = '';
        this.password = '';
        this.confirmPassword = '';
    }
}