using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public abstract class Configuration<C> where C : class, new()
{
    private static C instance;

    public static C Load()
    {
        if (instance == null)
        {
            var configPath = GetConfigPath();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(C));

            try
            {
                if (File.Exists(configPath))
                {
                    using (StreamReader streamReader = new StreamReader(configPath))
                    {
                        instance = xmlSerializer.Deserialize(streamReader) as C;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Configuration:Load -> Exception: " + e.Message);
            }
        }
        return instance ?? (instance = new C());
    }

    public static void Save()
    {
        if (instance == null) return;

        string configPath = GetConfigPath();

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(C));
        XmlSerializerNamespaces noNamespaces = new XmlSerializerNamespaces();
        noNamespaces.Add("", "");

        try
        {
            using (StreamWriter streamWriter = new StreamWriter(configPath))
            {
                xmlSerializer.Serialize(streamWriter, instance, noNamespaces);
            }
        }
        catch (Exception e)
        {
            Debug.Log("[Bulldoze It!] Configuration:Save -> Exception: " + e.Message);
        }
    }

    private static string GetConfigPath()
    {
        if (typeof(C).GetCustomAttributes(typeof(ConfigurationPathAttribute), true)
            .FirstOrDefault() is ConfigurationPathAttribute configPathAttribute)
        {
            return configPathAttribute.Value;
        }
        else
        {
            Debug.Log("[Bulldoze It!] Configuration:GetConfigPath -> ConfigurationPath attribute missing in " + typeof(C).Name);
            return typeof(C).Name + ".xml";
        }
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationPathAttribute : Attribute
{
    public ConfigurationPathAttribute(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }
}