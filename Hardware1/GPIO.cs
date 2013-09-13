using System.Runtime.InteropServices;

namespace LibPiGpio
{
    /*  A basic wrapper for "libpigpio" - a shared library for GPIO access on the Raspberry Pi
     *  To get libpigpio, visit my blog entry here:
     *  http://www.codehosting.net/blog/BlogEngine/post/Raspberry-Pi-Shared-Library-for-GPIO.aspx
    */

    public static class RpiGpio
    {
        public static PinSetter Pins { get; set; }

        [DllImport("libpigpio.so")]
        static extern void setup_io();
        [DllImport("libpigpio.so")]
        static extern void switch_gpio(int val, int pin);
        [DllImport("libpigpio.so")]
        static extern int check_gpio(int pin);
        [DllImport("libpigpio.so")]
        static extern void set_in(int gpio);
        [DllImport("libpigpio.so")]
        static extern void set_out(int gpio);

        static RpiGpio()
        {
            Pins = new PinSetter();
            setup_io();
        }

        public static void SetOutputPins(int[] outputs)
        {
            if (outputs == null || outputs.Length == 0) return;
            foreach (int output in outputs)
                set_out(output);
        }

        public static void SetInputPins(int[] inputs)
        {
            if (inputs == null || inputs.Length == 0) return;
            foreach (int input in inputs)
                set_in(input);
        }

        public class PinSetter
        {
            public bool this[int GpioPin]
            {
                set { switch_gpio(value ? 1 : 0, GpioPin); }
                get { return check_gpio(GpioPin) == 1; }
            }
        }
    }
}