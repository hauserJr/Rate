using HtmlAgilityPack;
using Rate.Lib.Bank;
using Rate.Lib.Enum;
using Rate.Lib.Models;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Linq;
namespace Rate.Lib
{
    public static class BankRate
    {
        private static List<DataMeta> Table = new List<DataMeta>();

        /// <summary>
        /// 取得匯率
        /// </summary>
        /// <param name="BankUrl">匯率URL</param>
        /// <param name="EnumBank">銀行類別</param>
        /// <returns></returns>
        public static DataMeta GetRate(EnumBank EnumBank)
        {
            try
            {
                var IsSearch = Table.Expires(EnumBank);
                if (IsSearch)
                {
                    var Result = GetRateUrl(EnumBank).XPathExpression(EnumBank);
                    Table.Add(Result);
                }
            }
            catch
            {
                return null;
            }
            return Table.Where(o => o.Key == EnumBank).FirstOrDefault();
        }

        #region Helper

        private static DataMeta XPathExpression(this Url BankUrl, EnumBank EnumBank)
        {
            var HtmlNode = BankUrl.GetHtmlNode();

            var Result =
                EnumBank.台新銀行 == EnumBank ?
                HtmlNode.SelectNodes(Taishin.XPathExpression).Taishin_GetRate(EnumBank) :
                EnumBank.臺灣銀行 == EnumBank ?
                HtmlNode.SelectNodes(Taiwan.XPathExpression).TaiwanBK_GetRate(EnumBank) :
                new DataMeta();

            return Result;
        }

        private static HtmlNode GetHtmlNode(this Url BankUrl)
        {
            var Doc = BankUrl.GetHtmlDocument();
            if (Doc != null)
                return Doc.DocumentNode;
            else
                return null;
        }

        private static HtmlDocument GetHtmlDocument(this Url BankUrl)
        {
            try
            {
                using (var Client = new System.Net.WebClient())
                {
                    byte[] HtmlBytes = Client.DownloadData(BankUrl.Value);
                    if (HtmlBytes != null && HtmlBytes.Length != 0)
                    {
                        string HtmlStr = Encoding.UTF8.GetString(HtmlBytes);
                        if (!string.IsNullOrEmpty(HtmlStr))
                        {
                            var HtmlDoc = new HtmlAgilityPack.HtmlDocument();
                            HtmlDoc.LoadHtml(HtmlStr);
                            return HtmlDoc;
                        }
                    }
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private static bool Expires(this List<DataMeta> Data, EnumBank EnumBank)
        {
            var Result = Data.Where(o => o.Key == EnumBank).FirstOrDefault();
            if (Result == null)
            {
                return true;
            }

            if (Result.Expire <= DateTime.Now)
            {
                Data.Remove(Result);
                return true;
            }

            return false;
        }

        private static Url GetRateUrl(this EnumBank EnumBank)
        {
            string UrlStr = string.Empty;
            switch (EnumBank)
            {
                case EnumBank.臺灣銀行:
                    UrlStr = @"https://rate.bot.com.tw/xrt/all/day";
                    break;
                case EnumBank.台新銀行:
                    UrlStr = @"https://www.taishinbank.com.tw/TS/TS06/TS0605/TS060502/index.htm?urlPath1=TS02&urlPath2=TS0202";
                    break;
                default:
                    EnumBank = EnumBank.臺灣銀行;
                    UrlStr = @"https://rate.bot.com.tw/xrt/all/day";
                    break;
            }
            return new Url(UrlStr);
        }
        #endregion

    }
}
