using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanFillScript : MonoBehaviour
{
    [SerializeField] Image image;
    int canMaxUses = 3;
    int canCurrentUses = 0;

    public void ReFill()
    {
        canCurrentUses = canMaxUses;
        UpdateImage();
    }

    public bool UseCan()
    {
        if(canCurrentUses == 0) { return false; }

        canCurrentUses -= 1;
        UpdateImage();
        return true;
    }

    void UpdateImage()
    {
        image.fillAmount = (float)canCurrentUses / canMaxUses;
    }
}
