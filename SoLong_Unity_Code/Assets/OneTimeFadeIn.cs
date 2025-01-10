using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeFadeIn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SceneTransition>().FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
