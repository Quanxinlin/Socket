using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketTCP
{
    // 声明委托
    delegate void AddOnLineDelegate(string str, bool bl);
    // 声明委托
    delegate void RecMsgDelegate(string str);

    public partial class FrmTCPServer : Form
    {
        public FrmTCPServer()
        {
            InitializeComponent();
            myAddOnline = AddOnline;
            myRcvMsg = RecMsg;
            myFileSave = FileSave;
        }
        private bool IsRunning = false;
        // 创建套接字
        Socket sock = null;
        //
        IPEndPoint endpoint = null;
        // 创建负责监听客户端连接的线程
        Thread threadListen = null;
        // 创建 URL 与 Socket 的字典集合
        Dictionary<string, Socket> DicSocket = new Dictionary<string, Socket>();
        AddOnLineDelegate myAddOnline;
        RecMsgDelegate myRcvMsg;
        FileSaveDelegate myFileSave;
        CancellationTokenSource cancellationTokenSource;
        // 存储已打开的客户端窗体引用
        List<FrmTCPClient> clientForms = new List<FrmTCPClient>();

        #region 开始监听
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_StartServer_Click(object sender, EventArgs e)
        {
            // 创建负责监听的套接字，注意其中参数：IPV4 字节流 TCP
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse(this.txt_IP.Text.Trim());
            // 根据 IPAddress 以及端口号创建 IPE 对象
            endpoint = new IPEndPoint(address, int.Parse(this.txt_Port.Text.Trim()));
            try
            {
                sock.Bind(endpoint);
                Invoke(myRcvMsg, "服务器开启成功！");
                MessageBox.Show("开启服务成功！", "打开服务");
            }
            catch (Exception ex)
            {
                MessageBox.Show("开启服务失败" + ex.Message, "打开服务");
                return;
            }
            sock.Listen(10);
            IsRunning = true;
            cancellationTokenSource = new CancellationTokenSource();
            threadListen = new Thread(() => ListenConnecting(cancellationTokenSource.Token));
            threadListen.IsBackground = true;
            threadListen.Start();
            this.btn_StartServer.Enabled = false;
            this.btn_closeServer.Enabled = true;
        }
        #endregion

        #region 监听线程
        /// <summary>
        /// 监听线程
        /// </summary>
        private void ListenConnecting(CancellationToken cancellationToken)
        {
            while (IsRunning)
            {
                try
                {
                    // 一旦监听到一个客户端的连接，将会创建一个与该客户端连接的套接字
                    Socket sockClient = sock.Accept();
                    string client = sockClient.RemoteEndPoint.ToString();
                    DicSocket.Add(client, sockClient);
                    Invoke(myAddOnline, client, true);
                    Invoke(myRcvMsg, client + "上线了!");
                    // 开启接受线程
                    Task.Run(() => ReceiveMsg(sockClient, cancellationToken), cancellationToken);
                }
                catch (Exception ex)
                {
                    // 处理异常，输出异常信息或采取相应措施
                    Console.WriteLine($"监听客户端连接时出现异常: {ex.Message}");
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
        }
        #endregion

        #region 接收线程
        /// <summary>
        /// 接收线程
        /// </summary>
        /// <param name="sockClient"></param>
        /// <param name="cancellationToken"></param>
        private void ReceiveMsg(object sockClient, CancellationToken cancellationToken)
        {
            Socket sckclient = sockClient as Socket;
            while (true)
            {
                // 定义一个 2M 缓冲区
                byte[] arrMsgRec = new byte[1024 * 1024 * 2];
                int length = -1;
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    length = sckclient.Receive(arrMsgRec);
                }
                catch (Exception )
                {
                    if (sckclient != null && sckclient.Connected)
                    {
                        string str = sckclient.RemoteEndPoint.ToString();
                        Invoke(myRcvMsg, str + "下线了!");
                    }
                    else
                    {
                        Invoke(myRcvMsg, "客户端已下线");
                    }
                    // 从列表中移除 URL
                    if (sckclient != null && sckclient.Connected)
                    {
                        string str = sckclient.RemoteEndPoint.ToString();
                        Invoke(myAddOnline, str, false);
                        lock (DicSocket)
                        {
                            DicSocket.Remove(str);
                        }
                    }
                    break;
                }
                if (length == 0)
                {
                    string str = sckclient.RemoteEndPoint.ToString();
                    Invoke(myRcvMsg, str + "下线了!");
                    // 从列表中移除 URL
                    Invoke(myAddOnline, str, false);
                    lock (DicSocket)
                    {
                        DicSocket.Remove(str);
                    }
                    break;
                }
                else
                {
                    if (arrMsgRec[0] == 0)
                    {
                        string strMsg = Encoding.UTF8.GetString(arrMsgRec, 1, length - 1);
                        string Msg = "[接收]" + sckclient.RemoteEndPoint.ToString() + "--" + GetCurrentTime() + ",Content:" + strMsg;
                        try
                        {
                            Invoke(myRcvMsg, Msg);
                        }
                        catch (Exception invokeEx)
                        {
                            // 处理 Invoke 异常
                            Console.WriteLine($"Invoke 异常: {invokeEx.Message}");
                        }
                    }
                    if (arrMsgRec[0] == 1)
                    {
                        Invoke(myFileSave, arrMsgRec, length);
                    }
                }
            }
        }
        #endregion

        ///
        /// 获取当前系统时间的方法
        /// 当前时间
        public string GetCurrentTime()
        {
            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
            return formattedTime;
        }

        #region 委托方法体
        private void AddOnline(string url, bool bl)
        {
            if (bl)
            {
                this.lbOnline.Items.Add(url);
            }
            else
            {
                this.lbOnline.Items.Remove(url);
            }
        }

        private void RecMsg(string str)
        {
            try
            {
                this.txt_Rcv.AppendText(str + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // 处理追加文本时的异常
                Console.WriteLine($"追加文本异常: {ex.Message}");
            }
        }

        private void FileSave(byte[] bt, int length)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "word files(*.docx)|*.docx|txt files(*.txt)|*.txt|xls files(*.xls)|*.xls|All files(*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string fileSavePath = sfd.FileName;
                    using (FileStream fs = new FileStream(fileSavePath, FileMode.Create))
                    {
                        fs.Write(bt, 0, length);
                        // 修正后的代码，添加完整的反括号
                        Invoke(new Action(() => this.txt_Rcv.AppendText("[保存]     保存文件成功" + fileSavePath + Environment.NewLine)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存异常" + ex.Message, "保存文件出现异常");
            }
        }
        #endregion

        #region 发送消息
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SendToSingle_Click(object sender, EventArgs e)
        {
            string StrMsg = this.txt_Send.Text.Trim();
            byte[] arrMsg = Encoding.UTF8.GetBytes(StrMsg);
            byte[] arrSend = new byte[arrMsg.Length + 1];
            arrSend[0] = 0;
            Buffer.BlockCopy(arrMsg, 0, arrSend, 1, arrMsg.Length);
            if (this.lbOnline.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择你要发送的对象!", "发送提示");
                return;
            }
            else
            {
                foreach (string item in this.lbOnline.SelectedItems)
                {
                    DicSocket[item].Send(arrSend);
                    string Msg = "[发送]" + item + "     " + StrMsg;
                    Invoke(myRcvMsg, Msg);
                }
            }
        }
        #endregion

        #region 群发消息
        /// <summary>
        /// 群发消息
        /// </summary>
        /// <param name="sender"></summary>
        /// <param name="e"></param>
        private void btn_SendToAll_Click(object sender, EventArgs e)
        {
            string StrMsg = this.txt_Send.Text.Trim();
            byte[] arrMsg = Encoding.UTF8.GetBytes(StrMsg);
            byte[] arrSend = new byte[arrMsg.Length + 1];
            arrSend[0] = 0;
            Buffer.BlockCopy(arrMsg, 0, arrSend, 1, arrMsg.Length);
            foreach (string item in this.DicSocket.Keys)
            {
                DicSocket[item].Send(arrSend);
                string Msg = "[发送]" + item + "     " + StrMsg;
                Invoke(myRcvMsg, Msg);
            }
            Invoke(myRcvMsg, "[群发]     黄小龙: 群发完毕!");
        }
        #endregion

        #region 打开客户端
        /// <summary>
        /// 打开客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Client_Click(object sender, EventArgs e)
        {
            FrmTCPClient objFrm = new FrmTCPClient();
            objFrm.Show();
            clientForms.Add(objFrm); // 存储打开的客户端窗体引用
        }
        #endregion

        #region 选择文件
        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "D:\\";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txt_SelectFile.Text = ofd.FileName;
            }
        }
        #endregion

        #region 发送文件
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="sender"></summary>
        /// <param name="e"></param>
        private void btn_SendFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_SelectFile.Text))
            {
                MessageBox.Show("请选择您要发送的文件！", "发送文件提示");
                return;
            }
            string online = this.lbOnline.SelectedItem.ToString().Trim();
            if (string.IsNullOrEmpty(online))
            {
                MessageBox.Show("请选择您要发送的对象！", "发送文件提示");
                return;
            }
            using (FileStream fs = new FileStream(txt_SelectFile.Text, FileMode.Open))
            {
                string filename = Path.GetFileName(txt_SelectFile.Text);
                string StrMsg = "发送文件为：" + filename;
                byte[] arrMsg = Encoding.UTF8.GetBytes(StrMsg);
                byte[] arrSend = new byte[arrMsg.Length + 1];
                arrSend[0] = 0;
                Buffer.BlockCopy(arrMsg, 0, arrSend, 1, arrMsg.Length);
                DicSocket[online].Send(arrSend);
                byte[] arrfileSend = new byte[1024 * 1024 * 2];
                int length = fs.Read(arrfileSend, 0, arrfileSend.Length);
                byte[] arrfile = new byte[length + 1];
                arrfile[0] = 1;
                Buffer.BlockCopy(arrfileSend, 0, arrfile, 1, length);
                DicSocket[online].Send(arrfile);
            }
        }
        #endregion

        private void FrmTCPServer_Load(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void closeServer_Click(object sender, EventArgs e)
        {
            try
            {
                // 先断开所有客户端连接
                foreach (var clientSocket in DicSocket.Values)
                {
                    if (clientSocket.Connected)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                    }
                }

                // 关闭所有已打开的客户端窗体
                foreach (var clientForm in clientForms)
                {
                    if (clientForm != null)
                    {
                        clientForm.Close();
                    }
                }
                clientForms.Clear();

                // 关闭服务器停止监听
                sock.Close();
                IsRunning = false;
                cancellationTokenSource.Cancel();
                // 标记所有客户端为下线
                foreach (var client in DicSocket.Keys)
                {
                    Invoke(myAddOnline, client, false);
                }
                // 清除 DicSocket 字典中的所有元素
                lock (DicSocket)
                {
                    DicSocket.Clear();
                }
                Invoke(myRcvMsg, "服务器关闭成功！");
                MessageBox.Show("关闭服务成功！", "关闭服务");
                threadListen.Join();
                this.btn_closeServer.Enabled = false;
                this.btn_StartServer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("关闭服务失败" + ex.Message, "关闭服务");
                return;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}