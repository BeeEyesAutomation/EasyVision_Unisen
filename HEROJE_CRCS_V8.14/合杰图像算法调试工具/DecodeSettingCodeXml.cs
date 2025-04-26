using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace 合杰图像算法调试工具
{
	internal class DecodeSettingCodeXml
	{
		~DecodeSettingCodeXml()
		{
		}

		public void TreeToXmlByLinq(TreeView TheTreeView, Dictionary<string, CMD_Tool.SettingMaskAndData> dictionary, Form form, string XMLFilePath)
		{
			XDeclaration declaration = new XDeclaration("1.0", "utf-8", "yes");
			XComment[] array = new XComment[2]
			{
				new XComment("This file is necessary for software to run properly"),
				new XComment("Create By Liang")
			};
			object[] content = array;
			XDocument xDocument = new XDocument(declaration, content);
			XElement xElement = new XElement("Root");
			foreach (TreeNode node in TheTreeView.Nodes)
			{
				XElement content2 = CreateElements(node, dictionary);
				xElement.Add(content2);
			}
			xDocument.Add(xElement);
			xDocument.Save(XMLFilePath);
		}

		private XElement CreateElements(TreeNode node, Dictionary<string, CMD_Tool.SettingMaskAndData> dictionary)
		{
			XElement xElement = CreateElement(node, dictionary);
			foreach (TreeNode node2 in node.Nodes)
			{
				XElement content = CreateElements(node2, dictionary);
				xElement.Add(content);
			}
			return xElement;
		}

		private XElement CreateElement(TreeNode node, Dictionary<string, CMD_Tool.SettingMaskAndData> dictionary)
		{
			try
			{
				return new XElement("Node", new XAttribute("Name", node.Name), new XAttribute("Text", node.Text), new XAttribute("Tips", node.ToolTipText), new XAttribute("Type", (char)dictionary[node.Name].Type), new XAttribute("Mask", dictionary[node.Name].Mask.ToString("X4")), new XAttribute("Data", dictionary[node.Name].Data.ToString("X2")));
			}
			catch
			{
				return new XElement("Node", new XAttribute("Name", node.Name), new XAttribute("Text", node.Text), new XAttribute("Tips", node.ToolTipText), new XAttribute("Type", " "), new XAttribute("Mask", " "), new XAttribute("Data", " "));
			}
		}

		private void XmlToControlText(Control.ControlCollection ctc, XmlNode node)
		{
			foreach (Control item in ctc)
			{
				if (item.Name == node.Attributes[0].Name)
				{
					item.Text = node.Attributes[0].Value;
				}
				if (item.HasChildren)
				{
					XmlToControlText(item.Controls, node);
				}
			}
		}

		public void XMLToTreeByLinq(string xmlPath, TreeView TheTreeView, Dictionary<string, CMD_Tool.SettingMaskAndData> dictionary, Form form)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(xmlPath);
			TheTreeView.Nodes.Clear();
			dictionary.Clear();
			RecursionTreeControl(xmlDocument.DocumentElement, TheTreeView.Nodes, dictionary, form);
		}

		private void RecursionTreeControl(XmlNode xmlNode, TreeNodeCollection nodes, Dictionary<string, CMD_Tool.SettingMaskAndData> dictionary, Form form)
		{
			foreach (XmlNode childNode in xmlNode.ChildNodes)
			{
				TreeNode treeNode = new TreeNode();
				if (childNode.Name == "Node")
				{
					try
					{
						treeNode.Name = childNode.Attributes["Name"].Value;
						treeNode.Text = childNode.Attributes["Text"].Value;
						treeNode.ToolTipText = childNode.Attributes["Tips"].Value;
						if (childNode.Attributes["Type"].Value != " " && childNode.Attributes["Type"].Value != "" && !dictionary.ContainsKey(treeNode.Name))
						{
							dictionary.Add(treeNode.Name, new CMD_Tool.SettingMaskAndData((byte)childNode.Attributes["Type"].Value[0], ushort.Parse(childNode.Attributes["Mask"].Value, NumberStyles.HexNumber), byte.Parse(childNode.Attributes["Data"].Value, NumberStyles.HexNumber)));
						}
					}
					catch (Exception)
					{
					}
					nodes.Add(treeNode);
				}
				else
				{
					XmlToControlText(form.Controls, childNode);
				}
				RecursionTreeControl(childNode, treeNode.Nodes, dictionary, form);
			}
		}
	}
}
