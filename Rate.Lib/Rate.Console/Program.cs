using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Rate.Lib;
using Rate.Lib.Enum;
using Rate.Lib.Models;
namespace Rate
{
    class Program
    {
        static int NoSpaceSize = 6;
        static int SpaceSize = 15;
        static string SerialNo = "No.";
        static string Currencty = "幣別";
        static string SpotBuying = "即期買入";
        static string SpotSelling = "即期賣出";
        static string CashBuying = "現鈔買入";
        static string CashSelling = "現鈔賣出";
        public Program()
        {

        }
        static void Main(string[] args)
        {
            var Exit = false;
            var Result = new DataMeta();
            while (!Exit)
            {
                ShowEnumList();
                var BankKey = Console.ReadLine();

                switch (Enum.Parse(typeof(EnumBank), BankKey))
                {
                    case EnumBank.臺灣銀行:
                        Result = BankRate.GetRate(EnumBank.臺灣銀行);
                        break;

                    case EnumBank.台新銀行:
                        Result = BankRate.GetRate(EnumBank.台新銀行);
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

            string Title = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}"
                , SerialNo
                , CalSpace(SerialNo, NoSpaceSize)
                , Currencty
                , CalSpace(Currencty, SpaceSize)
                , SpotBuying
                , CalSpace(SpotBuying, SpaceSize)
                , SpotSelling
                , CalSpace(SpotSelling, SpaceSize)
                , CashBuying
                , CalSpace(CashBuying, SpaceSize)
                , CashSelling
                , CalSpace(CashSelling, SpaceSize));

            Console.WriteLine(Title);

            foreach (var item in Result.Data.Select((value, index) => new { index, value}))
            {
                byte[] byteStr = Encoding.GetEncoding("big5").GetBytes(item.value.Currencty);
                Console.WriteLine(
                    string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11} \r\n"
                    , (item.index+1).ToString().PadLeft(2,'0')
                    , CalSpace((item.index + 1).ToString().PadLeft(2, '0'), NoSpaceSize)
                    , item.value.Currencty
                    , CalSpace(item.value.Currencty, SpaceSize)
                    , item.value.SpotBuying
                    , CalSpace(item.value.SpotBuying, SpaceSize)
                    , item.value.SpotSelling
                    , CalSpace(item.value.SpotSelling, SpaceSize)
                    , item.value.CashBuying
                    , CalSpace(item.value.CashBuying, SpaceSize)
                    , item.value.CashSelling
                    , CalSpace(item.value.CashSelling, SpaceSize)
                    ));
            }
            Console.WriteLine("--------------------------------------------------------------------------------------------\r\n");
        }

        public static void ShowEnumList()
        {
            List<EnumBank> EnumList = Program.EnumToList<EnumBank>();
            string BankList = "請輸入要查詢的銀行代號\r\n";
            foreach (var item in EnumList.Select((value, index) => new { value, index }))
            {
                if (item.index % 2 != 0)
                {
                    BankList += item.value + "(" + string.Format("{0:000}", (int)item.value) + ")\r\n";
                }
                else
                {
                    BankList += item.value + "(" + string.Format("{0:000}", (int)item.value) + ")   ";
                }
            }

            Console.WriteLine(BankList);
            Console.Write("請輸入代號：");
        }

        public static string CalSpace(string CalStr, int SpaceSize)
        {
            return "".PadLeft(SpaceSize - Encoding.GetEncoding("big5").GetBytes(CalStr).Length, ' ');
        }

        public static List<T> EnumToList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList<T>();
        }

        public static IEnumerable<T> EnumToEnumerable<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
