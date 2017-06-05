import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { IdentityService } from "../../../services/identity.service";
import { Subscription } from 'rxjs/Subscription';
import { ToastyService } from 'ng2-toasty';

@Component({
    selector: 'user',
    templateUrl: './user.component.html',
    providers: [IdentityService]
})
export class UserCabinetComponent implements OnDestroy {

    private identitySubscription: Subscription;

    constructor(
        private toastyService: ToastyService,
        private identityService: IdentityService,
        private router: Router
    ) {
        this.identitySubscription = this.identityService.loggedUser.subscribe(user => {

            if (!user) {
                toastyService.warning('Тільки зареєстрований користувач може ввійти!');
                this.router.navigate(['/login']);
            } else {
                switch (user.role) {
                case 'Student':
                    this.router.navigate(['/cabinet/student']);
                    break;
                case 'Coach':
                    this.router.navigate(['/cabinet/coach']);
                    break;
                case 'Referee':
                    this.router.navigate(['/cabinet/referee']);
                    break;
                case 'Moderator':
                    this.router.navigate(['/cabinet/moderator']);
                    break;
                case 'Administrator':
                    this.router.navigate(['/cabinet/admin']);
                    break;
                default:
                }
            }
        });
    }

    ngOnDestroy(): void {
        this.identitySubscription.unsubscribe();
    }
}
