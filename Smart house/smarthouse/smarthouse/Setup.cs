#define debug
using System;
using System.Collections.Generic;


namespace smarthouse
{
    static partial class Smarthouse
    {
        public static Services output = new Services();
        #region non-static classes
        public static Exchange con;
        public static Standart_Console sc;
        #endregion
        #region pins
        public const byte NetworkOut = 1;
        public const byte test_console = 200;
        public const int light = 11;
        public const int flash = 22;
        public const int rLed = 23;
        public const int gLed = 24;
        public const int bLed = 25;
        public const int netout = 253;
        #endregion
        static public void setup()
        {
            output.Add(NetworkOut, Exchange.Out);
            output.Add(test_console, Standart_Console.WriteLine);

            output.Add(rLed, Hardware.send);
            output.Add(gLed, Hardware.send);
            output.Add(bLed, Hardware.send);

            con = new Exchange();
            con.StartConnect(31337);
            sc = new Standart_Console(true);
            Console.WriteLine("Ended startconsole");

        }
    }
}
