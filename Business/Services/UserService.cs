using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services
{
    public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager)
        : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly UserManager<UserEntity> _userManager = userManager;

        public async Task<UserResult> GetUsersAsync()
        {
            var result = await _userRepository.GetAllAsync();
            return result.MapTo<UserResult>();
        }

        public async Task<UserResult> CreateUserAsync(SignUpFormData formData)
        {
            if (formData == null)
            {
                return new UserResult
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = "Form data is null",
                };
            }

            var exists = await _userRepository.ExistsAsync(u => u.Email == formData.Email);
            if (exists.Result)
            {
                return new UserResult
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = "A user with this email already exists.",
                };
            }

            var userEntity = formData.MapTo<UserEntity>();
            userEntity.UserName = formData.Email;
            userEntity.Email = formData.Email;

            var createResult = await _userManager.CreateAsync(userEntity, formData.Password);
            if (!createResult.Succeeded)
            {
                var emailError = createResult.Errors.FirstOrDefault(e =>
                    e.Code == "DuplicateEmail"
                );

                var message =
                    emailError != null
                        ? emailError.Description
                        : "An error occurred while creating the user.";

                return new UserResult
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = message,
                };
            }

            return new UserResult { Succeeded = true, StatusCode = 201 };
        }
    }
}
