using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System;
namespace doopxml
{
   public static class Program
    {
        static void Main(string[] args)
        {
            ListFilesInDirectory(@"C:\Users\AGarige\OneDrive - Prolifics Corporation Ltd.,\Desktop\campusreport");
            Console.Read();
        }
        public static void ListFilesInDirectory(string workingDirectory)
        {
            string[] filePaths = Directory.GetFiles(workingDirectory);
            foreach (string filePath in filePaths)
            {
                XDocument xdoc = XDocument.Load(filePath);
                XNamespace ns = XNamespace.Get("http://schemas.microsoft.com/sqlserver/reporting/2010/01/reportdefinition");
                var result = from name in xdoc.Descendants(ns+"DataSet").Descendants(ns+"Query")
                         select new
                         {
                             name = name.Element(ns+"CommandText").Value
                         };
                foreach (var lv in result)
                {
                    Console.WriteLine(lv.name);
                    if(lv.name!=null)
                    {
                        var lv1s = from lv1 in xdoc.Descendants(ns+"DataSet")  where lv1.Descendants(ns+"Query").Any()
                        select lv1.Attribute("Name").Value;
                        foreach (var lv1 in lv1s)
                        {
                            // Write each CommandText Value to a file.
                            using (StreamWriter sw = new StreamWriter($"{lv1}.txt"))
                            {
                                sw.WriteLine(lv.name);
                            }
                        }
                    }
                    else
                    {
                       Console.WriteLine("Error occured while processing the file");
                    }
                }
            }
        }
    }
}