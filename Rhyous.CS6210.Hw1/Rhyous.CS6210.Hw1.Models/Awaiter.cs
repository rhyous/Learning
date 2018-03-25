using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw1.Models
{
    public class Awaiter
    {
        public static async Task<T> AwaitAsync<T>(Func<T> func, int waitMilliseconds, T defaultT)
        {
            var cts = new CancellationTokenSource(waitMilliseconds);
            await Task.Run(() =>
            {
                T result;
                while (true)
                {
                    if ((result = func()) != null)
                        return result;
                }
            }, cts.Token);
            return defaultT;
        }

        public static async Task<bool> AwaitTrueAsync(Func<bool> func, int waitMilliseconds)
        {
            var cts = new CancellationTokenSource(waitMilliseconds);
            await Task.Run(() =>
            {
                bool result;
                while (true)
                {
                    if (result = func())
                        return result;
                }
            }, cts.Token);
            return false;
        }
    }
}
