using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.MoveAccountParent
{
	public class MoveLedgerAccountParentHandler : IRequestHandler<MoveLedgerAccountParentCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public MoveLedgerAccountParentHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(MoveLedgerAccountParentCommand command, CancellationToken cancellationToken)
		{
			var node = await _unitOfWork.LedgerAccounts.GetByIdAsync(command.LedgerAccountId)
				?? throw new NotFoundException("Không tìm thấy tài khoản kế toán nhóm");
			var oldPrefix = node.Path;
			var oldBaseLevel = node.Level;

			string newParentPath = "/";
			int newParentLevel = -1;

			if (command.NewParentLedgerAccountId.HasValue)
			{
				if (command.NewParentLedgerAccountId.Value == node.Id) 
					throw new ConflictException("Không thể di chuyển tài khoản kế toán vào chính nó");
				var parent = await _unitOfWork.LedgerAccounts.GetByIdAsync(command.NewParentLedgerAccountId.Value)
					?? throw new NotFoundException("Không tìm thấy tài khoản kế toán nhóm cha mới");
				if (parent.Path.StartsWith(oldPrefix))
					throw new ConflictException("Không thể di chuyển tài khoản kế toán vào một nhóm con của nó");
				newParentPath = parent.Path;
				newParentLevel = parent.Level;
			}

			node.ParentLedgerAccountId = command.NewParentLedgerAccountId;
			node.Path = $"{newParentPath}{node.Number}";
			node.Level = newParentLevel + 1;

			// Cập nhật toàn bộ subtree
			var subtree = await _unitOfWork.LedgerAccounts.GetSubtreeAsync(oldPrefix, asNoTracking: false, cancellationToken);
			foreach (var child in subtree.Where(x => x.Id != node.Id))
			{
				var suffix = child.Path.Substring(oldPrefix.Length);
				child.Path = node.Path + suffix;
				child.Level = node.Level + (child.Level - oldBaseLevel);
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
