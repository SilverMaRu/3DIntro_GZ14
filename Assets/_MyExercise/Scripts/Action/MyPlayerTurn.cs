using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerTurn : MonoBehaviour
{
    public enum Mode
    {
        FollowMouse,
        FollowCamera
    }
    public Mode mode = Mode.FollowMouse;

    private Character owner;
    private Rigidbody rgBody;
    private Transform cameraTrans;

    private float tanForward;

    // 向量Y轴上的投影乘数
    private Vector3 projectionY = Vector3.right + Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponent<Character>();
        rgBody = GetComponent<Rigidbody>();
        cameraTrans = Camera.main.transform;
        tanForward = Mathf.Tan(Camera.main.transform.rotation.eulerAngles.x * Mathf.PI / 180);
    }

    // Update is called once per frame
    void Update()
    {
        if(mode == Mode.FollowCamera)
        {
            TurnFollowCamera();
        }
    }

    private void FixedUpdate()
    {
        if (mode == Mode.FollowMouse)
        {
            TurnFollowMouse();
        }
    }

    private void TurnFollowMouse()
    {
        Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePointInFloor = Vector3.Scale(mousePointInWorld, projectionY);
        Vector3 forwardOffset = Vector3.Scale(cameraTrans.forward, projectionY).normalized * (mousePointInWorld.y / tanForward);
        Vector3 screenProjectToWorld = mousePointInFloor + forwardOffset;
        Vector3 toMPIF = screenProjectToWorld - transform.position;
        rgBody.MoveRotation(Quaternion.LookRotation(toMPIF));
    }

    private void TurnFollowCamera()
    {
        transform.forward = ((ICameraTarget)owner).cameraRoot.transform.forward;
        //rgBody.MoveRotation(((ICameraTarget)owner).cameraRoot.transform.rotation);
    }
}
