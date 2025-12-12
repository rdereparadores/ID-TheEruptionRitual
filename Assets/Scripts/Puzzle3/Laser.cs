using UnityEngine;

public class Laser : MonoBehaviour
{
    public int maxBounces = 5;

    private LineRenderer _lineRenderer;
    
    [SerializeField]
    private Transform startPoint;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, startPoint.position);
    }

    private void LateUpdate()
    {
        Physics.SyncTransforms();
        CastLaser(transform.position, transform.up);
    }

    private void CastLaser(Vector3 position, Vector3 direction)
    {
        _lineRenderer.SetPosition(0, startPoint.position);

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 300, 1))
            {
                position = hit.point;
                direction = Vector3.Reflect(direction, hit.normal);
                _lineRenderer.SetPosition(i + 1, hit.point);

                if (hit.transform.CompareTag("DoorLock"))
                {
                    hit.collider.GetComponent<DoorLock>().Unlock();
                    var dialogueTrigger = hit.collider.GetComponent<DoorLockDialogueTrigger>();
                    if (dialogueTrigger != null)
                    {
                        dialogueTrigger.Play();
                    }
                }
                
                if (!hit.transform.CompareTag("Mirror"))
                {
                    for (int j = i + 1; j <= maxBounces; j++)
                    {
                        _lineRenderer.SetPosition(j, hit.point);
                    }

                    break;
                }
            }
        }
    }
}
