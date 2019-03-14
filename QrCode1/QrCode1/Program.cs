using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Gma.QrCodeNet.Encoding;
using System.IO;
using Gma.QrCodeNet.Encoding.Windows.Render;
namespace QrCodeTest
{
    class Program
    {
        //public static System.Text.Encoding GetFileEncodeType(string filename)
        //{
        //    System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
        //    Byte[] buffer = br.ReadBytes(2);
        //    if (buffer[0] >= 0xEF)
        //    {
        //        if (buffer[0] == 0xEF && buffer[1] == 0xBB)
        //        {
        //            return System.Text.Encoding.UTF8;
        //        }
        //        else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
        //        {
        //            return System.Text.Encoding.BigEndianUnicode;
        //        }
        //        else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
        //        {
        //            return System.Text.Encoding.Unicode;
        //        }
        //        else
        //        {
        //            return System.Text.Encoding.Default;
        //        }
        //    }
        //    else
        //    {
        //        return System.Text.Encoding.Default;
        //    }
        //}

        //生成图片并新建文件夹，保存至文件夹内。
        // 1-> 001
        // 1923->192
        /// <summary>
        /// 根据行号生成对应的三位长度
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string getString(int s) {
            if (s <= 0) return null;
            string str = s.ToString(); 
            if (str.Length >= 3) //三位数直接取
            {
                str = str.Substring(0, 3);
            }
            else //加工。
            {
                if(str.Length == 1)
                {
                    str = "00" + str;
                }
                else
                {
                    str = "0" + str;
                }
            }
            return str;
        }
        /// <summary>
        /// 截取内容的前四个字符
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        //截取前面的四位字符
        public static string GetContSplit(string con)
        {
            while(con.Length < 4)
            {
                con += "?";
            }
            return con.Substring(0, 4);
        }


        private void SaveCodeFile(string content, int col_n,string path)
        {

            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode code = new QrCode();
            qrEncoder.TryEncode(content, out code);
            const int modelSizeInPixels = 4;
            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(modelSizeInPixels, QuietZoneModules.Two)
                , Brushes.Black, Brushes.White);

            //在目标文件夹创建一个空的.bmp文件
            string filename = path + "/" + Program.getString(col_n) + Program.GetContSplit(content) + ".bmp";
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                render.WriteToStream(code.Matrix, ImageFormat.Png, fs);
            }
        }

        public void GetQrCodePrint(string content)
        {
            //content被修改。
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = qrEncoder.Encode(content);

            for (int j = 0; j < qrCode.Matrix.Width; j++)
            {
                for (int i = 0; i < qrCode.Matrix.Width; i++)
                {

                    char charToPrint = qrCode.Matrix[i, j] ? '　' : '■';
                    Console.Write(charToPrint);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            Program obj = new Program();
            //判断是否有命令行参数输入
            if (args.Length  == 1)
            {
                //强制用户用""来输入文件地址。
                string target = args[0].StartsWith("-f") ? args[0].Substring(2) : args[0];

                if(!target.StartsWith("\"") || !target.EndsWith("\""))
                {
                    Console.WriteLine("Input path with \"\"");
                    return;
                }
                if (!File.Exists(target))
                {
                    Console.WriteLine("Invalid path");
                    return;
                }
                //Encoding encoding = Program.GetFileEncodeType(target);
                StreamReader srReadFile = new StreamReader(target,Encoding.UTF8);

                //判断是否需要存储图片。
                if (args[0].StartsWith("-f"))
                {
                    
                    //创建一个文件夹。文件夹储存在exe所在目录下。
                    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    path = path + "pics";
                    Console.WriteLine(path);
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path,true);
                    }
                    Directory.CreateDirectory(path);
                    Console.WriteLine("正在生成目录。");

                    int i = 1;
                    while (!srReadFile.EndOfStream)
                    {
                        Console.WriteLine("正在生成文件。");
                        string strReadLine = srReadFile.ReadLine(); //读取每行数据
                        Console.WriteLine(strReadLine);
                        obj.SaveCodeFile(strReadLine, i, path);
                        i++;
                    }
                    srReadFile.Close();
                }
                else
                {
                    int i = 1;
                    while (!srReadFile.EndOfStream)
                    {
                        string strReadLine = srReadFile.ReadLine(); //读取每行数据
                        Console.WriteLine("这是第" + i + "行信息生成的二维码");
                        obj.GetQrCodePrint(strReadLine);
                        Console.WriteLine();
                        i++;
                    }

                }
            }
            else
            {
                Console.Write("Type some text to QR code: ");
                string text = Console.ReadLine();
                while (text.Length > 20)
                {
                    Console.WriteLine("Length too long, please input again:");
                    text = Console.ReadLine();
                }
                obj.GetQrCodePrint(text);
            }
            Console.ReadKey();
        }
    }
}
