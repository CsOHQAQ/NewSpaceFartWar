using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class CustomVolumeComponent : VolumeComponent, IPostProcessComponent, IDisposable
{
    /// <summary>
    /// ��ͬһ����㣬��Ⱦִ�е�˳��
    /// </summary>
    public virtual int OrderInPass => 0;
    /// <summary>
    /// �����
    /// </summary>
    public virtual CustomPostProcessInjectionPoint InjectionPoint => CustomPostProcessInjectionPoint.AfterOpaqueAndSky;
    /// <summary>
    /// ��Ⱦ��׼������������׼�����ʵ�
    /// </summary>
    public abstract void SetUp();
    /// <summary>
    /// ִ����Ⱦ��
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="renderingData"></param>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    public abstract void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination);
    /// <summary>
    /// �Ƿ�������Ⱦ�����һ������û��Ч�������������������false�Խ�ʡ����
    /// </summary>
    /// <returns></returns>
    public abstract bool IsActive();

    public virtual bool IsTileCompatible() => false;
    /// <summary>
    /// ������Դ����ֹ����©�ĵ�������
    /// </summary>
    /// <param name="disposing"></param>
    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
public enum CustomPostProcessInjectionPoint
{
    AfterOpaqueAndSky,
    BeforePostProcess,
    AfterPostProcess,
}
