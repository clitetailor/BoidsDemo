using UnityEngine;
using System.Collections;

public class Squadron : MonoBehaviour
{
	public GameObject spaceCraftPrefab;

	public int spaceCraftNum = 1;
	public int sectionSize = 36;

	public GameObject[] spaceCrafts;

	public float rotationSpeed = 0.4f;
	public float maxSpeed = 3f;

	public float maintainedDistance = 5f;
	public float neighbourDistance = 8f;

	public bool matchVelocity = true;
	public bool chaseTarget = true;
	public bool normalizeToTargetVelocity = true;

	void Start ()
	{
		spaceCrafts = new GameObject[spaceCraftNum];

		for (int i = 0; i < spaceCraftNum; ++i)
		{
			Vector3 position = new Vector3((i - 8) * 2, 0, 0);
			Quaternion rotationQuatenion = Quaternion.identity;

			//Vector3 position = new Vector3(Random.Range(-sectionSize, sectionSize), Random.Range(-sectionSize, sectionSize), 0);

			spaceCrafts[i] = (GameObject) Instantiate(spaceCraftPrefab, position,  rotationQuatenion);

			SpaceCraft spaceCraftComponent = spaceCrafts[i].GetComponent<SpaceCraft>();
			spaceCraftComponent.squadron = this;
			spaceCraftComponent.id = i;
		}
	}
}
