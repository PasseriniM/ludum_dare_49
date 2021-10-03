using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamTrapScript : MonoBehaviour
{
	private Collider2D collider2Dcustom;

	public GameObject player;
	private Transform playerTransform;
	private Gauges gauges;

	public float increaseSteamPerSecond = 5;

	private void Awake()
	{
		collider2Dcustom = GetComponent<Collider2D>();
		gauges = player.GetComponent<Gauges>();
		playerTransform = player.GetComponent<Transform>();
	}

	private void FixedUpdate()
	{
		if (collider2Dcustom.OverlapPoint(playerTransform.position))
		{
			gauges.HeatUp(Time.fixedDeltaTime * increaseSteamPerSecond);
		}
	}
}
