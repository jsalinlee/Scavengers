using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {
	public float speed;
	Vector3 movDir;
	public void CreateProjectile()
	{
		
	}
	public void SetDirection(CardinalDirection direction)
	{
		if(direction == CardinalDirection.up)
		{
			//movDir = Vector2.up;
		}
		else if(direction == CardinalDirection.left)
		{
			transform.Rotate(0, 0, 90);
		}
		else if(direction == CardinalDirection.down)
		{
			transform.Rotate(0, 0, 180);
		}
		else if(direction == CardinalDirection.right)
		{
			transform.Rotate(0, 0, -90);
		}
	}
	void Update () {
		transform.Translate(Vector2.up * speed * Time.deltaTime);
	}
}
