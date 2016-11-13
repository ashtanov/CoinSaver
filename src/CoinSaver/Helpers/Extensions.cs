using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using System.Reflection;

namespace CoinSaver
{
    public static class HtmlExtensions
    {
        public static HtmlString EnumDisplayNameFor(this Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayName != null)
            {
                return new HtmlString(displayName.Name);
            }

            return new HtmlString(item.ToString());
        }

        public static HtmlString EnumColorFor(this Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            ColorAttribute color = (ColorAttribute)member[0].GetCustomAttributes(typeof(ColorAttribute), false).FirstOrDefault();

            if (color != null)
            {
                return new HtmlString(color.HexColor);
            }

            return new HtmlString(item.ToString());
        }

        public static IQueryable<Models.Purchase> WhereDateBetween(this IQueryable<Models.Purchase> @this, DateTime start, DateTime end)
        {
            DateTime startFormat = start.Date;
            DateTime endFormat = end.AddDays(1).AddSeconds(-1);
            return @this.Where(x => x.Date >= startFormat && x.Date <= endFormat);
        }

        public static bool IsInt(this string @this)
        {
            int y;
            return !string.IsNullOrEmpty(@this) && int.TryParse(@this, out y);
        }
    }
}
