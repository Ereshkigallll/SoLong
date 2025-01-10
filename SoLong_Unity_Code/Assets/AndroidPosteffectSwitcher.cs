using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AndroidPosteffectSwitcher : MonoBehaviour
{
    [SerializeField] Volume volumeToSwitch;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        volumeToSwitch.enabled = false;
#endif
    }

}
