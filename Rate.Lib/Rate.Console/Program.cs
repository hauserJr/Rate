using System;
using System.Security.Policy;
using System.Text;
using Rate.Lib;
using Rate.Lib.Enum;

namespace Rate
{
    class Program
    {
        static void Main(string[] args)
        {
            bool Exit = false;
            while (!Exit)
            {
                Console.WriteLine("請輸入銀行代碼，\r\n台新(0)\r\n");
                var BankKey = Console.ReadLine();
                switch (BankKey)
                {
                    case "0":
                        Url BankUrl = new Url("https://www.taishinbank.com.tw/TS/TS06/TS0605/TS060502/index.htm?urlPath1=TS02&urlPath2=TS0202");
                        var Result = BankUrl.GetRate(EnumBank.台新);
                        Console.WriteLine("--------------------------------------------------------------------------------------------\r\n" 
                            + EnumBank.台新 + "匯率" 
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
                        break;
                    default:
                        Exit = true;
                        break;
                }
            }
        }
    }
}
