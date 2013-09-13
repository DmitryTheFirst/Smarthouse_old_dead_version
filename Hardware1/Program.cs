using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibPiGpio;
using System.Net.Sockets; 

namespace Hardware1
{
    class Program
    {
        static int[] input = new int[] { 4, 10 };
        static int[] output = new int[] { 11, 22, 23, 24, 25 };
        static void Main(string[] args)
        {
            Console.WriteLine("Sup! Hardware1 is talking to you, motherfucker!");
            Console.WriteLine("Version:  0.0");
        }

        static void Setup()
        {
            RpiGpio.SetInputPins(input);
            RpiGpio.SetOutputPins(output);
        }



        static void setValue()
        {
            //RpiGpio.Pins[light] = value;  
        }
    }
}
