using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HighlightEdge : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material material;

    private void Update()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            material = spriteRenderer.material;
        }
        if (spriteRenderer.sprite != null)
        {
            Vector4 tilingandoffset = new Vector4(spriteRenderer.sprite.textureRect.x / spriteRenderer.sprite.texture.width, spriteRenderer.sprite.textureRect.y / spriteRenderer.sprite.texture.height, spriteRenderer.sprite.textureRect.width / spriteRenderer.sprite.texture.width, spriteRenderer.sprite.textureRect.height / spriteRenderer.sprite.texture.height);
            material.SetVector("_TilingAndOffset", tilingandoffset);
        }
    }

    public void SetProperties(Vector2 lightOffset, Color color, Vector2 shadowOffset, float shadowIntensity, float angle = 180, float angleOffset = 0)
    {
        if (material != null)
        {
            material.SetVector("_LightOffset", lightOffset);
            material.SetColor("_Color", color);
            material.SetVector("_ShadowOffset", shadowOffset);
            material.SetFloat("_ShadowIntensity", shadowIntensity);
            material.SetFloat("_Angle", angle);
            material.SetFloat("_AngleOffset", angleOffset);
        }
        else
        {
            var material = GetComponent<SpriteRenderer>().sharedMaterial;
            material.SetVector("_LightOffset", lightOffset);
            material.SetColor("_Color", color);
            material.SetVector("_ShadowOffset", shadowOffset);
            material.SetFloat("_ShadowIntensity", shadowIntensity);
            material.SetFloat("_Angle", angle);
            material.SetFloat("_AngleOffset", angleOffset);
        }
    }

    private void Reset()
    {
        spriteRenderer = null;
    }
}
