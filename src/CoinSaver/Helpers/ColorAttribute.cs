using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ColorAttribute : Attribute
    {
        public readonly string HexColor;
        public ColorAttribute(string hexColor)
        {
            HexColor = hexColor;
        }
    }
}
