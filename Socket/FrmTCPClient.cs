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
    delegate void FileSaveDelegate(byte[] bt, int length);
    public partial class FrmTCPClient : Form
    {
        public FrmTCPClient()
        {
            InitializeComponent();
            MyFileSave = FileSave;
        }

        // Socket 对象
        Socket sockClient = null;

        // 接收线程
        Thread thrClient = null;

        // 运行标志位
        private bool IsRunning = true;

        // 文件保存委托对象
        FileSaveDelegate MyFileSave;

        #region 连接服务器
        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            IPAddress address = IPAddress.Parse(this.txt_IP.Text.Trim());

            IPEndPoint Ipe = new IPEndPoint(address, int.Parse(this.txt_Port.Text.Trim()));

            sockClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                this.txt_Rcv.AppendText("与服务器连接中......" + Environment.NewLine);
                sockClient.Connect(Ipe);
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败" + ex.Message, "建立连接");
                this.txt_Rcv.AppendText("与服务器连接失败." + Environment.NewLine);
                return;
            }

            this.txt_Rcv.AppendText("与服务器连接成功" + Environment.NewLine);
            this.btn_Connect.Enabled = false;
            this.btn_Disconnect.Enabled = true;
            this.btn_Send.Enabled = true;
            this.btn_SendFile.Enabled = true;

            thrClient = new Thread(ReceiceMsg);
            thrClient.IsBackground = true;
            thrClient.Start();
        }
        #endregion

        #region 接收消息
        /// <summary>
        /// 接收消息
        /// </summary>
        private void ReceiceMsg()
        {
            while (IsRunning)
            {
                // 定义一个 2M 缓冲区
                byte[] arrMsgRec = new byte[1024 * 1024 * 2];

                int length = -1;

                try
                {
                    length = sockClient.Receive(arrMsgRec);
                }
                catch (SocketException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (!this.IsDisposed && this.txt_Rcv != null && this.txt_Rcv.IsHandleCreated)
                    {
                        try
                        {
                            Invoke(new Action(() =>
                            {
                                if (ex is ObjectDisposedException)
                                {
                                    this.txt_Rcv.AppendText("对象已释放，连接已断开" + Environment.NewLine);
                                }
                                else if (ex is InvalidOperationException)
                                {
                                    this.txt_Rcv.AppendText("操作无效，连接可能已断开" + Environment.NewLine);
                                }
                                else
                                {
                                    this.txt_Rcv.AppendText("断开连接: " + ex.Message + Environment.NewLine);
                                }
                            }));
                        }
                        catch (Exception invokeEx)
                        {
                            // 处理 Invoke 引发的异常
                            Console.WriteLine("Invoke 异常: " + invokeEx.Message);
                        }
                    }
                    break;
                }

                if (length > 0)
                {
                    // 表示接受到的为消息类型
                    if (arrMsgRec[0] == 0)
                    {
                        string strMsg = Encoding.UTF8.GetString(arrMsgRec, 1, length - 1);
                        string Msg = "[接收]" + strMsg + Environment.NewLine;
                        if (!this.IsDisposed && this.txt_Rcv != null && this.txt_Rcv.IsHandleCreated)
                        {
                            Invoke(new Action(() => this.txt_Rcv.AppendText(Msg)));
                        }
                    }
                    // 表示接收到的为文件类型
                    if (arrMsgRec[0] == 1)
                    {
                        if (!this.IsDisposed && this.txt_Rcv != null && this.txt_Rcv.IsHandleCreated)
                        {
                            Invoke(MyFileSave, arrMsgRec, length);
                        }
                    }
                }
            }
        }
        #endregion

        #region 委托方法体
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
                        fs.Write(bt, 1, length - 1);
                        if (!this.IsDisposed && this.txt_Rcv != null && this.txt_Rcv.IsHandleCreated)
                        {
                            Invoke(new Action(() => this.txt_Rcv.AppendText("[保存]     保存文件成功" + fileSavePath + Environment.NewLine)));
                        }
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
        private void btn_Send_Click(object sender, EventArgs e)
        {
            string strMsg = "来自" + this.txt_Name.Text.Trim() + ":  " + this.txt_Send.Text.Trim() + ",Send time:" + GetCurrentTime();
            byte[] arrMsg = Encoding.UTF8.GetBytes(strMsg);

            byte[] arrSend = new byte[arrMsg.Length + 1];
            arrSend[0] = 0;
            Buffer.BlockCopy(arrMsg, 0, arrSend, 1, arrMsg.Length);

            sockClient.Send(arrSend);
            if (!this.IsDisposed && this.txt_Rcv != null && this.txt_Rcv.IsHandleCreated)
            {
                Invoke(new Action(() => this.txt_Rcv.AppendText("[发送]" + this.txt_Send.Text.Trim() + Environment.NewLine)));
            }
        }
        #endregion

        #region 窗体关闭事件
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmTCPClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsRunning = false;
            sockClient?.Close();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SendFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_SelectFile.Text))
            {
                MessageBox.Show("请选择您要发送的文件！", "发送文件提示");
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

                sockClient.Send(arrSend);


                byte[] arrfileSend = new byte[1024 * 1024 * 2];
                int length = fs.Read(arrfileSend, 0, arrfileSend.Length);

                byte[] arrfile = new byte[length + 1];
                arrfile[0] = 1;
                Buffer.BlockCopy(arrfileSend, 0, arrfile, 1, length);

                sockClient.Send(arrfile);
            }
        }
        #endregion

        // 新增主动断开连接的按钮点击事件
        private void btn_Disconnect_Click(object sender, EventArgs e)
        {
            try
            {
                IsRunning = false;
                if (sockClient.Connected)
                {
                    sockClient.Shutdown(SocketShutdown.Both);
                    sockClient.Close();
                }
                this.txt_Rcv.AppendText("已断开与服务器的连接" + Environment.NewLine);
                this.btn_Connect.Enabled = true;
                this.btn_Disconnect.Enabled = false;
                this.btn_Send.Enabled = false;
                this.btn_SendFile.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("断开连接失败: " + ex.Message, "断开连接");
            }
        }

        private void FrmTCPClient_Load(object sender, EventArgs e)
        {

        }
    }
}