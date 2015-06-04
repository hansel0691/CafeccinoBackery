using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupplyStock;

namespace UsingCost.Utils
{
    public static class ContextUtils
    {
        public static bool CheckDuplicity<T>(IEnumerable<T> list, T supply, out IEnumerable<T> sameOnes)
        {
            sameOnes = null;
            return false;
        }
    }
}
