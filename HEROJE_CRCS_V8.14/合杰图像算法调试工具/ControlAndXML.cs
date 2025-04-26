using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Heroje_Debug_Tool.BaseClass;

namespace 合杰图像算法调试工具
{
	internal class ControlAndXML
	{
		~ControlAndXML()
		{
		}

		private void ControlTextAddXml(Control.ControlCollection ctc, XElement node, string str)
		{
			foreach (Control item in ctc)
			{
				if (item.Text != "" && item.Name != "" && item.Name != "TxbConfigFilePath" && item.Name != "TxbSoftVersion" && item.Name != "CobSelectLanguage")
				{
					node.Add(new XElement(str, new XAttribute(item.Name, item.Text)));
				}
				if (item.Name == "MesMain")
				{
					foreach (ToolStripMenuItem item2 in ((MenuStrip)item).Items)
					{
						node.Add(new XElement(str, new XAttribute(item2.Name, item2.Text)));
						foreach (ToolStripMenuItem dropDownItem in item2.DropDownItems)
						{
							if (dropDownItem.Text != "" && dropDownItem.Name != "")
							{
								node.Add(new XElement(str, new XAttribute(dropDownItem.Name, dropDownItem.Text)));
							}
						}
					}
				}
				string name = item.GetType().Name;
				if (name.Equals("MenuStrip"))
				{
					foreach (ToolStripItem item3 in ((MenuStrip)item).Items)
					{
						node.Add(new XElement(str, new XAttribute(item3.Name, item3.Text)));
						string name2 = item3.GetType().Name;
						if (!name2.Equals("ToolStripMenuItem"))
						{
							continue;
						}
						ToolStripMenuItem toolStripMenuItem3 = (ToolStripMenuItem)item3;
						foreach (ToolStripMenuItem dropDownItem2 in toolStripMenuItem3.DropDownItems)
						{
							if (dropDownItem2.Text != "" && dropDownItem2.Name != "")
							{
								node.Add(new XElement(str, new XAttribute(dropDownItem2.Name, dropDownItem2.Text)));
							}
						}
					}
				}
				else if (name.Equals("ToolStrip"))
				{
					foreach (ToolStripItem item4 in ((ToolStrip)item).Items)
					{
						node.Add(new XElement(str, new XAttribute(item4.Name, item4.Text)));
					}
				}
				else if (name.Equals("StatusStrip"))
				{
					foreach (ToolStripItem item5 in ((StatusStrip)item).Items)
					{
						node.Add(new XElement(str, new XAttribute(item5.Name, item5.Text)));
					}
				}
				else if (name.Equals("ComboBox"))
				{
					node.Add(new XElement(str, new XAttribute(item.Name, item.Text)));
					int num = 0;
					foreach (string item6 in ((ComboBox)item).Items)
					{
						num++;
						node.Add(new XElement(str, new XAttribute(item.Name + "_item_" + num, item6)));
					}
				}
				else if (name.Equals("ListView"))
				{
					node.Add(new XElement(str, new XAttribute(item.Name, item.Text)));
					ListView listView = (ListView)item;
					int num2 = 0;
					foreach (ColumnHeader column in listView.Columns)
					{
						num2++;
						node.Add(new XElement(str, new XAttribute(item.Name + "_columns_" + num2, column.Text)));
					}
				}
				if (item.HasChildren)
				{
					ControlTextAddXml(item.Controls, node, str);
				}
			}
		}

		private void XmlToControlText(Control.ControlCollection ctc, XmlNode node)
		{
			foreach (Control item in ctc)
			{
				if (item.Name == node.Attributes[0].Name)
				{
					item.Text = node.Attributes[0].Value;
					break;
				}
				if (item.Name == "MesMain")
				{
					foreach (ToolStripMenuItem item2 in ((MenuStrip)item).Items)
					{
						if (item2.Name == node.Attributes[0].Name)
						{
							item2.Text = node.Attributes[0].Value;
							return;
						}
						foreach (ToolStripMenuItem dropDownItem in item2.DropDownItems)
						{
							if (dropDownItem.Name == node.Attributes[0].Name)
							{
								dropDownItem.Text = node.Attributes[0].Value;
								return;
							}
						}
					}
				}
				if (item.GetType().Name.Equals("MenuStrip"))
				{
					foreach (ToolStripItem item3 in ((MenuStrip)item).Items)
					{
						if (item3.Name == node.Attributes[0].Name)
						{
							item3.Text = node.Attributes[0].Value;
							return;
						}
						if (!item3.GetType().Name.Equals("ToolStripMenuItem"))
						{
							continue;
						}
						ToolStripMenuItem toolStripMenuItem3 = (ToolStripMenuItem)item3;
						foreach (ToolStripMenuItem dropDownItem2 in toolStripMenuItem3.DropDownItems)
						{
							if (dropDownItem2.Name == node.Attributes[0].Name)
							{
								dropDownItem2.Text = node.Attributes[0].Value;
								return;
							}
						}
					}
				}
				else if (item.GetType().Name.Equals("ToolStrip"))
				{
					foreach (ToolStripItem item4 in ((ToolStrip)item).Items)
					{
						if (item4.Name == node.Attributes[0].Name)
						{
							item4.Text = node.Attributes[0].Value;
							return;
						}
					}
				}
				else if (item.GetType().Name.Equals("StatusStrip"))
				{
					foreach (ToolStripItem item5 in ((StatusStrip)item).Items)
					{
						if (item5.Name == node.Attributes[0].Name)
						{
							item5.Text = node.Attributes[0].Value;
							return;
						}
					}
				}
				else if (item.GetType().Name.Equals("ComboBox"))
				{
					if (node.Attributes[0].Name.Contains(item.Name) && node.Attributes[0].Name.Contains("_item_"))
					{
						try
						{
							int index = Convert.ToInt32(node.Attributes[0].Name.Replace(item.Name + "_item_", "")) - 1;
							((ComboBox)item).Items[index] = node.Attributes[0].Value;
						}
						catch (Exception ex)
						{
							LogRecord.WriteError(ex);
						}
					}
				}
				else if (item.GetType().Name.Equals("ListView") && node.Attributes[0].Name.Contains(item.Name) && node.Attributes[0].Name.Contains("_columns_"))
				{
					ListView listView = (ListView)item;
					try
					{
						int index2 = Convert.ToInt32(node.Attributes[0].Name.Replace(item.Name + "_columns_", "")) - 1;
						listView.Columns[index2].Text = node.Attributes[0].Value;
					}
					catch (Exception ex2)
					{
						LogRecord.WriteError(ex2);
					}
				}
				if (item.HasChildren)
				{
					XmlToControlText(item.Controls, node);
				}
			}
		}

		public void MainControlToXmlByLinq(Form form, string XMLFilePath)
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
			XElement xElement2 = new XElement(form.Name);
			xElement.Add(xElement2);
			string text = form.Text;
			if (ToolCfg.ThisSoftware != 0 && ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Hide_Heroje_Logo)
			{
				text = text.Replace("合杰", "");
				text = text.Replace("合傑", "");
				text = text.Replace("HEROJE", "");
				text = text.Replace("HeroJe", "");
			}
			xElement2.Add(new XElement("Language", new XAttribute("SoftwareTitle", text)));
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(XMLFilePath);
			if (fileNameWithoutExtension == "ChineseS")
			{
				xElement2.Add(new XElement("Language", new XAttribute("SelectLanguage", "简体中文")));
			}
			else if (fileNameWithoutExtension == "ChineseT")
			{
				xElement2.Add(new XElement("Language", new XAttribute("SelectLanguage", "繁体中文")));
			}
			else
			{
				xElement2.Add(new XElement("Language", new XAttribute("SelectLanguage", Path.GetFileNameWithoutExtension(XMLFilePath))));
			}
			ControlTextAddXml(form.Controls, xElement2, "Language");
			xDocument.Add(xElement);
			xDocument.Save(XMLFilePath);
		}

		public void SubControlToXmlByLinq(Form form, string XMLFilePath)
		{
			XDocument xDocument = new XDocument();
			xDocument = XDocument.Load(XMLFilePath);
			XElement root = xDocument.Root;
			XElement xElement = new XElement(form.Name);
			string text = form.Text;
			if (ToolCfg.ThisSoftware != 0 && ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Hide_Heroje_Logo)
			{
				text = text.Replace("合杰", "");
				text = text.Replace("合傑", "");
				text = text.Replace("HEROJE", "");
				text = text.Replace("HeroJe", "");
			}
			xElement.Add(new XElement("Language", new XAttribute("SoftwareTitle", text)));
			ControlTextAddXml(form.Controls, xElement, "Language");
			root.Add(xElement);
			xDocument.Save(XMLFilePath);
		}

		public void XMLToControlByLinq(string xmlPath, Form form)
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(xmlPath);
				RecursionTreeControl(xmlDocument.DocumentElement, form);
			}
			catch (Exception)
			{
			}
		}

		private void RecursionTreeControl(XmlNode xmlNode, Form form)
		{
			string text = "";
			foreach (XmlNode childNode in xmlNode.ChildNodes)
			{
				TreeNode treeNode = new TreeNode();
				if (childNode.ParentNode.Name == form.Name && childNode.Name == "Language")
				{
					if ("SoftwareTitle" == childNode.Attributes[0].Name)
					{
						text = childNode.Attributes[0].Value;
						if ((form.Name == "MainForm" || form.Name == "ComDebugTool" || form.Name == "FormNet" || form.Name == "CMD_Tool") && ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Heroje_Standard)
						{
							if (ToolCfg.ConfigPath.Contains("ChineseS"))
							{
								if (!text.Contains("合杰"))
								{
									text = "合杰" + text;
								}
							}
							else if (ToolCfg.ConfigPath.Contains("ChineseT"))
							{
								if (!text.Contains("合傑"))
								{
									text = "合傑" + text;
								}
							}
							else if (!text.ToUpper().Contains("HEROJE"))
							{
								text = "HEROJE " + text;
							}
						}
						form.Text = text;
					}
					XmlToControlText(form.Controls, childNode);
				}
				RecursionTreeControl(childNode, form);
			}
		}

		public void AddTextToXml(Dictionary<MultLanguageText.TextDef, MultLanguageText.StringPairStu> DatList, string XMLFilePath)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(XMLFilePath);
			XmlNode documentElement = xmlDocument.DocumentElement;
			XmlElement xmlElement = xmlDocument.CreateElement("MsgContentList");
			documentElement.AppendChild(xmlElement);
			for (int i = 0; i < DatList.Count; i++)
			{
				XmlElement xmlElement2 = xmlDocument.CreateElement("MsgContent");
				xmlElement.AppendChild(xmlElement2);
				XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("Id");
				xmlAttribute.Value = i.ToString();
				xmlElement2.SetAttributeNode(xmlAttribute);
				XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute("MessageBoxInfoDef");
				MultLanguageText.TextDef textDef = (MultLanguageText.TextDef)i;
				xmlAttribute2.Value = textDef.ToString();
				xmlElement2.Attributes.Append(xmlAttribute2);
				XmlElement xmlElement3 = xmlDocument.CreateElement("TitleText");
				xmlElement3.InnerText = DatList[(MultLanguageText.TextDef)i].TitleText;
				xmlElement2.AppendChild(xmlElement3);
				XmlElement xmlElement4 = xmlDocument.CreateElement("ContentText");
				xmlElement4.InnerText = DatList[(MultLanguageText.TextDef)i].ContentText;
				xmlElement2.AppendChild(xmlElement4);
			}
			xmlDocument.Save(XMLFilePath);
		}

		public Dictionary<MultLanguageText.TextDef, MultLanguageText.StringPairStu> ReadMsgTextInfoFromXml(string XMLFilePath)
		{
			Dictionary<MultLanguageText.TextDef, MultLanguageText.StringPairStu> dictionary = new Dictionary<MultLanguageText.TextDef, MultLanguageText.StringPairStu>();
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(XMLFilePath);
				XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("MsgContent");
				if (elementsByTagName.Count > 0)
				{
					foreach (XmlNode item in elementsByTagName)
					{
						int key = Convert.ToInt32(item.Attributes["Id"].Value);
						string innerText = item["TitleText"].InnerText;
						string innerText2 = item["ContentText"].InnerText;
						Console.WriteLine("ID: " + key + ", m_name: " + innerText + ", m_desc: " + innerText2);
						dictionary.Add((MultLanguageText.TextDef)key, new MultLanguageText.StringPairStu(innerText, innerText2));
					}
				}
				else
				{
					dictionary = null;
				}
			}
			catch (Exception)
			{
				dictionary = null;
			}
			return dictionary;
		}
	}
}
