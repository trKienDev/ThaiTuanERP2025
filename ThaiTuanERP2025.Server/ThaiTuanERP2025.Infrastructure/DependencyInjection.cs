using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Permissions;
using ThaiTuanERP2025.Application.Account.Roles;
using ThaiTuanERP2025.Application.Account.Users;
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
using ThaiTuanERP2025.Application.Expense.Invoices;
using ThaiTuanERP2025.Application.Files;
using ThaiTuanERP2025.Application.Finance.BudgetApprovers;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Application.Finance.BudgetGroups;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Application.Finance.BudgetPlans;
using ThaiTuanERP2025.Application.Finance.CashoutCodes;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Core.Repositories;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Domain.Files.Repositories;
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
using ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations;
using ThaiTuanERP2025.Infrastructure.StoredFiles.FileStorage;
using ThaiTuanERP2025.Infrastructure.StoredFiles.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Application.Core.Followers;

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
			services.AddScoped<IStoredFileReadRepository, StoredFileReadRepository>();

			// Account
			services.AddScoped<IUserWriteRepository, UserWriteRepository>();
			services.AddScoped<IUserReadRepostiory, UserReadRepository>();
			services.AddScoped<IUserManagerAssignmentRepository, UserManagerAssignmentRepository>();
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
			services.AddScoped<IBudgetApproverReadRepository, BudgetApproverReadRepository>();
			services.AddScoped<IBudgetApproverWriteRepository, BudgetApproverWriteRepository>();
			services.AddScoped<ILedgerAccountTypeRepository, LedgerAccountTypeRepository>();
			services.AddScoped<ILedgerAccountRepository, LedgerAccountRepository>();
			services.AddScoped<ILedgerAccountReadRepository, LedgerAccountReadRepository>();
			services.AddScoped<ICashoutCodeWriteRepository, CashoutCodeWriteRepository>();
			services.AddScoped<ICashoutCodeReadRepository, CashoutCodeReadRepository>();
			services.AddScoped<ICashoutGroupRepository, CashoutGroupRepository>();

			// Expense
			services.AddScoped<IInvoiceRepository, InvoiceRepository>();
			services.AddScoped<IInvoiceReadRepository, InvoiceReadRepository>();
			services.AddScoped<IInvoiceFileRepository, InvoiceFileRepository>();
			services.AddScoped<ISupplierRepository, SupplierRepository>();
			services.AddScoped<IOutgoingBankAccountRepository, OutgoingBankAccountRepository>();
			services.AddScoped<IOutgoingPaymentRepository, OutgoingPaymentRepository>();
			services.AddScoped<IExpenseWorkflowTemplateRepository, ExpenseWorkflowTemplateRepository>();
			services.AddScoped<IExpenseStepTemplateRepository, ExpenseStepTemplateRepository>();
			services.AddScoped<IExpenseStepInstanceRepository, ExpenseStepInstanceRepository>();
			services.AddScoped<IExpenseWorkflowInstanceRepository, ExpenseWorkflowInstanceRepository>();
			services.AddScoped<IExpensePaymentRepository, ExpensePaymentRepository>();
			services.AddScoped<IExpensePaymentCommentRepository, ExpensePaymentCommentRepository>();
			services.AddScoped<IExpensePaymentCommentTagRepository, ExpensePaymentCommentTagRepository>();
			services.AddScoped<IExpensePaymentCommentAttachmentRepository, ExpensePaymentCommentAttachmentRepository>();

			// Core
			services.AddScoped<IFollowerReadRepository, FollowerReadRepository>();
			services.AddScoped<IFollowerWriteRepository, FollowerWriteRepository>();
			services.AddScoped<IUserNotificationReadRepository, UserNotificationReadRepository>();
			services.AddScoped<IUserNotificationWriteRepository, UserNotificationWriteRepository>();
			services.AddScoped<IUserReminderReadRepository, UserReminderReadRepository>();
			services.AddScoped<IUserReminderWriteRepository, UserReminderWriteRepository>();	

			// Authentication
			services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

			// ========= File Storage (MinIO) =========
			services.Configure<FileStorageOptions>(cfg.GetSection("Minio"));
			services.AddScoped<IFileStorage, LocalFileStorage>();
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
				opt.TypeDigits[DocumentType.ExpensePayment] = "01";
				opt.TypeDigits[DocumentType.OutgoingPayment] = "02";
				opt.TypeDigits[DocumentType.Invoice] = "03";
			});

			// DomainEventDispatcher
			services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

			// Logging
			services.AddScoped<ILoggingService, SerilogLoggingService>();

			// Jwt
			services.Configure<JwtSettings>(cfg.GetSection("Jwt"));

			// Service
			services.AddScoped<ICurrentRequestIpProvider, CurrentRequestIpProvider>();

			return services;
		}
	}
}
