using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetTrans;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = targetTrans.position - transform.position;
    }

    private void LateUpdate()
    {
        transform.position = targetTrans.position - offset;
    }
}
