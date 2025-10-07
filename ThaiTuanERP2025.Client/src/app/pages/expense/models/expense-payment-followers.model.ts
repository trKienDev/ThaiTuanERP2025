import { UserDto } from "../../account/models/user.model";

export interface ExpensePaymentFollowerDto {
      id: string;
      expensePaymentId: string;
      userId: string;
      user: UserDto;
}

export interface ExpensePaymentFollowerRequest {
      expensePaymentId: string;
      userId: string;
}