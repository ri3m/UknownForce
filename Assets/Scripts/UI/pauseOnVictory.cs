using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
public class pauseOnVictory : UIManager
{
    [SerializeField] GameObject music;
    [SerializeField] GameObject VictoryMusic;
    public void VictoryPause()
    {
        CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.Menu);
        //GoToPage(2);
        Time.timeScale = 0;
        music.SetActive(false);
        VictoryMusic.SetActive(true);
    }
}
