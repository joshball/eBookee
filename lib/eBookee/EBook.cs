using System;
using System.Collections.Generic;
using eBookee.Core;

namespace eBookee
{
    public interface IEBook
    {
        IEnumerable<ContentSection> ContentSections { get; set; }
        string Title { get; set; }
    }

    public class EBookFactory
    {
        public static IEBook ParseEBook(string eBookFileName)
        {
            var eBookFile = new EBookFile(eBookFileName);
            if (eBookFile.EBookType == EBookType.EPub)
            {
                return new EPubBook(eBookFile);
            }
            throw new Exception("We can only handle epubs for now");

        }
    }

}
