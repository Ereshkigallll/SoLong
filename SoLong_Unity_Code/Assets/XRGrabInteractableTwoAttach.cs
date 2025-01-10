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
        // ��ȡ˽���ֶ� m_WasKinematic �� FieldInfo
        wasKinematicField = typeof(XRGrabInteractable).GetField("m_WasKinematic", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    // ��¶һ�������������޸� m_WasKinematic ֵ
    public void SetWasKinematic(bool value)
    {
        if (wasKinematicField != null)
        {
            wasKinematicField.SetValue(this, value);
        }
    }

    // ��¶һ��������������ȡ m_WasKinematic ֵ
    public bool GetWasKinematic()
    {
        if (wasKinematicField != null)
        {
            return (bool)wasKinematicField.GetValue(this);
        }
        return false;
    }

    // ��д SetupRigidbodyGrab ������ȷ����ץȡʱ���� m_WasKinematic
    protected override void SetupRigidbodyGrab(Rigidbody rigidbody)
    {
        base.SetupRigidbodyGrab(rigidbody);
        rigidbody.isKinematic = (bool)wasKinematicField.GetValue(this);
    }

    // ��д SetupRigidbodyDrop ������ȷ���ڷ���ʱ���� m_WasKinematic
    protected override void SetupRigidbodyDrop(Rigidbody rigidbody)
    {
        base.SetupRigidbodyDrop(rigidbody);
        rigidbody.isKinematic = (bool)wasKinematicField.GetValue(this);
    }
}
