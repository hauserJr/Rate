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
    public static class Rate
    {
        private static List<DataMeta> Table = new List<DataMeta>();
        public static DataMeta GetRate(this Url BankUrl, EnumBank EnumBank)
        {
            try
            {
                var IsSearch = Table.Expires(EnumBank);
                if (IsSearch)
                {
                    var Result = BankUrl.XPathExpression(EnumBank);
                    Table.Add(Result);
                }
            }
            catch
            {

            }
            return Table.Where(o => o.Key == EnumBank).FirstOrDefault();
        }


        private static HtmlNode GetHtmlNode(this Url Url)
        {
            var Doc = Url.GetHtmlDocument();
            if (Doc != null)
                return Doc.DocumentNode;
            else
                return null;
        }

        private static HtmlDocument GetHtmlDocument(this Url Url)
        {
            try
            {
                using (var Client = new System.Net.WebClient())
                {
                    byte[] HtmlBytes = Client.DownloadData(Url.Value);
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

        private static DataMeta XPathExpression(this Url BankUrl, EnumBank EnumBank)
        {
            var HtmlNode = BankUrl.GetHtmlNode();

            var Result = 
                EnumBank.台新銀行 == EnumBank ?
                HtmlNode.SelectNodes(TaiShin.XPathExpression).TaiShin_GetRate(EnumBank) :
                EnumBank.台灣銀行 == EnumBank ?
                HtmlNode.SelectNodes(TaiwanBK.XPathExpression).TaiwanBK_GetRate(EnumBank) :
                new DataMeta();

            return Result;
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
    }
}
