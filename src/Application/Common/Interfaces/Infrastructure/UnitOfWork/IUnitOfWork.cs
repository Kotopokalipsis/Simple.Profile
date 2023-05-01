using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Infrastructure.Repositories.Interfaces;

namespace Application.Common.Interfaces.Infrastructure.UnitOfWork;

public interface IUnitOfWork
{
    IProfileRepository ProfileRepository { get; }
    Task Commit(CancellationToken ct = default);
}