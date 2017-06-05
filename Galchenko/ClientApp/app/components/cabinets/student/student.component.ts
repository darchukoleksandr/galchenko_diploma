import { Component, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { IdentityService } from "../../../services/identity.service";
import { TeamViewModel } from "../../../models/teams";
import { StudentViewModel } from "../../../models/users";
import { CompetitionJoinRequestViewModel, CompetitionPointsViewModel } from "../../../models/competitions";
import { Subscription } from 'rxjs/Subscription';
import { ToastyService } from 'ng2-toasty';

@Component({
    templateUrl: './student.component.html',
    providers: [IdentityService]
})
export class StudentCabinetComponent implements OnDestroy {
    private team: TeamViewModel;

    private studentRating: number;
    private showMyPoints: boolean;
    private myPoints: CompetitionPointsViewModel[];

    private competitions: CompetitionJoinRequestViewModel[];
    private loggedUser: StudentViewModel;
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

            this.loggedUser = user as StudentViewModel;
            this.showMyPoints = false;

            this.http.get('/api/users/students/' + this.loggedUser.id).subscribe(result => {
                this.loggedUser = result.json() as StudentViewModel;
            });

            if (this.loggedUser.team) {
                this.http.get('/api/competitions/soon/' + this.loggedUser.team.id).subscribe(result => {
                    this.competitions = result.json() as CompetitionJoinRequestViewModel[];
                });
            }
            
            this.http.get('/api/competitions/points/player/' + this.loggedUser.id).subscribe(result => {
                this.myPoints = result.json() as CompetitionPointsViewModel[];
            });
            this.http.get('/api/rating/student/' + this.loggedUser.id).subscribe(result => {
                var rating = result.json() as number;
                if (rating)
                    this.studentRating = rating;
                this.studentRating = 0;
            });
        });

    }

    showPoints(value: boolean) {
        this.showMyPoints = value;
    }

    leaveTeam() {
        if (confirm('Are you sure?')) {
            this.http.post('api/teams/request/leave', this.loggedUser.id).subscribe(result => {
                if (result.status === 200) {
                    this.toastyService.success('Команда покинута!');
                    this.router.navigate(['/teams']);
                }
            });
        }
    }

    ngOnDestroy(): void {
        this.identitySubscription.unsubscribe();
    }
}
