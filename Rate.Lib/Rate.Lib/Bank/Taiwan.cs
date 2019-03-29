using Rate.Lib.Enum;
using Rate.Lib.Models;
using System;
using HtmlAgilityPack;
using System.Linq;
namespace Rate.Lib.Bank
{
    public static class Taiwan
    {
        private readonly static int Expire = 15;
        /// <summary>
        /// XPath的條件
        /// </summary>
        public static string XPathExpression
        {
            get
            {
                return @"//tbody/tr";
            }
        }

        public static DataMeta TaiwanBK_GetRate(this HtmlNodeCollection Node, EnumBank EnumBank)
        {
            DataMeta Table = new DataMeta();
            RateData TempTable = new RateData();
            Table.CreateDate = DateTime.Now;
            Table.Expire = DateTime.Now.AddMinutes(Expire);
            Table.Key = EnumBank;

            //int LIndex = (int)EnumRate.幣別;
            foreach (var item in Node.Select((value,index) => new { index, value}))
            {
                Table.Data.Add(new RateData()
                {
                    Currencty = item.value.SelectNodes(@"//div[@class=""hidden-phone print_show""]")[item.index].InnerText
                                .Replace("\r\n", "")
                                .Replace("              ", "")
                                .Replace(" ", "")
                                .Replace("(", "")
                                .Replace(")", "")
                                .TrimStart(' ').TrimEnd(' ').Replace("\r\n", ""),
                    CashBuying = item.value.SelectNodes(@"//td[@data-table=""本行現金買入""]")[item.index].InnerText,
                    CashSelling = item.value.SelectNodes(@"//td[@data-table=""本行現金賣出""]")[item.index].InnerText,
                    SpotBuying = item.value.SelectNodes(@"//td[@data-table=""本行即期買入""]")[item.index].InnerText,
                    SpotSelling = item.value.SelectNodes(@"//td[@data-table=""本行即期賣出""]")[item.index].InnerText
                });
            }
            return Table;
        }
    }
}
