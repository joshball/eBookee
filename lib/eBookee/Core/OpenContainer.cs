using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace eBookee.Core
{
    public class ArchiveEntry
    {
        public ArchiveEntry(ZipArchiveEntry zipArchiveEntry, string contents)
        {
            this.Name = zipArchiveEntry.Name;
            this.FullName = zipArchiveEntry.FullName;
            this.Contents = contents;
        }

        public string Name { get; set; }
        public string FullName { get; set; }
        public string Contents { get; set; }
    }

    public class OpenContainer
    {
        public string FileName { get; set; }
        // must be a text document in ASCII that contains the string 'application/epub+zip' and FIRST file in zip archive
        public string Mimetype { get; set; }
        public string ContainerXml { get; set; }
        public string RootFile { get; set; }

        public Dictionary<string, ArchiveEntry> ArchiveEntries { get; set; }


        public OpenContainer(string fileName)
        {
            FileName = fileName;
            ArchiveEntries = new Dictionary<string, ArchiveEntry>();

            using (var zipToOpen = new FileStream(fileName, FileMode.Open))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    Console.WriteLine("Archive has {0} entries", archive.Entries.Count);
                    foreach (var zipArchiveEntry in archive.Entries)
                    {
                        using (var stream = zipArchiveEntry.Open())
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            var contents = reader.ReadToEnd();
                            var ae = new ArchiveEntry(zipArchiveEntry, contents);
                            ArchiveEntries.Add(zipArchiveEntry.FullName, ae);
                        }
                    }
                }
            }
            ParseContainerXml();
            ParseRootFile(this.RootFile);
        }

        // 
        // Parse the META-INF/container.xml file
        //
        //  <?xml version = '1.0' encoding='utf-8'?>
        //  <container xmlns = "urn:oasis:names:tc:opendocument:xmlns:container" version="1.0">
        //    <rootfiles>
        //      <rootfile media-type="application/oebps-package+xml" full-path="38233/content.opf"/>
        //    </rootfiles>
        //  </container>

        public void ParseContainerXml()
        {
            var ae = ArchiveEntries["META-INF/container.xml"];
            
            var doc = new XmlDocument();
            doc.LoadXml(ae.Contents);

            if (doc.DocumentElement == null)
            {
                throw new Exception("XML Null");
            }
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("a", "urn:oasis:names:tc:opendocument:xmlns:container");

            var rootfiles = doc.SelectNodes("//a:rootfiles/a:rootfile", nsmgr);
            if (rootfiles == null || rootfiles.Count < 1)
            {
                Console.WriteLine(ae.Contents);
                throw new Exception("container/rootfiles should have at least one rootfile!");
            }
            if (rootfiles.Count != 1)
            {
                Console.WriteLine(ae.Contents);
                Console.WriteLine("WARNING! Multiple rootfiles, not sure how to handle this yet... please file a bug and include the epub!");
//                throw new Exception("container/rootfiles should have only one rootfile");
            }
            var rootfile = rootfiles[0];
            if (rootfile.Attributes == null)
            {
                Console.WriteLine(ae.Contents);
                throw new Exception("rootfile has no attributes!");
            }
            var rootfileMediaType = rootfile.Attributes["media-type"];
            if (rootfileMediaType == null)
            {
                Console.WriteLine(ae.Contents);
                Console.WriteLine("Rootfile has no media type!");
            }
            if (rootfileMediaType != null && rootfileMediaType.Value != "application/oebps-package+xml")
            {
                Console.WriteLine(ae.Contents);
                Console.WriteLine("Rootfile has unknown media type: {0}", rootfileMediaType.Value);
            }
            var rootfileFullPath = rootfile.Attributes["full-path"];
            if (rootfileFullPath == null)
            {
                Console.WriteLine(ae.Contents);
                throw new Exception("rootfile has not full-path attribute!");
            }
            this.RootFile = rootfileFullPath.Value;
        }

        public void ParseRootFile(string rootFile)
        {
            Console.WriteLine("Parsing RootFile: {0}", rootFile);
            var ae = ArchiveEntries[rootFile];

            var doc = new XmlDocument();
            doc.LoadXml(ae.Contents);

            if (doc.DocumentElement == null)
            {
                throw new Exception("XML Null");
            }
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("a", "http://www.idpf.org/2007/opf");

            var metaData = doc.SelectNodes("//a:metadata", nsmgr);
            if (metaData == null || metaData.Count < 1)
            {
                Console.WriteLine(ae.Contents);
                throw new Exception("container/rootfiles should have at least one rootfile!");
            }
//            if (rootfiles.Count != 1)
//            {
//                Console.WriteLine(ae.Contents);
//                Console.WriteLine("WARNING! Multiple rootfiles, not sure how to handle this yet... please file a bug and include the epub!");
//                //                throw new Exception("container/rootfiles should have only one rootfile");
//            }
//            var rootfile = rootfiles[0];
//            if (rootfile.Attributes == null)
//            {
//                Console.WriteLine(ae.Contents);
//                throw new Exception("rootfile has no attributes!");
//            }
//            var rootfileMediaType = rootfile.Attributes["media-type"];
//            if (rootfileMediaType == null)
//            {
//                Console.WriteLine(ae.Contents);
//                Console.WriteLine("Rootfile has no media type!");
//            }
//            if (rootfileMediaType != null && rootfileMediaType.Value != "application/oebps-package+xml")
//            {
//                Console.WriteLine(ae.Contents);
//                Console.WriteLine("Rootfile has unknown media type: {0}", rootfileMediaType.Value);
//            }
//            var rootfileFullPath = rootfile.Attributes["full-path"];
//            if (rootfileFullPath == null)
//            {
//                Console.WriteLine(ae.Contents);
//                throw new Exception("rootfile has not full-path attribute!");
//            }
//            this.RootFile = rootfileFullPath.Value;
        }


    }
}