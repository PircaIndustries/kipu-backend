using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.IAM.Domain.Model.Queries;
using Kipu.API.IAM.Domain.Repositories;

namespace Kipu.API.IAM.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        return await userRepository.FindByIdAsync(query.Id);
    }

    public async Task<User?> Handle(GetUserByEmailQuery query)
    {
        return await userRepository.FindByEmailAsync(query.Email);
    }
}
