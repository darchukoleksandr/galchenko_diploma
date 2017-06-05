import { TeamViewModel } from "./teams";
import { CompetitionViewModel } from "./competitions";

export abstract class ApplicationUserViewModel {
    public id: string;
    public userName: string;
    public firstName: string;
    public lastName: string;
    public role: string;
}

export class RefereeViewModel extends ApplicationUserViewModel {
    constructor(
        id: string,
        userName: string,
        firstName: string,
        lastName: string
    ) {
        super();
        this.id = id;
        this.userName = userName;
        this.lastName = lastName;
        this.firstName = firstName;
    }
}

export class CoachViewModel extends ApplicationUserViewModel {
    public team: TeamViewModel;

    constructor(
        id: string,
        userName: string,
        firstName: string,
        lastName: string,
        team: TeamViewModel
    ) {
        super();
        this.id = id;
        this.userName = userName;
        this.lastName = lastName;
        this.firstName = firstName;
        this.team = team;
    }
}

export class ModeratorViewModel extends ApplicationUserViewModel {
    public competitions: CompetitionViewModel[];

    constructor(
        id: string,
        userName: string,
        firstName: string,
        lastName: string,
        competitions: CompetitionViewModel[]
    ) {
        super();
        this.id = id;
        this.userName = userName;
        this.lastName = lastName;
        this.firstName = firstName;
        this.competitions = competitions;
    }
}

export class StudentViewModel extends ApplicationUserViewModel {
    public team: TeamViewModel;
    public rating: number;

    constructor(
        id: string,
        userName: string,
        firstName: string,
        lastName: string,
        team: TeamViewModel,
        rating?: number
    ) {
        super();
        this.id = id;
        this.userName = userName;
        this.lastName = lastName;
        this.firstName = firstName;
        this.team = team;
        this.rating = rating;
    }
}
