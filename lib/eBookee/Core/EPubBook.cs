using System.Collections.Generic;

namespace eBookee.Core
{
    public class EPubBook : IEBook
    {
        public IEnumerable<ContentSection> ContentSections { get; set; }
        public EBookFile EBookFile { get; set; }
        public string Title { get; set; }


        public EPubBook()
        {
            this.ContentSections = new List<ContentSection>();
        }

        public EPubBook(EBookFile eBookFile) : this()
        {
            EBookFile = eBookFile;
        }

    }
}