using System;
using System.Collections.Generic;
using System.IO;

namespace SetWindowVisible.Helper
{
    [Serializable]
    public class Dict : XmlSerializeHelper<Dict>
    {
        private readonly string _xmlFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, AppDomain.CurrentDomain.SetupInformation.ApplicationName.Substring(0, AppDomain.CurrentDomain.SetupInformation.ApplicationName.LastIndexOf('.')) + ".xml");

        public Dict()
        {
            Datas = new List<Data>();
            XmlFilePath = _xmlFilePath;
        }

        public List<Data> Datas { get; set; }

        public void Load()
        {
            Load(_xmlFilePath);
        }

        protected override void LoadCore(Dict obj)
        {
            Datas = obj.Datas;
        }
    }
}