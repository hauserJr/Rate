using Rate.Lib.Enum;
using Rate.Lib.Models;
using System;
using HtmlAgilityPack;
namespace Rate.Lib.Bank
{
    public static class Taishin
    {
        private readonly static int Expire = 15;
        /// <summary>
        /// XPath的條件
        /// </summary>
        public static string XPathExpression
        {
            get
            {
                return @"//tbody/tr[@bgcolor=""#FFFFFF""]/td[@class=""size13nocolor""]";
            }
        }

        public static DataMeta Taishin_GetRate(this HtmlNodeCollection Node, EnumBank EnumBank)
        {
            DataMeta Table = new DataMeta();
            RateData TempTable = new RateData();
            Table.CreateDate = DateTime.Now;
            Table.Expire = DateTime.Now.AddMinutes(Expire);
            Table.Key = EnumBank;

            int LIndex = (int)EnumRate.幣別;
            foreach (var item in Node)
            {
                if (LIndex == (int)EnumRate.幣別)
                {
                    TempTable = new RateData();
                }
                switch (LIndex)
                {
                    case (int)EnumRate.幣別:
                        TempTable.Currencty = item.InnerText;
                        break;
                    case (int)EnumRate.即期買入:
                        TempTable.SpotBuying = item.InnerText;
                        break;
                    case (int)EnumRate.即期賣出:
                        TempTable.SpotSelling = item.InnerText;
                        break;
                    case (int)EnumRate.現鈔買入:
                        TempTable.CashBuying = item.InnerText;
                        break;
                    case (int)EnumRate.現鈔賣出:
                        TempTable.CashSelling = item.InnerText;
                        break;
                }
                if (LIndex == (int)EnumRate.現鈔賣出)
                {
                    Table.Data.Add(TempTable);
                }
                LIndex = LIndex == (int)EnumRate.現鈔賣出 ? (int)EnumRate.幣別 : LIndex + 1;
            }
            return Table;
        }
    }
}
