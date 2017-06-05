import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

@Component({
    selector: 'registration-form',
    templateUrl: './registration.component.html'
})
export class RegistrationComponent {
    private registreationUrl = 'api/account/register';

    private roles = [
        { value: 'Coach', display: 'Тренер' },
        { value: 'Referee', display: 'Рефері' },
        { value: 'Student', display: 'Студент' },
        { value: 'Moderator', display: 'Модератор' }
    ];

    constructor(
        private http: Http,
        private router: Router,
        private toastyService: ToastyService
    ) {
        
    }

    result = new RegisterViewModel('', '', '', '', '', '', 'Student');

    onSubmit() {
        if (this.result.password !== this.result.confirmPassword) {
            this.toastyService.warning('Паролі не співпадають!');
            return;
        }

        this.result.userName = this.result.userName.replace(/\s+$/, '');
        this.result.password = this.result.password.replace(/\s+$/, '');
        this.result.confirmPassword = this.result.confirmPassword.replace(/\s+$/, '');
        this.result.firstName = this.result.firstName.replace(/\s+$/, '');
        this.result.lastName = this.result.lastName.replace(/\s+$/, '');
        this.result.email = this.result.email.replace(/\s+$/, '');

        this.http.post('/api/accounts/register', this.result).subscribe(result => {
            if (result.status === 200) {
                this.toastyService.success('Профіль створено!');
                this.router.navigate(['']);
            }
            console.log(result);
        });
    }
}

class RegisterViewModel {
    public email: string;
    public userName: string;
    public password: string;
    public confirmPassword: string;
    public firstName: string;
    public lastName: string;
    public role: string;

    constructor(
        email: string,
        userName: string,
        password: string,
        confirmPassword: string,
        firstName: string,
        lastName: string,
        role: string
    ) {
        this.email = email;
        this.userName = userName;
        this.password = password;
        this.confirmPassword = confirmPassword;
        this.lastName = lastName;
        this.role = role;
        this.firstName = firstName;
    }
}