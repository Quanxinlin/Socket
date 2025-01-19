namespace SocketTCP
{
    partial class FrmTCPServer
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.txt_Port = new System.Windows.Forms.TextBox();
            this.txt_Rcv = new System.Windows.Forms.TextBox();
            this.txt_Send = new System.Windows.Forms.TextBox();
            this.lbOnline = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_SelectFile = new System.Windows.Forms.TextBox();
            this.btn_SelectFile = new System.Windows.Forms.Button();
            this.btn_SendFile = new System.Windows.Forms.Button();
            this.btn_StartServer = new System.Windows.Forms.Button();
            this.btn_SendToSingle = new System.Windows.Forms.Button();
            this.btn_SendToAll = new System.Windows.Forms.Button();
            this.btn_Client = new System.Windows.Forms.Button();
            this.btn_closeServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(594, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "本   机   IP：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(591, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 28);
            this.label2.TabIndex = 0;
            this.label2.Text = "端    口   号：";
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(747, 27);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(118, 31);
            this.txt_IP.TabIndex = 1;
            this.txt_IP.Text = "127.0.0.1";
            // 
            // txt_Port
            // 
            this.txt_Port.Location = new System.Drawing.Point(747, 89);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(118, 31);
            this.txt_Port.TabIndex = 1;
            this.txt_Port.Text = "1002";
            // 
            // txt_Rcv
            // 
            this.txt_Rcv.Location = new System.Drawing.Point(12, 29);
            this.txt_Rcv.Multiline = true;
            this.txt_Rcv.Name = "txt_Rcv";
            this.txt_Rcv.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Rcv.Size = new System.Drawing.Size(558, 200);
            this.txt_Rcv.TabIndex = 1;
            // 
            // txt_Send
            // 
            this.txt_Send.Location = new System.Drawing.Point(12, 270);
            this.txt_Send.Multiline = true;
            this.txt_Send.Name = "txt_Send";
            this.txt_Send.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Send.Size = new System.Drawing.Size(558, 178);
            this.txt_Send.TabIndex = 1;
            // 
            // lbOnline
            // 
            this.lbOnline.FormattingEnabled = true;
            this.lbOnline.ItemHeight = 24;
            this.lbOnline.Location = new System.Drawing.Point(600, 171);
            this.lbOnline.Name = "lbOnline";
            this.lbOnline.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbOnline.Size = new System.Drawing.Size(265, 76);
            this.lbOnline.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(594, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 28);
            this.label3.TabIndex = 0;
            this.label3.Text = "在 线 列 表：";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txt_SelectFile
            // 
            this.txt_SelectFile.Location = new System.Drawing.Point(12, 483);
            this.txt_SelectFile.Name = "txt_SelectFile";
            this.txt_SelectFile.Size = new System.Drawing.Size(558, 31);
            this.txt_SelectFile.TabIndex = 1;
            // 
            // btn_SelectFile
            // 
            this.btn_SelectFile.Location = new System.Drawing.Point(600, 479);
            this.btn_SelectFile.Name = "btn_SelectFile";
            this.btn_SelectFile.Size = new System.Drawing.Size(123, 35);
            this.btn_SelectFile.TabIndex = 3;
            this.btn_SelectFile.Text = "选择文件";
            this.btn_SelectFile.UseVisualStyleBackColor = true;
            this.btn_SelectFile.Click += new System.EventHandler(this.btn_SelectFile_Click);
            // 
            // btn_SendFile
            // 
            this.btn_SendFile.Location = new System.Drawing.Point(760, 481);
            this.btn_SendFile.Name = "btn_SendFile";
            this.btn_SendFile.Size = new System.Drawing.Size(105, 33);
            this.btn_SendFile.TabIndex = 3;
            this.btn_SendFile.Text = "发送文件";
            this.btn_SendFile.UseVisualStyleBackColor = true;
            this.btn_SendFile.Click += new System.EventHandler(this.btn_SendFile_Click);
            // 
            // btn_StartServer
            // 
            this.btn_StartServer.Location = new System.Drawing.Point(611, 261);
            this.btn_StartServer.Name = "btn_StartServer";
            this.btn_StartServer.Size = new System.Drawing.Size(105, 33);
            this.btn_StartServer.TabIndex = 3;
            this.btn_StartServer.Text = "启动服务";
            this.btn_StartServer.UseVisualStyleBackColor = true;
            this.btn_StartServer.Click += new System.EventHandler(this.btn_StartServer_Click);
            // 
            // btn_SendToSingle
            // 
            this.btn_SendToSingle.Location = new System.Drawing.Point(611, 318);
            this.btn_SendToSingle.Name = "btn_SendToSingle";
            this.btn_SendToSingle.Size = new System.Drawing.Size(105, 33);
            this.btn_SendToSingle.TabIndex = 3;
            this.btn_SendToSingle.Text = "发送消息";
            this.btn_SendToSingle.UseVisualStyleBackColor = true;
            this.btn_SendToSingle.Click += new System.EventHandler(this.btn_SendToSingle_Click);
            // 
            // btn_SendToAll
            // 
            this.btn_SendToAll.Location = new System.Drawing.Point(611, 365);
            this.btn_SendToAll.Name = "btn_SendToAll";
            this.btn_SendToAll.Size = new System.Drawing.Size(105, 33);
            this.btn_SendToAll.TabIndex = 3;
            this.btn_SendToAll.Text = "群发消息";
            this.btn_SendToAll.UseVisualStyleBackColor = true;
            this.btn_SendToAll.Click += new System.EventHandler(this.btn_SendToAll_Click);
            // 
            // btn_Client
            // 
            this.btn_Client.Location = new System.Drawing.Point(611, 412);
            this.btn_Client.Name = "btn_Client";
            this.btn_Client.Size = new System.Drawing.Size(105, 33);
            this.btn_Client.TabIndex = 3;
            this.btn_Client.Text = "客户端";
            this.btn_Client.UseVisualStyleBackColor = true;
            this.btn_Client.Click += new System.EventHandler(this.btn_Client_Click);
            // 
            // btn_closeServer
            // 
            this.btn_closeServer.Location = new System.Drawing.Point(760, 261);
            this.btn_closeServer.Name = "btn_closeServer";
            this.btn_closeServer.Size = new System.Drawing.Size(105, 33);
            this.btn_closeServer.TabIndex = 4;
            this.btn_closeServer.Text = "关闭服务";
            this.btn_closeServer.UseMnemonic = false;
            this.btn_closeServer.UseVisualStyleBackColor = true;
            this.btn_closeServer.Click += new System.EventHandler(this.closeServer_Click);
            // 
            // FrmTCPServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(904, 553);
            this.Controls.Add(this.btn_closeServer);
            this.Controls.Add(this.btn_SendToAll);
            this.Controls.Add(this.btn_SendToSingle);
            this.Controls.Add(this.btn_StartServer);
            this.Controls.Add(this.btn_Client);
            this.Controls.Add(this.btn_SendFile);
            this.Controls.Add(this.btn_SelectFile);
            this.Controls.Add(this.lbOnline);
            this.Controls.Add(this.txt_Send);
            this.Controls.Add(this.txt_Rcv);
            this.Controls.Add(this.txt_SelectFile);
            this.Controls.Add(this.txt_Port);
            this.Controls.Add(this.txt_IP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmTCPServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "基于Socket开发的TCP通信";
            this.Load += new System.EventHandler(this.FrmTCPServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.TextBox txt_Port;
        private System.Windows.Forms.TextBox txt_Rcv;
        private System.Windows.Forms.TextBox txt_Send;
        private System.Windows.Forms.ListBox lbOnline;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_SelectFile;
        private System.Windows.Forms.Button btn_SelectFile;
        private System.Windows.Forms.Button btn_SendFile;
        private System.Windows.Forms.Button btn_StartServer;
        private System.Windows.Forms.Button btn_SendToSingle;
        private System.Windows.Forms.Button btn_SendToAll;
        private System.Windows.Forms.Button btn_Client;
        private System.Windows.Forms.Button btn_closeServer;
    }
}

