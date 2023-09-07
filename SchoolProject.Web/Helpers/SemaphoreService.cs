namespace SchoolProject.Web.Helpers;

public class SemaphoreService
{
    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);


    public SemaphoreService()
    {
        Semaphore = new SemaphoreSlim(1);
    }

    public SemaphoreSlim Semaphore { get; }


    public async Task<T> Run<T>(Func<Task<T>> func)
    {
        await _semaphoreSlim.WaitAsync();

        try
        {
            return await func();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }


    public async Task Run(Func<Task> func)
    {
        await _semaphoreSlim.WaitAsync();

        try
        {
            await func();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }


    public async Task WaitAsync()
    {
        await _semaphore.WaitAsync();
    }

    public void Release()
    {
        _semaphore.Release();
    }
}