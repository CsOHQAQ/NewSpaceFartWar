using System;

public interface IMainDataManager
{
    void LoadFrom();
    void SaveTo();
    void RefreshNum(string Num);
    string DisplayNum();
}

