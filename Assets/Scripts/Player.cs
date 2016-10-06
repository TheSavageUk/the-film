// Player.cs - Player logic
// Copyright (c) 2015, Savage Software

// Includes

using UnityEngine;
using System.Collections;

// Player class

public class Player:MonoBehaviour
{
	// Methods
	
	// Start
	// Called when this object starts

	void Start()
	{
		// remove and lock cursor
		Cursor.visible=false;
		Cursor.lockState=CursorLockMode.Locked;
	}
	
	// Update
	// Called to update on each frame

	void Update()
	{
	}
}
