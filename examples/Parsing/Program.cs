using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBookee;

namespace Parsing
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("usage: parsing EBOOK_FILENAME");
                Environment.Exit(-1);
            }
            var ebook = new EBook(args[0]);
            Console.WriteLine("Ebook: {0}", ebook.Title);
            foreach (var contentSection in ebook.ContentSections)
            {
                Console.WriteLine(" - {0} {1} has {2} letters of text", contentSection.Id, contentSection.Name, contentSection.AsText.Length);
            }
        }
    }
}
