using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
namespace xmlTree
{
    class FileToXml
    {
        private StringBuilder sb=new StringBuilder();
        private string filepath;
        XmlWriter xw;
        XmlDocument xmlDocument;
        /*public string filePath
        {
            set
            {
                this.filepath = value;
            }
            get
            {
                return this.filepath;
            }
        }*/

        public FileToXml(string FilePath)
        {
            this.filepath = FilePath;
        }
         
        public string FileTree()
        {
            InitXml();
             FileTree(filepath,"/"+filepath.Substring(filepath.LastIndexOf("\\")+1));
            xmlDocument.Save("F:\\test.xml");
            return "";
        }

        private string FileTree(string FilePath,string xpath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(FilePath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (var item in fileInfos)
            {
                FileInfoToXml(item, xpath);
            }
            foreach (var item in directoryInfo.GetDirectories())
            {
                DirToXml(item, xpath);
            }

            return sb.ToString();
        }
        private void InitXml()
        {
            xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.AppendChild(xmlDeclaration);
            XmlNode xmlNode = xmlDocument.CreateNode("element", filepath.Substring(filepath.LastIndexOf("\\")+1), "");
            xmlDocument.AppendChild(xmlNode);
            //XElement xElement = new XElement("root");
            //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            //xmlWriterSettings.Encoding = new UTF8Encoding(false);
            //xmlWriterSettings.Indent = true;
            //xw = XmlWriter.Create("F:\\test.xml", xmlWriterSettings);
            //xElement.Save(xw);
            //xw.Flush();
            //xw.Close();
            //xmlDocument.Load("F:\\test.xml");
        }

        //Add file information to xml
        private void FileInfoToXml(FileInfo fileInfo,string xpath)
        {
            string tempXpath = xpath;
            //Initialization one node
            /*XElement XElement = new XElement(
                new XElement(fileInfo.Name,
                    new XElement("date", fileInfo.LastWriteTime),
                    new XElement("hash", fileInfo.GetHashCode().ToString()),
                    new XElement("size", fileInfo.Length)
                ));
                */
            XmlNode xnode= xmlDocument.CreateNode("element", fileInfo.Name, "");
            XmlElement xmlElement = (XmlElement)xnode;
            xmlElement.SetAttribute("type", "file");
            XmlNode x1node = xmlDocument.CreateNode("element", "date", "");
            x1node.InnerText = fileInfo.LastWriteTime.ToLongDateString();
            XmlNode x2node = xmlDocument.CreateNode("element", "hash", "");
            x2node.InnerText = fileInfo.GetHashCode().ToString();
            XmlNode x3node = xmlDocument.CreateNode("element", "size", "");
            x3node.InnerText = fileInfo.Length.ToString();
            
           
            //append child node to Parent
            
            XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath);
            xmlNode.AppendChild(xnode);
            xmlNode = xmlDocument.SelectSingleNode(xpath + "/" + fileInfo.Name);
            xmlNode.AppendChild(x1node);
            xmlNode.AppendChild(x2node);
            xmlNode.AppendChild(x3node);
        }

        //Add directory information to xml
        private void DirToXml(DirectoryInfo directoryInfo,string xpath)
        {
            XmlNode xmlNode = xmlDocument.CreateNode("element", directoryInfo.Name, "");
            XmlElement xmlElement = (XmlElement)xmlNode;
            xmlElement.SetAttribute("type", "directory");
            XmlNode xNode = xmlDocument.SelectSingleNode(xpath);
            xNode.AppendChild(xmlNode);
            FileTree(directoryInfo.FullName, xpath + "/" + directoryInfo.Name);
        }
    }
}
