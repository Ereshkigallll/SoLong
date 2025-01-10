using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrabHandPose : MonoBehaviour
{
    public HandData leftHandPose;
    public HandData rightHandPose;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;
    private Quaternion[] startingFingersRotations;
    private Quaternion[] finalFingersRotations;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractableTwoAttach grabInteractable = GetComponent<XRGrabInteractableTwoAttach>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(ReleasePose);
        leftHandPose.gameObject.SetActive(false);
        rightHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.parent.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;
            if (handData.handType == HandData.HandModelType.Left)
            {
                SetHandDataValues(handData, leftHandPose);
                SetHandData(handData, finalHandPosition, finalHandRotation, finalFingersRotations);
                handData.root.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                leftHandPose.gameObject.SetActive(true);
            }
            else if (handData.handType == HandData.HandModelType.Right)
            {
                SetHandDataValues(handData, rightHandPose);
                SetHandData(handData, finalHandPosition, finalHandRotation, finalFingersRotations);
                handData.root.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                rightHandPose.gameObject.SetActive(true);
            }

        }
    }

    public void ReleasePose(SelectExitEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.parent.GetComponentInChildren<HandData>();

            handData.root.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            leftHandPose.gameObject.SetActive(false);
            rightHandPose.gameObject.SetActive(false);

            handData.animator.enabled = true;
            SetHandData(handData, startingHandPosition, startingHandRotation, startingFingersRotations);
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x, h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x, h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);
        //finalHandPosition = new Vector3(h2.root.localPosition.x / (h2.root.localScale.x * h2.root.parent.parent.localScale.x), h2.root.localPosition.y / (h2.root.localScale.y * h2.root.parent.parent.localScale.y), h2.root.localPosition.z / (h2.root.localScale.z * h2.root.parent.parent.localScale.z));

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingersRotations = new Quaternion[h1.fingers.Length];
        finalFingersRotations = new Quaternion[h2.fingers.Length];

        for (int i = 0; i < h1.fingers.Length; i++)
        {
            startingFingersRotations[i] = h1.fingers[i].localRotation;
            finalFingersRotations[i] = h2.fingers[i].localRotation;
        }
    }

    public void SetHandData(HandData handData, Vector3 handPosition, Quaternion handRotation, Quaternion[] fingersRotations)
    {
        handData.root.localPosition = handPosition;
        //handData.root.position = handPosition;
        handData.root.localRotation = handRotation;

        for (int i = 0; i < handData.fingers.Length; i++)
        {
            handData.fingers[i].localRotation = fingersRotations[i];
        }
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Hand Poses/Mirror Pose From Left to Right")]

    public static void MirrorPoseL2R()
    {
        GrabHandPose grabHandPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
        if (grabHandPose != null)
        {
            grabHandPose.MirrorPose(grabHandPose.leftHandPose, grabHandPose.rightHandPose);
        }
    }

    [MenuItem("Tools/Hand Poses/Mirror Pose From Right to Left")]

    public static void MirrorPoseR2L()
    {
        GrabHandPose grabHandPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
        if (grabHandPose != null)
        {
            grabHandPose.MirrorPose(grabHandPose.rightHandPose, grabHandPose.leftHandPose);
        }
    }
#endif
    public void MirrorPose(HandData poseToMirror, HandData mirroredPose)
    {
        mirroredPose.root.localPosition = new Vector3(-poseToMirror.root.localPosition.x, poseToMirror.root.localPosition.y, poseToMirror.root.localPosition.z);
        mirroredPose.root.localRotation = new Quaternion(poseToMirror.root.localRotation.x, -poseToMirror.root.localRotation.y, -poseToMirror.root.localRotation.z, poseToMirror.root.localRotation.w);

        for (int i = 0; i < poseToMirror.fingers.Length; i++)
        {
            mirroredPose.fingers[i].localRotation = new Quaternion(poseToMirror.fingers[i].localRotation.x, -poseToMirror.fingers[i].localRotation.y, -poseToMirror.fingers[i].localRotation.z, poseToMirror.fingers[i].localRotation.w);
        }
    }
}