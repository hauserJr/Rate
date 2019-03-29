using Rate.Lib.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rate.Lib.Models
{
    public class DataMeta
    {

        /// <summary>
        /// Key
        /// </summary>
        public EnumBank Key { get; set; }

        /// <summary>
        /// 資料產生時間
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 資料有效時間
        /// </summary>
        public DateTime? Expire { get; set; }

        public List<RateData> Data { get; set; } = new List<RateData>();
    }

    public class RateData
    {

        public string Currencty { get; set; }


        /// <summary>
        /// 即期買入
        /// </summary>
        public string SpotBuying { get; set; }


        /// <summary>
        /// 即期賣出
        /// </summary>
        public string SpotSelling { get; set; }

        /// <summary>
        /// 現鈔買入
        /// </summary>
        public string CashBuying { get; set; }


        /// <summary>
        /// 現鈔賣出
        /// </summary>
        public string CashSelling { get; set; }

    }
}
