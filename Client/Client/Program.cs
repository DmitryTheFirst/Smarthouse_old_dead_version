using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter sw = new System.IO.StreamWriter("log.txt");
            try
            {
                //Thread.Sleep(5000);
                Console.WriteLine("Client");
                Exchange ext = new Exchange();
                ext.Connect("192.168.0.7", 31337);
                //ext.Connect("192.168.0.141", 31337);

                //Console.ReadKey();
                //Console.WriteLine("Sent! Easy");
                //ext.sck.Send(new byte[2] { 255, 127 });
                //Console.ReadKey();
                //Console.WriteLine("Sent! Easy");
                //ext.sck.Send(new byte[2] { 255, 0 });
                //Console.ReadKey();

                //for (
                int i = 1; 
                //i < 2147483647; i++)
                //{
                   
                    //Console.WriteLine("___________________");
                    //Console.WriteLine("Sending Hard!");
                    byte[] value = Encoding.UTF8.GetBytes("So this shit works fine, isn't it?");
                    byte[] size = new byte[4];
                    size = BitConverter.GetBytes((UInt32)value.Length);
                    ext.sck.Send(new byte[2] { 23, (byte)(size[3] + 128) });
                    Console.WriteLine("Sent 2");
                    ext.sck.Send(new byte[3] { size[0], size[1], size[2] });
                    Console.WriteLine("Sent 3");
                    Console.WriteLine("Sending BIIG");
           
                    ext.sck.Send(value);
                    //Console.WriteLine("Sent BIIG");
                    Console.WriteLine("Sent!");
                    //-------------------------recieve------------------------------------------------
                    byte[] first = new byte[2];
                    ext.sck.Receive(first);
                    byte pin = first[0];
                    bool type = !(first[1] < 128);

                    Console.WriteLine("Recieved");

                    if (type)
                    {
                        //неудача. Нас ждет анальная боль. Много байт. К хуям бесперебойность!
                        byte[] temp = new byte[3];
                        ext.sck.Receive(temp, 0, 3, SocketFlags.None);//качаем ещё 3 бита размера
                        byte[] s = new byte[4] { temp[0], temp[1], temp[2], (byte)(first[1] - 128) };//здесь у нас будет храниться размер
                        // Console.WriteLine("Recieved size bytes");
                        UInt32 size1 = BitConverter.ToUInt32(s, 0);
                        //Console.WriteLine("Size is:" + size1);
                        value = new byte[size1];
                        ext.sck.Receive(value, SocketFlags.None);//принимаем невъебенно большой файл и забиваем его в value 
                    }
                    else
                    {
                        value = new byte[] { first[1] }; //збc, всего 1 байт.
                    }

                    Console.WriteLine(i + "      " + BitConverter.ToInt32(value, 0));
                    sw.WriteLine(i + "      " + BitConverter.ToInt32(value, 0));
                //}
            }
            catch (Exception ex)
            {
                sw.WriteLine(ex.Message);
                sw.Close();
            }
            Thread.Sleep(90000);
        }
    }
    class Exchange
    {
        public Socket sck;
        public void Connect(string ip, int port)
        {
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Connect(IPAddress.Parse(ip), port);
            Console.WriteLine("Tcp connected!");
            #region Recieve append
            Int32 append;
            byte[] rec = new byte[4];
            sck.Receive(rec);
            append = BitConverter.ToInt32(rec, 0);
            Console.WriteLine("append " + append);
            #endregion
            #region gener md5+append
            string md5 = getKeyToSend(append);
            #endregion
            #region send md5+append
            rec = new byte[32];
            rec = Encoding.UTF8.GetBytes(md5);
            sck.Send(rec);
            Console.WriteLine("md5 " + md5);
            #endregion
            #region Generate ID
            Random rnd = new Random();
            Int32 ID = rnd.Next(Int32.MinValue, Int32.MaxValue);
            #endregion
            #region Send code
            sck.Send(BitConverter.GetBytes(ID));
            #endregion
        }
        #region MD5
        string getKey()
        {
            return GetMD5(Encoding.UTF8.GetBytes("liberty1234323456&independence" + DateTime.Now.DayOfYear.ToString() + DateTime.Now.Year.ToString()));
        }
        string getKeyToSend(int append)
        {
            return GetMD5(Encoding.UTF8.GetBytes(getKey() + append));
        }
        public string GetMD5(byte[] bytes)
        {
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);
            return hash;
        }
        #endregion
        #region sending/recieving methods

        bool recieve(out byte pin, out byte[] res)
        {
            byte[] fbuff = new byte[4];
            try
            {
                sck.Receive(fbuff);
                pin = fbuff[0];
                if (fbuff[1] >= 128)
                {
                    res = new byte[] { fbuff[1] };
                }
                else
                {
                    fbuff[1] &= 0x7F; //заменяем первый бит с 1 на 0, чтобы считать как обычное число
                    res = new byte[BitConverter.ToUInt32(fbuff, 1)];
                    sck.Receive(res);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Recieving error: " + ex.Message);
                res = null;
                pin = 0;
                return false;
            }
        }

        public void send(byte pin, byte[] buff)
        {
            byte[] res;
            try
            {
                if (buff.Length <= 1)
                {
                    res = new byte[] { pin, buff[0] };
                }
                else
                {
                    res = new byte[4] { pin, 0, 0, 0 };
                    BitConverter.GetBytes((UInt32)buff.Length | 0x800).CopyTo(res, 1); //в res теперь лежит первое отправляемое число
                    sck.Send(res);
                    sck.Send(buff);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sendinng error: " + ex.Message);
            }
        }

        #endregion
        #region class
        public class Crypt//ok
        {
            int date;
            public RijndaelManaged rijndael;
            public Crypt(int dt)
            {
                date = dt;
                rijndael = new RijndaelManaged();
                rijndael.Key = KeyGen(date);
                rijndael.GenerateIV();
            }
            public byte[] EncryptBytes(byte[] message)
            {
                if ((message == null) || (message.Length == 0))
                {
                    return message;
                }

                if (rijndael == null)
                {
                    throw new ArgumentNullException("alg");
                }

                using (var stream = new MemoryStream())
                using (var encryptor = rijndael.CreateEncryptor())
                using (var encrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                {
                    encrypt.Write(message, 0, message.Length);
                    encrypt.FlushFinalBlock();
                    return stream.ToArray();
                }
            }
            public byte[] DecryptBytes(byte[] message)
            {
                if ((message == null) || (message.Length == 0))
                {
                    return message;
                }

                if (rijndael == null)
                {
                    throw new ArgumentNullException("alg");
                }

                using (var stream = new MemoryStream())
                using (var decryptor = rijndael.CreateDecryptor())
                using (var encrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
                {
                    encrypt.Write(message, 0, message.Length);
                    encrypt.FlushFinalBlock();
                    return stream.ToArray();
                }
            }
            public byte[] GetKeyToSend()
            {
                byte[] res = rijndael.IV;
                return res;
            }
            public string GetMD5(byte[] bytes)
            {
                MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
                byte[] byteHash = CSP.ComputeHash(bytes);
                string hash = string.Empty;
                foreach (byte b in byteHash)
                    hash += string.Format("{0:x2}", b);
                return hash;
            }
            #region private
            private byte[] KeyGen(int date)
            {
                return Encoding.Default.GetBytes(GetMD5(Encoding.UTF8.GetBytes("liberty1234323456&independence" + date)));
            }
            #endregion
        }
        #endregion
    }
}
