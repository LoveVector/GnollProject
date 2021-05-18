using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    public GameObject helpPanel;
    bool boolPress;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            boolPress = !boolPress;
            helpPanel.SetActive(boolPress);
        }
    }
}
