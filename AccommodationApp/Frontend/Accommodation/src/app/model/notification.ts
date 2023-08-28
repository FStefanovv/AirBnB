export class Notification {
    userId: string;
    notificationContent: string;
    createdAt: string; 
    
    constructor() {
        this.userId = '';
        this.notificationContent = '';
        this.createdAt = '';
    }
}