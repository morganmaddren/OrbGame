using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IOrbCenter
{
	
}

public class OrbCenter : MonoBehaviour, IOrbCenter
{
    IOrb orb;

	void Awake()
	{
        orb = this.transform.parent.GetComponent<Orb>();
	}

	void Start()
	{
		
	}
	
	void Update()
	{
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IBumper bumper = collision.AsBumper();
        if (bumper != null)
        {
            bumper.OnCenterBump(orb);
        }
    }
}