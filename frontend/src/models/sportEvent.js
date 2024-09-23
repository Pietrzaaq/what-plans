export default class Event  {
    constructor(
        areaId,
        createdBy,
        creationDate,
        eventStartDate,
        eventEndDate
    ) {
        this.areaId = areaId;
        this.createdBy = createdBy;
        this.creationDate = creationDate;
        this.eventStartDate = eventStartDate;
        this.eventEndDate = eventEndDate;
    }
}