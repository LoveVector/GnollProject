using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class receivedash : MonoBehaviour
{
    Text dashText;
    public PlayerDash dash;
    // Start is called before the first frame update
    void Start()
    {
        dashText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        dashText.text = dash.dashAttempts.ToString() + " /3";
    }
}
