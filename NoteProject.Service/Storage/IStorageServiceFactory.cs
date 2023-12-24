using TwentyTwenty.Storage;

namespace NoteProject.Service.Storage;

public interface IStorageServiceFactory
{
    IStorageProvider GetProvider();
}
