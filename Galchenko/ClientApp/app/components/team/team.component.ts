import { Component, OnDestroy  } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Http } from '@angular/http';
import { TeamViewModel, TeamJoinRequestViewModel } from "../../models/teams";
import { ApplicationUserViewModel, StudentViewModel, CoachViewModel } from "../../models/users";
import { IdentityService } from "../../services/identity.service";
import { ToastyService } from 'ng2-toasty';

@Component({
    templateUrl: './team.component.html',
    providers: [IdentityService]
})

export class TeamDetailsComponent implements OnDestroy {
    private team = new TeamViewModel(0, '', null, null);

    private routerSubscription: Subscription;

    private isUserTeamCoach: boolean;

    private isStudent: boolean;

    private isStudentInTeam: boolean;

    private loggedUser: ApplicationUserViewModel;
    private identitySubscription: Subscription;

    constructor(
        private http: Http,
        private activateRoute: ActivatedRoute,
        private router: Router,
        private toastyService: ToastyService,
        private identityService: IdentityService
    ) {
        this.routerSubscription = activateRoute.params.subscribe(params => {
            if (!params['id'])
                this.router.navigate(['/teams']);
            else
            {
                this.identitySubscription = this.identityService.loggedUser.subscribe(user => {
                    if (user) {
                        if (user.role === 'Coach')
                            this.loggedUser = user as CoachViewModel;
                        if (user.role === 'Student') {
                            this.loggedUser = user as StudentViewModel;
                            this.isStudent = true;
                        }
                        this.loggedUser = user as ApplicationUserViewModel;


                        this.http.get('/api/teams/' + params['id']).subscribe(result => {
                            this.team = result.json() as TeamViewModel;
                            if (this.loggedUser.role === 'Coach') {
                                if (this.team.coach.id === this.loggedUser.id) {
                                    this.isUserTeamCoach = true;
                                }
                            }
                            if (this.loggedUser.role === 'Student') {
                                if ((this.loggedUser as StudentViewModel).team) {
                                    this.isStudentInTeam = true;
                                }
                            }
                        });
                    }

                    this.http.get('/api/teams/' + params['id']).subscribe(result => {
                        this.team = result.json() as TeamViewModel;
                    });
                });
            }
        });

    }

    expelPlayer(player: StudentViewModel) {
        this.http.post('api/teams/expel', player).subscribe(result => {
            if (result.status === 200) {
                this.toastyService.success('Дані оновлено!');
                this.team.students = this.team.students.filter(student => student !== player);
            }
        });
    }

    joinRequest() {
        var request = new TeamJoinRequestViewModel(this.team, this.loggedUser as StudentViewModel);

        this.http.post('api/teams/request/join', request).subscribe(result => {
            if (result.status === 200)
                this.toastyService.success('Запит успішно надіслано!');
            if (result.status === 203)
                this.toastyService.warning('Запит вже було надіслано!');
        });
    }

    ngOnDestroy(){
        this.routerSubscription.unsubscribe();
    }
}