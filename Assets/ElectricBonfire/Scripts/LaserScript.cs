using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float Length
	{
		get
		{
			return length;
		}

		set
		{
			var t = Mathf.Clamp(value, 0.5f, float.MaxValue);
			length = t;
			Resize();
		}
	}

	[SerializeField]
	[Min(0.5f)]
	private float length;

    private void Resize()
	{
		var g = transform.GetChild(0);
		var middle = g.GetChild(1);
		var end = g.GetChild(2);

		var l = length - 0.5f;

		middle.localPosition = new Vector3(l / 2, 0, 0);
		middle.localScale = new Vector3(2 * l, 1, 1);
		end.localPosition = new Vector3(l + 0.25f, 0, 0);
	}

	private void OnValidate()
	{
		Resize();
	}
}
