using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class MyPlayerShoot : MonoBehaviour
{
    // 射击频率
    [Header("射击频率")]
    public float shootF = 0.1f;
    // 射程
    [Header("射程")]
    public float range = 20f;

    private Transform gunBarrelTrans;
    private Light fireLight;
    private LineRenderer lineRend;

    private float lightTime = 0.05f;
    private float lastShootTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        gunBarrelTrans = transform.Find("GunBarrelEnd");
        fireLight = GetComponentInChildren<Light>(true);
        lineRend = GetComponentInChildren<LineRenderer>(true);
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time - lastShootTime>shootF)
        {
            Vector3 endPoint = gunBarrelTrans.forward * range;
            RaycastHit hit;
            if(Physics.Raycast(gunBarrelTrans.position, gunBarrelTrans.forward, out hit, range))
            {
                endPoint = hit.point - gunBarrelTrans.position;
                if(hit.transform.tag == "Enemy")
                {
                    EventManager.OnEvent("HurtCharacter", gameObject, hit.transform.gameObject);
                }
            }
            ResetLineRendererEndPoint(gunBarrelTrans.InverseTransformVector(endPoint));
            OpenLight();
            lastShootTime = Time.time;
        }
    }

    private void OpenLight()
    {
        fireLight.enabled = true;
        lineRend.enabled = true;
        Invoke("CloseLight", lightTime);
    }

    private void ResetLineRendererEndPoint(Vector3 endPoint)
    {
        lineRend.SetPosition(1, endPoint);
    }

    private void CloseLight()
    {
        fireLight.enabled = false;
        lineRend.enabled = false;
    }
}
