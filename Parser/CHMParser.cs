﻿
////maybe store all these private methods in a helper module
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Parser
{
    public class ParsedCHM
    {
        private List<List<Element>> blocks = new List<List<Element>>(); //breadth first from root
        private List<Element> buff = new List<Element>();
        private List<string> hrefs = new List<string>();
        private string title;

        //HtmlNode hNode
        //checks if hNode represents a StepsBlock
        //return: bool
        private bool isStepsBlock(HtmlNode hNode)
        {
            Regex regex = new Regex("^[1-9].*$"); //TODO: handle case of list in Table
            if (hNode.Name == "ol")
            {
                return true;
            }
            /*else if (hNode.Name == "ul")
            {
                return true;
            }*/
            /* else if (hNode.Name == "p" && regex.IsMatch(hNode.InnerText))
            / {
                 return true;
             }*/
            else
            {
                return false;
            }
        }

        private Element hNodeToElement(HtmlNode temp)
        {
            if (temp.Name == "img")
            {
                return new Element(false, temp.GetAttributeValue("src", "NaN"), "img");
            }
            return new Element(true, temp.InnerText, temp.Name);
        }

        private bool nodeNameValidAdd(HtmlNode hNode)
        {
            return hNode.Name == "#text" || hNode.Name == "img" || hNode.Name == "li";
        }

        //buff Block of Elements taken in by reference
        //HtmlNode temp
        //checks if temp is an image or just textual element and adds accordingly
        //used in addElementToBuffGuarded and Constructor
        private void addElementToBuff(HtmlNode hNode)
        {
            this.buff.Add(hNodeToElement(hNode));
        }

        private void addElementAsBlock(HtmlNode hNode)
        {
            var list = new List<Element>();
            list.Add(hNodeToElement(hNode));
            this.blocks.Add(list);
        }

        private bool timeToDump(string guardString)
        {
            return (guardString == "NaN" || guardString == "1") && this.buff.Count() != 0;
        }

        private HtmlNode getFirstNameOccurance(IEnumerable<HtmlNode> nodes, string name)
        {
            HtmlNode node = null;
            foreach (HtmlNode check in nodes)
            {
                if (check.Name == name)
                {
                    node = check;
                    break;
                }
            }
            return node;
        }

        private void dumpBuffGuard(HtmlNode hNode)
        {
            HtmlNode firstLi = getFirstNameOccurance(hNode.ChildNodes, "li");
            firstLi = firstLi == null ? hNode : firstLi;
            string guardString = firstLi.GetAttributeValue("value", "NaN");
            if(timeToDump(guardString))
            {
                blocks.Add(this.buff);
                this.buff = new List<Element>();
            }
        }

        private List<HtmlNode> handleChildren(HtmlNode hNode)
        {
            List<HtmlNode> retList = new List<HtmlNode>();
            if (isStepsBlock(hNode)) //&& hNode.Name != "p")
            {               
                foreach (HtmlNode child in hNode.ChildNodes)
                {
                    string guardString = child.GetAttributeValue("value", "NaN");
                    retList.Add(child);
                }
                
            }
            else
            {
                retList.Add(hNode);
            }
            return retList;
        }

        private bool contentNull(HtmlNode hNode)
        {            
            string content = hNode.InnerText;
            Regex regexHtmlChar = new Regex("&.*?;");
            content = regexHtmlChar.Replace(content, "");
            return string.IsNullOrWhiteSpace(content);
        }

        private void checkHyperLink(HtmlNode hNode)
        {
            if (hNode.GetAttributeValue("href", "NaN") != "NaN")
            {
                this.hrefs.Add(hNode.GetAttributeValue("href", "NaN"));
            }
            else
            {
                foreach (HtmlNode child in hNode.ChildNodes)
                {
                    checkHyperLink(child);
                }
            }
        }
        
        //buff Block of Elements taken in by reference
        //HtmlNode temp
        //puts elements of a steps block into a Block of Elements
        //used in CHMParser
        private void handleBlock(HtmlNode hNode, Action<HtmlNode> action)
        {
            foreach (HtmlNode element in handleChildren(hNode))
            {
                if (element.Name != "img" && !contentNull(element))
                {
                    action(element);
                    checkHyperLink(element);
                }
            }           
        }
        
        public ParsedCHM(HtmlDocument hDoc)
        {
            HtmlNode titleNode = getFirstNameOccurance(hDoc.DocumentNode.Descendants(), "title");
            this.title = titleNode.InnerText;
            HtmlNode body = getFirstNameOccurance(hDoc.DocumentNode.Descendants(), "body");
            foreach (HtmlNode hNode in body.ChildNodes)
            {
                if (isStepsBlock(hNode))
                {
                    dumpBuffGuard(hNode);
                    handleBlock(hNode, new Action<HtmlNode>((node) => { addElementToBuff(node); }));
                }
                else
                {
                    handleBlock(hNode, new Action<HtmlNode>((node) => { addElementAsBlock(node); }));
                }
            }
            if (this.buff.Count != 0)
            {
                this.blocks.Add(this.buff);
            }
        }

        public void print()
        {
            Console.WriteLine("Title: " + this.title);
            var count = 0;
            foreach (List<Element> elements in this.blocks)
            {
                Console.WriteLine("Block: ");
                foreach (Element e in elements)
                {
                    ++count;
                    Console.WriteLine("     Data: " + e.data);
                    Console.WriteLine("     IsText: " + e.isText);
                    Console.WriteLine("     Tag: " + e.name);
                    Console.WriteLine(" ");
                }
                Console.WriteLine("----------------------------------------------------------------\n");
            }
            Console.WriteLine("# of Elements: " + count);
            Console.WriteLine("# of Blocks: " + this.blocks.Count());
            Console.WriteLine("\nHyperLinks:");
            foreach (string href in this.hrefs)
            {
                Console.WriteLine("     " + href);
            }
        }

        public void sendOff()
        {
            ;//TODO
        }


        // GETTERS and SETTERS are being used for testing purposes

        public string Title
        {
            get { return this.title; }
        }

        public List<List<Element>> Blocks
        {
            get { return this.blocks; }
        }

        public List<string> Hrefs
        {
            get { return this.hrefs;  }
        }
    }
}
