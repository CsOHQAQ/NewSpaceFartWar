using QxFramework.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 游戏管理器，用于管理之前由MonoSingleton所有逻辑
/// </summary>
public class GameMgr : MonoSingleton<GameMgr>
{
    [Header("要加载的模块")]
    public List<ModuleEnum> modulesToLoad = new List<ModuleEnum>();

    /// <summary>
    /// 所有模块列表
    /// </summary>
    private readonly List<ModulePair> _modules = new List<ModulePair>();

    //public static bool Enable;

    /// <summary>
    /// 初始化所有模块
    /// </summary>
    public void InitModules()
    {
        ClearModule();
        HashSet<Type> modules = new HashSet<Type>();
        foreach (var type in GetType().Assembly.GetTypes())
        {
            if (type.IsSubclassOf(typeof(LogicModuleBase)) && !type.IsAbstract)
            {
                modules.Add(type);
            }
        }
        foreach (var type in modules)
        {
            if (modulesToLoad.Contains(item => { return item.ToString() == type.Name; }))
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.Name == "I" + type.Name)
                    {
                        LogicModuleBase module = Activator.CreateInstance(type) as LogicModuleBase;
                        _modules.Add(new ModulePair(iface, module));
                        try
                        {
                            module.Init();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 清除所有模块。
    /// 请务必在LogicModuleBase的OnDestroy中管理好对应数据，因为如果模块有引用可能无法被GC，
    /// 导致存在多个LogicModuleBase的实例，可能导致一些意外的情况。（尤其是使用了MessageManager）
    /// </summary>
    public void ClearModule()
    {
        foreach (var module in _modules)
        {
            module.Module.OnDestroy();
        }
        _modules.Clear();
        GC.Collect();
    }

    public static T Get<T>()
    {
        var type = typeof(T);
        var pair = Instance._modules.Find((m) => m.ModuleType == type);
        if (pair == null)
        {
            Debug.Log("[GameMgr]未注册的模块" + type.Name);

            return default(T);
        }
        if (pair.Initialized == false)
        {
            try
            {
                pair.Module.Awake();
                pair.Initialized = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return (T)(object)pair.Module;
    }

    private void Update()
    {
        for (int i = 0; i < _modules.Count; i++)
        {
            var module = _modules[i];
            try
            {
                module.Module.Update();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _modules.Count; i++)
        {
            var module = _modules[i];
            try
            {
                module.Module.FixedUpdate();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _modules.Count; i++)
        {
            var module = _modules[i];
            try
            {
                module.Module.OnDestroy();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    private class ModulePair
    {
        public readonly Type ModuleType;
        public readonly LogicModuleBase Module;

        public bool Initialized;

        public ModulePair(Type moduleType, LogicModuleBase module)
        {
            ModuleType = moduleType;
            Module = module;
        }
    }
}

/// <summary>
/// 模块名要和Enum的名字完全相同才能正常加载
/// </summary>
public enum ModuleEnum
{
    Unknow = 0,
    MainDataManager,
    EventManager,
    ItemManager,
    GameTimeManager,
    Max,
}