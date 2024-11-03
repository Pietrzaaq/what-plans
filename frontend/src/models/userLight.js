export default class UserLight {
    constructor({
        username,
        firstName,
        lastName,
        isAdmin,
        isOrganizer,
        avatarUrl
        } = {}) {
        this.username = username;
        this.firstName = firstName;
        this.lastName = lastName;
        this.isAdmin = isAdmin;
        this.isOrganizer = isOrganizer;
        this.avatarUrl = avatarUrl;
    }
}