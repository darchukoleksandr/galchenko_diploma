import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { FormsModule } from '@angular/forms';

import { ToastyModule, ToastyService } from 'ng2-toasty';
//import { IdentityService } from "./services/identity.service";

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginComponent } from './components/login/login.component';
import { RefereeComponent } from './components/referee/referee.component';
import { CoachComponent } from './components/coaches/coaches.component';

import { TeamDetailsComponent } from './components/team/team.component';
import { TeamsComponent } from './components/teams/teams.component';
import { StudentComponent } from './components/student/student.component';
import { CompetitionComponent } from './components/competition/competition.component';
import { CompetitionsComponent } from './components/competitions/competitions.component';

import { UserCabinetComponent } from './components/cabinets/user/user.component';
import { StudentCabinetComponent } from './components/cabinets/student/student.component';
import { ModeratorCabinetComponent } from './components/cabinets/moderator/moderator.component';
import { CoachCabinetComponent } from './components/cabinets/coach/coach.component';
import { RefereeCabinetComponent } from './components/cabinets/referee/referee.component';

import { NewTeamComponent } from "./components/newTeam/newTeam.component";

@NgModule({
    bootstrap: [AppComponent],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        ToastyModule.forRoot(),
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full'},
            { path: 'home', component: HomeComponent },
            { path: 'registration', component: RegistrationComponent },
            { path: 'login', component: LoginComponent },

            { path: 'user', component: UserCabinetComponent },
            { path: 'cabinet/student', component: StudentCabinetComponent },
            { path: 'cabinet/coach', component: CoachCabinetComponent },
            { path: 'cabinet/moderator', component: ModeratorCabinetComponent },
            { path: 'cabinet/referee', component: RefereeCabinetComponent },

            { path: 'referees', component: RefereeComponent },
            { path: 'coaches', component: CoachComponent },
            { path: 'teams', component: TeamsComponent },
            { path: 'newTeam', component: NewTeamComponent },
            { path: 'competitions', component: CompetitionsComponent },
            { path: 'students', component: StudentComponent },
            
            { path: 'team/:id', component: TeamDetailsComponent },
            { path: 'competition/:id', component: CompetitionComponent },

            { path: '**', redirectTo: 'home' }
        ])
    ],
    declarations: [
        HomeComponent,
        AppComponent,
        NavMenuComponent,
        RegistrationComponent,
        LoginComponent,
        RefereeComponent,
        CoachComponent,
        TeamsComponent,
        NewTeamComponent,
        StudentComponent,
        CompetitionComponent,
        CompetitionsComponent,

        TeamDetailsComponent,

        UserCabinetComponent,
        StudentCabinetComponent,
        RefereeCabinetComponent,
        ModeratorCabinetComponent,
        CoachCabinetComponent
    ],
//    exports: [ToastyModule],
    providers: [ToastyService]
})

export class AppModule {

}
