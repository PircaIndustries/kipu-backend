using Kipu.API.NCR.Domain.Model.Aggregates;
using Kipu.API.Ncr.Domain.Model.ValueObjects;
using Kipu.API.NCR.Domain.Model.ValueObjects;
using Kipu.API.NCR.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Ncr.Application.Internal.CommandServices;

public class NcrCommandService
{
    private readonly INcrRepository _ncrRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NcrCommandService(INcrRepository ncrRepository, IUnitOfWork unitOfWork)
    {
        _ncrRepository = ncrRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<NCR.Domain.Model.Aggregates.Ncr, string>> HandleCreateAsync(string title, string description, string taskName, int severity, int projectId)
    {
        try
        {
            if (!Enum.IsDefined(typeof(NcrSeverity), severity))
                return new Result<NCR.Domain.Model.Aggregates.Ncr, string>.Failure("Invalid severity value.");

            var ncrSeverity = (NcrSeverity)severity;
            var ncr = new NCR.Domain.Model.Aggregates.Ncr(title, description, taskName, ncrSeverity, projectId);

            await _ncrRepository.AddAsync(ncr);
            await _unitOfWork.CompleteAsync();

            return new Result<NCR.Domain.Model.Aggregates.Ncr, string>.Success(ncr);
        }
        catch (Exception ex)
        {
            return new Result<NCR.Domain.Model.Aggregates.Ncr, string>.Failure(ex.Message);
        }
    }
}