using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCooler
{
    internal interface ICooler
    {
        public void On();
        public void Off();
    }

    internal class Cooler : ICooler
    {
        public void Off()
        {
            Console.WriteLine("The cooler is turned on\n");
        }
        public void On()
        {
            Console.WriteLine("The cooler is turned off\n");
        }
    }
}
