using UnityEngine;
using System.Collections;

public class SpaceCraft : MonoBehaviour
{
	public Squadron squadron;
	public int id;

	public static float applyRuleDeltaTime = 0.01f;
	float lastApplyRule = 0f;

	public Vector3 lastVelocity;

	GameObject[] spaceCrafts;

	public static float neighbourhoodRadius = 5f;
	public static float safeRadius = 4f;

	public static float rotationSpeed = 3f;
	public static float maxSpeed = 5f;

	public static float speedFactor = 2f;

	public static float pursuitFactor = 2f;
	public static float cohensionFactor = 10f;
	public static float alignmentFactor = 0.8f;

	public bool goalReached = false;
	public static float goalRadius = 5f;
	public float lastGoalReach = 0f;

	public static float targetFieldRadius = 10f;
	public static float targetRadius = 2f;

	public GameObject starShipEnterprise;

	void Start()
	{
		spaceCrafts = squadron.spaceCrafts;
		lastVelocity = Vector3.zero;

		starShipEnterprise = GameObject.FindGameObjectWithTag("StarShip");
	}

	void Update()
	{
		Vector3 targetVelocity;

		lastApplyRule += Time.deltaTime;
		if (lastApplyRule > applyRuleDeltaTime)
		{
			targetVelocity = applyRuleVelocity();

			rotateTo(targetVelocity);
			moveForward(targetVelocity);

			lastApplyRule -= applyRuleDeltaTime;
		}
	}

	private Vector3 applyRuleVelocity()
	{
		Vector3 cohensionVelocity = Vector3.zero, aligmentVelocity = Vector3.zero, pursuitVelocity;

		Vector3 targetVelocity;

		int nearbySpaceCraftCount = 0;

		pursuitVelocity = (starShipEnterprise.transform.position - transform.position).normalized * pursuitFactor;

		foreach (GameObject spaceCraft in spaceCrafts)
		{
			float distance = Vector3.Distance(transform.position, spaceCraft.transform.position);
			if (distance < neighbourhoodRadius && spaceCraft != this.gameObject)
			{
				nearbySpaceCraftCount++;

				Vector3 direction = (spaceCraft.transform.position - transform.position).normalized;

				cohensionVelocity += (distance - safeRadius) * direction * cohensionFactor;
				
				aligmentVelocity += spaceCraft.GetComponent<SpaceCraft>().lastVelocity;
			}
		}

		if (goalReached)
		{
			Vector3 goalDistanceVector = transform.position -starShipEnterprise.transform.position;

			pursuitVelocity = transform.up * 2f - goalDistanceVector.normalized / (targetRadius - Vector3.Magnitude(goalDistanceVector)) * 5;

			if (lastGoalReach > 6f)
			{
				goalReached = false;
				lastGoalReach = 0f;
			}
			else
			{
				lastGoalReach += Time.deltaTime;
			}
		}
		else
		{
			if (Vector3.Distance(transform.position, starShipEnterprise.transform.position) < targetFieldRadius)
			{
				goalReached = true;
			}
		}

		//toTargetVelocity = Vector3.zero;

		if (nearbySpaceCraftCount > 0)
		{
			targetVelocity = (cohensionVelocity + aligmentVelocity * alignmentFactor) / nearbySpaceCraftCount + pursuitVelocity;
		}
		else
		{
			targetVelocity = pursuitVelocity;
		}

		return targetVelocity;
	}

	void rotateTo(Vector3 direction)
	{
		float rotateAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, rotateAngle), rotationSpeed * Time.deltaTime);
	}

	void moveForward(Vector3 targetVelocity)
	{
		float angle = Vector3.Angle(transform.up, targetVelocity);
		float magnitude = Vector3.Magnitude(targetVelocity);

		float speed = magnitude / Mathf.Pow(1.005f, angle);

		if (speed > maxSpeed)
		{
			speed = maxSpeed;
		}

		transform.Translate(0, Time.deltaTime * speed * speedFactor, 0);

		lastVelocity = transform.up * speed;
	}
}