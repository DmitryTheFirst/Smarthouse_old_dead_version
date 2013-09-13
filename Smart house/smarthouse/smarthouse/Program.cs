using System;
using System.Threading;

namespace smarthouse
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server---15---  Raspberry");
            Smarthouse.setup();
            Thread.Sleep(10000000);//debug
        }
    }
}


/*              sck.Receive(fbuff);
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
 */

/* sender
static public void Sender(byte[] data, byte pin)
{
    var v = con.sessions.Where(a => a.status == true && a.listento.Contains(pin) == true);

    foreach (Session s in v)
    {
        s.send(pin, data);
    }
}
*/

/*handless lamer, do this code. NOW!
 *  #region handless lamer, do this code. NOW!
        static private void saveState()
        {
            Stream sr = File.OpenWrite("startup.cfg");
            BufferedStream bs = new BufferedStream(sr);
            BinaryWriter bw = new BinaryWriter(bs);
            #region set
            //bw.Write(lightValue);
            bw.Write(RpiGpio.Pins[flash]);
            bw.Write(RpiGpio.Pins[rLed]);
            bw.Write(RpiGpio.Pins[gLed]);
            bw.Write(RpiGpio.Pins[bLed]);
            #endregion
            #region closeAll
            bw.Flush();
            bs.Flush();
            sr.Flush();
            bw.Close();
            bs.Close();
            sr.Close();
            #endregion
        }
        static private void loadState()
        {
            Stream sr = File.OpenWrite("startup.cfg");
            BufferedStream bs = new BufferedStream(sr);
            BinaryWriter bw = new BinaryWriter(bs);
            #region set
            //bw.Write(lightValue);
            bw.Write(RpiGpio.Pins[flash]);
            bw.Write(RpiGpio.Pins[rLed]);
            bw.Write(RpiGpio.Pins[gLed]);
            bw.Write(RpiGpio.Pins[bLed]);
            #endregion
            #region closeAll
            bw.Flush();
            bs.Flush();
            sr.Flush();
            bw.Close();
            bs.Close();
            sr.Close();
            #endregion
        }
        #endregion

*/


#region Fuck! 371 lines of useless code :(


//#region sending/recieving methods
//       bool recieveByte(out byte[] buff)
//       {
//           buff = new byte[1];
//           try
//           {
//               sck.Receive(buff);
//               Console.WriteLine(buff[0]);
//               return true;
//           }
//           catch (Exception ex)
//           {
//               Console.WriteLine("Recieving error: " + ex.Message);
//               return false;
//           }
//       }

//       public void sendByte(byte pin)
//       {
//           try
//           {
//               sck.Send(new byte[1] { pin });
//           }
//           catch (Exception ex)
//           {
//               Console.WriteLine("Sendinng error: " + ex.Message);
//           }
//       }

//       bool recieveData(out byte[] buff)
//       {
//           byte[] nbuff = new byte[4];

//           try
//           {
//               sck.Receive(nbuff);
//               buff = new byte[BitConverter.ToInt32(nbuff, 0)];
//               sck.Receive(buff);
//               return true;
//           }
//           catch (SocketException ex)
//           {
//               Console.WriteLine("Socket error: " + ex.Message);
//               this.sck = null;
//               this.status = false;
//               buff = null;
//               return false;
//           }
//       }

//       void sendData(byte[] buff)
//       {
//           byte[] nbuff = BitConverter.GetBytes(buff.Length);
//           try
//           {
//               sck.Send(nbuff);
//               sck.Send(buff);
//           }
//           catch (Exception ex)
//           {
//               Console.WriteLine("Recieving error: " + ex.Message);
//           }
//       }
//       #endregionc


//public class Crypt//ok
//{
//    int date;
//    public RijndaelManaged rijndael;
//    public Crypt(int dt)
//    {
//        date = dt;
//        rijndael = new RijndaelManaged();
//        rijndael.Key = KeyGen(date);
//    }
//    public void SetIV(byte[] IV)
//    {
//        rijndael.IV = IV;
//    }
//    public byte[] EncryptBytes(byte[] message)
//    {
//        if ((message == null) || (message.Length == 0))
//        {
//            return message;
//        }

//        if (rijndael == null)
//        {
//            throw new ArgumentNullException("alg");
//        }

//        using (var stream = new MemoryStream())
//        using (var encryptor = rijndael.CreateEncryptor())
//        using (var encrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
//        {
//            encrypt.Write(message, 0, message.Length);
//            encrypt.FlushFinalBlock();
//            return stream.ToArray();
//        }
//    }
//    public byte[] DecryptBytes(byte[] message)
//    {
//        if ((message == null) || (message.Length == 0))
//        {
//            return message;
//        }

//        if (rijndael == null)
//        {
//            throw new ArgumentNullException("alg");
//        }

//        using (var stream = new MemoryStream())
//        using (var decryptor = rijndael.CreateDecryptor())
//        using (var encrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
//        {
//            encrypt.Write(message, 0, message.Length);
//            encrypt.FlushFinalBlock();
//            return stream.ToArray();
//        }
//    }
//    public byte[] GetKeyToSend()
//    {
//        byte[] res = rijndael.IV;
//        return res;
//    }
//    public string GetMD5(byte[] bytes)
//    {
//        //создаем объект для получения средст шифрования  
//        MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

//        //вычисляем хеш-представление в байтах  
//        byte[] byteHash = CSP.ComputeHash(bytes);

//        string hash = string.Empty;

//        //формируем одну цельную строку из массива  
//        foreach (byte b in byteHash)
//            hash += string.Format("{0:x2}", b);

//        return hash;
//    }
//    #region private
//    private byte[] KeyGen(int date)
//    {
//        return Encoding.Default.GetBytes(GetMD5(Encoding.UTF8.GetBytes("liberty1234323456&independence" + date)));
//    }
//    #endregion
//}



//static class Exchange
//{
//    static List<Socket> list = new List<Socket>();
//    static Socket sck;
//    static public void Connect()
//    {
//        #region Set socket
//        sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        sck.Blocking = false;
//        sck.NoDelay = true;
//        sck.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.141"), 31337));
//        sck.Listen(128);
//        #endregion
//        #region Set binding to event
//        SocketAsyncEventArgs sa = new SocketAsyncEventArgs();
//        sa.Completed += sa_Completed;
//        sck.AcceptAsync(sa);
//        #endregion
//    }
//    private static void sa_Completed(object sender, SocketAsyncEventArgs e)
//    {
//        list.Add((Socket)sender);
//    }
//    #region shit
//    //static void socketWork(Socket sck, int id)
//    //{
//    //    #region RSA(IV) SEND
//    //    var rijndael1 = new RijndaelManaged();
//    //    rijndael1.Key = Crypt.KeyGen();
//    //    rijndael1.GenerateIV();
//    //    byte[] iv = rijndael1.IV;
//    //    WriteLine(ref sck, iv);
//    //    #endregion
//    //    string s;
//    //    string test = SafeReadLine(ref sck, iv);
//    //    if (test == "Connected")
//    //    {
//    //        Console.WriteLine(id + ": " + test);
//    //        do
//    //        {
//    //            s = SafeReadLine(ref sck, iv).Trim().ToLower();
//    //            Console.WriteLine(id + ": " + s);
//    //            switch (s.Substring(0, s.IndexOf(' ')))
//    //            {
//    //                case "switch":
//    //                    bool value = (s.Substring(s.LastIndexOf(' ') + 1) == "on" ? true : false);
//    //                    switch (s.Substring(7, s.LastIndexOf(' ') - 7))
//    //                    {
//    //                        case "light":
//    //                            SmartHouse.Light(value);
//    //                            break;
//    //                        case "flash":
//    //                            SmartHouse.Flash(value);
//    //                            break;
//    //                        case "rled":
//    //                            SmartHouse.RLed(value);
//    //                            break;
//    //                        case "gled":
//    //                            SmartHouse.GLed(value);
//    //                            break;
//    //                        case "bled":
//    //                            SmartHouse.BLed(value);
//    //                            break;
//    //                        case "all":
//    //                            SmartHouse.All(value);
//    //                            break;
//    //                        default:
//    //                            Console.WriteLine("Wrong command!");
//    //                            break;
//    //                    }
//    //                    break;

//    //                case "set":

//    //                    break;
//    //                default:
//    //                    Console.WriteLine("Wrong command!");
//    //                    break;



//    //                    Console.WriteLine("Wrong command");



//    //            }
//    //            //work
//    //        } while (sck.Connected);
//    //        Console.WriteLine(id + ": " + "Disconnected");
//    //    }
//    //    else
//    //    {
//    //        Console.WriteLine(id + ": " + test);
//    //        Console.WriteLine(id + ": " + "Wrong auth!");
//    //    }
//    //}
//    //static public void WriteLine(ref Socket sck, string str)
//    //{
//    //    byte[] nbuff2;
//    //    byte[] sbuff2;

//    //    sbuff2 = Encoding.Default.GetBytes(str);
//    //    nbuff2 = BitConverter.GetBytes(sbuff2.Length);
//    //    try
//    //    {
//    //        sck.Send(nbuff2);
//    //        sck.Send(sbuff2);
//    //    }
//    //    catch (SocketException ex)
//    //    {
//    //        Console.WriteLine("No damn conection, sir! Some info: " + ex.Message);

//    //    }
//    //}
//    //static public void WriteLine(ref Socket sck, byte[] sbuff2)
//    //{
//    //    byte[] nbuff2;
//    //    nbuff2 = BitConverter.GetBytes(sbuff2.Length);
//    //    try
//    //    {
//    //        sck.Send(nbuff2);
//    //        sck.Send(sbuff2);
//    //    }
//    //    catch (SocketException ex)
//    //    {
//    //        Console.WriteLine("No damn conection, sir! Some info: " + ex.Message);

//    //    }
//    //}
//    //public static string ReadLine(ref Socket sck)
//    //{
//    //    byte[] nbuff1 = new byte[4];
//    //    byte[] sbuff1;
//    //    try
//    //    {
//    //        sck.Receive(nbuff1);
//    //        int l = BitConverter.ToInt32(nbuff1, 0);
//    //        sbuff1 = new byte[l];
//    //        sck.Receive(sbuff1);
//    //        return Encoding.Default.GetString(sbuff1);
//    //    }
//    //    catch (SocketException ex)
//    //    {
//    //        Console.WriteLine("No damn conection, sir! Some info: " + ex.Message);

//    //        return "";
//    //    }
//    //}
//    //public static byte[] ReadBytes(ref Socket sck)
//    //{
//    //    byte[] nbuff1 = new byte[4];
//    //    byte[] sbuff1;
//    //    try
//    //    {
//    //        sck.Receive(nbuff1);
//    //        int l = BitConverter.ToInt32(nbuff1, 0);
//    //        sbuff1 = new byte[l];
//    //        sck.Receive(sbuff1);
//    //        return sbuff1;
//    //    }
//    //    catch (SocketException ex)
//    //    {
//    //        Console.WriteLine("No damn conection, sir! Some info: " + ex.Message);

//    //        return null;
//    //    }
//    //}
//    //public static string SafeReadLine(ref Socket sck, byte[] iv)
//    //{
//    //    byte[] nbuff1 = new byte[4];
//    //    byte[] sbuff1;
//    //    try
//    //    {
//    //        sck.Receive(nbuff1);
//    //        int l = BitConverter.ToInt32(nbuff1, 0);
//    //        sbuff1 = new byte[l];
//    //        sck.Receive(sbuff1);

//    //        var rijndael2 = new RijndaelManaged();
//    //        rijndael2.Key = Crypt.KeyGen();
//    //        rijndael2.IV = iv;
//    //        string res = Encoding.Default.GetString(Crypt.DecryptBytes(rijndael2, sbuff1));
//    //        return res;
//    //    }
//    //    catch (SocketException ex)
//    //    {
//    //        Console.WriteLine("No damn conection, sir! Some info: " + ex.Message);
//    //        return null;
//    //    }
//    //    catch (DecoderFallbackException ex)
//    //    {
//    //        Console.WriteLine("Wrong key: " + ex.Message);
//    //        return null;
//    //    }
//    //}
//    //static public void SafeWriteLine(string str, byte[] iv)
//    //{
//    //    byte[] nbuff2;
//    //    byte[] sbuff2;

//    //    var rijndael1 = new RijndaelManaged();
//    //    rijndael1.Key = Crypt.KeyGen();
//    //    rijndael1.IV = iv;

//    //    sbuff2 = Crypt.EncryptBytes(rijndael1, Encoding.Default.GetBytes(str));
//    //    nbuff2 = BitConverter.GetBytes(sbuff2.Length);
//    //    try
//    //    {
//    //        sck.Send(nbuff2);
//    //        sck.Send(sbuff2);
//    //    }
//    //    catch (SocketException ex)
//    //    {
//    //        Console.WriteLine("No damn conection, sir! Some info: " + ex.Message);

//    //    }
//    //}
//    #endregion
//}
#endregion
