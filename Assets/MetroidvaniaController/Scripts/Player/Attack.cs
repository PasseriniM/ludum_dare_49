using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
	public float dmgValuePerSecond = 4;

	public GameObject throwableObject;
	public GameObject attackCheck;
	public GameObject laser;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool isTimeToCheck = false;

	private bool attack = false;
	private bool isAttacking = false;

	public GameObject cam;

	Gauges gauges;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		gauges = GetComponent<Gauges>();
	}

	public bool IsAttacking()
    {
		return isAttacking;
    }

	public void OnAttack(InputAction.CallbackContext context)
    {
		if (context.started && canAttack)
		{
			canAttack = false;
			attack = true;
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (attack && gauges.CanAttack())
		{
			isAttacking = true;
			laser.SetActive(true);
			animator.SetBool("IsAttacking", isAttacking);
			StartCoroutine(AttackCooldown());
			attack = false;
			gauges.OnAttack();
		}

		if (animator.GetBool("IsAttacking"))
		{
			//DoDamage();
		}
	}

	IEnumerator AttackCooldown()
	{
        while (animator.GetBool("IsAttacking"))
        { 
			yield return new WaitForSeconds(0.1f);
		}

        canAttack = true;
		isAttacking = false;
		laser.SetActive(false);
	}

    public void DoDamage()
	{
		dmgValuePerSecond = Mathf.Abs(dmgValuePerSecond);

		Collider2D attackCollider = attackCheck.GetComponent<Collider2D>();
		int numColliders = 10;
		Collider2D[] colliders = new Collider2D[numColliders];
		ContactFilter2D contactFilter = new ContactFilter2D();
		contactFilter.NoFilter();
		int hits = attackCollider.OverlapCollider(contactFilter,colliders);
		for (int i = 0; i < hits; i++)
        {
			if(colliders[i].gameObject.tag == "Enemy")
            {
				if (colliders[i].transform.position.x - transform.position.x < 0)
				{
					dmgValuePerSecond = -dmgValuePerSecond;
				}
				float dmg = dmgValuePerSecond * Time.fixedDeltaTime;
				colliders[i].gameObject.SendMessage("ApplyDamage", dmg);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
        }
	}
}
