using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class RecordControllerInput : MonoBehaviour {

    private string[] joystickNames;
    private float xAxis, yAxis, gorkHorizontal, clydeHorizontal, UIHorizontal, gorkVertical, clydeVertical, UIVertical;
    private List<string> lastPressedKeys = new List<string>();

    private void Start()
    {
        foreach (string a in Input.GetJoystickNames())
        {
             print(a);        
        }
    }

    void Update () {
        joystickNames = Input.GetJoystickNames();
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        gorkHorizontal = Input.GetAxis("GorkHorizontal");
        gorkVertical = Input.GetAxis("GorkVertical");
        clydeHorizontal = Input.GetAxis("ClydeHorizontal");
        clydeVertical = Input.GetAxis("ClydeVertical");
        UIHorizontal = Input.GetAxis("UIHorizontal");
        UIVertical = Input.GetAxis("UIVertical");
        
        
        foreach ( KeyCode curKey in Enum.GetValues(typeof(KeyCode)) )
        {
            if (Input.GetKeyDown(curKey))
            {
                lastPressedKeys.Add(curKey.ToString());
                if (lastPressedKeys.Count > 10)
                    lastPressedKeys.RemoveAt(0);
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Joysticks:");
        foreach (var curName in joystickNames)
            GUILayout.Label(string.Format("   {0}", curName));
        GUILayout.Label(string.Format("Axes: (x: {0}, y: {1}\n gorkX: {2}, gorkY: {3}\n clydeX: {4}, clydeY: {5}\n UIX: {6}, UIY: {7})", xAxis, yAxis, gorkHorizontal, gorkVertical, clydeHorizontal, clydeVertical, UIHorizontal, UIVertical));

        GUILayout.Label("Last pressed keys:");
        foreach (var curKeyName in lastPressedKeys)
            GUILayout.Label(string.Format("   {0}", curKeyName));
    }
}