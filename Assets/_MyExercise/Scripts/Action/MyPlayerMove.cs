using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class MyPlayerMove : MonoBehaviour
{
    public float moveVelocity = 5f;

    private Character owner;
    private Rigidbody rgBody;
    private Transform cameraTrans;

    // 向量Y轴上的投影乘数
    private Vector3 projectionY = Vector3.right + Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponent<Character>();
        Debug.Log("owner = " + owner);
        rgBody = GetComponent<Rigidbody>();
        cameraTrans = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = GetMoveDirection();
        Vector3 oldPosition = transform.position;
        Vector3 newPosition = oldPosition + direction * moveVelocity * Time.fixedDeltaTime;
        rgBody.MovePosition(newPosition);
        SendEventMessage(oldPosition != newPosition);
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 forward = ((ICameraTarget)owner).cameraRoot.transform.forward;
        Vector3 right = cameraTrans.right;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        return (forward * v + right * h).normalized;
    }

    private void SendEventMessage(bool isMoving)
    {
        if (isMoving)
        {
            EventManager.OnEvent("CharacterMove", gameObject);
        }
        else
        {
            EventManager.OnEvent("CharacterIde", gameObject);
        }
    }
}
