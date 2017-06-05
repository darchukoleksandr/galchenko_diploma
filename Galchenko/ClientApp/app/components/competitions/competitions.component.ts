import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { CompetitionViewModel } from "../../models/competitions";
import { KindOfSport } from "../../models/models";

@Component({
    templateUrl: './competitions.component.html'
})

export class CompetitionsComponent {
    private sports: KindOfSport[];
    private competitions: CompetitionViewModel[];
    private filteredCompetitions: CompetitionViewModel[];
    private selectedSport: number;

    private dateFrom: Date;
    private dateTo: Date;

    private selectedCompetitions: number;

    constructor(
        private http: Http,
        private activateRoute: ActivatedRoute
    )
    {
        this.http.get('/api/sports/all').subscribe(result => {
            this.sports = result.json() as KindOfSport[];
            this.sports.unshift(new KindOfSport('All', 0));
        });

        this.http.get('/api/competitions/all').subscribe(result => {
            this.competitions = result.json() as CompetitionViewModel[];
            this.filteredCompetitions = this.competitions.filter(competition => competition);
        });
    }

    sortByDate() {
        if (!this.dateFrom || !this.dateTo) {
            return;
        }
        console.log(this.dateFrom);
        console.log(this.dateTo);
        if (this.selectedSport == 0) {
            this.filteredCompetitions = this.competitions.filter(competition => competition.date > this.dateFrom && competition.date < this.dateTo);
            return;
        }

        this.filteredCompetitions = this.competitions.filter(competition =>
            competition.date > this.dateFrom &&
            competition.date < this.dateTo &&
            competition.kindOfSport.id == this.selectedSport
        );
    }

    private sortBySport() {
        if (!this.dateFrom || !this.dateTo) {
            this.sortByDate();
            return;
        }
        if (this.selectedSport == 0) {
            this.filteredCompetitions = this.competitions.filter(competition => competition);
            return;
        }

        this.filteredCompetitions = this.competitions.filter(competition => competition.kindOfSport.id == this.selectedSport);
    }
}