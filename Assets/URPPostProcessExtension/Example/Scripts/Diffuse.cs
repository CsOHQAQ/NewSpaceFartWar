using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Post-processing/Diffuse", typeof(UniversalRenderPipeline))]
public class Diffuse : CustomVolumeComponent
{
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0, -1f, 1f);
    public Texture2DParameter renderTexture = new Texture2DParameter(null);

    Material material;
    string shaderName = "Shader Graphs/Diffuse";

    public override bool IsActive() => material != null && !Mathf.Approximately(intensity.value, 0);

    public override CustomPostProcessInjectionPoint InjectionPoint => CustomPostProcessInjectionPoint.BeforePostProcess;

    public override void SetUp()
    {
        if (material == null)
        {
            material = CoreUtils.CreateEngineMaterial(shaderName);
        }
    }

    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        if (material == null)
        {
            return;
        }

        material.SetTexture("_Mask", renderTexture.value);
        material.SetFloat("_Intensity", intensity.value);
        cmd.Blit(source, destination, material);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(material);
    }
}