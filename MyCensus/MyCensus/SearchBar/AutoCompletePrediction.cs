//
// AutoCompletePrediction.cs
//
// Author:
//       Alex Smith <alex@duriancode.com>
//
// Copyright (c) 2017 (c) Alexander Smith
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Newtonsoft.Json;

namespace MyCensus.PlacesSearchBar
{
	/// <summary>
	/// Auto complete prediction.
	/// </summary>
	public class AutoCompletePrediction
    {
        [JsonProperty("brand")]
        /// <summary>
        /// 品牌(终端所销售的啤酒品牌)青岛啤酒、雪花啤酒、Budweiser百威、其他
        /// </summary>
        public string Brand { get; set; }


        [JsonProperty("productname")]

        /// <summary>
        ///  产品名称(扫码+手输)
        /// </summary>
        public string ProductName { get; set; }


        [JsonProperty("annualsales")]
        /// <summary>
        ///  年销量（填写整数 年/箱）4大类品牌（青岛啤酒、雪花啤酒、Budweiser百威、其他）分为四档的容量，高档及以上，中档及中档高，中档低、其他
        /// </summary>
        public string AnnualSales { get; set; }


        [JsonProperty("packingform")]
        /// <summary>
        ///  包装形式(瓶、听、桶)(所售啤酒产品的最小包装形式)
        /// </summary>
        public string PackingForm { get; set; }


        [JsonProperty("productprovider")]
        /// <summary>
        /// 产品供货商
        /// </summary>
        public string ProductProvider { get; set; }


        [JsonProperty("channelattributes")]
        /// <summary>
        /// 渠道属性((一批、二批、其他)（销售啤酒产品给此终端的渠道商在其代理的啤酒产品渠道中的层级，包括一批、二批和其他（一批、二批之外））
        /// </summary>
        public string ChannelAttributes { get; set; }



        [JsonProperty("specification")]
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }


        [JsonProperty("barcode")]
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }


        [JsonProperty("grade")]
        /// <summary>
        /// 档次
        /// </summary>
        public string Grade { get; set; }


        public string Detalls { get; set; }
    }

}
