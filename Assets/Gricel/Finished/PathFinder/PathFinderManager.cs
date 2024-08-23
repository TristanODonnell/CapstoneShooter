using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderManager : MonoBehaviour
{
    public float incrementSize = 0.5f;
	public float maxJump = 5f;
	public float maxHeightBetweenNodes = 0.3f;
	private Vector3 sizeMin => transform.position - transform.localScale * 0.5f;
	private Vector3 sizeMax => transform.position + transform.localScale * 0.5f;

	public List<PathNode> nodes;
	private void OnDrawGizmos()
	{
		var scl = transform.localScale;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, scl);

		var mag = scl.magnitude * Vector3.one;
		Gizmos.color = Color.red;
		Gizmos.DrawCube(sizeMin, mag * 0.025f);
		Gizmos.color = Color.magenta;
		Gizmos.DrawCube(sizeMax, mag * 0.025f);
	}

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	public static PathNode GetClosestPathNode(Vector3 pos)
	{
		var closestDistance = float.MaxValue;
		var closestNode = (PathNode)null;
		foreach (var node in FindObjectsByType<PathNode>(FindObjectsInactive.Include, FindObjectsSortMode.None))
		{
			var distance = (node.transform.position - pos).sqrMagnitude;

			if (closestDistance > distance)
			{
				closestDistance = distance;
				closestNode = node;
			}
		}

		return closestNode;
	}

	[ContextMenu("Checkbake")]
	public void Bake()
	{
		//If someone messed up the increments then this entire method will not ever run
		if (incrementSize <= 0f)
			return;


		//Get rid of all previously added nodes
		Debug.Log("Removing old nodes");
		RemoveNodes();
		
		
		//Shoot raycasts all around
		Debug.Log("Analyzing terrain");
		#region 


		var getTotalSize = (sizeMax - sizeMin);
		int sizex = Mathf.RoundToInt(getTotalSize.x / incrementSize);
		int sizez = Mathf.RoundToInt(getTotalSize.z / incrementSize);



		//Make an array of all types to ignore

		System.Type[] ignoreTypes = { typeof(Rigidbody), typeof(CharacterController) };



		//Make a minimum position to ignore and a 3D array of all the posible positions

		var vectorIgnore = Vector3.one * float.MinValue;
		Vector3[][] positions = new Vector3[sizex][];

		//Test the X position
		for (var x = 0; x < positions.Length; x++)
		{
			positions[x] = new Vector3[sizez];

			//Test the z position
			for (int z = 0; z < sizez; z++)
			{

				//Make base values for the current position
				#region


				//Make vectors based on the for loops
				var posx = incrementSize * Vector3.right * (float)x;
				var posz = incrementSize * Vector3.forward * (float)z;



				//Detect the distance between the roof of the cube and the floor
				var distance = (sizeMax - sizeMin).y;



				//Shoot many raycasts to fore one to hit the highest floor
				var rays = Physics.RaycastAll(sizeMax - posx - posz, Vector3.down, distance);



				//Make the desired position an imposible vector to allow modifications
				var desiredPosition = vectorIgnore;
				var maxHeight = 0f;


				#endregion

				//Test all raycasted targets in the current XZ position
				#region


				foreach (var item in rays)
				{


					//Two overridable values to know how to continue the loop
					var hasDetectedForbiddenType = false;
					maxHeight = desiredPosition.y;



					//Detect the collider does not own a forbidden component to ignore
					foreach (var iType in ignoreTypes)
						if (item.collider.GetComponent(iType))
							hasDetectedForbiddenType = true;

					if (hasDetectedForbiddenType || item.collider.isTrigger)
						continue;



					//The point hit is higher than the previous hit point and save the new highest hit point
					if (item.point.y > maxHeight)
						desiredPosition = item.point;
				}


				#endregion

				//Place the new position in the detected spot or in an imposible spot to make the system ignore it
				positions[x][z] = desiredPosition;
			}
		}



#endregion


		// Make nodes mimic raycastresults
		Debug.Log("Adding nodes");
		#region


		//Make an array for all posible nodes
		var nodeAmmount = sizex * sizez;
		PathNode[] nodesNew = new PathNode[nodeAmmount];


		//Iterate in all the previously raycasted positions
		var node_index = -1;
		foreach (var x in positions)
			foreach (var pos in x)
			{
				node_index++;


				//Ignore if the position given is part of an imposible vector
				if (pos == vectorIgnore)
					continue;



				//Create a node in the given position if all goes well
				var nodeSelected = PathNode.Create($"node temp", pos, transform, incrementSize * 0.5f);
				nodesNew[node_index] = nodeSelected;
			}


		//Add all given nodes to the node list and eliminate NULL leftovers
		this.nodes = new(nodesNew);
		this.nodes.RemoveAll(i => i == null);


		#endregion



		//Interconnect all nodes
		Debug.Log("Connecting nodes");
		#region


		foreach (var item in nodes)
		{
			var pos = item.transform.position;
			foreach (var next in nodes)
			{
				if (next == item)
					continue;

				//check if is repeated
				#region
				{
					var nextCont = next.next.Contains(item);
					var nextDrop = next.drop.Contains(item);
					var nextJump = next.jump.Contains(item);
					if (nextCont || nextDrop || nextJump)
						continue;

				}
				#endregion

				//Detect closest to make walk path
				#region


				var nextPos = next.transform.position;
				var magnitude = (pos - nextPos);
				var distanceMin = Mathf.Sqrt(incrementSize*incrementSize*3) + 0.1f;


				var height = Mathf.Abs((next.transform.position - item.transform.position).y);
				//If the node is fully walkable then there is no need to make a ledge drop or a jump
				if (magnitude.magnitude < distanceMin && height < maxHeightBetweenNodes)
				{
					item.next.Add(next);
					next.next.Add(item);
					continue;
				}

				#endregion


				//Detect closest to make a drop from ledge or a jump
				#region


				var prevMag = Mathf.Abs(magnitude.y);
				magnitude.y = 0;

				if (magnitude.magnitude < distanceMin)
				{
					var canDrop = pos.y > nextPos.y;
					//Make drop from ledge
					if (canDrop)
					{
						item.drop.Add(next);

						if (prevMag < maxJump)
							next.jump.Add(item);
						continue;
					}
					//Make jumping gap
					if (prevMag < maxJump)
					{
						item.jump.Add(next);
						next.drop.Add(item);
					}

				}


				#endregion
			}
		}


		#endregion


		//Merge Some nodes
		Debug.Log("Merging nodes");
		MergeNodes();

		//Add nice names
		Debug.Log("Finnishing up");
		RenameNodes();
	}
	[ContextMenu("Test merge nodes")]
	public void MergeNodes()
	{
		var cancel = true;
		do
		{
			var array = nodes.ToArray();
			cancel = true;
			foreach (var item in array)
			{
				if (item == null)
					continue;
				var mergedNode = item.MergeWithNeighbor(this);
				if (!mergedNode)
					continue;
				cancel = false;
			}
		}
		while (!cancel);
	}

	[ContextMenu("Remove all nodes")]
	public void RemoveNodes()
	{
		if (nodes == null)
			nodes = new();


		List<PathNode> all = new(FindObjectsByType<PathNode>(FindObjectsInactive.Include, FindObjectsSortMode.None));

		foreach (var item in nodes)
		{
			if (item)
			{
				foreach (var any in all)
				{
					while (any.next.Contains(item))
						any.next.Remove(item);
					while (any.jump.Contains(item))
						any.jump.Remove(item);
					while (any.drop.Contains(item))
						any.drop.Remove(item);
				}
				all.Remove(item);
				if (Application.isPlaying)
					Destroy(item.gameObject);
				else
					DestroyImmediate(item.gameObject);
			}
		}
		nodes = new();
	}
	public void RenameNodes()
	{
		nodes.RemoveAll(i => null == i);
		for (var i = 0; i < nodes.Count; i++)
		{
			PathNode item = nodes[i];
			item.name = $"{i}";
		}
	}
	void Update()
    {
        
    }
}
