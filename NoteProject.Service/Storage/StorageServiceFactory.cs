using Microsoft.Extensions.Hosting;
using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Local;

namespace NoteProject.Service.Storage;

public class StorageServiceFactory : IStorageServiceFactory
{
    private readonly IHostingEnvironment _hostingEnvironment;

    public StorageServiceFactory(IHostingEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public IStorageProvider GetProvider()
    {
        return LocalProvider;
    }

    private IStorageProvider LocalProvider => new LocalStorageProvider(_hostingEnvironment.ContentRootPath);
}
