using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Application.Files.Repositories;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Authentication;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Common.Services;
using ThaiTuanERP2025.Infrastructure.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations;
using ThaiTuanERP2025.Infrastructure.StoredFiles.FileStorage;
using ThaiTuanERP2025.Infrastructure.StoredFiles.Repositories;

namespace ThaiTuanERP2025.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
		{
			// ========= Database =========
			services.AddScoped<ThaiTuanERP2025DbContext>();
			services.AddDbContext<ThaiTuanERP2025DbContext>(options => {
				options.UseSqlServer(cfg.GetConnectionString("ThaiTuanERP2025Db"), sqlOptions =>
				{
					sqlOptions.EnableRetryOnFailure();
				});
			});
			services.AddScoped<IUnitOfWork, AppUnitOfWork>();

			// ========= Repositories =========
			services.AddScoped<iJWTProvider, JwtProvider>();
			services.AddScoped<IUnitOfWork, AppUnitOfWork>();
			services.AddScoped<ICodeGenerator, CodeGenerator>();
			services.AddScoped<IStoredFilesRepository, StoredFilesRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IUserManagerAssignmentRepository, UserManagerAssignmentRepository>();
			services.AddScoped<IUserReadRepository, UserReadRepository>();
			services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			services.AddScoped<IGroupRepository, GroupRepository>();
			services.AddScoped<IUserGroupRepository, UserGroupRepository>();
			services.AddScoped<IBudgetCodeRepository, BudgetCodeRepository>();
			services.AddScoped<IBudgetGroupRepository, BudgetGroupRepository>();
			services.AddScoped<IBudgetPeriodRepository, BudgetPeriodRepository>();
			services.AddScoped<IBudgetPlanRepository, BudgetPlanRepository>();
			services.AddScoped<ILedgerAccountTypeRepository, LedgerAccountTypeRepository>();
			services.AddScoped<ILedgerAccountRepository, LedgerAccountRepository>();
			services.AddScoped<ICashoutCodeRepository, CashoutCodeRepository>();
			services.AddScoped<ICashoutGroupRepository, CashoutGroupRepository>();
			services.AddScoped<ITaxRepository, TaxRepository>();
			services.AddScoped<IWithholdingTaxTypeRepository, WithholdingTaxTypeRepository>();
			services.AddScoped<IInvoiceRepository, InvoiceRepository>();
			services.AddScoped<IInvoiceLineRepository, InvoiceLineRepository>();
			services.AddScoped<IInvoiceFollowerRepository, InvoiceFollowerRepository>();
			services.AddScoped<IInvoiceFileRepository, InvoiceFileRepository>();
			services.AddScoped<ISupplierRepository, SupplierRepository>();
			services.AddScoped<IBankAccountRepository, BankAccountRepository>();
			services.AddScoped<IApprovalWorkflowTemplateRepository, ApprovalWorkflowTemplateRepository>();
			services.AddScoped<IApprovalStepTemplateRepository, ApprovalStepTemplateRepository>();
			services.AddScoped<IApprovalStepInstanceRepository, ApprovalStepInstanceRepository>();
			services.AddScoped<IApprovalWorkflowInstanceRepository, ApprovalWorkflowInstanceRepository>();
			services.AddScoped<IExpensePaymentRepository, ExpensePaymentRepository>();

			// ========= File Storage (MinIO) =========
			services.Configure<FileStorageOptions>(cfg.GetSection("Minio"));
			services.AddScoped<IFileStorage, LocalFileStorage>();

			return services;
		}
	}
}
