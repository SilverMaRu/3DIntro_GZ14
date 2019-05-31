using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const float MAXANGLESPEED = 300;
    private const float MINANGLESPEED = 30;
    private const int MAXCHUSHIONLEVEL = 100;

    public Transform followTargetTrans;
    public Transform lookAtTargetTrans;
    public bool useLookAt = false;

    // 镜头移动时 X轴的缓冲等级
    [Header("镜头移动时 X轴的缓冲等级")]
    [Range(0, MAXCHUSHIONLEVEL)]
    public int cushionPitch = 0;
    // 镜头移动时 Y轴的缓冲等级
    [Header("镜头移动时 Y轴的缓冲等级")]
    [Range(0, MAXCHUSHIONLEVEL)]
    public int cushionYaw = 0;

    // Y轴旋转角 左右旋转
    public float yawAngle { get { return transform.localRotation.eulerAngles.y; } }
    // X轴旋转角 上下旋转
    public float pitchAngle { get { return mainCameraTrans.localRotation.eulerAngles.x; } }

    private Transform rigTrans;
    private Transform mainCameraTrans;

    private Vector3 rightZero = Vector3.up + Vector3.forward;
    private Vector3 upZero = Vector3.right + Vector3.forward;

    private int cushionYawBack;
    private int cushionPitchBack;
    private Vector3 rigPositionBack;
    private float yawAngleBack;
    private float pitchAngleBack;

    private bool isResetting = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rigTrans = transform.GetChild(0);
        mainCameraTrans = Camera.main.transform;
        if (followTargetTrans != null)
        {
            ICameraTarget target = followTargetTrans.GetComponent<ICameraTarget>();
            if (target != null) target.cameraRoot = this;
        }
        if (lookAtTargetTrans != null && useLookAt)
        {
            OnLookAt(lookAtTargetTrans);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RootFollow();
        if (useLookAt && !isResetting)
        {
            LookAt();
        }
        else if (!useLookAt)
        {
            ChangeCameraByMouseInput();
        }
        if (Input.GetKeyDown(KeyCode.L)) OnLookAt(lookAtTargetTrans, 2);
    }

    private void RootFollow()
    {
        transform.position = followTargetTrans.position;
    }

    private void ChangeCameraByMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float yawAngle = mouseX * 2;
        float pitchAngle = -mouseY * 5;
        Yaw(yawAngle);
        Pitch(pitchAngle);
    }

    public void Yaw(float angle)
    {
        YawTo(transform.localRotation.eulerAngles.y + angle);
    }

    public void YawTo(float angle)
    {
        Quaternion newRotation = Quaternion.Euler(Vector3.Scale(transform.localRotation.eulerAngles, upZero) + Vector3.up * angle);

        if (cushionYaw <= 0)
        {
            transform.localRotation = newRotation;
        }
        else
        {
            //transform.localRotation = Quaternion.Lerp(transform.rotation, newRotation, cushionYaw * Time.deltaTime);
            float angleSpeed = Mathf.Lerp(MAXANGLESPEED, MINANGLESPEED, cushionYaw / MAXCHUSHIONLEVEL);
            transform.localRotation = Quaternion.RotateTowards(transform.rotation, newRotation, angleSpeed * Time.deltaTime);
        }
    }

    public void Pitch(float angle)
    {
        float newAngleX = mainCameraTrans.localRotation.eulerAngles.x + angle;
        if (newAngleX < 180)
        {
            newAngleX = Mathf.Min(newAngleX, 90);
        }
        else
        {
            newAngleX = Mathf.Max(newAngleX, 270);
        }
        PitchTo(newAngleX);
    }

    public void PitchTo(float angle)
    {
        Quaternion newRotation = Quaternion.Euler(Vector3.Scale(mainCameraTrans.localRotation.eulerAngles, rightZero) + Vector3.right * angle);
        if (cushionPitch <= 0)
        {
            mainCameraTrans.localRotation = newRotation;
        }
        else
        {
            float angleSpeed = Mathf.Lerp(MAXANGLESPEED, MINANGLESPEED, cushionPitch / MAXCHUSHIONLEVEL);
            mainCameraTrans.localRotation = Quaternion.RotateTowards(mainCameraTrans.localRotation, newRotation, angleSpeed * Time.deltaTime);
        }
    }

    public void OnLookAt(Transform lookAtTargetTrans, float lookAtTime)
    {
        if (useLookAt) return;
        OnLookAt(lookAtTargetTrans);
        StartCoroutine(Delay(OffLookAt, lookAtTime));
    }

    public void OnLookAt(Transform lookAtTargetTrans)
    {
        if (useLookAt) return;
        this.lookAtTargetTrans = lookAtTargetTrans;
        useLookAt = true;
        // 记录当前数值
        cushionYawBack = cushionYaw;
        cushionPitchBack = cushionPitch;
        //Vector3 rootEulerAngleBack = transform.localRotation.eulerAngles;
        rigPositionBack = rigTrans.localPosition;
        yawAngleBack = yawAngle;
        pitchAngleBack = pitchAngle;
        //Vector3 rigEulerAngleBack = rigTrans.localRotation.eulerAngles;
        //Vector3 mainCameraEulerAngleBack = mainCameraTrans.localRotation.eulerAngles;
        //  修改缓冲等级
        cushionYaw = 50;
        cushionPitch = 70;
    }

    public void OffLookAt()
    {
        StartCoroutine(Step(ResetCameraAngle, ResetCushion));
    }

    private bool ResetCameraAngle()
    {
        isResetting = true;
        rigTrans.localPosition = rigPositionBack;
        YawTo(yawAngleBack);
        PitchTo(pitchAngleBack);
        bool isDone = Mathf.Abs(yawAngle - yawAngleBack) < 1 && Mathf.Abs(pitchAngle - pitchAngleBack) < 0.5;
        return isDone;
    }

    private bool ResetCushion()
    {
        cushionYaw = cushionYawBack;
        cushionPitch = cushionPitchBack;
        useLookAt = false;
        isResetting = false;
        return true;
    }

    private void LookAt()
    {
        if (lookAtTargetTrans == null) return;
        // 计算目标方向
        Vector3 rootToLookAtTarget = (lookAtTargetTrans.position - transform.position).normalized;
        float rootToLookAtTargetYawAngle = Quaternion.LookRotation(rootToLookAtTarget).eulerAngles.y;
        YawTo(rootToLookAtTargetYawAngle);

        Vector3 cameraToLookAtTarget = (lookAtTargetTrans.position - mainCameraTrans.position).normalized;
        float camearToLookAtTargetPitchAngle = Quaternion.LookRotation(cameraToLookAtTarget).eulerAngles.x;
        PitchTo(camearToLookAtTargetPitchAngle);
    }

    private IEnumerator Delay(System.Action delayAction, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        delayAction();
    }

    private IEnumerator Step(params System.Func<bool>[] steps)
    {
        int i = 0;
        while (i < steps.Length)
        {
            if (steps[i]())
            {
                i++;
            }
            yield return null;
        }
    }

    private IEnumerator Loop(System.Func<bool> loopFunc)
    {
        while (!loopFunc())
        {
            yield return null;
        }
    }

    //    private IEnumerator Loop(System.Action loopAction, System.Func<bool> isDone)
    //    {
    //        while (!isDone())
    //        {
    //            loopAction();
    //            yield return null;
    //        }
    //    }
}
