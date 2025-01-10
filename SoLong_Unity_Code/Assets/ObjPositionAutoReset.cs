using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjPositionAutoReset : MonoBehaviour
{
    [SerializeField] AudioClip resetSound;
    private Vector3 origPosition;
    private Quaternion origRotation;
    public GameObject targetTransform;
    private float timer = 0;
    public bool needReset = false;
    [SerializeField] private float timeThreshold = 3f;

    void Start()
    {
        origPosition = this.transform.position;
        origRotation = this.transform.rotation;
        timer = 0;

        GetComponent<XRGrabInteractable>().selectEntered.AddListener(GrabCheck);
        GetComponent<XRGrabInteractable>().selectExited.AddListener(ReleaseCheck);
    }

    void Update()
    {
        //Debug.Log(timer);
        if (needReset && !GetComponent<XRGrabInteractable>().isSelected && transform.position!=origPosition && transform.rotation!=origRotation)
        {
            //Debug.Log("Object position changed");
            timer += Time.deltaTime;
            if (timer > timeThreshold)
            {
                Debug.Log("Resetting object position");
                transform.position = origPosition;
                transform.rotation = origRotation;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.VelocityTracking;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                
                //create a audiosource if there is none
                if (GetComponent<AudioSource>() == null)
                {
                    gameObject.AddComponent<AudioSource>();
                }
                GetComponent<AudioSource>().playOnAwake = false;
                GetComponent<AudioSource>().clip = resetSound;
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().volume = 0.8f;
                GetComponent<AudioSource>().spatialize = true;
                GetComponent<AudioSource>().spatializePostEffects = true;
                GetComponent<AudioSource>().outputAudioMixerGroup = targetTransform.GetComponent<AudioSource>().outputAudioMixerGroup;
                GetComponent<AudioSource>().Play();
                timer = 0;
                //needReset = false; // 重置后将needReset设置为false
            }
        }
    }

    private void GrabCheck(SelectEnterEventArgs args)
    {
        timer = 0;
        needReset = false; // 当对象被抓取时，不需要重置
    }

    private void ReleaseCheck(SelectExitEventArgs args)
    {
        needReset = true; // 当对象被释放时，开始计时重置
    }

    private void OnDisable()
    {
        GetComponent<XRGrabInteractable>().selectEntered.RemoveListener(GrabCheck);
        GetComponent<XRGrabInteractable>().selectExited.RemoveListener(ReleaseCheck);
    }

    public void SetTarget(GameObject target)
    {
        targetTransform = target;
        origPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        origRotation = new Quaternion(target.transform.rotation.x, target.transform.rotation.y, target.transform.rotation.z, target.transform.rotation.w);
    }
}
