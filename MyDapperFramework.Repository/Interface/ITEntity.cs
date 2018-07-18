using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDapperFramework.Repository.Interface
{
    /// <summary>
    /// DB实体接口
    /// </summary>
    public interface ITEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? CreateTime { get; set; }

        ///// <summary>
        ///// 数据状态
        ///// </summary>
        //int DataState { get; set; }

    }


    /// <summary>
    /// 数据状态
    /// </summary>
    [Flags]
    public enum DataState : int
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0, //使用位标志后这里不要设置为0x0
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Deleted = 1,

        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Ban = 2
    }

}
