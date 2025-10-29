using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetGroup : AuditableEntity
	{
		private BudgetGroup() { }
		public BudgetGroup(string code, string name) {
			if (string.IsNullOrWhiteSpace(code))
				throw new ArgumentNullException($"code {code} cannot be null");

			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException($"name {name} cannot be null");

			Code = code;
			Name = name;
		}

		public string Code	{ get; private set; } = string.Empty;
		public string Name { get; private set; } = string.Empty;

		public ICollection<BudgetCode> BudgetCodes { get; private set; } = new List<BudgetCode>();

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		#region Update 
		internal void UpdateBudgetGroup(string code, string name) {
			if (string.IsNullOrWhiteSpace(code))
				throw new ArgumentNullException($"code {code} cannot be null");

			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException($"name {name} cannot be null");

			Code = code;
			Name = name;
		}
		#endregion

		#region Add BudgetCode
		internal void AddBudgetCodesByIds(IEnumerable<Guid> budgetCodeIds, IEnumerable<BudgetCode> allBudgetCodes)
		{
			if (budgetCodeIds == null || !budgetCodeIds.Any())
				return;

			var toAdd = allBudgetCodes .Where(b => budgetCodeIds.Contains(b.Id)).ToList();

			foreach (var code in toAdd)
			{
				if (!BudgetCodes.Any(b => b.Id == code.Id))
				{
					BudgetCodes.Add(code);
				}
			}
		}
		#endregion

		#region Remove Budget Code
		internal void RemoveBudgetCodes(IEnumerable<Guid> budgetCodeIds)
		{
			if (budgetCodeIds == null || !budgetCodeIds.Any())
				return;

			var toRemove = BudgetCodes
				.Where(b => budgetCodeIds.Contains(b.Id))
				.ToList();

			foreach (var code in toRemove)
			{
				BudgetCodes.Remove(code);
			}
		}
		#endregion

	}
}
