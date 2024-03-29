using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MC_Mgr : MonoBehaviour
{
private Animator animator;
        private int velHorHash;
        private int attackHash;
        private int attackTriggerHash;

        private bool isAttacking;
        private bool isFacingRight = true;

        private float horizontalMotion;

        public Transform attackPoint;
        public LayerMask cookieLayers;
        public LayerMask raisinLayers;
        public float attackRange = 0.2f;

        private float timeAttackStep = 0.2f;
        private float timeAttackStarts = 0f;
        private bool isDead = false;

        private float deathTime;
        public GameObject loopHandle;

        void Start()
        {

            animator = GetComponent<Animator>();
            velHorHash = Animator.StringToHash("vel_hor");
            attackHash = Animator.StringToHash("attack");
            attackTriggerHash = Animator.StringToHash("attack_trigger");
            isAttacking = false;

        }

        void Update()
        {

            horizontalMotion = Input.GetAxis("Horizontal") * Time.deltaTime * 80.0f;

            

            if (horizontalMotion > 0.0f)
            {
                isFacingRight = true;
                transform.eulerAngles = new Vector3(0.0f,90.0f,0.0f);
                animator.SetFloat( velHorHash, 1.0f );
            }
            else if (horizontalMotion < 0.0f)
            {
                isFacingRight = false;
                transform.eulerAngles = new Vector3(0.0f,270.0f,0.0f);
                animator.SetFloat( velHorHash, 1.0f );
            }
            else
            {
                animator.SetFloat( velHorHash, 0.0f );
            }

            if ((isDead)&&(Time.time >= deathTime + 2.0f))
            {
                // We go to the Main Menu after dying
                SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex - 1 );
            }

            if ((isAttacking)&(Time.time >= timeAttackStarts + timeAttackStep))
            {
                Attack();
                isAttacking = false;
            }

            if ((!isAttacking) && (Input.GetKeyDown(KeyCode.Space)))
            {

                timeAttackStarts = Time.time;
                // Animation is played
                animator.ResetTrigger(attackTriggerHash);
                animator.SetTrigger(attackTriggerHash);
                isAttacking = true;
                
            }




        }


        void Attack()
        {
            // Detect cookie blocks in range of attack
            Collider[] hitCookies = Physics.OverlapSphere(attackPoint.position,
                                                          attackRange,
                                                          cookieLayers);

            foreach(Collider cookie in hitCookies)
            {
                //We give info about from where the character is attacking the cookie
                cookie.GetComponent<CookieBlockMgr>().TakeDamage( isFacingRight );
            }

            // Detect raisins in range of attack
            Collider[] hitRaisins = Physics.OverlapSphere(attackPoint.position,
                                                          attackRange,
                                                          raisinLayers);

            foreach(Collider raisin in hitRaisins)
            {
                raisin.GetComponent<RaisinMgr>().TakeDamage( );
                Debug.Log("A raisin is hit");
            }

        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
            {
                return;
            }
            Gizmos.DrawWireSphere( attackPoint.position, attackRange );
        }

        public void Die()
        {

            animator.SetLayerWeight(2, 1.0f);
            animator.SetTrigger("die_trigger");
            deathTime = Time.time;
            isDead = true;

            loopHandle = GameObject.Find("Loop");
            loopHandle.GetComponent<LoopMgr>().BlockMotion();


        }

}
