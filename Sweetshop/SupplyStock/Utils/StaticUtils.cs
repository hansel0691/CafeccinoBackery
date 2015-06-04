using System;

namespace SupplyStock.Utils
{
    public static class StaticUtils
    {
        public static string SmartString(this double number)
        {
            return (Math.Abs(number) >= 1 || Math.Abs(number - 0) < 0.000000001) ? number.ToString("0.00") : number.ToString("0.00000").TrimEnd('0');
        }
    }
}
