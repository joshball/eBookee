using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBookee
{
    public class EBook
    {
        public EBook(string ebookFile)
        {
            this.ContentSections = new List<ContentSection>();
        }

        public IEnumerable<ContentSection> ContentSections { get; set; }
        public string Title { get; set; }
    }
}
