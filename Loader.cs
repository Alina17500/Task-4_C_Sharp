using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4._1
{
    internal class Loader : ILoader
    {
        public string LoadOil(int amount)
        {
            return $"Погрузчик загрузил {amount} баррелей нефти.";
        }
    }
}
