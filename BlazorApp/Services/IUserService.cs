﻿using ApiContracts;
namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(UserCreateDto request);
    Task UpdateUserAsync(int id, UserUpdateDto request);
    Task<UserDto> GetSingleAsync(int id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task DeleteUserAsync(int id);
}