import { StudentViewModel, CoachViewModel } from "./users";
import { KindOfSport } from "./models";

export class TeamViewModel {
    public id: number;
    public name: string;
    public kindOfSport: KindOfSport;
    public coach: CoachViewModel;
    public students: StudentViewModel[];
    public rating: number;
    constructor(
        id: number,
        name: string,
        kindOfSport: KindOfSport,
        coach: CoachViewModel,
        rating?: number
    ) {
        this.id = id;
        this.name = name;
        this.kindOfSport = kindOfSport;
        this.coach = coach;
        this.rating = rating;
    }
}

export class TeamJoinRequestViewModel {
    public id: number;
    public team: TeamViewModel;
    public student: StudentViewModel;
    public result: boolean;
    constructor(
        team: TeamViewModel,
        student: StudentViewModel
    ) {
        this.team = team;
        this.student = student;
    }
}

export class TeamJoinRequestResultViewModel {
    public teamJoinRequestId: number;
    public teamId: number;
    public studentId: string;
    public result: boolean;
    constructor(
        teamJoinRequestId: number,
        teamId: number,
        studentId: string,
        result: boolean
    ) {
        this.teamJoinRequestId = teamJoinRequestId;
        this.teamId = teamId;
        this.result = result;
        this.studentId = studentId;
    }
}

export class CreateTeamViewModel {
    public name: string;
    public kindOfSportId: number;
    public coachId: string;

    constructor(
        name: string,
        kindOfSportId: number,
        coachId: string
    ) {
        this.name = name;
        this.kindOfSportId = kindOfSportId;
        this.coachId = coachId;
    }
}
