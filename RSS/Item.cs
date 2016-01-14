using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RSS
{
    public class Item
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string Guid { get; set; }

        public string PubDate { get; set; }
     
        public List<author> Authors = new List<author>();
        
        public bool Read { get; set; }

        public Subscription ParentSubscription { get; set; }

        public bool IsDownloaded { get; set; }
        
        public bool CanBeDownloaded 
		{
			get
			{
				return !String.IsNullOrEmpty (Link) && !String.IsNullOrEmpty (Path.GetExtension (Link));
			}
		}
        
        public int MbSize;

        public string FilePath { get; set; }

        public int RowNum;

        public delegate void DownloadProgressEvent(string MbString, float percent, int row);

        public delegate void DownloadCompleteEvent(int row, int size);

        public event DownloadCompleteEvent DownloadComplete;

        public event DownloadProgressEvent DownloadProgress;

		public Item()
		{
			Title = string.Empty;
			Description = string.Empty;
			Link = string.Empty;
			Guid = string.Empty;
			PubDate = string.Empty;
			FilePath = string.Empty;
		}

        public void DownloadFile()
        {
            if (!String.IsNullOrEmpty(Link))
            {
                if (!Directory.Exists(RSSConfig.DownloadFolder))
                {
                    Directory.CreateDirectory(RSSConfig.DownloadFolder);
                }

                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += client_DownloadProgressChanged;
                    client.DownloadFileCompleted += client_DownloadFileCompleted;
                    client.DownloadFileAsync(new Uri(Link), FilePath);
                }
            }
        }


        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IsDownloaded = true;
         
            ((WebClient)sender).DownloadProgressChanged -= client_DownloadProgressChanged;
            ((WebClient)sender).DownloadFileCompleted -= client_DownloadFileCompleted;

            DownloadProgressEvent progressCopy = DownloadProgress;
            DownloadCompleteEvent completeCopy = DownloadComplete;
            if (completeCopy != null)
            {
                completeCopy(RowNum, MbSize);

                DownloadComplete -= completeCopy;
                DownloadProgress -= progressCopy;
            }

        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MbSize = (int)(e.TotalBytesToReceive / (1024 * 1024));
            string MbString = (e.BytesReceived / (1024 * 1024)).ToString() + "/" + (e.TotalBytesToReceive / (1024 * 1024)).ToString() + "Mb";
            
            DownloadProgressEvent copy = DownloadProgress;
            if (copy != null)
            {
                copy(MbString, e.ProgressPercentage, RowNum);
            }
        }

        private string GetFileType()
        {
            string type = "";
            string[] strArr = Link.Split('.');
            if (strArr.Length > 0)
            {
                type = "." + strArr[strArr.Length - 1];
            }
            return type;
        }

        private string GetCleanFileName()
        {
            string filename = Title;
            string cleanFileName = "";
            if (!string.IsNullOrEmpty(filename))
            {
                cleanFileName = filename.Replace(":", "").Replace("\\", "").Replace("/", "");
            }
            return cleanFileName;
        }



        public bool CheckIsDownloaded()
        {
            bool isDownloaded = false;

            string[] files = Directory.GetFiles(RSSConfig.DownloadFolder);
            foreach (string str in files)
            {
                string cleanFileName = GetCleanFileName();
                if (str.Contains(cleanFileName))
                {
                    FilePath = str;
                    isDownloaded = true;
                    break;
                }
            }
            IsDownloaded = isDownloaded;
            return IsDownloaded;
        }

        public int GetFileSizeMb()
        {
            int size = 0;
            if (File.Exists(FilePath))
            {
                FileInfo info = new FileInfo(FilePath);
                size = (int)(info.Length / (1024 * 1024));
            }
            MbSize = size;
            return MbSize;
        }


        public bool DeleteFile()
        {
            bool success = false;
            string url = Link;
            if (CanBeDownloaded)
            {
                if (File.Exists(FilePath))
                {
                    File.SetAttributes(FilePath, FileAttributes.Normal);
                    File.Delete(FilePath);
                }

                success = !File.Exists(FilePath);
                if (success)
                {
                    MbSize = 0;
                }
            }
            return success;
        }

        public void CalculateFilePath()
        {
            FilePath = RSSConfig.DownloadFolder + GetCleanFileName() + GetFileType();
        }

    }

    public class author
    {
        public string name;
     
        public string email;
    }
}

