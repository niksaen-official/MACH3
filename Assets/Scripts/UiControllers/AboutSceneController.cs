using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutSceneController : BaseUiController
{

    public void OpenUrl(string url)
    {
       Application.OpenURL(url);
    }
}
