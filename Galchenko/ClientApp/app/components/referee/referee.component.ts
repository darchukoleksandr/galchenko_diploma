import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { RefereeViewModel } from "../../models/users";

@Component({
    templateUrl: './referee.component.html'
})

export class RefereeComponent {

    private referees: RefereeViewModel[];

    private formOpen: boolean;
    private newReferee: RefereeViewModel;

    constructor(private http: Http) {
        this.http.get('/api/users/referees/all').subscribe(result => {
            this.referees = result.json() as RefereeViewModel[];
        });
    }

}
