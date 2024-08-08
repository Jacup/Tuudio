namespace Tuudio.Infrastructure.Services.Interfaces;

public interface IUnitOfWork
{
    IClientRepository ClientRepository { get; }

    IActivityRepository ActivityRepository { get; }

    IPassTemplateRepository PassTemplateRepository { get; }

    IPassRepository PassRepository { get; }

    Task<T> ExecuteAsync<T>(Func<Task<T>> action);
}