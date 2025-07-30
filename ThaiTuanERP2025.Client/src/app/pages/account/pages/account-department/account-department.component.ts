import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { environment } from "../../../../../environments/environment";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [ CommonModule, FormsModule, HttpClientModule ],
      templateUrl: './account-department.component.html',
      styleUrl: './account-department.component.scss'
})
export class AccountDepartmentComponent {
      private readonly API_URL = `${environment.apiUrl}/department`;
      newDepartment = {
            code: '',
            name: '',
      };

      departments: { code: string, name: string}[] = [];

      constructor(private http: HttpClient){}

      addDepartment() {
            this.http.post(this.API_URL, this.newDepartment).subscribe({
                  next: () => {
                        this.departments.push({ ...this.newDepartment });
                        this.newDepartment = { code: '', name: '' };
                  }, 
                  error: (err) => {
                        alert("Lỗi khi thêm phòng ban: ");
                        throw new Error(`Lỗi khi thêm phòng ban: ${err}`);
                  }
            });
      }
}