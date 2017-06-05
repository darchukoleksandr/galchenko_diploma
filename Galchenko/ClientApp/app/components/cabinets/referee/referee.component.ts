import { Component, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { IdentityService } from "../../../services/identity.service";
import { RefereeViewModel, StudentViewModel } from "../../../models/users";
import { CompetitionPointsViewModel, CompetitionViewModel } from "../../../models/competitions";
import { TeamViewModel } from "../../../models/teams";
import { Subscription } from 'rxjs/Subscription';
import { ToastyService } from 'ng2-toasty';

@Component({
    templateUrl: './referee.component.html',
    providers: [IdentityService]
})
export class RefereeCabinetComponent implements OnDestroy {
    private competitions: CompetitionViewModel[];

    private selectedCompetition: CompetitionViewModel;
    private selectedCompetitionTeams: TeamViewModel[];

    private selectedCompetitionStudent = new StudentViewModel('', '', '', '', undefined);
    private selectedCompetitionTeam: TeamViewModel;
    private selectedCompetitionPoints: CompetitionPointsViewModel[];
    private selectedCompetitionNewPoint: CompetitionPointsViewModel;

    private loggedUser: RefereeViewModel;
    private identitySubscription: Subscription;

    constructor(
        private http: Http,
        private toastyService: ToastyService,
        private identityService: IdentityService,
        private router: Router
    ) {
        this.identitySubscription = this.identityService.loggedUser.subscribe(user => {

            if (!user) {
                toastyService.warning('Тільки зареєстрований користувач може ввійти!');
                this.router.navigate(['']);
            }

            this.loggedUser = user as RefereeViewModel;

            this.http.get('/api/competitions/referee/' + this.loggedUser.id).subscribe(result => {
                this.competitions = result.json() as CompetitionViewModel[];
            });
        });

    }

    dateCheck() {
        return new Date().getTime() > Date.parse(this.selectedCompetition.date.toString());
    }

    selectTeam(team: TeamViewModel) {
        this.selectedCompetitionTeam = team;
        this.selectedCompetitionNewPoint.student = this.selectedCompetitionStudent;
    }

    removeFromList(pointToDelete: CompetitionPointsViewModel) {
        this.http.post('/api/competitions/points/delete', pointToDelete).subscribe(result => {
            if (result.status === 200) {
                this.toastyService.success('Інформація оновлена!');
                this.selectedCompetitionPoints = this.selectedCompetitionPoints.filter(point => point !== pointToDelete);
            }
        });
    }

    addScoreToList() {
        if (this.selectedCompetitionNewPoint.student.id === '') {
            return;
        }
        if (this.selectedCompetitionPoints.filter(point => point.student.id === this.selectedCompetitionNewPoint.student.id).length > 0) {
            this.toastyService.warning('Студент вже знаходиться в списку!');
            return;
        }

        this.http.post('/api/competitions/points/submit', this.selectedCompetitionNewPoint).subscribe(result => {
            if (result.status === 200) {

                this.selectedCompetitionNewPoint.id = result.json() as number;

                this.selectedCompetitionNewPoint.student.team = new TeamViewModel(this.selectedCompetitionTeam.id, this.selectedCompetitionTeam.name, this.selectedCompetitionTeam.kindOfSport, undefined);

                this.selectedCompetitionPoints.unshift(this.selectedCompetitionNewPoint);
                console.log(this.selectedCompetitionNewPoint);

                this.selectedCompetitionNewPoint =
                    new CompetitionPointsViewModel(this.selectedCompetition, this.selectedCompetitionStudent, 1);

                this.toastyService.success('Інформація збережена!');
            }
            if (result.status === 404) {
                this.toastyService.warning('В системі відсутній *******!');
            }
        });
    }

    private selectCompetition(competition: CompetitionViewModel) {
        this.selectedCompetition = competition;
        if (competition === null) {
            return;
        }

        this.http.get('/api/competitions/' + this.selectedCompetition.id + '/teams').subscribe(result => {
            this.selectedCompetitionTeams = result.json() as TeamViewModel[];
            this.selectedCompetitionTeam = this.selectedCompetitionTeams[0];

            if (this.selectedCompetitionTeams.length !== 0)
                this.selectedCompetitionNewPoint =
                    new CompetitionPointsViewModel(this.selectedCompetition, this.selectedCompetitionStudent, 1);

            this.http.get('/api/competitions/points/' + this.selectedCompetition.id).subscribe(result => {
                this.selectedCompetitionPoints = result.json() as CompetitionPointsViewModel[];
            });
        });
    }

    ngOnDestroy(): void {
        this.identitySubscription.unsubscribe();
    }
}
