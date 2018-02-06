using System;
using System.Collections.Generic;

namespace Rhyous.CS6210.Hw1.Models
{
    public class Disease : Dictionary<int, string>
    {
        #region Singleton

        private static readonly Lazy<Disease> Lazy = new Lazy<Disease>(() => new Disease());

        public static Disease Instance { get { return Lazy.Value; } }
        
        internal Disease()
        {
            Add(1, "Influenza");
            Add(2, "ChickenPox");
            Add(3, "Measles");
        }
        #endregion
    }
}