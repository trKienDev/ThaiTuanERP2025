﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class UserRepository : IUserRepository {
		private readonly ThaiTuanERP2025DbContext _context;
		public UserRepository(ThaiTuanERP2025DbContext context)
		{
			_context = context;
		}

		public async Task<User?> GetByIdAsync(Guid id) {
			return await _context.Users.Include(u => u.Department)
				.Include(u => u.Department)
				.Include(u => u.UserGroups).ThenInclude(ug => ug.Group)
				.FirstOrDefaultAsync(u => u.Id == id);
		}
		public async Task<User?> GetByUsernameAsync(string username) {
			return await _context.Users.Include(u => u.UserGroups).ThenInclude(ug => ug.Group)
				.FirstOrDefaultAsync(u => u.Username == username);
		}
		public async Task<List<User>> GetAllAsync() {
			return await _context.Users.Include(u => u.Department)
				.Include(u => u.UserGroups).ThenInclude(ug => ug.Group).ToListAsync();
		}

		public async Task AddAsync(User user) {
			await _context.Users.AddAsync(user);
		}

		public async Task UpdateAysnc(User user) {
			_context.Users.Update(user);
			await _context.SaveChangesAsync();
		}
		
		public void Remove(User user) {
			_context.Users.Remove(user);
		}
	}
}
