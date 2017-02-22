using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Parser
{
    struct Element
    {
        
        public bool isText;
        public string data;
        public Element(bool isText, string data)
        {
            this.isText = isText;
            this.data = data;
        }
    }
    class ParsedCHM
    {
        private List<List<Element>> blocks = new List<List<Element>>(); //breadth first from root
        private List<Element> buff = new List<Element>();
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
            else if (hNode.Name == "ul")
            {
                return true;
            }
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
            if (temp.Name == "img")//what about nested </b> or </i> or </strong> or </font><- gotta get the data into a good format
            {
                return new Element(false, temp.GetAttributeValue("src", "NaN"));
            }
            return new Element(true, temp.InnerText);
        }

        //buff Block of Elements taken in by reference
        //HtmlNode temp
        //checks if temp is an image or just textual element and adds accordingly
        //used in addElementToBuffGuarded and Constructor
        private void addElementToBuff(HtmlNode temp)
        {
            this.buff.Add(hNodeToElement(temp));
        }

        private void addElementAsBlock(HtmlNode temp)
        {
            var list = new List<Element>();
            list.Add(hNodeToElement(temp));
            this.blocks.Add(list);
        }
        //buff Block of Elements taken in by reference
        //HtmlNode temp
        //checks buff can expect any new elements, if not dumps buff
        //used in handleStepsBLock
        /*private void addElementToBuffGuarded(HtmlNode temp)
        {
            //dumps buff if doesn't expect more elements
            string guardString = temp.GetAttributeValue("value", "NaN");
            if (guardString == "NaN" || guardString == "1")
            {
                Console.WriteLine("in guard");
                blocks.Add(this.buff);       
                this.buff = new List<Element>();
            }
            addElementToBuff(temp);        
        }*/

        private void dumpBuffGuard(HtmlNode hNode)
        {
            string guardString = hNode.GetAttributeValue("value", "NaN");
            if (guardString == "NaN" || guardString == "1")
            {
                Console.WriteLine("in guard");
                blocks.Add(this.buff);
                this.buff = new List<Element>();
            }
        }

        //buff Block of Elements taken in by reference
        //HtmlNode temp
        //puts elements of a steps block into a Block of Elements
        //used in CHMParser
        private void handleBlock(HtmlNode hNode, Action<HtmlNode> action)
        {
            // if (hNode.Name == "ol")
            // {
                Stack<HtmlNode> explicitStack = new Stack<HtmlNode>();
                HtmlNode temp = hNode;
                do
                {
                    foreach (HtmlNode child in temp.ChildNodes.Reverse())
                    {
                        //Console.WriteLine(temp.InnerText);
                        explicitStack.Push(child);
                    }
                    if (explicitStack.Count == 0)
                    {
                        break;
                    }

                    temp = explicitStack.Pop();
                    action(temp);

                } while (true);
            // }
        }
        
        public ParsedCHM(HtmlDocument hDoc)
        {
            /*Element e = new Element();
            e.isText = true;
            e.data = hDoc.DocumentNode.Descendants().TakeWhile((x) => x.Name != "title").First().Name;
            List<Element> title = new List<Element>();
            title.Add(e);
            blocks.Add(title);*/
            HtmlNode body = null;
           foreach (HtmlNode check in hDoc.DocumentNode.Descendants())
            {
                if (check.Name == "body")
                {
                    body = check;
                    break;
                }
            }
            Console.Write(body.Name);

            //var tags = hDoc.DocumentNode.Descendants();
            //foreach (HtmlNode hNode in tags)
            //{
            foreach (HtmlNode hNode in body.ChildNodes)
            {
                Console.WriteLine("Line");
                Console.WriteLine(hNode.Name);
                if (isStepsBlock(hNode))
                {
                    Console.WriteLine("try");
                    Console.WriteLine();
                    Console.WriteLine(hNode.ChildNodes.First().HasAttributes);//quot specifier makes it seeen as  #text
                                                                                //definitely needs to massage html further
                    Console.WriteLine(hNode.ChildNodes.First().Name);
                    dumpBuffGuard(hNode.FirstChild);
                    Console.WriteLine();
                    Console.WriteLine();
                    //Console.WriteLine(hNode.FirstChild.OuterHtml);
                    handleBlock(hNode, new Action<HtmlNode>((node) => { addElementToBuff(node); }));
                }
                else
                {
                    /*Console.WriteLine(hNode.InnerText);
                    List<Element> inList = new List<Element>();
                    inList.Add(hNodeToElement(hNode));
                    this.blocks.Add(inList);*/
                    handleBlock(hNode, new Action<HtmlNode>((node) => { addElementAsBlock(node); }));
                }
            }
            //}
            
        }
        public void print()
        {
            var count = 0;
            foreach (List<Element> elements in this.blocks)
            {
                foreach (Element e in elements)
                {
                    ++count;
                    Console.WriteLine();
                    Console.WriteLine(e.data);
                    Console.WriteLine(e.isText);
                    Console.WriteLine();
                
                }
            }
            Console.WriteLine(count);
        }
        public void sendOff()
        {
            ;//TODO
        }
    }
}
