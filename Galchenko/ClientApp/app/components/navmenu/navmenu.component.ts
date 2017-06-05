import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { IdentityService } from "../../services/identity.service";

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css'],
    providers: [IdentityService]
})
export class NavMenuComponent {
    
    constructor(
        private http: Http,
        private router: Router,
        private identityService: IdentityService
    ) {
    }

    logout() {
        this.http.post('/api/accounts/Logout', null).subscribe(result => {
            if (result.status === 200) {
                this.identityService.updateValue(undefined);
                this.router.navigateByUrl('/login');
            }
//            console.log(result);
        });
    }

}
