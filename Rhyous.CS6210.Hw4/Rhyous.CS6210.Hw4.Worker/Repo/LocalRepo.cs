namespace Rhyous.CS6210.Hw4
{
    public class LocalRepo : IRepo
    {
        public IMasterFileReader Reader => new LocalMasterFileReader();
        public IMasterFileWriter Writer => new LocalMasterFileWriter();
        public IUnitOfWorkFileReader UnitOfWorkReader => new LocalUnitOfWorkFileReader();
        public IUnitOfWorkFileWriter UnitOfWorkWriter => new LocalUnitOfWorkFileWriter();
    }
}
