using System;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Tests.App
{
    class Program
    {
        static void Main(string[] args)
        {           
            DateTime dt1 = "31.03.2019".ToDateTime().StartOfWeek();
            DateTime dt2 = "31.03.2019".ToDateTime().EndOfWeek();
            Console.WriteLine(dt1.ToString());
        }
    }
}
