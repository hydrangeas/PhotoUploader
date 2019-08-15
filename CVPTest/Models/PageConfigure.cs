using System;
namespace CVPTest.Models
{
    /// <summary>
    /// _Layoutの各要素を制御する
    /// </summary>
    public class PageConfigure
    {
        public PageConfigure()
        {
        }

        public Boolean IsHome { get; set; } = false;
        public Boolean IsUpload { get; set; } = false;

        /// <summary>
        /// Homeにいるときにメニューを白文字にしておく
        /// </summary>
        public string HomeStatus
        {
            get
            {
                if (IsHome) return "active";
                return "";
            }
        }
        /// <summary>
        /// Uploadにいるときにメニューを白文字にしておく
        /// </summary>
        public string UploadStatus
        {
            get
            {
                if (IsUpload) return "active";
                return "";
            }
        }

        /// <summary>
        /// Homeにいるときにクリックできないようにする
        /// </summary>
        public string HomeClickable
        {
            get
            {
                if (IsHome) return "disabled";
                return "";
            }
        }

        /// <summary>
        /// Uploadにいるときにクリックできないようにする
        /// </summary>
        public string UploadClickable
        {
            get
            {
                if (IsUpload) return "disabled";
                return "";
            }
        }
    }
}
