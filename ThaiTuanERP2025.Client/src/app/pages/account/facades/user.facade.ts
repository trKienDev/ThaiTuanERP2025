import { inject, Injectable } from "@angular/core";
import { CreateUserRequest, UpdateUserRequest, UserDto } from "../models/user.model";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { UserService } from "../services/user.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class UserFacade extends BaseCrudFacade<UserDto, CreateUserRequest, UpdateUserRequest>{
      constructor() {
            super(inject(UserService));
      }
      readonly users$: Observable<UserDto[]> = this.list$;
}