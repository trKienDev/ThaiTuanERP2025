import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { GroupApiService } from "../../services/api/group-api.service";
import { FormsModule } from "@angular/forms";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { GroupModel } from "../../models/group.model";

@Component({
      selector: 'account-group',
      standalone: true,
      imports: [CommonModule, FormsModule ],
      templateUrl: './account-group.component.html',
})
export class AccountGroupComponent {

      
}