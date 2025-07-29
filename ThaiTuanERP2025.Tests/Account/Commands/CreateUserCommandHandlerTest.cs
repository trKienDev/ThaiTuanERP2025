using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.CreateUser;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Tests.Account.Commands
{
	public class CreateUserCommandHandlerTest
	{
		[Fact]
		public async Task Handle_Should_Create_User_And_Return_Dto() {
			// Arrange
			var userRepositoryMock = new Mock<IUserRepository>();
			var handler = new CreateUserCommandHandler(userRepositoryMock.Object);

			var command = new CreateUserCommand
			{
				FullName = "John Doe",
				Username = "johndoe",
				EmployeeCode = "EMP001",
				Password = "password123",
				AvatarUrl = "",
				Role = "Admin",
				Position = "Manager",
				DepartmentId = Guid.NewGuid(),
				Email = "john@example.com",
				Phone = "0123456789"
			};

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(command.FullName, result.FullName);
			Assert.Equal(command.Username, result.Username);	
			userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
		} 
	}
}
