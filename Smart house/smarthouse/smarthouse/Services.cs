using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smarthouse
{
    class Services
    {
        static Dictionary<byte, Action<object>> services = new Dictionary<byte, Action<object>>();

        public void Add(byte pin, Action<object> Del)
        {
            services.Add(pin, Del);
        }

        public void SetValue(byte pin, object cmd)
        {
            //check incoming pin/ It can be not on the list
            object obj = new pinvalue(pin, cmd);
            new System.Threading.Thread(() => services[pin](obj)).Start();
        }
    }

    class pinvalue
    {
        public byte pin;
        public object value;

        public pinvalue(byte pin, object value)
        {
            this.pin = pin;
            this.value = value;
        }
    }

}
