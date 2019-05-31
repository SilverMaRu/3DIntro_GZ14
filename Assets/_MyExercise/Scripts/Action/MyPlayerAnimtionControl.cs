using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class MyPlayerAnimtionControl : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        EventManager.BindingEvent<GameObject>("CharacterMove", OnCharacterMove);
        EventManager.BindingEvent<GameObject>("CharacterIde", OnCharacterIde);
        EventManager.BindingEvent<GameObject>("CharacterDie", OnCharacterDie);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCharacterMove(GameObject moveCharacterGameObj)
    {
        if (moveCharacterGameObj == gameObject)
        {
            anim.SetBool("IsMoving", true);
        }
    }

    private void OnCharacterIde(GameObject ideCharacterGameObj)
    {
        if (ideCharacterGameObj == gameObject)
        {
            anim.SetBool("IsMoving", false);
        }
    }

    private void OnCharacterDie(GameObject deathCharacterGameObj)
    {
        if (deathCharacterGameObj == gameObject)
        {
            anim.SetTrigger("Die");
        }
    }
}
