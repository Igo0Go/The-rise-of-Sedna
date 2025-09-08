using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPC_Interaction : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private LayerMask interactiveMask;
    [SerializeField, Min(1)]
    private float range = 2f;

    public event Action<string, string> NewInteractiveObjectFound;
    public event Action TakeInteractiveObject;
    public event Action LostInteractiveObject;

    private Collider lastCollider;
    private InteractiveObject lastInteractiveObject;

    private void Update()
    {
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo, 
            range, interactiveMask))
        {
            if(lastCollider != hitInfo.collider)
            {
                lastCollider = hitInfo.collider;

                if(lastCollider.TryGetComponent(out lastInteractiveObject))
                {
                    (string name, string action) data = lastInteractiveObject.GetData();
                    NewInteractiveObjectFound?.Invoke(data.name, data.action);
                }
            }
        }
        else
        {
            LostLastInteractiveObject();
        }
    }

    private void LostLastInteractiveObject()
    {
        lastCollider = null;
        lastInteractiveObject = null;
        LostInteractiveObject?.Invoke();
    }

    public void OnUseInput()
    {
        if(lastInteractiveObject != null)
        {
            lastInteractiveObject.Use();
            LostLastInteractiveObject();
            TakeInteractiveObject?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
