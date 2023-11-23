using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    Image image;
    SpriteAtlas atlas;

    private void OnEnable()
    {
        image = GetComponent<Image>();
        atlas = Resources.Load<SpriteAtlas>("Atlas/UIIcon");
        image.sprite = atlas.GetSprite("Heart_Full");
    }
    public void HeartSprite(bool heartSet)
    {
        if(heartSet)
        {
            image.sprite = atlas.GetSprite("Heart_Full");

        }
        else
        {
            image.sprite = atlas.GetSprite("Heart_Empty");

        }
    }
}
