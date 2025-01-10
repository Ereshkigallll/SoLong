using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToCredit : MonoBehaviour
{
    public void GoToCreditScene()
    {
        GameObject.Find("SceneManager").GetComponent<SceneTransition>().GoToSceneAsyc(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
