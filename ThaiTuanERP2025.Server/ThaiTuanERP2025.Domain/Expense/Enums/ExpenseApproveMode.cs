namespace ThaiTuanERP2025.Domain.Expense.Enums
{
	public enum ExpenseApproveMode: byte {
		Standard = 0, // cố định approverIds (select user)  (khớp UI "standard")
		Condition = 1,  // resolve theo điều kiện (manager-department, v.v.) (khớp UI "condition")
	}
}
