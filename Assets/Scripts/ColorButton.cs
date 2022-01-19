using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor()
    {
        ShirtManager SM = GameObject.Find("ShirtManager").GetComponent<ShirtManager>();
        SM.SetColor(GetComponent<Image>().color);
        SM.WhiteBox.position = transform.position;
    }
}
