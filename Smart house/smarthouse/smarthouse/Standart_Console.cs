using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace smarthouse
{
    class Standart_Console
    {
        Thread t;
        public Standart_Console(bool val)
        {
            Reading(val);
        }
        public static void WriteLine(object cmd)
        {
            byte pin = ((pinvalue)cmd).pin;
            string value;
            if (((pinvalue)cmd).value.GetType().ToString() == "System.Byte[]")
             value= Encoding.UTF8.GetString((byte[])(((pinvalue)cmd).value));
            else
            value = ((pinvalue)cmd).value.ToString();
            Console.WriteLine("Test:  {0}  {1}", pin, value);
        }
        public void Reading(bool val)
        {



            if (val && (t == null || t.ThreadState == ThreadState.Unstarted || t.ThreadState == ThreadState.Stopped))
            {
                t = new Thread(delegate()
                {
                    do
                    {
                        Read(Console.ReadLine());
                    } while (true);
                });
                t.Start();

            }
            else
            {
                Console.WriteLine(t.ThreadState);
                t.Abort();
            }
        }
        void Read(string s)
        {
            Smarthouse.output.SetValue(Smarthouse.test_console, s);
        }
    }
}
