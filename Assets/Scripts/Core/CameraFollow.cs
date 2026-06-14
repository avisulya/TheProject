using UnityEngine;
 
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float     smoothSpeed = 8f;
    [SerializeField] private Vector3   offset = new Vector3(0, 12, -8);
 
    private void LateUpdate()
    {
        if (target == null) return;
        var desired   = target.position + offset;
        var smoothed  = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;
        transform.LookAt(target.position);
    }
}