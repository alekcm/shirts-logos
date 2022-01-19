using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerButton : MonoBehaviour
{
    public ShirtManager ShirtManager;
    public int LayerNumber;
    public Image InnerColor;
    public Image[] AnotherButtons;
    public Sprite Selected, NotSelected;

    public bool IsActive = false;
    // Start is called before the first frame update
    public void SetActive()
    {
        for (int i = 0; i < AnotherButtons.Length; i++)
        {
            AnotherButtons[i].sprite = NotSelected;
            AnotherButtons[i].GetComponent<LayerButton>().IsActive = false;
        }
        ShirtManager.CurrentLayer = LayerNumber;
        for (int i = 0; i < ShirtManager.ColorButtons.Length; i ++)
            if (ShirtManager.ColorButtons[i].GetComponent<Image>().color == InnerColor.color)
            {
                ShirtManager.WhiteBox.position = ShirtManager.ColorButtons[i].transform.position;
            }

        IsActive = true;
        GetComponent<Image>().sprite = Selected;
    }
}
