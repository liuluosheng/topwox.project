using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using Newtonsoft.Json.Converters;

namespace X.Data.Attributes.Shema
{

    /// <summary>
    /// 上传控件UI设定
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class UploadAttribute : Attribute
    {
        public UploadAttribute()
        {
            ButtonText = "选择文件";
        }

        /// <summary>
        /// 按钮文本
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ButtonText { get; set; }
        /// <summary>
        /// 允许上传的文件类型，如：image/png,image/jpeg
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FileType { get; set; }
        /// <summary>
        /// 服务端路径
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Action { get; set; }

        /// <summary>
        /// 是否可上传多文件
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Multiple { get; set; }

    }

}

