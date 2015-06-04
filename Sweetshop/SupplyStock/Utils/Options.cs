using System;
using System.IO;
using System.Xml.Serialization;

namespace SupplyStock.Utils
{
    [Serializable]
    public class Options :  ICloneable
    {
        #region Constructor

        public Options()
        {
            CurrencyRatio = 24;
        }
        public Options(double currencyRatio = 24, string path = "app_config.stg")
        {
            CurrencyRatio = currencyRatio;
            Path = path;
        }

        #endregion
        #region Properties

        public double CurrencyRatio { get; set; }
        public static string Path { get; set; }

        #endregion
        #region Methods

        public void SaveOptions()
        {
            var mySerializer = new XmlSerializer(typeof(Options));
            using (var myWriter = new StreamWriter(Path))
                mySerializer.Serialize(myWriter, this);
        }

        #endregion

        public object Clone()
        {
            return new Options(CurrencyRatio, Path);
        }
        public void Copy(Options o)
        {
            CurrencyRatio = o.CurrencyRatio;
            
        }
    }
}
