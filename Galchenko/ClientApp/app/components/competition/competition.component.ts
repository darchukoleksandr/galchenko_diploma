import { Component, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { CompetitionViewModel, CompetitionJoinRequestViewModel, CompetitionPointsViewModel } from "../../models/competitions";
import { ApplicationUserViewModel } from "../../models/users";
import { TeamViewModel } from "../../models/teams";
import { Subscription } from 'rxjs/Subscription';
import { IdentityService } from "../../services/identity.service";
import { ToastyService } from 'ng2-toasty';

@Component({
    templateUrl: './competition.component.html',
    providers: [IdentityService]
})

export class CompetitionComponent implements OnDestroy {
    private competition: CompetitionViewModel;
    private competitionTeams: TeamViewModel[];
    private loggedUser: ApplicationUserViewModel;

    private results: CompetitionPointsViewModel[];

    private loggedCoachTeam: TeamViewModel;

    private coachTeamCanJoin: boolean;

    private competitionSelection: number;
    private activateRouteSubscription: Subscription;
    private identityServiceSubscription: Subscription;
    
    constructor(
        private http: Http,
        private activateRoute: ActivatedRoute,
        private identityService: IdentityService,
        private toastyService: ToastyService
    )
    {
        this.activateRouteSubscription = activateRoute.params.subscribe(params => {
            this.competitionSelection = params['id'];

            this.http.get('/api/competitions/' + this.competitionSelection).subscribe(result => {
                this.competition = result.json() as CompetitionViewModel;

                this.http.get('/api/competitions/' + this.competition.id + '/teams/granted').subscribe(result => {
                    this.competitionTeams = result.json() as TeamViewModel[];

                    //                this.http.get('/api/competitions/points/' + this.selectedCompetition.id).subscribe(result => {
                    //                    this.selectedCompetitionPoints = result.json() as CompetitionPointsViewModel[];
                    //                });
                });

                this.http.get('/api/competitions/points/results/' + this.competition.id).subscribe(result => {
                    this.results = result.json() as CompetitionPointsViewModel[];
                });

            });
        });

        this.identityServiceSubscription = this.identityService.loggedUser.subscribe(user => {
            if (user) {
                this.loggedUser = user;

                if (this.loggedUser.role === 'Coach') {
                    this.http.get('/api/teams/coach/' + this.loggedUser.id).subscribe(result => {
                        this.loggedCoachTeam = result.json() as TeamViewModel;
                    }, () => {
//                        this.toastyService.warning('Для участі у змаганні вам потрібна команда!');
                    });
                }
            }
        });
    }

    teamResult(teamId: number) {
        var sum = 0;
        this.results.forEach((a) => {
            if (a.student.team.id === teamId) {
                sum += a.score;
            }
        });
        return sum;
    }

    dateCheck() {
        return new Date().getTime() < Date.parse(this.competition.date.toString());
    }

    joinCompetition() {
        if (!this.loggedCoachTeam) {
            this.toastyService.warning('У вас немає команди!');
            return;
        }
        if (this.loggedCoachTeam.students.length === 0) {
            this.toastyService.warning('У вашій команді немає гравців!');
            return;
        }
        if (this.loggedCoachTeam.kindOfSport.id !== this.competition.kindOfSport.id) {
            this.toastyService.warning('У вашої команди інший вид спорту!');
            return;
        }

        var request = new CompetitionJoinRequestViewModel(this.competition, this.loggedCoachTeam);

        this.http.post('api/competitions/requests', request).subscribe(result => {
            if (result.status === 200)
                this.toastyService.success('Запит надіслано!');
//            if (result.status === 202)
//                this.toastyService.warning('У вашої команди інший вид спорту!');
            if (result.status === 203)
                this.toastyService.warning('Запит вже було надіслано!');
        });
    }

    ngOnDestroy() {
        this.activateRouteSubscription.unsubscribe();
        this.identityServiceSubscription.unsubscribe();
    }
}