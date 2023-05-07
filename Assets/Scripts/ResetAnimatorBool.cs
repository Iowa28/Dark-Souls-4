using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    // [SerializeField]
    // private string targetBool;
    // [SerializeField]
    // private bool status;
    [SerializeField]
    private string[] targetsBool;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (string target in targetsBool)
        {
            animator.SetBool(target, false);
        }
    }
}
