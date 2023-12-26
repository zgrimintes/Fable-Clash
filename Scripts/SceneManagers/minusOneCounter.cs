using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minusOneCounter : MonoBehaviour
{
	OffFinghtManager offFinghtManager;

	private void Start()
	{
		offFinghtManager = GetComponentInParent<OffFinghtManager>();
	}

	public void minusOne(int n)
	{
		offFinghtManager.minusOneCountdown(n);
	}
}
