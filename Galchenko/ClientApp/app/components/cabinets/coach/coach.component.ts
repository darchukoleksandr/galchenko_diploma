import { Component, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { IdentityService } from "../../../services/identity.service";
import { KindOfSport } from "../../../models/models";
import { TeamJoinRequestResultViewModel, TeamJoinRequestViewModel, TeamViewModel } from "../../../models/teams";
import { ApplicationUserViewModel, CoachViewModel } from "../../../models/users";
import { Subscription } from 'rxjs/Subscription';
import { ToastyService } from 'ng2-toasty';

@Component({
    templateUrl: './coach.component.html',
    providers: [IdentityService]
})
export class CoachCabinetComponent implements OnDestroy {
    private team: TeamViewModel;
    private joinRequests: TeamJoinRequestViewModel[];

    private kindOfSports: KindOfSport[];

    private isUpdateActivated: boolean;
    private teamUpdate: TeamViewModel;


    private loggedUser: ApplicationUserViewModel;
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

            this.loggedUser = user as ApplicationUserViewModel;

            this.http.get('/api/coaches/' + this.loggedUser.id).subscribe(result => {
                this.loggedUser = result.json() as CoachViewModel;
            });

            this.http.get('/api/sports/all').subscribe(result => {
                this.kindOfSports = result.json() as KindOfSport[];
            });

            this.http.get('/api/teams/coach/' + this.loggedUser.id).subscribe(result => {
                this.team = result.json() as TeamViewModel;
                this.teamUpdate = this.team;
            }, () => {

            });

            this.http.get('/api/teams/request/all').subscribe(result => {
                this.joinRequests = result.json() as TeamJoinRequestViewModel[];
            });
        });

    }
    // ------------------------ Teams
    private showCoachTeams() {
        this.router.navigate(['/teams']);
    }

    private createNewTeam() {
        this.router.navigate(['/newTeam']);
    }

    // ------------------------ Requests
    private confirmRequest(request: TeamJoinRequestViewModel) {
        if (request.student.team.name !== null) {
            this.toastyService.warning('Студент вже має команду!');
            return;
        }

        var result = new TeamJoinRequestResultViewModel(request.id, request.team.id, request.student.id, true);

        this.http.post('/api/teams/request/addToTeam', result).subscribe(result => {
            if (result.status === 200) {
                request.result = true;
            }
        });
    }

    private rejectRequest(request: TeamJoinRequestViewModel) {
        var result = new TeamJoinRequestResultViewModel(request.id, request.team.id, request.student.id, true);

        this.http.post('/api/teams/request/addToTeam', result).subscribe(result => {
            if (result.status === 200) {
                request.result = false;
            }
        });
    }

    activateUpdate() {
        this.isUpdateActivated = true;
    }

    updateTeam() {
        this.http.post('/api/teams/update', this.teamUpdate).subscribe(result => {
            if (result.status === 200) {
                this.toastyService.success('Дані оновлено!');
            }
        });
        this.isUpdateActivated = false;
    }

    ngOnDestroy(): void {
        this.identitySubscription.unsubscribe();
    }
}
