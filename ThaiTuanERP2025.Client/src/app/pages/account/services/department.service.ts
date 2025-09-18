import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { CreateDepartmentRequest, DepartmentDto, UpdateDepartmentRequest } from "../models/department.model";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: 'root' }) 
export class DepartmentService extends BaseCrudService<DepartmentDto, CreateDepartmentRequest, UpdateDepartmentRequest> {
      constructor(http: HttpClient) {
            super(http,`${environment.apiUrl}/department`);
      }
}