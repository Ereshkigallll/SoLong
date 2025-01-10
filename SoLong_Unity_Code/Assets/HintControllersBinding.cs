using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintControllersBinding : MonoBehaviour
{
    private GameObject hintControllerLeft, hintControllerRight;
    // Start is called before the first frame update
    void Start()
    {
        hintControllerLeft = this.transform.Find("XRControllerLeft").gameObject;
        hintControllerRight = this.transform.Find("XRControllerRight").gameObject;
        hintControllerLeft.transform.SetParent(GameObject.Find("Left Controller").transform);
        hintControllerRight.transform.SetParent(GameObject.Find("Right Controller").transform);
        
        hintControllerLeft.transform.localPosition = new Vector3(-0.0154f, 0, -0.05f);
        hintControllerLeft.transform.localRotation = Quaternion.Euler(0, 180, 0);
        hintControllerRight.transform.localPosition = new Vector3(0.0154f, 0, -0.05f);
        hintControllerRight.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestoryHintController()
    {
        StartCoroutine(DestroyHintControllerCoroutine());
    }

    IEnumerator DestroyHintControllerCoroutine()
    {
        yield return new WaitForSeconds(1.55f);
        Destroy(hintControllerLeft);
        Destroy(hintControllerRight);
    }
}
