using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Common.Authentication;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Application.Expense.Invoices;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Domain.Files.Repositories;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Domain.Followers.Repositories;
using ThaiTuanERP2025.Domain.Notifications.Repositories;
using ThaiTuanERP2025.Infrastructure.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Authentication;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Common.Services;
using ThaiTuanERP2025.Infrastructure.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Followers.Repositories;
using ThaiTuanERP2025.Infrastructure.Notifications.Repositories;
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
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			/// ========= Repositories =========
			// Common
			services.AddScoped<IJWTProvider, JwtProvider>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<ICodeGenerator, CodeGenerator>();

			// Files
			services.AddScoped<IStoredFilesRepository, StoredFilesRepository>();

			// Account
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IUserReadRepostiory, UserReadRepository>();
			services.AddScoped<IUserManagerAssignmentRepository, UserManagerAssignmentRepository>();
			services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			services.AddScoped<DepartmentReadRepository>();
			services.AddScoped<IGroupRepository, GroupRepository>();
			services.AddScoped<IUserGroupRepository, UserGroupRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<IPermissionRepository, PermissionRepository>();
			services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
			services.AddScoped<IUserRoleRepository, UserRoleRepository>();

			// Finance
			services.AddScoped<IBudgetCodeRepository, BudgetCodeRepository>();
			services.AddScoped<IBudgetCodeReadRepository, IBudgetCodeReadRepository>();
			services.AddScoped<IBudgetGroupRepository, BudgetGroupRepository>();
			services.AddScoped<IBudgetPeriodRepository, BudgetPeriodRepository>();
			services.AddScoped<IBudgetPlanRepository, BudgetPlanRepository>();
			services.AddScoped<ILedgerAccountTypeRepository, LedgerAccountTypeRepository>();
			services.AddScoped<ILedgerAccountRepository, LedgerAccountRepository>();
			services.AddScoped<ILedgerAccountReadRepository, LedgerAccountReadRepository>();
			services.AddScoped<ICashoutCodeRepository, CashoutCodeRepository>();
			services.AddScoped<ICashoutGroupRepository, CashoutGroupRepository>();

			// Expense
			services.AddScoped<IInvoiceRepository, InvoiceRepository>();
			services.AddScoped<IInvoiceReadRepository, InvoiceReadRepository>();
			services.AddScoped<IInvoiceLineRepository, InvoiceLineRepository>();
			services.AddScoped<IInvoiceFollowerRepository, InvoiceFollowerRepository>();
			services.AddScoped<IInvoiceFileRepository, InvoiceFileRepository>();
			services.AddScoped<ISupplierRepository, SupplierRepository>();
			services.AddScoped<IBankAccountRepository, BankAccountRepository>();
			services.AddScoped<IOutgoingBankAccountRepository, OutgoingBankAccountRepository>();
			services.AddScoped<IOutgoingPaymentRepository, OutgoingPaymentRepository>();
			services.AddScoped<IApprovalWorkflowTemplateRepository, ApprovalWorkflowTemplateRepository>();
			services.AddScoped<IApprovalStepTemplateRepository, ApprovalStepTemplateRepository>();
			services.AddScoped<IApprovalStepInstanceRepository, ApprovalStepInstanceRepository>();
			services.AddScoped<IApprovalWorkflowInstanceRepository, ApprovalWorkflowInstanceRepository>();
			services.AddScoped<IExpensePaymentRepository, ExpensePaymentRepository>();
			services.AddScoped<IExpensePaymentCommentRepository, ExpensePaymentCommentRepository>();
			services.AddScoped<IExpensePaymentCommentTagRepository, ExpensePaymentCommentTagRepository>();
			services.AddScoped<IExpensePaymentCommentAttachmentRepository, ExpensePaymentCommentAttachmentRepository>();

			// Notifications & Reminders
			services.AddScoped<INotificationRepository, NotificationRepository>();
			services.AddScoped<ITaskReminderRepository, TaskReminderRepository>();
			services.AddScoped<IFollowerRepository, FollowerRepository>();		

			// ========= File Storage (MinIO) =========
			services.Configure<FileStorageOptions>(cfg.GetSection("Minio"));
			services.AddScoped<IFileStorage, LocalFileStorage>();

			return services;
		}
	}
}
