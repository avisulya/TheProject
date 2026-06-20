// Assets/Scripts/Dungeon/DoorTP.cs
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DoorTP : MonoBehaviour
{
    [SerializeField] private float arrivalOffset = 1.5f;

    [HideInInspector] public DoorTP connectedDoorTP;
    private bool _cooldown;

    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (connectedDoorTP == null || _cooldown) return;
        if (!other.TryGetComponent<CharacterController>(out var cc)) return;

        var targetPos = connectedDoorTP.transform.position
                       + connectedDoorTP.transform.forward * connectedDoorTP.arrivalOffset;

        cc.enabled = false;                 // avoid CharacterController fighting the teleport
        other.transform.position = targetPos;
        cc.enabled = true;

        StartCoroutine(CooldownRoutine());
        connectedDoorTP.StartCoroutine(connectedDoorTP.CooldownRoutine());
    }

    public IEnumerator CooldownRoutine()
    {
        _cooldown = true;
        yield return new WaitForSeconds(0.5f);
        _cooldown = false;
    }
}