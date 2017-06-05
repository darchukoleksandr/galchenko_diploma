import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { ApplicationUserViewModel } from "../../models/users";
import { IdentityService } from "../../services/identity.service";
import { ToastyService } from 'ng2-toasty';

@Component({
    selector: 'login-form',
    templateUrl: './login.component.html',
    providers: [IdentityService]
})
export class LoginComponent {

    private result = new LoginViewModel('', '', false);

    constructor(
        private http: Http,
        private router: Router,
        private toastyService: ToastyService,
        private identityService: IdentityService
    ) {

    }


    login() {
        this.http.post('/api/accounts/Login', this.result).subscribe(
            result => {
            if (result.status === 200) {
                this.identityService.updateValue(result.json() as ApplicationUserViewModel);
                this.router.navigate(['user']);
            }
            },
            () => {
                this.toastyService.warning('Невірно введені дані!');
            }
        );
    }

    logout() {
        this.http.post('/api/accounts/Logout', null).subscribe(result => {
            if (result.status === 200) {
                this.identityService.updateValue(undefined);
                this.router.navigateByUrl('/login');
            }
        });
    }
}

class LoginViewModel {
    public userName: string;
    public password: string;
    public rememberMe: boolean;

    constructor(
        userName: string,
        password: string,
        rememberMe: boolean
    ) {
        this.password = password;
        this.userName = userName;
        this.rememberMe = rememberMe;
    }
}
