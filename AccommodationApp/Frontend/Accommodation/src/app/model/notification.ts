export class Notification {
    userId: string;
    notificationContent: string;
    createdAt: Date; 
    
    constructor() {
        this.userId = '';
        this.notificationContent = '';
        this.createdAt = new Date();
    }
}