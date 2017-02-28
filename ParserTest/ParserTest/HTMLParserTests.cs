using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using HtmlAgilityPack;

namespace ParserTest
{
    [TestClass]
    public class HTMLParserTests
    {
        [TestMethod]
        public void TestDeleteUserFile_OutputOnly()
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load("D:/SeniorDesign/ActivAID/parser/Parser/HelpFiles/DeleteUser.html");
            ParsedCHM parsed = new ParsedCHM(htmlDoc);

            // Assert.AreEqual(parsed.Blocks, 4);
            // Assert.AreEqual(parsed.Hrefs, 4);
            Assert.AreEqual(parsed.Title, "Deleting a User");
        }

        [TestMethod]
        public void TestDeletingDrivesFiles_OutputOnly()
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load("D:/SeniorDesign/ActivAID/parser/Parser/HelpFiles/DeletingDrivers.htm");
            ParsedCHM parsed = new ParsedCHM(htmlDoc);

            // Assert.AreEqual(parsed.Blocks, 4);
            // Assert.AreEqual(parsed.Hrefs, 4);
            Console.WriteLine("Title: " + parsed.Title);
            Assert.AreEqual(parsed.Title, "Deleting a Driver");
        }

        [TestMethod]
        public void TestNewUserFile_OutputOnly()
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load("D:/SeniorDesign/ActivAID/parser/Parser/HelpFiles/NewUser.html");
            ParsedCHM parsed = new ParsedCHM(htmlDoc);

            // Assert.AreEqual(parsed.Blocks, 4);
            // Assert.AreEqual(parsed.Hrefs, 4);
            Assert.AreEqual(parsed.Title, "Adding a new user");
        }

        [TestMethod]
        public void TestNodeDragDropFile_OutputOnly()
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load("D:/SeniorDesign/ActivAID/parser/Parser/HelpFiles/NodeDragDrop.html");
            ParsedCHM parsed = new ParsedCHM(htmlDoc);

            // Assert.AreEqual(parsed.Blocks, 4);
            // Assert.AreEqual(parsed.Hrefs, 4);
            Assert.AreEqual(parsed.Title, "Node Drag-Drop");
        }

        [TestMethod]
        public void TestPackageManagerCreationFile_OutputOnly()
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load("D:/SeniorDesign/ActivAID/parser/Parser/HelpFiles/PackageMngr_Creation.html");
            ParsedCHM parsed = new ParsedCHM(htmlDoc);

            // Assert.AreEqual(parsed.Blocks, 4);
            // Assert.AreEqual(parsed.Hrefs, 4);
            Assert.AreEqual(parsed.Title, "Package Manager");
        }
    }
}
