using TMPro;
using UnityEngine;

public class LevelTimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("1: " + LevelManager.Instance.levelTimer);
        //Debug.Log("2: " + (int)LevelManager.Instance.levelTimer);
        //Debug.Log("3: " + (int)LevelManager.Instance.levelTimer / 60);
        //Debug.Log("4: " + (int)LevelManager.Instance.levelTimer % 60);

        int min = ((int)LevelManager.Instance.levelTimer) / 60;
        string minStr = "" + min;
        if (min < 10) minStr = "0" + minStr;

        int sec = ((int)LevelManager.Instance.levelTimer) % 60;
        string secStr = "" + sec;
        if (sec < 10) secStr = "0" + secStr;

        text.text = minStr + ":" + secStr;
    }
}