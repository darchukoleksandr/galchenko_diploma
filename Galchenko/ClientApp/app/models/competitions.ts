import { RefereeViewModel, ModeratorViewModel, StudentViewModel } from "./users";
import { KindOfSport } from "./models";
import { TeamViewModel } from "./teams";

export class CompetitionViewModel {
    public id: number;
    public name: string;
    public place: string;
    public date: Date;
    public kindOfSport: KindOfSport;
    public referee: RefereeViewModel;
    public moderator: ModeratorViewModel;
}

export class CompetitionJoinRequestViewModel {
    public id: number;
    public competition: CompetitionViewModel;
    public team: TeamViewModel;
    public result: boolean;
    constructor(
        competition: CompetitionViewModel,
        team: TeamViewModel
    ) {
        this.competition = competition;
        this.team = team;
    }
}

//Відправляється на сервер в базу даних
export class CompetitionJoinRequestResultViewModel {
    public requestId: number;
    public competitionId: number;
    public teamId: number;
    public result: boolean;
    constructor(
        requestId: number,
        competitionId: number,
        teamId: number,
        result: boolean
    ) {
        this.requestId = requestId;
        this.competitionId = competitionId;
        this.teamId = teamId;
        this.result = result;
    }
}

export class CreateCompetitionViewModel {
    public id: number;
    public name: string;
    public place: string;
    public date: Date;
    public kindOfSportId: number;
    public refereeId: string;
    public moderatorId: string;
    constructor(
        name: string,
        place: string,
        date: Date,
        kindOfSportId: number,
        refereeId: string,
        moderatorId: string
    ) {
        this.name = name;
        this.place = place;
        this.date = date;
        this.kindOfSportId = kindOfSportId;
        this.refereeId = refereeId;
        this.moderatorId = moderatorId;
    }
}

export class CompetitionPointsViewModel {
    public id: number;
    public competition: CompetitionViewModel;
//    public studentId: string;
    public student: StudentViewModel;
    public score: number;
    constructor(
        competition: CompetitionViewModel,
//        studentId: string,
        student: StudentViewModel,
        score: number
    ) {
        this.id = 0;
        this.competition = competition;
//        this.studentId = studentId;
        this.student = student;
        this.score = score;
    }
}

