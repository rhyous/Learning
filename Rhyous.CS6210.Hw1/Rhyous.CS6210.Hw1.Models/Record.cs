using Rhyous.CS6210.Hw1.Interfaces;
using System;

namespace Rhyous.CS6210.Hw1.Models
{
    public class Record : IRecord
    {
        public static int _ObjId = 0;

        public int Id
        {
            get { return _Id > 0 ? _Id : (_Id = ++_ObjId); }
            set { _Id = value; }
        } private int _Id;

        public int Disease { get; set; }

        public DateTime DiagnosisDate { get; set; }
    }
}