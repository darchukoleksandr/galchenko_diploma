import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http } from '@angular/http';
import { ApplicationUserViewModel } from "../models/users";

@Injectable()
export class IdentityService{
    public loggedUser: Observable<ApplicationUserViewModel>;

    private observer: any; // I don't know the proper type for this.
    
    constructor(
        private http: Http
    ) {
        this.loggedUser = new Observable(observer => this.observer = observer);
        this.http.get('/api/accounts/current')
            .subscribe(
            result =>
            {
                if (result.status === 200) {
                    this.updateValue(result.json().result as ApplicationUserViewModel);
                }
            },
            error => {
                this.updateValue(undefined);
//                toastyService.info('It is recommended for you to register!');
            }
        );
    }

    public updateValue(value) {
//        console.log(value);
//        console.log(this.loggedUser.valueOf());
//        console.log(this.observer);
        if (this.observer) {
            this.observer.next(value);
        }
    }
}