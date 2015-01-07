using System;
using System.IO;

namespace eBookee.Core
{
    public class EBookFile
    {
        public string FileName { get; set; }
        public string FullFilePath { get; set; }
        public string DirectoryName { get; set; }
        public string BaseFileName { get; set; }
        public string FileExtension { get; set; }
        public EBookType EBookType { get; set; }

        public EBookFile(string fileName)
        {
            SetFileInfo(fileName);
        }

        protected EBookType ParseType(string extension)
        {
            switch (extension.ToLower())
            {
                case "epub":
                    return EBookType.EPub;
                case "txt":
                    return EBookType.Text;
                case "mobi":
                    return EBookType.Mobi;
                case "pdf":
                    return EBookType.PDF;
                default:
                    throw new Exception(string.Format("Unknown EBook Extension: {0}", extension));
            }
        }

        protected void SetFileInfo(string fileName)
        {
            var epubFileInfo = new FileInfo(fileName);

            if (!epubFileInfo.Exists)
            {
                throw new Exception(String.Format("eBook file {0} does not exist", fileName));
            }

            if (epubFileInfo.Directory == null)
            {
                throw new Exception(String.Format("EPUB file {0} does not exist in a directory???", fileName));
            }

            var epubFileBaseName = Path.GetFileNameWithoutExtension(epubFileInfo.Name);
            if (epubFileBaseName == null)
            {
                throw new Exception(String.Format("EPUB file {0} must have an extension", fileName));
            }


            this.FileName = epubFileInfo.Name;
            this.FullFilePath = epubFileInfo.FullName;
            this.DirectoryName = epubFileInfo.DirectoryName;
            this.BaseFileName = epubFileBaseName;
            this.FileExtension = epubFileInfo.Extension;
            this.EBookType = ParseType(epubFileInfo.Extension);

            if (EBookType != EBookType.EPub)
            {
                throw new Exception("eBooke only handles epubs for now");
            }

        }
    }
}