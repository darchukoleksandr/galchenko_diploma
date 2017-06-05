import { Component, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { IdentityService } from "../../services/identity.service";
import { CompetitionPointsViewModel } from "../../models/competitions";
import { TeamViewModel } from "../../models/teams";
import { ApplicationUserViewModel } from "../../models/users";
import { KindOfSport } from "../../models/models";
import { Subscription } from 'rxjs/Subscription';

@Component({
    templateUrl: './teams.component.html',
    providers: [IdentityService]
})

export class TeamsComponent implements OnDestroy{
    private sports: KindOfSport[];
    private teams: TeamViewModel[];

    private selectedSport: number;
    private filteredTeams: TeamViewModel[];

    private showTop: boolean;
    private teamPoints: CompetitionPointsViewModel[];

    private loggedUser: ApplicationUserViewModel;
    private identitySubscription: Subscription;

    private teamSelection: number;

    private showPointsRate(value: boolean) {
        this.showTop = value;
    }

    private sortTeams() {
        if (this.selectedSport == 0)
            this.filteredTeams = this.teams;
        else
            this.filteredTeams = this.teams.filter(team => team.kindOfSport.id == this.selectedSport);
    }

    private showLoggedCoachTeams() {
        this.filteredTeams = this.teams.filter(team => team.coach.id === this.loggedUser.id);
    }

    constructor(
        private http: Http,
        private activateRoute: ActivatedRoute,
        private identityService: IdentityService
    )
    {
        this.identitySubscription = this.identityService.loggedUser.subscribe(coach => {
            this.loggedUser = coach as ApplicationUserViewModel;
        });

        this.showTop = false;

        this.http.get('/api/sports/all').subscribe(result => {
            this.sports = result.json() as KindOfSport[];
            this.sports.unshift(new KindOfSport('Âñ³', 0));
        });
        this.http.get('/api/teams/all').subscribe(result => {
            this.teams = result.json() as TeamViewModel[];
            this.filteredTeams = this.teams;
        });
    }

    ngOnDestroy(): void {
        this.identitySubscription.unsubscribe();
    }
}