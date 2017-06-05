import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { StudentViewModel } from "../../models/users";

@Component({
    templateUrl: './student.component.html'
})

export class StudentComponent {
    private students: StudentViewModel[];
    private filteredStudents: StudentViewModel[];
    
    private showTeam: boolean = true;

    sortAll() {
        this.filteredStudents = this.students;
        this.showTeam = true;
    }

    sortNoTeam() {
        this.filteredStudents = this.students.filter(student => student.team == null);
        this.showTeam = false;
    }

    sortTeam() {
        this.filteredStudents = this.students.filter(student => student.team != null);
        this.showTeam = true;
    }

    constructor(
        private http: Http
    )
    {
        this.http.get('/api/users/students/all').subscribe(result => {
            this.students = result.json() as StudentViewModel[];
            this.filteredStudents = this.students;
        });
    }
}