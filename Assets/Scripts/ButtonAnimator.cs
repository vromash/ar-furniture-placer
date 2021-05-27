using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void DeleteStateUnset()
    {
        animator.SetBool("delete", false);
    }

    public void SaveStateUnset()
    {
        animator.SetBool("save", false);
    }
}
