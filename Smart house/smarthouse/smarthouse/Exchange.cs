using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace smarthouse
{
    class Exchange
    {
        Socket listener;
        List<Session> sessions = new List<Session>();
        List<buff_item> outbuff = new List<buff_item>();

        Int32 append;
        #region accept
        public void StartConnect(int port)
        {
            append = 0;
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint myEP = new IPEndPoint(IPAddress.Any, port);
            listener.Bind(myEP);
            listener.Listen(10);
            listener.BeginAccept(EndAccept, null);
            Listener(true);
        }

        void EndAccept(System.IAsyncResult ar)
        {
            Console.WriteLine("New guy connected");
            listener.BeginAccept(EndAccept, null); //listen to other clients
            #region check
            Socket sck = listener.EndAccept(ar);
            #region Send append
            Console.WriteLine("Append " + append);
            sck.Send(BitConverter.GetBytes(append));
            unchecked { append++; };
            #endregion
            #region recieve md5+append
            byte[] rec = new byte[32];
            int count = sck.Receive(rec);
            if (count != 32)
            {
                Console.WriteLine("Wrong private key. Recieved " + count + " bytes");
                return;
            }
            string nmd5 = Encoding.UTF8.GetString(rec);
            Console.WriteLine("md5 " + nmd5);
            #endregion
            #region gener md5+append
            string md5 = getKeyToSend(append - 1);
            #endregion
            #region comparison
            if (md5 == nmd5)
            {
                Console.WriteLine("Right private password.");
                #region restore or start new session
                Int32 ID;
                byte[] buff = new byte[4];
                sck.Receive(buff);
                ID = BitConverter.ToInt32(buff, 0);
                Session[] idArr = sessions.Where(a => a.ID == ID).Take(1).ToArray();
                if (idArr.Length == 1)
                {
                    int eid = sessions.IndexOf(idArr[0]);
                    Console.WriteLine("Restore session " + ID);
                    sessions[eid].RestoreSession(sck);
                }
                else
                {
                    Console.WriteLine("No such session. I will create new one!");
                    sessions.Add(new Session(ID, sck));
                }

                StateObject so = new StateObject();
                so.workSocket = sck;
                sck.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, SocketFlags.None, EndRecieve, so);
                #endregion
            }
            else
            {
                Console.WriteLine("Wrong private password.");
            }
            #endregion
            #endregion
        }
        #endregion
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
        class StateObject
        {
            public Socket workSocket = null;
            public const int BUFFER_SIZE = 2;
            public byte[] buffer = new byte[BUFFER_SIZE];
        }
        void EndRecieve(System.IAsyncResult ar)
        {
            StateObject so = (StateObject)ar.AsyncState;
            byte pin = so.buffer[0];
            bool type = !(so.buffer[1] < 128);
            byte[] value;
            if (type)
            {
                //неудача. Нас ждет анальная боль. Много байт. К хуям бесперебойность!
                byte[] temp = new byte[3];
                so.workSocket.Receive(temp, 0, 3, SocketFlags.None);//качаем ещё 3 бита размера
                byte[] s = new byte[4] { temp[0], temp[1], temp[2], (byte)(so.buffer[1] - 128) };//здесь у нас будет храниться размер
                UInt32 size = BitConverter.ToUInt32(s, 0);
                value = new byte[size];
                so.workSocket.Receive(value, SocketFlags.None);//принимаем невъебенно большой файл и забиваем его в value 
            }
            else
            {
                value = new byte[] { so.buffer[1] }; //збc, всего 1 байт.
            }

            Smarthouse.output.SetValue(pin, value);//sending 

            so.workSocket.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, SocketFlags.None, EndRecieve, so); //вылетает ошибка при обрывании коннекта
        }
        private void Listener(bool value)
        {
            #region Thread
            System.Threading.Thread t = new System.Threading.Thread(delegate()
            {
                do
                {
                    if (outbuff.Count > 0)
                    {
                        var v = sessions.Where(a => a.status == true && a.listento.Contains(outbuff[0].pin) == true);
                        foreach (Session s in v)
                        {
                            s.send(outbuff[0].pin, outbuff[0].value);
                        }
                        outbuff.RemoveAt(0);
                    }
                    Thread.Sleep(25);
                } while (true);
            });
            #endregion
            if (value == true)
            {
                t.Start();
            }
            else
            {
                if (t.ThreadState != ThreadState.Stopped && t.ThreadState != ThreadState.StopRequested)
                {
                    t.Abort();
                }
            }
        }//обход списка на выдачу

        private class Session
        {
            #region vars
            public Int32 ID;
            public bool status;
            public List<byte> listento = new List<byte>();
            public Socket sck;
            #endregion
            public Session(int ID, Socket sck)
            {
                #region setting
                this.sck = sck;
                this.ID = ID;
                status = true;
                listento.Add(Smarthouse.test_console);
                Console.WriteLine("Creating new session!");
                #endregion

            }
            public void RestoreSession(Socket sck)
            {
                #region setting
                this.sck = sck; // fucking hightload
                status = true;
                #endregion
            }
            public void send(byte pin, object buff)
            {
                byte[] res;

                if (((byte[])(buff)).Length <= 1 && ((byte[])(buff))[0] <= 128)
                {
                    res = new byte[] { pin, ((byte[])(buff))[0] };
                }
                else
                {
                    byte[] size = new byte[4];
                    size = BitConverter.GetBytes(((byte[])(buff)).Length);
                    sck.Send(new byte[2] { pin, (byte)(size[3] + 128) });
                    sck.Send(new byte[3] { size[0], size[1], size[2] });
                    res = ((byte[])(buff));
                }
                sck.Send(res);
            }
        }
        public static void Out(object cmd)
        {
            Smarthouse.con.outbuff.Add((buff_item)cmd);
        }
    }

    class buff_item
    {
        public byte pin;
        public object value;
        public buff_item(byte pin, byte[] value)
        {
            this.pin = pin;
            this.value = value;
        }
    }
}
