// PortalToDungeon.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalToDungeon : MonoBehaviour
{
    private bool _playerInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInside = false;
    }

    private void Update()
    {
        if (_playerInside && Keyboard.current.eKey.wasPressedThisFrame)
            SceneTransition.LoadWorldMap();
    }
}