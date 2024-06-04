using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWebsite : MonoBehaviour
{
    [SerializeField] string url;

    public void OpenURL()
    {
        Application.OpenURL(url);
    }
}