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
            Exchange con = new Exchange();
            con.StartConnect();
            con.recieve();
            Console.ReadKey();
        }
    }



    public class Exchange
    {
        Socket sck;
        Crypt crypt;
        public void StartConnect()
        {
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Connect(IPAddress.Loopback, 31337);
        }
        public void sendSmh(string s)
        {
            sck.Send(Encoding.UTF8.GetBytes(s));
        }
        public void recieve()
        {
            byte[] buff = new byte[16];
            sck.Receive(buff);
            Console.WriteLine("Recieved public pass");
            crypt = new Crypt(DateTime.Now.DayOfYear,buff);
           

            byte[] mess=new byte[16];
            sck.Receive(mess);
            

            Console.WriteLine(Encoding.UTF8.GetString(crypt.DecryptBytes(mess)));
        }

        #region class
        public class Crypt//ok
        {
            int date;
            RijndaelManaged rijndael;
            public Crypt(int dt, byte[] IV)
            {
                date = dt;
                rijndael = new RijndaelManaged();
                rijndael.Key = KeyGen(date);
                rijndael.IV = IV;
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
            #region private
            private byte[] KeyGen(int date)
            {
                return Encoding.Default.GetBytes(GetHashString("liberty1234323456&independence" + date));
            }
            private string GetHashString(string s)
            {
                //переводим строку в байт-массим  
                byte[] bytes = Encoding.Unicode.GetBytes(s);

                //создаем объект для получения средст шифрования  
                MD5CryptoServiceProvider CSP =
                    new MD5CryptoServiceProvider();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = CSP.ComputeHash(bytes);

                string hash = string.Empty;

                //формируем одну цельную строку из массива  
                foreach (byte b in byteHash)
                    hash += string.Format("{0:x2}", b);

                return hash;
            }
            #endregion
        }
        #endregion

    }

}
