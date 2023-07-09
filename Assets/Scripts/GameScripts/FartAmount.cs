using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteAlways]
public class FartAmount : MonoBehaviour
{
    [Range(0, 1f)]
    public float amount;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        MessageManager.Instance.Get<PlayerMessage>().RegisterHandler(PlayerMessage.FartAmount, (sender, args) =>
        {
            PlayerController playerController = GetComponentInParent<PlayerController>();
            if (sender is PlayerController player && playerController != null && player.playerIndex == playerController.playerIndex)
            {
                amount = 1 - (args as UIArgs<float>).Data;
            }
        });
    }

    // Update is called once per frame
    private void Update()
    {
        if (Application.isPlaying)
        {
            spriteRenderer.material.SetFloat("_Amount", amount * 0.15f);
        }
        else
        {
            spriteRenderer.sharedMaterial.SetFloat("_Amount", amount * 0.15f);
        }
    }

    private void OnDestroy()
    {
        MessageManager.Instance.RemoveAbout(this);
    }
}
