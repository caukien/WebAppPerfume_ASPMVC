using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebAppPerfume.Helper
{
    public class Currency
    {
        public static decimal ConvertVNĐtoUSD(decimal amountInVNĐ)
        {
            // Tỉ giá VNĐ sang USD
            decimal exchangeRate = 1 / 25000m;
            // Chuyển đổi số tiền
            return amountInVNĐ * exchangeRate;
        }
    }
}