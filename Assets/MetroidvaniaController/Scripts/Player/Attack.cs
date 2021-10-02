using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
	public float dmgValue = 4;
	public float laserRange = 10;

	public GameObject throwableObject;
	public Transform attackCheck;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool isTimeToCheck = false;

	private bool attack = false;

	public GameObject cam;

	Gauges gauges;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		gauges = GetComponent<Gauges>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
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
			animator.SetBool("IsAttacking", true);
			StartCoroutine(AttackCooldown());
			attack = false;
			gauges.OnAttack();
		}
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.25f);
		canAttack = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
/*		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
        }*/
		
		Vector3 direction = new Vector3(System.Math.Sign(transform.localScale.x), 0f,0f);
		RaycastHit2D[] hits = Physics2D.RaycastAll(attackCheck.position, direction, laserRange);
		for (int i = 0; i < hits.Length; i++)
        {
			if(hits[i].collider.gameObject.tag == "Enemy")
            {
				if (hits[i].collider.transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				hits[i].collider.gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
        }
	}
}
