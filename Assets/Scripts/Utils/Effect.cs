using UnityEngine;

public class Effect : MonoBehaviour
{

    [SerializeField] private Animator animator;

    void Start()
    {
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
    }


}
