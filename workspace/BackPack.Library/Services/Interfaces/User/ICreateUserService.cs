﻿
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.User;

namespace BackPack.Library.Services.Interfaces.User
{
    public interface ICreateUserService
    {
        Task<BaseResponse> CreateSuperUserAsync(SuperUserRequest request);
    }
}
