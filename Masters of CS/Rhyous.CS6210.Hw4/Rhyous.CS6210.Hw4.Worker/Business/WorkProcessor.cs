using log4net;
using Rhyous.StringAlgorithms;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public delegate void WorkCompleteEventHandler(Object sender, WorkCompleteEventArgs e);

    public class WorkCompleteEventArgs : EventArgs
    {
        public UnitOfWork UnitOfWork { get; set; }
    }
    public class WorkProcessor : IWorkProcessor
    {
        private readonly IWorkerLogger _Logger;
        public WorkProcessor(IWorkerLogger logger) { _Logger = logger; }
        public bool IsProcessing { get; set; }

        public event WorkCompleteEventHandler OnComplete;

        public async Task<UnitOfWork> ProcessWorkAsync(IUnitOfWorkFileReader reader, UnitOfWork unitOfWork)
        {
            _Logger?.Debug("Started processing: " + unitOfWork?.Id);
            if (IsProcessing)
                throw new Exception("Already processing a unit of work.");
            IsProcessing = true;
            var text = await reader.ReadAllTextAsync(unitOfWork.AssociateFile);
            var strings = Regex.Split(text, "\\s+", RegexOptions.Multiline);
            var result = Levenshtein.WagnerFischer(strings[0], strings[1]);
            unitOfWork.Result = result[result.GetLength(0) - 1, result.GetLength(1) - 1];
            OnComplete?.Invoke(this, new WorkCompleteEventArgs { UnitOfWork = unitOfWork });
            IsProcessing = false;
            _Logger?.Debug("Finished processing: " + unitOfWork?.Id);
            return unitOfWork;
        }
    }
}