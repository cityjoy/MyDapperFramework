using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDapperFramework.Repository.Entity
{
   public class XPager
   { 
       /// <summary>
       /// 
       /// </summary>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <param name="recordCount"></param>
       public XPager(int pageIndex, int pageSize, int recordCount)
       {
           this.PageIndex = pageIndex < 1 ? 1 : pageIndex;
           this.PageSize = pageSize < 1 ? 10 : pageSize;
           this.RecordCount = recordCount;
       }

       /// <summary>
       /// 页码表唯一标识
       /// </summary>
       public string ID
       {
           get;
           set;
       }

       private int _pageIndex = 1;
       /// <summary>
       /// 第几页
       /// </summary>
       public int PageIndex
       {
           get { return _pageIndex; }
           set { _pageIndex = value; }
       }

       private int _pageSize = 10;
       /// <summary>
       /// 每页数量
       /// </summary>
       public int PageSize
       {
           get { return _pageSize; }
           set { _pageSize = value; }
       }

       /// <summary>
       /// 总数量
       /// </summary>
       public int RecordCount
       {
           get;
           set;
       }


       private int _pagerNumber = 5;
       /// <summary>
       /// 页码显示数
       /// </summary>
       public int PagerNumber
       {
           get { return _pagerNumber; }
           set { _pagerNumber = value; }
       }

       private string _urlFormat = string.Empty;
       /// <summary>
       /// URL地址格式
       /// </summary>
       public string UrlFormat
       {
           get { return _urlFormat; }
           set { _urlFormat = value; }
       }

       /// <summary>
       /// 锚点名称
       /// </summary>
       public string Anchor
       {
           get;
           set;
       }

       /// <summary>
       /// 统计信息内容格式
       /// </summary>
       public string CountInfo
       {
           get;
           set;
       }

       /// <summary>
       /// 无数据信息
       /// </summary>
       public string NoDataInfo
       {
           get;
           set;
       }

       private int? _totalPage = null;
       /// <summary>
       /// 总页数
       /// </summary>
       public int TotalPage
       {
           get
           {
               if (_totalPage == null)
               {
                   _totalPage = GetTotalPage();
               }
               return _totalPage.Value;
           }
           private set { _totalPage = value; }
       }

       private int? _startPage = null;
       /// <summary>
       /// 开始页码
       /// </summary>
       public int StartPage
       {
           get
           {
               if (_startPage == null)
               {
                   _startPage = GetStartPage();
               }
               return _startPage.Value;
           }
           set { _startPage = value; }
       }

       private int? _endPage = null;
       /// <summary>
       /// 结束页码
       /// </summary>
       public int EndPage
       {
           get
           {
               if (_endPage == null)
               {
                   _endPage = GetEndPage();
               }
               return _endPage.Value;
           }
           set { _endPage = value; }
       }

       private string _prevUrl;
       /// <summary>
       /// 上一页URL
       /// </summary>
       public string PrevUrl
       {
           get
           {
               if (_prevUrl == null)
               {
                   _prevUrl = GetPrevUrl();
               }
               return _prevUrl;
           }
           private set { _prevUrl = value; }
       }

       private string _nextUrl;
       /// <summary>
       /// 下一页URL
       /// </summary>
       public string NextUrl
       {
           get
           {
               if (_nextUrl == null)
               {
                   _nextUrl = GetNextUrl();
               }
               return _nextUrl;
           }
           private set { _nextUrl = value; }
       }

       /// <summary>
       /// Javascript函数名
       /// </summary>
       public string JsMethod
       {
           get;
           set;
       }


       /// <summary>
       /// 获取总页数
       /// </summary>
       /// <returns></returns>
       public int GetTotalPage()
       {
           int totalPage = 0;
           if (this.RecordCount < 0)
           {
               totalPage = 0;
           }
           else
           {
               totalPage = Convert.ToInt32(Math.Floor((decimal)(this.RecordCount / this.PageSize)));
               if (this.RecordCount % this.PageSize != 0)
               {
                   totalPage += 1;
               }
           }
           return totalPage;
       }


       /// <summary>
       /// 获取开始页码
       /// </summary>
       /// <returns></returns>
       public int GetStartPage()
       {
           int totalPage = this.TotalPage;
           if (this.PageIndex > totalPage)
           {
               this.PageIndex = totalPage;
           }
           if (this.PageIndex < 1)
           {
               this.PageIndex = 1;
           }

           int leftRange = Convert.ToInt32(Math.Floor((decimal)(this.PagerNumber / 2)));
           int rightRange = this.PagerNumber - leftRange;
           int startPage = this.PageIndex - leftRange;
           int endPage = this.PageIndex + rightRange;

           if (endPage > totalPage)
           {
               startPage = startPage - rightRange;
           }

           if (startPage < 1)
           {
               startPage = 1;
           }
           else if (startPage > totalPage)
           {
               startPage = totalPage;
           }
           return startPage;
       }

       /// <summary>
       /// 获取结束页码
       /// </summary>
       /// <returns></returns>
       public int GetEndPage()
       {
           int totalPage = this.TotalPage;
           if (this.PageIndex > totalPage)
           {
               this.PageIndex = totalPage;
           }
           if (this.PageIndex < 1)
           {
               this.PageIndex = 1;
           }

           int leftRange = Convert.ToInt32(Math.Floor((decimal)(this.PagerNumber / 2)));
           int rightRange = this.PagerNumber - leftRange;
           int startPage = this.PageIndex - leftRange;
           int endPage = this.PageIndex + rightRange;

           if (startPage < 1)
           {
               endPage = endPage + leftRange;
           }

           if (endPage < 1)
           {
               endPage = 1;
           }
           else if (endPage > totalPage)
           {
               endPage = totalPage;
           }
           return endPage;
       }

       /// <summary>
       /// 获取URL
       /// </summary>
       /// <param name="pageIndex"></param>
       /// <returns></returns>
       public string GetUrl(int pageIndex)
       {
           string url = string.Format(this.UrlFormat, pageIndex);
           if (!string.IsNullOrEmpty(this.Anchor))
           {
               url = string.Concat(url, "#", this.Anchor);
           }
           return url;
       }

       /// <summary>
       /// 获取上一页URL
       /// </summary>
       /// <returns></returns>
       public string GetPrevUrl()
       {
           int prevPageIndex = this.PageIndex - 1;
           if (prevPageIndex <= 0)
           {
               prevPageIndex = 1;
           }
           return this.GetUrl(prevPageIndex);
       }

       /// <summary>
       /// 获取下一页URL
       /// </summary>
       /// <returns></returns>
       public string GetNextUrl()
       {
           int nextPageIndex = this.PageIndex + 1;
           int totalPage = this.TotalPage;
           if (nextPageIndex >= totalPage)
           {
               nextPageIndex = totalPage;
           }
           return this.GetUrl(nextPageIndex);
       }


   }
}
