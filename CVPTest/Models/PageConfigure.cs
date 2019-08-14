using System;
namespace CVPTest.Models
{
    public class PageConfigure
    {
        public PageConfigure()
        {
        }

        public Boolean IsHome { get; set; } = false;
        public Boolean IsUpload { get; set; } = false;

        public string HomeStatus
        {
            get
            {
                if (IsHome) return "active";
                return "";
            }
        }
        public string UploadStatus
        {
            get
            {
                if (IsUpload) return "active";
                return "";
            }
        }

        public string HomeClickable
        {
            get
            {
                if (IsHome) return "disabled";
                return "";
            }
        }
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
