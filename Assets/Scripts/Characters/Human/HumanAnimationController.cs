using UnityEngine;

namespace Characters.Human
{
    public class HumanAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int Speed = Animator.StringToHash("Speed");

        public void SetSpeed(float speed)
        {
            animator.SetFloat(Speed, speed);
        }
    }
}