using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataNormalizer.Core.Service
{
    [Serializable]
    public class BusySymbol
    {
        public int ID { get; set; }
        public bool IsDataNet { get; set; }
        public bool IsTickNet { get; set; }
        public string UserName { get; set; }
      
    }
}
