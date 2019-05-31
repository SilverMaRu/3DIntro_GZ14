using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class SimpleHeath : MonoBehaviour
{
    public int maxHp = 100;

    private int currentHp = 100;

    private Collider coll;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        coll = GetComponent<Collider>();

        currentHp = maxHp;

        EventManager.BindingEvent<GameObject, GameObject>("HurtCharacter", OnHurtCharacter);

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected virtual void OnHurtCharacter(GameObject attacker, GameObject victim)
    {
        if (victim == gameObject)
        {
            currentHp--;
            EventManager.OnEvent("CharacterHpChanged", currentHp);
            if (currentHp <= 0)
            {
                EventManager.OnEvent("CharacterDie", gameObject);
            }
        }
    }

    protected virtual void StartSinking()
    {
        UnityEngine.AI.NavMeshAgent nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nav != null) nav.enabled = false;
        coll.enabled = false;
        Destroy(gameObject, 2f);
    }
}
