using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HideText : MonoBehaviour
{
    bool isHid = false;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            hideInfo();
        }
    }

    void hideInfo()
    {
        if (!isHid)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = "(press H to show)";
            gameObject.GetComponent<TextMeshProUGUI>().verticalAlignment = VerticalAlignmentOptions.Top;
        }
        else
        {
            gameObject.GetComponent<TextMeshProUGUI>().verticalAlignment = VerticalAlignmentOptions.Middle;
            gameObject.GetComponent<TextMeshProUGUI>().text = "Controls: \r\nA/D - Left/Right\r\nDouble A/D - Dash\r\nSPACE - Jump\r\nJ - Normal Attack\r\nK-Heavy Attack\r\nl - Ranged Attack\r\nI - Magic Attack\r\nLSHIFT - Special Attack\r\nESC - Pause\r\n\r\n(press H to Hide)";
        }

        isHid = !isHid;
    }
}
