using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using SupplyStock.Utils;

namespace BackeryApp.ClassUtils
{
    public static class StaticUtils
    {
        public static Control Find(this UserControl userControl, string userControlName)
        {
            var gridChildren = ((Grid)userControl.Parent).Children;
            return (from object child in gridChildren select child as Control).FirstOrDefault(childControl => childControl != null && childControl.Name == userControlName);
        }

        public static void AutoRemove(this UserControl userControl)
        {
            var name = userControl.Name;
            var gridChildren = ((Grid) userControl.Parent).Children;
            for (int i = 0; i < gridChildren.Count; i++)
            {
                var control = gridChildren[i] as Control;
                if (control == null || control.Name != name) continue;
                gridChildren.RemoveAt(i);
                return;
            }
        }

        public static double TotalCost(this IEnumerable<SupplyAmount> supplies)
        {
            var supplyAmounts = supplies as SupplyAmount[] ?? supplies.ToArray();
            return supplyAmounts.Select(sa => sa.Cost).Sum(c => c.AmountCUC);
        }
    }
}
