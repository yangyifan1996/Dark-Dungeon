using System.Collections.Generic;
using System.IO;
using System.Xml;

public class EncryptKeys
{
    public Dictionary<string, int> Keys { get; private set; }

    public EncryptKeys()
    {
        Keys = new Dictionary<string, int>();
    }

    public void LoadFromStream(Stream stream)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(stream);

        XmlNode encryptKeysNode = doc.SelectSingleNode("EncryptKeys");
        if (encryptKeysNode != null)
        {
            foreach (XmlNode keyNode in encryptKeysNode)
            {
                string path = keyNode.Attributes["Path"].Value;
                int key = int.Parse(keyNode.Attributes["Key"].Value);

                Keys.Add(path, key);
            }
        }
    }

    public void WriteToSteam(Stream stream)
    {
        XmlDocument doc = new XmlDocument();
        doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

        XmlElement elementEncryptKeys = doc.CreateElement("EncryptKeys");
        doc.AppendChild(elementEncryptKeys);

        foreach (string path in Keys.Keys)
        {
            XmlElement keyInfo = doc.CreateElement("Key");

            keyInfo.SetAttribute("Path", path);
            keyInfo.SetAttribute("Key", Keys[path].ToString());

            elementEncryptKeys.AppendChild(keyInfo);
        }
        doc.Save(stream);
    }
}

