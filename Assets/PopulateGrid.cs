using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
	public GameObject prefab;
	public int numberToCreate;

	void Start()
	{
		Populate();
	}
	void Populate()
	{
		GameObject newObj; // Create GameObject instance

		for (int i = 0; i < numberToCreate; i++)
		{
			 // Create new instances of our prefab until we've created as many as we specified
			newObj = (GameObject)Instantiate(prefab, transform);
			
			// Randomize the color of our image
			newObj.GetComponent<Image>().color = Random.ColorHSV(); 
		}

	}
}