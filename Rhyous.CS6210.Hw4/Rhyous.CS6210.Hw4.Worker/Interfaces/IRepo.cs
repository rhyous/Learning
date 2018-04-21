namespace Rhyous.CS6210.Hw4
{
    public interface IRepo
    {
        IMasterFileReader Reader { get; }
        IUnitOfWorkFileReader UnitOfWorkReader { get; }
        IUnitOfWorkFileWriter UnitOfWorkWriter { get; }
        IMasterFileWriter Writer { get; }
    }
}