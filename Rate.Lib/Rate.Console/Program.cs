using System;
using System.Security.Policy;
using System.Text;
using Rate.Lib;
using Rate.Lib.Enum;
using Rate.Lib.Models;

namespace Rate
{
    class Program
    {
        static void Main(string[] args)
        {
            bool Exit = false;
            Url BankUrl = new Url("-");
            DataMeta Result = new DataMeta();
            while (!Exit)
            {
                Console.WriteLine("請輸入銀行代碼，\r\n台灣銀行(004)  台新銀行(812)\r\n");
                var BankKey = Console.ReadLine();
                switch (Enum.Parse(typeof(EnumBank), BankKey))
                {
                    case EnumBank.台灣銀行:
                        BankUrl = new Url("https://rate.bot.com.tw/xrt/all/day");
                        Result = BankUrl.GetRate(EnumBank.台灣銀行);
                        
                        break;
                    case EnumBank.台新銀行:
                        BankUrl = new Url("https://www.taishinbank.com.tw/TS/TS06/TS0605/TS060502/index.htm?urlPath1=TS02&urlPath2=TS0202");
                        Result = BankUrl.GetRate(EnumBank.台新銀行);
                        
                        break;
                    default:
                        Exit = true;
                        break;
                }
                ShowResult(Result);
            }
        }

        public static void ShowResult (DataMeta Result)
        {
            Console.WriteLine("--------------------------------------------------------------------------------------------\r\n"
                           + Result.Key + "匯率"
                           + " 資料更新時間為：" + Result.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss.fff")
                           + " 下次更新時間為：" + Result.Expire.Value.ToString("yyyy/MM/dd HH:mm:ss.fff")
                           + "\r\n--------------------------------------------------------------------------------------------");
            Console.WriteLine("幣別                即期買入            即期賣出            現鈔買入            現鈔賣出");
            foreach (var item in Result.Data)
            {
                byte[] byteStr = Encoding.GetEncoding("big5").GetBytes(item.Currencty);
                Console.WriteLine(
                    string.Format("{0}{5}{1}{6}{2}{7}{3}{8}{4} \r\n"
                    , item.Currencty
                    , item.SpotBuying
                    , item.SpotSelling
                    , item.CashBuying
                    , item.CashSelling
                    , "".PadLeft(20 - Encoding.GetEncoding("big5").GetBytes(item.Currencty).Length, ' ')
                    , "".PadLeft(20 - Encoding.GetEncoding("big5").GetBytes(item.SpotBuying).Length, ' ')
                    , "".PadLeft(20 - Encoding.GetEncoding("big5").GetBytes(item.SpotSelling).Length, ' ')
                    , "".PadLeft(20 - Encoding.GetEncoding("big5").GetBytes(item.CashBuying).Length, ' ')
                    , "".PadLeft(20 - Encoding.GetEncoding("big5").GetBytes(item.CashSelling).Length, ' ')
                    ));
            }
            Console.WriteLine("--------------------------------------------------------------------------------------------\r\n");
        }
    }
}
