using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Application.Responses;
using Application.Common.Interfaces.Infrastructure.UnitOfWork;
using Application.Common.Responses;
using Ardalis.GuardClauses;
using Domain.Models;
using MediatR;

namespace Application.Profiles.Commands;

public record NewProfileCommand : IRequest<IBaseResponse<ProfileModel>>
{
    public Guid UserId { get; init; }
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public string Country { get; init; }
}

public class NewProfileCommandHandler : IRequestHandler<NewProfileCommand, IBaseResponse<ProfileModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public NewProfileCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
    }

    public async Task<IBaseResponse<ProfileModel>> Handle(NewProfileCommand request, CancellationToken ct)
    {
        return new BaseResponse<ProfileModel>
        {
            StatusCode = 201,
        };
    }
}
