using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.IAM.Domain.Model.Queries;

namespace Kipu.API.IAM.Application.Services;

public interface IUserQueryService
{
    Task<User?> Handle(GetUserByIdQuery query);
    Task<User?> Handle(GetUserByEmailQuery query);
}
