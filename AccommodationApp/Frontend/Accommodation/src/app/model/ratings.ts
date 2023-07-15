export class CreateRatingDTO {
    RatedEntityId: string = '';
    Grade: number = 0;
    RatedEntityType: number = 0;
}

export class RatingDTO {
    Id: string = '';
    Grade: number = 0;
    RatingDate: Date = new Date();
    UserId: string = '';
}

export class RatedEntity {
    Id: string = '';
    AverageRating: number = 0;
    Type: number = 0;
}



