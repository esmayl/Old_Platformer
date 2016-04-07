using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour
{
    public Text debugText;

    void Start()
    {
        if (debugText == null)
        {
            debugText = GameObject.Find("DebugText").GetComponent<Text>();
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public ActionType CheckTouch(Touch touch)
    {
        if (touch.position.x > Screen.width / 4 * 3 && touch.position.x < Screen.width)
        {
            debugText.text = "Jump";
            return ActionType.Jump;
        }

        if (touch.position.x > Screen.width / 4 * 2 && touch.position.x < Screen.width/4*3 && touch.position.y < Screen.height/2)
        {
            debugText.text = "Attack1";
            return ActionType.Attack1;
        }

        if (touch.position.x > Screen.width/4*2 && touch.position.x < Screen.width/4*3 &&
            touch.position.y > (Screen.height/2f) + 1)
        {
            debugText.text = "Attack2";
            return ActionType.Attack2;
        }

        if (touch.position.x < Screen.width / 6f)
        {
            debugText.text = "Move left";
            return ActionType.MoveL;
        }
        if (touch.position.x > Screen.width / 6f && touch.position.x < Screen.width/6f*2)
        {
            debugText.text = "Move right";
            return ActionType.MoveR;
        }


        return ActionType.NoAction;

    }
}
