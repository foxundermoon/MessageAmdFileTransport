using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Forms;

namespace FileDownloadAndUpload.Core.Xmpp.UI
{
    public partial class Manager : Form
    {
        private ListView listView1;
        private TextBox textBox2;
        private Button button2;
        private Button button1;
        private TextBox textBox1;
        private Button button3;
        private XmppServer xmppserver;
    
        public Manager()
            : base()
        {
            xmppserver = XmppServer.GetInstance();
            InitializeComponent();
            InitializeOther();
        }

        private void InitializeOther()
        {
            //xmppserver.ConnectionEncrease += xmppserver_ConnectionEncrease;
            //xmppserver.ConnectionDecrease += xmppserver_ConnectionDecrease;
            foreach(var item in xmppserver.XmppConnectionDic)
            {
                listView1.Items.Add(item.Key+"");
            }
        }

        void xmppserver_ConnectionDecrease()
        {
        }

        void xmppserver_ConnectionEncrease()
        {
        }

        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(332, 29);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(121, 433);
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(8, 220);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(318, 242);
            this.textBox2.TabIndex = 9;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(249, 187);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "发送消息";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(85, 187);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "清空";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 29);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(318, 152);
            this.textBox1.TabIndex = 6;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(167, 187);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "推送通知";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Manager
            // 
            this.ClientSize = new System.Drawing.Size(464, 466);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string msg = textBox1.Text;
            if (string.IsNullOrWhiteSpace(msg))
            {
                i("不能发送空消息");
            }
            else
            {
                xmppserver.Broadcast(msg, Xmpp.Type.Message);
                textBox1.Text = string.Empty;

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string msg = textBox1.Text;
            if(string.IsNullOrWhiteSpace(msg))
            {
                i("不能发送空消息");
            }
            else
            {
                xmppserver.Broadcast(msg, Xmpp.Type.Notification);
                textBox1.Text = string.Empty;

            }
        }

        private void i(string msg)
        {
            textBox2.AppendText(msg + "\r\n");
        }

    }
}