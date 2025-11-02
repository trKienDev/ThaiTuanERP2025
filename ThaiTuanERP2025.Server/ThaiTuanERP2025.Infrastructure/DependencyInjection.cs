using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Alerts.Notifications;
using ThaiTuanERP2025.Application.Alerts.TaskReminders;
using ThaiTuanERP2025.Application.Authentication.Repositories;
using ThaiTuanERP2025.Application.Common.Authentication;
using ThaiTuanERP2025.Application.Common.Events;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Options;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Application.Expense.Invoices;
using ThaiTuanERP2025.Application.Files;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Domain.Alerts.Repositories;
using ThaiTuanERP2025.Domain.Common.Enums;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Domain.Files.Repositories;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Domain.Followers.Repositories;
using ThaiTuanERP2025.Infrastructure.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Account.Repositories.Read;
using ThaiTuanERP2025.Infrastructure.Account.Repositories.Write;
using ThaiTuanERP2025.Infrastructure.Alerts.Background;
using ThaiTuanERP2025.Infrastructure.Alerts.Repositories.Read;
using ThaiTuanERP2025.Infrastructure.Alerts.Repositories.Write;
using ThaiTuanERP2025.Infrastructure.Authentication.Repositories;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Common.Security;
using ThaiTuanERP2025.Infrastructure.Common.Services;
using ThaiTuanERP2025.Infrastructure.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Followers.Repositories;
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
			services.AddScoped<IStoredFileReadRepository, StoredFileReadRepository>();

			// Account
			services.AddScoped<IUserWriteRepository, UserWriteRepository>();
			services.AddScoped<IUserReadRepostiory, UserReadRepository>();
			services.AddScoped<IUserManagerAssignmentRepository, UserManagerAssignmentRepository>();
			services.AddScoped<IDepartmentWriteRepository, DepartmentWriteRepository>();
			services.AddScoped<DepartmentReadRepository>();
			services.AddScoped<IGroupRepository, GroupRepository>();
			services.AddScoped<IUserGroupRepository, UserGroupRepository>();
			services.AddScoped<IRoleWriteRepository, RoleWriteRepository>();
			services.AddScoped<IPermissionRepository, PermissionRepository>();
			services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
			services.AddScoped<IUserRoleRepository, UserRoleRepository>();

			// Finance
			services.AddScoped<IBudgetCodeRepository, BudgetCodeRepository>();
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
			services.AddScoped<IInvoiceFileRepository, InvoiceFileRepository>();
			services.AddScoped<ISupplierRepository, SupplierRepository>();
			services.AddScoped<IBankAccountRepository, BankAccountRepository>();
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

			// Notifications & Reminders
			services.AddScoped<INotificationWriteRepository, NotificationWriteRepository>();
			services.AddScoped<INotificationReadRepository, NotificationReadRepository>();
			services.AddScoped<ITaskReminderWriteRepository, TaskReminderWriteRepository>();
			services.AddScoped<ITaskReminderReadRepository, TaskReminderReadRepository>();
			services.AddScoped<IFollowerRepository, FollowerRepository>();

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

			// Host Service
			services.AddHostedService<TaskReminderExpiryHostedService>();
			
			// Task Reminder
			services.Configure<TaskReminderExpiryOptions>(
				cfg.GetSection("TaskReminderExpiry")
			);

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
