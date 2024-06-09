namespace Tuudio.Infrastructure.Services.Interfaces;

public interface IUnitOfWork
{
    IClientRepository ClientRepository { get; }

    Task CompleteAsync();

    Task<T> ExecuteAsync<T>(Func<Task<T>> action);
}