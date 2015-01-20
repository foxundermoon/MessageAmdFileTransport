using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace FileDownloadAndUpload.Core.Common
{
    public class ValidateCode
    {
        static char[][] numbers;
        static object myLock = new object();
        public static char[][] Numbers
        {
            get
            {
                lock (myLock)
                {

                    if (numbers == null)
                    {
                        numbers = new char[][] { 
                        "零oO0〇".ToCharArray(),
                        "一一1①Ⅰ壹I㈠".ToCharArray(),
                        "二Ⅱ贰②㈡".ToCharArray(),
                        "三3叁Ⅲ③彡氵Ⅲ弎㈢参".ToCharArray(),
                        "四④4Ⅳ肆泗亖㈣".ToCharArray(),
                        "五5⑤Ⅴ㈤伍午吴".ToCharArray(),
                        "六6⑥Ⅵ陆流留陸㈥".ToCharArray(),
                        "七7Ⅶ⑦柒㈦".ToCharArray(),
                        "八8⑧Ⅷ捌㈧玐仈".ToCharArray(),
                        "九9⑨Ⅸ久玖氿㈨勼".ToCharArray()
                        };
                    }
                    return numbers;
                }

            }
        }

        public static string EncodeCode(string intcode)
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            foreach(var n in Numbers)
            {
                sb.Append(n[r.Next(0, n.Length - 1)]);
            }
            return sb.ToString();
        }

        public static string GenerateCode(int length)
        {
            //char[] allchars = "0123456789abcdefghigklmnopqrstuvwxyz".ToArray();
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(r.Next(0, 9));
            }
            return sb.ToString();
        }

        public static System.Web.Mvc.ActionResult WriteImage(string encoded, HttpResponseBase Response)
        {
            int randAngle = 10; //随机转动角度  
            int mapwidth = (int)(encoded.Length * 23);
            Bitmap map = new Bitmap(mapwidth, 28);//创建图片背景  
            Graphics graph = Graphics.FromImage(map);
            graph.Clear(Color.AliceBlue);//清除画面，填充背景  
            graph.DrawRectangle(new Pen(Color.Black, 0), 0, 0, map.Width - 1, map.Height - 1);//画一个边框  
            //graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//模式  
            Random rand = new Random();
            //背景噪点生成    www.jb51.net
            Pen blackPen = new Pen(Color.LightGray, 0);
            for (int i = 0; i < 50; i++)
            {
                int x = rand.Next(0, map.Width);
                int y = rand.Next(0, map.Height);
                graph.DrawRectangle(blackPen, x, y, 1, 1);
            }
            //验证码旋转，防止机器识别    
            char[] chars = encoded.ToCharArray();//拆散字符串成单字符数组  
            //文字距中  
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            //定义颜色  
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体  
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            for (int i = 0; i < chars.Length; i++)
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(5);
                Font f = new System.Drawing.Font(font[findex], 13, System.Drawing.FontStyle.Bold);//字体样式(参数2为字体大小)  
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                Point dot = new Point(16, 16);
                //graph.DrawString(dot.X.ToString(),fontstyle,new SolidBrush(Color.Black),10,150);//测试X坐标显示间距的  
                float angle = rand.Next(-randAngle, randAngle);//转动的度数  
                graph.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置  
                graph.RotateTransform(angle);
                graph.DrawString(chars.ToString(), f, b, 1, 1, format);
                //graph.DrawString(chars.ToString(),fontstyle,new SolidBrush(Color.Blue),1,1,format);  
                graph.RotateTransform(-angle);//转回去  
                graph.TranslateTransform(2, -dot.Y);//移动光标到指定位置  
            }
            //graph.DrawString(randomcode,fontstyle,new SolidBrush(Color.Blue),2,2); //标准随机码  
            //生成图片  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            map.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            Response.ClearContent();
            Response.ContentType = "image/gif";
            Response.BinaryWrite(ms.ToArray());
            graph.Dispose();
            map.Dispose();
            return null;
        }
    }
}