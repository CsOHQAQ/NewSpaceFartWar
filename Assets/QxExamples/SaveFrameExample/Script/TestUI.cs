using QxFramework.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SaveFramework
{
    public class TestUI : UIBase
    {
        private void Awake()
        {
            Get<Button>("ShowData").onClick.SetListener(Display);
            Get<Button>("SaveToMemory").onClick.SetListener(SaveToMemory);
            Get<Button>("SaveToDisk").onClick.SetListener(Save);
            Get<Button>("Load").onClick.SetListener(Load);
        }

        public override void OnDisplay(object args)
        {
            base.OnDisplay(args); 
            if (GameMgr.Get<IMainDataManager>().LoadFrom())
            {
                Get<Text>("DiskText").text = GameMgr.Get<IMainDataManager>().DisplayNum();
                Get<Text>("MemoryText").text = GameMgr.Get<IMainDataManager>().DisplayNum();
            }
            else
            {
                Get<Text>("DiskText").text = "无存档";
            }
        }

        public void Save()
        {
            GameMgr.Get<IMainDataManager>().SaveTo();
            Get<Text>("DiskText").text = GameMgr.Get<IMainDataManager>().DisplayNum();
        }

        public void Load()
        {
            if (GameMgr.Get<IMainDataManager>().LoadFrom())
            {
                Get<Text>("MemoryText").text = GameMgr.Get<IMainDataManager>().DisplayNum();
            }
        }

        public void Display()
        {
            Get<InputField>("InputText").text = GameMgr.Get<IMainDataManager>().DisplayNum();
        }

        public void SaveToMemory()
        {
            GameMgr.Get<IMainDataManager>().RefreshNum(Get<InputField>("InputText").text);
            Get<Text>("MemoryText").text = GameMgr.Get<IMainDataManager>().DisplayNum();
        }
    }
}