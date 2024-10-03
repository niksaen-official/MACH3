using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseUiController : MonoBehaviour
{
    public virtual void BackButtonOnClick() => SceneManager.LoadScene(0);
}
