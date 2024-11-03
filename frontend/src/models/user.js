export default class User  {
    constructor({
        id,
        username,
        firstName,
        lastName,
        birthDate,
        culture,
        isAdmin,
        isOrganizer,
        registerDate,
        lastVisitDate,
        avatarUrl
        } = {}) {
        this.id = id;
        this.username = username;
        this.firstName = firstName;
        this.lastName = lastName;
        this.birthDate = birthDate;
        this.culture = culture;
        this.isAdmin = isAdmin;
        this.isOrganizer = isOrganizer;
        this.registerDate = registerDate;
        this.lastVisitDate = lastVisitDate;
        this.avatarUrl = avatarUrl;
    }
}