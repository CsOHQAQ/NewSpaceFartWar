using System;

namespace SaveFramework
{
    public interface IMainDataManager
    {
        /// <summary>
        /// 加载存档
        /// </summary>
        /// <returns>是否加载成功</returns>
        bool LoadFrom();
        /// <summary>
        /// 保存存档
        /// </summary>
        void SaveTo();
        /// <summary>
        /// 刷新存档中的数据
        /// </summary>
        /// <param name="Num">要保存的数据（不要在意为什么是Num）</param>
        void RefreshNum(string Num);
        /// <summary>
        /// 从存档中获取数据
        /// </summary>
        /// <returns>获取的数据</returns>
        string DisplayNum();
    }
}