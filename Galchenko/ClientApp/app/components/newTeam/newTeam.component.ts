import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Http } from '@angular/http';
import { IdentityService } from "../../services/identity.service";
import { CoachViewModel } from "../../models/users";
import { KindOfSport } from "../../models/models";
import { CreateTeamViewModel } from "../../models/teams";
import { Subscription } from 'rxjs/Subscription';
import { ToastyService } from 'ng2-toasty';

@Component({
    templateUrl: './newTeam.component.html',
    providers: [IdentityService]
})

export class NewTeamComponent implements OnDestroy {

    private newTeam: CreateTeamViewModel;
    private sports: KindOfSport[];
    private loggedUser: CoachViewModel;
    private identitySubscription: Subscription;

    private onSubmit() {
        this.newTeam.coachId = (this.loggedUser).id;
        this.http.post('/api/teams/create', this.newTeam).subscribe(result => {
            if (result.status === 200) {
                this.toastyService.success("Команду створено!");
                this.router.navigate(['user']);
            }
        });
    }

    constructor(
        private http: Http,
        private identityService: IdentityService,
        private toastyService: ToastyService,
        private router: Router
    ) {
        this.identitySubscription = identityService.loggedUser.subscribe(loggedUser => {
            this.loggedUser = loggedUser as CoachViewModel;
            console.log(loggedUser);
            console.log(this.loggedUser);
            if (loggedUser === undefined) {
                toastyService.warning('Тільки зареєстрований користувач може ввійти!');
                this.router.navigate(['']);
            }
            if (loggedUser.role !== 'Coach') {
//                toastyService.warning('Only coach can create a team!');
                this.router.navigate(['']);
            }

            this.http.get('/api/sports/all').subscribe(result => {
                this.sports = result.json() as KindOfSport[];
            });

            this.loggedUser = loggedUser as CoachViewModel;
        });

            this.newTeam = new CreateTeamViewModel('', 0, '');
    }

    ngOnDestroy(): void {
         this.identitySubscription.unsubscribe();
    }
}