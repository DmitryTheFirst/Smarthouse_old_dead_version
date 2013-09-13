using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace smarthouse
{
    static class Hardware
    {
        class Device
        {
            public string ip;
            public byte[] pins;
            public Socket sck;
            public Device(string ip, byte[] pins)
            {
                this.ip = ip;
                this.pins = pins;
                sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //sck.Connect(ip, 1337);   делается в startconnect
            }
        }

        static Device[] devices = new Device[] { new Device("192.168.0.141", new byte[] {23, 24, 25 }) };
        //static List<Task> tasks = new List<Task>();
        
        static void setup()
        {
            startconnect();
            startlistening();
        }

        static void startconnect()
        {
            foreach (Device device in devices)
            {
                device.sck.BeginConnect(device.ip, 1337, endconnect, device);
            }
        }
        static void endconnect(System.IAsyncResult ar)
        {
            ar.AsyncState.GetType();
        }
        
        static void startlistening()
        {
           //перебрать массив устройств, начать асинхронный прием
            foreach (Device device in devices)
            {
                //device.sck.BeginReceive(device.ip, 1337, endconnect, device);
            }

        }
        static void endrecieve(System.IAsyncResult ar)
        {

        }
        
        public static void send(object cmd)
        {
            byte pin = ((pinvalue)cmd).pin;
            byte[] value = (byte[])(((pinvalue)cmd).value);

            Standart_Console.WriteLine(cmd);
        }
        #region methods
        /*
            static void Light(byte[] cmd)
            {
                if (cmd[0] < 128)
                {
                    bool value = cmd[0] > 0;
                    RpiGpio.Pins[light] = value;  //где-то !value т к я мудак и неправильно припаял релешку главного света =(
                    //Log("Light " + (value ? "on" : "off"));
                }
            }
            static void Flash(byte[] cmd)
            {
                if (cmd[0] < 128)
                {
                    bool value = cmd[0] > 0;
                    RpiGpio.Pins[flash] = !value;
                    //Log("Flash " + (value ? "on" : "off"));
                }
            }
            static void RLed(byte[] cmd)
            {
                if (cmd[0] < 128)
                {
                    bool value = cmd[0] > 0;
                    RpiGpio.Pins[rLed] = !value;
                    //Log("Red led " + (value ? "on" : "off"));
                }
            }
            static void GLed(byte[] cmd)
            {
                if (cmd[0] < 128)
                {
                    bool value = cmd[0] > 0;
                    RpiGpio.Pins[gLed] = !value;
                    //Log("Green led " + (value ? "on" : "off"));
                }
            }
            static void BLed(byte[] cmd)
            {
                if (cmd[0] < 128)
                {
                    bool value = cmd[0] > 0;
                    RpiGpio.Pins[bLed] = !value;
                    //Log("Blue led " + (value ? "on" : "off"));
                }
            }
             */




        /*
         * static private void Button()
        {
            if (RpiGpio.Pins[button] == true)
                Console.WriteLine("Button   " + DateTime.Now.ToString());
        }
        static private void IR1()
        {
            if (RpiGpio.Pins[ir1] == true)
            {
                Console.WriteLine("R1   " + DateTime.Now.ToString());
                NetworkOut(new byte[] { ir1, 1 });
            }
        }
         */
        #endregion
    }
}