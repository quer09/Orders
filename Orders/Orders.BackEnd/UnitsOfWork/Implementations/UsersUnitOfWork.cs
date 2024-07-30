using Microsoft.AspNetCore.Identity;
using Orders.BackEnd.Repositories.Interfaces;
using Orders.BackEnd.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;

namespace Orders.BackEnd.UnitsOfWork.Implementations
{
    public class UsersUnitOfWork : IUsersUnitOfWork
    {
        private readonly IUsersRepository _userRepository;

        public UsersUnitOfWork(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password) => await _userRepository.AddUserAsync(user, password);

        public async Task AddUserToRoleAsync(User user, string roleName) => await _userRepository.AddUserToRoleAsync(user, roleName);

        public async Task CheckRoleAsync(string roleName) => await _userRepository.CheckRoleAsync(roleName);

        public async Task<User> GetUserAsync(string email) => await _userRepository.GetUserAsync(email);

        public async Task<bool> IsUserInRoleAsync(User user, string roleName) => await _userRepository.IsUserInRoleAsync(user, roleName);
    }
}