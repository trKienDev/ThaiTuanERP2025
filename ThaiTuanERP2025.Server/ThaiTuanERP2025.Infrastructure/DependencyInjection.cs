using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Permissions;
using ThaiTuanERP2025.Application.Account.Roles;
using ThaiTuanERP2025.Application.Authentication.Repositories;
using ThaiTuanERP2025.Application.Shared.Authentication;
using ThaiTuanERP2025.Application.Shared.Events;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Application.Shared.Options;
using ThaiTuanERP2025.Application.Shared.Security;
using ThaiTuanERP2025.Application.Shared.Services;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Core.Services;
using ThaiTuanERP2025.Application.Finance.BudgetApprovers;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Application.Finance.BudgetGroups;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Application.Finance.CashoutCodes;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Core.Repositories;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Account.Repositories.Read;
using ThaiTuanERP2025.Infrastructure.Account.Repositories.Write;
using ThaiTuanERP2025.Infrastructure.Authentication.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared;
using ThaiTuanERP2025.Infrastructure.Shared.Security;
using ThaiTuanERP2025.Infrastructure.Shared.Services;
using ThaiTuanERP2025.Infrastructure.Core.Repositories;
using ThaiTuanERP2025.Infrastructure.Core.Repositories.Read;
using ThaiTuanERP2025.Infrastructure.Core.Repositories.Write;
using ThaiTuanERP2025.Infrastructure.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read;
using ThaiTuanERP2025.Infrastructure.Finance.Repositories.Write;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Realtime;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Repositories;
using ThaiTuanERP2025.Application.Core.OutboxMessages;
using ThaiTuanERP2025.Infrastructure.BackgroundJobs;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes;
using ThaiTuanERP2025.Application.Finance.CashoutGroups;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Services;
using ThaiTuanERP2025.Infrastructure.Finance.Services;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Services;
using ThaiTuanERP2025.Infrastructure.Expense.Repositories.Read;
using ThaiTuanERP2025.Infrastructure.Expense.Repositories.Write;
using ThaiTuanERP2025.Application.Expense.Suppliers;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Account.Users.Services;
using ThaiTuanERP2025.Infrastructure.Account.Services;
using ThaiTuanERP2025.Application.Finance.BudgetTransasctions;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Repositories;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations;
using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments;
using ThaiTuanERP2025.Application.Core.Comments;
using ThaiTuanERP2025.Domain.StoredFiles.Repositories;
using ThaiTuanERP2025.Application.Core.FileAttachments.Repositories;

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

			/// ========= Repositories =========
			// Common
			services.AddScoped<IJWTProvider, JwtProvider>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<ICodeGenerator, CodeGenerator>();

			// Files
			services.AddScoped<IFileAttachmentWriteRepository, FileAttachmentWriteRepository>();
			services.AddScoped<IFileAttachmentReadRepository, FileAttachmentReadRepository>();

			// Account
			services.AddScoped<IUserReadRepostiory, UserReadRepository>();
			services.AddScoped<IUserWriteRepository, UserWriteRepository>();
			services.AddScoped<IUserManagerAssignmentReadRepository, UserManagerAssignmentReadRepository>();
			services.AddScoped<IUserManagerAssignmentWriteRepository, UserManagerAssignmentWriteRepository>();
			services.AddScoped<IDepartmentWriteRepository, DepartmentWriteRepository>();
			services.AddScoped<IDepartmentReadRepository, DepartmentReadRepository>();
			services.AddScoped<IGroupRepository, GroupRepository>();
			services.AddScoped<IUserGroupRepository, UserGroupRepository>();
			services.AddScoped<IRoleReadRepository, RoleReadRepository>();
			services.AddScoped<IRoleWriteRepository, RoleWriteRepository>();
			services.AddScoped<IPermissionWriteRepository, PermissionWriteRepository>();
			services.AddScoped<IPermissionReadRepository, PermissionReadRepository>();
			services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
			services.AddScoped<IUserRoleRepository, UserRoleRepository>();

			// Finance
			services.AddScoped<IBudgetCodeReadRepository, BudgetCodeReadRepository>();
			services.AddScoped<IBudgetCodeWriteRepository, BudgetCodeWriteRepository>();
			services.AddScoped<IBudgetGroupReadRepository,  BudgetGroupReadRepository>();
			services.AddScoped<IBudgetGroupWriteRepository, BudgetGroupWriteRepository>();
			services.AddScoped<IBudgetPeriodWriteRepository, BudgetPeriodWriteRepository>();
			services.AddScoped<IBudgetPeriodReadRepository, BudgetPeriodReadRepository>();
			services.AddScoped<IBudgetPlanReadRepository, BudgetPlanReadRepository>();
			services.AddScoped<IBudgetPlanWriteRepository, BudgetPlanWriteRepository>();
			services.AddScoped<IBudgetTransactionReadRepository, BudgetTransasctionReadRepository>();
			services.AddScoped<IBudgetTransactionWriteRepository, BudgetTransactionWriteRepository>();
			services.AddScoped<IBudgetPlanDetailWriteRepository, BudgetPlanDetailWriteRepository>();
			services.AddScoped<IBudgetApproverReadRepository, BudgetApproverReadRepository>();
			services.AddScoped<IBudgetApproverWriteRepository, BudgetApproverWriteRepository>();
			services.AddScoped<ILedgerAccountTypeReadRepository, LedgerAccountTypeReadRepository>();
			services.AddScoped<ILedgerAccountTypeWriteRepository, LedgerAccountTypeWriteRepository>();
			services.AddScoped<ILedgerAccountWriteRepository, LedgerAccountWriteRepository>();
			services.AddScoped<ILedgerAccountReadRepository, LedgerAccountReadRepository>();
			services.AddScoped<ICashoutCodeWriteRepository, CashoutCodeWriteRepository>();
			services.AddScoped<ICashoutCodeReadRepository, CashoutCodeReadRepository>();
			services.AddScoped<ICashoutGroupReadRepository, CashoutGroupReadRepository>();
			services.AddScoped<ICashoutGroupWriteRepository, CashoutGroupRepository>();

			// Expense
			services.AddScoped<ISupplierReadRepository, SupplierReadRepository>();
			services.AddScoped<ISupplierWriteRepository, SupplierWriteRepository>();
			services.AddScoped<IOutgoingBankAccountReadRepository, OutgoingBankAccountReadRepository>();
			services.AddScoped<IOutgoingBankAccountWriteRepository, OutgoingBankAccountWriteRepository>();
			services.AddScoped<IOutgoingPaymentReadRepository, OutgoingPaymentReadRepository>();
			services.AddScoped<IOutgoingPaymentWriteRepository, OutgoingPaymentWriteRepository>();
			services.AddScoped<IExpenseWorkflowTemplateReadRepository, ExpenseWorkflowTemplateReadRepository>();
			services.AddScoped<IExpenseWorkflowTemplateWriteRepository, ExpenseWorkflowTemplateWriteRepository>();
			services.AddScoped<IExpenseStepTemplateReadRepository, ExpenseStepTemplateReadRepository>();
			services.AddScoped<IExpenseStepTemplateWriteRepository, ExpenseStepTemplateWriteRepository>();
			services.AddScoped<IExpenseStepTemplateReadRepository, ExpenseStepTemplateReadRepository>();
			services.AddScoped<IExpenseStepInstanceReadRepository, ExpenseStepInstanceReadRepository>();
			services.AddScoped<IExpenseStepInstanceWriteRepository, ExpenseStepInstanceWriteRepository>();
			services.AddScoped<IExpenseWorkflowInstanceReadRepository, ExpenseWorkflowInstanceReadRepository>();
			services.AddScoped<IExpenseWorkflowInstanceWriteRepository, ExpenseWorkflowInstanceWriteRepository>();
			services.AddScoped<IExpensePaymentReadRepository, ExpensePaymentReadRepository>();
			services.AddScoped<IExpensePaymentWriteRepository, ExpensePaymentWriteRepository>();
			services.AddScoped<IExpensePaymentItemsWriteRepository, ExpensePaymentItemsWriteRepository>();
			services.AddScoped<IExpensePaymentAttachmentWriteRepository, ExpensePaymentAttachmentWriteRepository>();

			// Core
			services.AddScoped<IFollowerReadRepository, FollowerReadRepository>();
			services.AddScoped<IFollowerWriteRepository, FollowerWriteRepository>();
			services.AddScoped<IUserNotificationReadRepository, UserNotificationReadRepository>();
			services.AddScoped<IUserNotificationWriteRepository, UserNotificationWriteRepository>();
			services.AddScoped<IUserReminderReadRepository, UserReminderReadRepository>();
			services.AddScoped<IUserReminderWriteRepository, UserReminderWriteRepository>();	
			services.AddScoped<IOutboxMessageReadRepository, OutboxMessageReadRepository>();
			services.AddScoped<IOutboxMessageWriteRepository, OutboxMessageWriteRepository>();
			services.AddScoped<ICommentWriteRepository, CommentWriteRepository>();
			services.AddScoped<ICommentReadRepository, CommentReadRepository>();
			services.AddScoped<ICommentAttachmentWriteRepository, CommentAttachmentWriteRepository>();
			services.AddScoped<ICommentMentionWriteRepository, CommentMentionWriteRepository>();


			// Authentication
			services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

			// ========= File Storage (MinIO) =========
			services.Configure<FileStorageOptions>(cfg.GetSection("Minio"));
			services.AddOptions<FileStorageOptions>()
				.Bind(cfg.GetSection(FileStorageOptions.SectionName))
				.ValidateDataAnnotations()
				.Validate(o => !string.IsNullOrWhiteSpace(o.BasePath), "BasePath is required")
				.PostConfigure(o =>
				{
					// Chuẩn hoá path tuyệt đối (dùng forward/backward đều OK)
					o.BasePath = Path.GetFullPath(o.BasePath);
				});

			//// Realtime
			services.AddScoped<IRealtimeNotifier, SignalRealtimeNotifier>();
			services.AddScoped<INotificationService, NotificationService>();
			services.AddScoped<IReminderService, ReminderService>();

			// DocumentSubIdOptions
			services.Configure<DocumentSubIdOptions>(opt => {
				// Expense
				opt.TypeDigits[DocumentType.ExpensePayment] = "11";
				opt.TypeDigits[DocumentType.AdvancedPayment] = "12";
				opt.TypeDigits[DocumentType.AdvancedExpensePayment] = "13";
				opt.TypeDigits[DocumentType.OutgoingPayment] = "14";
			});

			// DomainEventDispatcher
			services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

			// Logging
			services.AddScoped<ILoggingService, SerilogLoggingService>();

			// Jwt
			services.Configure<JwtSettings>(cfg.GetSection("Jwt"));

			// Service
			services.AddScoped<ICurrentRequestIpProvider, CurrentRequestIpProvider>();
			services.AddHostedService<OutboxProcessorHostedService>();
			services.AddScoped<ILedgerAccountTypeExcelReader, LedgerAccountTypeExcelReaderService>();
			services.AddScoped<ILedgerAccountExcelReader, LedgerAccountExcelReaderService>();
			services.AddScoped<IUserManagerService, UserManagerService>();

			return services;
		}
	}
}
