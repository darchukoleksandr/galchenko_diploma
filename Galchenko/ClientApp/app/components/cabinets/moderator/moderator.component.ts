import { Component, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { IdentityService } from "../../../services/identity.service";
import { CompetitionViewModel, CompetitionPointsViewModel, CompetitionJoinRequestViewModel, CompetitionJoinRequestResultViewModel, CreateCompetitionViewModel } from "../../../models/competitions";
//import { TeamJoinRequestResultViewModel, TeamJoinRequest, TeamViewModel } from "../../../models/teams";
import { ApplicationUserViewModel, ModeratorViewModel, RefereeViewModel } from "../../../models/users";
import { KindOfSport } from "../../../models/models";
import { Subscription } from 'rxjs/Subscription';
import { ToastyService } from 'ng2-toasty';

@Component({
    templateUrl: './moderator.component.html',
    providers: [IdentityService]
})
export class ModeratorCabinetComponent implements OnDestroy {
    private selectedCompetition: CompetitionViewModel;
    private competitions: CompetitionViewModel[];
    private joinRequests: CompetitionJoinRequestViewModel[];
    private filteredjoinRequests: CompetitionJoinRequestViewModel[];
    private referees: RefereeViewModel[];
    private sports: KindOfSport[];

    hideModal: boolean = false;
    isCompetitionOutOfDate: boolean;
    isCompetitionHasPoints: boolean;

    private kindOfSportToChange: KindOfSport;

    private newKindOfSport: KindOfSport = new KindOfSport('');
    private formNewSportOpened: boolean;

    private newCompetition: CreateCompetitionViewModel;

    private loggedUser: ApplicationUserViewModel;
    private identitySubscription: Subscription;

    constructor(
        private http: Http,
        private toastyService: ToastyService,
        private identityService: IdentityService,
        private router: Router
    ) {
        this.identitySubscription = this.identityService.loggedUser.subscribe(user => {

            if (!user) {
                toastyService.warning('Тільки зареєстрований користувач може ввійти!');
                this.router.navigate(['']);
            }

            this.loggedUser = user as ApplicationUserViewModel;

            this.http.get('/api/users/moderators/' + this.loggedUser.id).subscribe(result => {
                this.loggedUser = result.json() as ModeratorViewModel;
            });

            this.http.get('/api/competitions/moderator/' + this.loggedUser.id).subscribe(result => {
                this.competitions = result.json() as CompetitionViewModel[];
            });

            this.http.get('/api/competitions/requests/all').subscribe(result => {
                this.joinRequests = result.json() as CompetitionJoinRequestViewModel[];
            });

            this.http.get('/api/users/referees/all').subscribe(result => {
                this.referees = result.json() as RefereeViewModel[];
            });

            this.http.get('/api/sports/all').subscribe(result => {
                this.sports = result.json() as KindOfSport[];
            });

            this.newCompetition = new CreateCompetitionViewModel('', '', new Date(), 0, '', this.loggedUser.id);
        });

    }

    selectCompetition(competition) {
        this.selectedCompetition = competition;
        
//        this.isCompetitionOutOfDate = new Date().getTime() > Date.parse(this.selectedCompetition.date.toString());
//        this.http.get('/api/competitions/points/', this.selectedCompetition.id).subscribe(result => {
//            this.isCompetitionHasPoints = result.json() as boolean;
//        });


        this.filteredjoinRequests = this.joinRequests.filter(request => request.competition.id === this.selectedCompetition.id);
    }
    
    // ------------------------ New competition
    createNewCompetition() {
        this.http.post('/api/competitions/create', this.newCompetition).subscribe(result => {
            if (result.status === 200) {
                this.toastyService.success("Змагання створено!");

                this.http.get('/api/competitions/moderator/' + this.loggedUser.id).subscribe(result => {
                    this.competitions = result.json() as CompetitionViewModel[];
                    console.log(this.competitions);
                });

                this.hideModal = true;
            }
        });
    }
    // ------------------------ Requests
    private confirmRequest(request: CompetitionJoinRequestViewModel) {
        if (request.result) {
            this.toastyService.warning("Участь даної команди вже підтверджена!");
            return;
        }

        if (this.filteredjoinRequests.filter(request => request.result).length >= 2) {
            this.toastyService.warning("Тільки дві команди можуть змагатися!");
            return;
        }

        var result = new CompetitionJoinRequestResultViewModel(request.id, request.competition.id, request.team.id, true);

        this.http.post('/api/competitions/requests/result', result).subscribe(result => {
            if (result.status === 200) {
                request.result = true;
            }
        });
    }

    private rejectRequest(request: CompetitionJoinRequestViewModel) {
        var result = new CompetitionJoinRequestResultViewModel(request.id, request.competition.id, request.team.id, false);

        this.http.post('/api/competitions/requests/result', result).subscribe(result => {
            if (result.status === 200) {
                request.result = false;
            }
            if (result.status === 203) {
                this.toastyService.warning('Не можна відмінити участь команди, оскількі вже були записані результати!');
            }
        });
    }

    private openNewSportForm() {
        this.formNewSportOpened = true;
    }

    private createNewSport() {
        this.http.post('/api/sports/create', this.newKindOfSport).subscribe(result => {
            if (result.status === 200) {
                this.toastyService.success('Новий вид спорту успішно додано!');
                this.formNewSportOpened = false;
                this.newKindOfSport.sport = '';

                this.http.get('/api/sports/all').subscribe(result => {
                    this.sports = result.json() as KindOfSport[];
                });
            }
        });
    }

    changeSportOpenModal(sport: KindOfSport) {
        this.kindOfSportToChange = sport;
    }

    changeSport() {
        this.http.post('/api/sports/update', this.kindOfSportToChange).subscribe(result => {
            if (result.status === 200) {
                this.toastyService.success('Дані успішно оновлено!');
                //                this.kindOfSports = this.kindOfSports.filter(kind => kind !== sport);
            }
        });

        this.kindOfSportToChange = null;
    }

    deleteSport(sport: KindOfSport) {
        this.http.post('/api/sports/delete', sport).subscribe(result => {
            if (result.status === 200) {
                this.toastyService.success('Новий вид спорту успішно додано!');
                this.sports = this.sports.filter(kind => kind !== sport);
            }
            if (result.status === 203) {
                this.toastyService.warning('Не можна видалити вид спорту, оскільки існують команди які ним займаються!');
            }
        });
    }

    ngOnDestroy(): void {
        this.identitySubscription.unsubscribe();
    }
}
