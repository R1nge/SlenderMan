using UnityEngine;

namespace Characters.Human
{
    public class HumanAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

        public void SetSpeed(float speed)
        {
            animator.SetFloat(Speed, speed);
        }
        
        public void SetDirection(Vector2 dir)
        {
            animator.SetFloat(Vertical, dir.x);
            animator.SetFloat(Horizontal, dir.y);
        }
    }
}