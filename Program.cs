﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            /*List<HtmlDocument> parseDocs = new List<HtmlDocument>();
            for (int i = 0; i < args.Length; ++i)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(args[i]);
                parseDocs.Add(doc);
            }*/
            List<HtmlDocument> parseDocs = new List<HtmlDocument>();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load("C:\\Users\\Matthew\\Desktop\\TestCHM\\DeleteUser.html");
            parseDocs.Add(htmlDoc);
            foreach (HtmlDocument hDoc in parseDocs)
              {
                  ParsedCHM parsed = new ParsedCHM(hDoc);
                //parsed.sendOff();
                //parsed.print();
              }
            /*HtmlDocument doc = new HtmlDocument();
            doc.Load("DeleteUser.html");
            var root = doc.DocumentNode.Descendants();
            foreach(HtmlNode hNode in root)
            {
                Console.WriteLine(hNode.Name);
            }*/
                while (true) {; }
        }
    }
}
