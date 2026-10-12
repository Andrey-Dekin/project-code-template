namespace WinterDrift.Features.UI
{
    using System.Collections.Generic;
    using DekDev.CDebug;
    using DekDev.Extensions.AbstractViews;
    using UnityEngine;

    /// <summary>
    /// Random sprite on enable.
    /// </summary>
    public class RandomSpriteOnEnable : AbstractImageView
    {
        [SerializeField] protected List<Sprite> sprites = new List<Sprite>();

        protected virtual void OnEnable() => UpdateImageView();

        protected override void UpdateImageView()
        {
            if (sprites.Count < 1)
            {
                CDebug.LogError("Sprite list is empty!");

                return;
            }

            imageView.sprite = sprites[Random.Range(0, sprites.Count)];
        }
    }
}