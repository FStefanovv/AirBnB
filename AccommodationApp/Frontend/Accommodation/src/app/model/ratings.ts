export class CreateRatingDTO {
    ratedEntityId: string = '';
    grade: number = 0;
    ratedEntityType: number = 0;
}

export class RatingDTO {
    id: string = '';
    grade: number = 0;
    ratingDate: Date = new Date();
    userId: string = '';
}

export class RatingWithUsernameDTO {
   
    grade: number = 0;
    ratingDate: Date = new Date();
    username: string = '';
}

export class RatedEntity {
    id: string = '';
    averageRating: number = 0;
    type: number = 0;
}

export class RatingInfoDTO {
    id: string = '';
    grade: number = 0;
}



