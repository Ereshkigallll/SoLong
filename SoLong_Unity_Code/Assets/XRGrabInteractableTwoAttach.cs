using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.parent.GetComponentInChildren<HandData>();
            if (handData.handType == HandData.HandModelType.Left)
            {
                attachTransform = leftAttachTransform;
                secondaryAttachTransform = rightAttachTransform;
            }
            else if (handData.handType == HandData.HandModelType.Right)
            {
                attachTransform = rightAttachTransform;
                secondaryAttachTransform = leftAttachTransform;
            }
        }
        base.OnSelectEntering(args);
    }

    private FieldInfo wasKinematicField;

    protected override void Awake()
    {
        base.Awake();
        // 获取私有字段 m_WasKinematic 的 FieldInfo
        wasKinematicField = typeof(XRGrabInteractable).GetField("m_WasKinematic", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    // 暴露一个公共方法来修改 m_WasKinematic 值
    public void SetWasKinematic(bool value)
    {
        if (wasKinematicField != null)
        {
            wasKinematicField.SetValue(this, value);
        }
    }

    // 暴露一个公共方法来获取 m_WasKinematic 值
    public bool GetWasKinematic()
    {
        if (wasKinematicField != null)
        {
            return (bool)wasKinematicField.GetValue(this);
        }
        return false;
    }

    // 重写 SetupRigidbodyGrab 方法，确保在抓取时设置 m_WasKinematic
    protected override void SetupRigidbodyGrab(Rigidbody rigidbody)
    {
        base.SetupRigidbodyGrab(rigidbody);
        rigidbody.isKinematic = (bool)wasKinematicField.GetValue(this);
    }

    // 重写 SetupRigidbodyDrop 方法，确保在放下时设置 m_WasKinematic
    protected override void SetupRigidbodyDrop(Rigidbody rigidbody)
    {
        base.SetupRigidbodyDrop(rigidbody);
        rigidbody.isKinematic = (bool)wasKinematicField.GetValue(this);
    }
}
