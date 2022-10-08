using QxFramework.Core;

namespace SaveFramework
{
    public class MainDataManager : LogicModuleBase, IMainDataManager
    {
        private MainData _mainData;

        public MainData MainData
        {
            get
            {
                //因为每次读取时都会重新生成一份数据，之前获取到的数据可能是过时的，获取存档数据的时候请务必从QXData里直接获取
                _mainData = QXData.Instance.Get<MainData>();
                return _mainData;
            }
        }

        public override void Init()
        {
            base.Init();
            //注册数据
            if (!RegisterData(out _mainData))
            {
                //如果数据不存在，就初始化一份
                InitMainData();
            }
        }

        private void InitMainData()
        {
            //在注册数据时如果数据不存在会自动创建一份，所以这里可以直接访问
            MainData.DataNumber = ReadFromCSV();
        }

        private string ReadFromCSV()
        {
            return QXData.Instance.TableAgent.GetString("Test", "1", "Value");
        }

        public bool LoadFrom()
        {
            return QXData.Instance.LoadFromFile("SaveFramework.json");
        }

        public void SaveTo()
        {
            QXData.Instance.SaveToFile("SaveFramework.json");
        }

        public void RefreshNum(string Num)
        {
            MainData.DataNumber = Num;
        }
        public string DisplayNum()
        {
            return MainData.DataNumber;
        }
    }

    public class MainData : GameDataBase
    {
        public string DataNumber;
    }
}