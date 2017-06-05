import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { KindOfSport } from "../../models/models";
import { CoachViewModel } from "../../models/users";

import { ToastyService, ToastyConfig } from 'ng2-toasty';

@Component({
    templateUrl: './coaches.component.html'
})

export class CoachComponent {
    private coaches: CoachViewModel[];
    
    private sports: KindOfSport[];

    constructor(
        private http: Http,
        private toastyService: ToastyService,
        private toastyConfig: ToastyConfig
    )
    {
        this.http.get('/api/coaches/all').subscribe(result => {
            this.coaches = result.json() as CoachViewModel[];
        });
        this.http.get('/api/sports/all').subscribe(result => {
            this.sports = result.json() as KindOfSport[];
        });
    }
}