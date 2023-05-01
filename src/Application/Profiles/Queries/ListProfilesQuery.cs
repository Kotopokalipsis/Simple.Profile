using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Application.Responses;
using Application.Common.Interfaces.Infrastructure.UnitOfWork;
using Application.Common.Responses;
using Ardalis.GuardClauses;
using Domain.Models;
using MediatR;

namespace Application.Profiles.Queries;

public record ListProfilesQuery : IRequest<IBaseResponse<ProfileModel>>;

public class ListProfilesQueryHandler : IRequestHandler<ListProfilesQuery, IBaseResponse<ProfileModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ListProfilesQueryHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
    }
    
    public async Task<IBaseResponse<ProfileModel>> Handle(ListProfilesQuery request, CancellationToken cancellationToken)
    {
        return new BaseResponse<ProfileModel>
        {
            StatusCode = 200,
            Data = new ProfileModel {}
        };
    }
}