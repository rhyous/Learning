using Rhyous.CS6210.Hw4.Models;

namespace Rhyous.CS6210.Hw4
{
    public class WorkCompletionReporter : RequestClient
    {
        protected override TResponse OnSuccess<TResponse>(TResponse response)
        {
            return base.OnSuccess(response);
        }

        protected override TResponse OnFailure<TResponse>(object o)
        {
            return base.OnFailure<TResponse>(o);
        }

        protected override TResponse OnTimeout<TResponse>(object o)
        {
            return base.OnTimeout<TResponse>(o);
        }
    }
}