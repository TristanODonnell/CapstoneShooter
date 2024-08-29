using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PathNode : MonoBehaviour
{
	public float radius;

	public List<PathNode> jump, drop, next;

	private void OnDrawGizmos()
	{
		void DrawLineTo(List<PathNode> paths, Color color, float zOffset, bool drawExtraOnDestination = false)
		{
			color.a = 0.4f;
			Gizmos.color = color;
			var offset = zOffset * Vector3.up;

			foreach (var item in paths)
			{
				if (!item)
				{
					paths.Remove(item);
					return;
				}
				Gizmos.DrawLine(item.transform.position + offset, transform.position + offset);
				if (drawExtraOnDestination)
					Gizmos.DrawSphere(transform.position + (item.transform.position - transform.position)* 0.9f + offset, 0.2f);
			}
		}


		DrawLineTo(jump, Color.blue, 0.2f, true);
		DrawLineTo(drop, Color.red, -0.2f, true);
		DrawLineTo(next, Color.white, 0);


		var col = Color.green;
		col.a = 0.2f;
		Gizmos.color = col;
		Gizmos.DrawCube(transform.position + Vector3.up * 0.1f, radius * 2 * (Vector3.one - Vector3.up) + Vector3.up);
	}
	private void OnDrawGizmosSelected()
	{

		var col = Color.green;
		Gizmos.color = col;
		Gizmos.DrawWireCube(transform.position + Vector3.up * 0.1f, radius * 2 * (Vector3.one - Vector3.up) + Vector3.up);
	}
	public static PathNode Create(string name, Vector3 position, Transform parent, float radius)
	{
		var created = Instantiate(Resources.Load<PathNode>("Node"));
		created.Set(position, radius);
		created.transform.localScale = Vector3.one * radius * 2f;
		created.transform.parent = parent;
		created.gameObject.layer = parent.gameObject.layer;
		created.name = name;
		return created;
	}
	public void Set(Vector3 position, float radius)
	{
		this.radius = radius;
		transform.position = position;
		jump = new();
		drop = new();
		next = new();

	}

	public bool IsHigherThan(PathNode checkBelow)
	{
		return (transform.position.y - checkBelow.transform.position.y) > 0f;
	}


	[ContextMenu("test merge")]
	public PathNode MergeWithNeighbor(PathFinderManager id)
	{

		List<PathNode> neighbors = new();
		var hasDetectedNeighbors = MergeNeighborRec(3, neighbors, this, this);

		if (!hasDetectedNeighbors)
			return null;

		//Make new point merged

		Vector3 pos = Vector3.zero;
		foreach (var makePosition in neighbors)
			pos += makePosition.transform.position;
		pos *= 0.25f;

		var merged = Create("Merged", pos, transform.parent, radius * 2f);
		if (!id.nodes.Contains(merged))
			id.nodes.Add(merged);

		//Get rid of all references of the neighbors
		var neighborArray = neighbors.ToArray();
		foreach (var remove in neighborArray)
			NodeFunction_SafelyMerge(merged, remove);

		return merged;
	}
	private bool MergeNeighborRec(int attempts, List<PathNode> takenItems, PathNode start, PathNode prev)
	{
		takenItems.Add(this);

		foreach (var item in next)
		{
			var radiusThis = radius;
			var radiusRef = start.radius;
			var radiusDiference = Mathf.Abs(radiusThis - radiusRef);
			var radiusMaxDifference = 0.01f;
			var radiusIsClose = radiusDiference < radiusMaxDifference;

			if (!item.IsStraightFrom(this) || !radiusIsClose)
				continue;

			if (attempts <= 0)
			{
				if (item == start)
					return true;
				continue;
			}
			if (start == item || prev == item)
				continue;

			if (item.MergeNeighborRec(attempts - 1, takenItems, start, this))
				return true;
		}
		takenItems.Remove(this);
		return false;
	}



	public static void NodeFunction_SafelyMerge(PathNode inheritor, PathNode remove)
	{
		var array = remove.next.ToArray();
		foreach (var i in array)
		{
			remove.next.Remove(remove);
			if (!i.next.Contains(inheritor))
				i.next.Add(inheritor);
			if (!inheritor.next.Contains(i))
				inheritor.next.Add(i);

		}

		foreach (var i in remove.jump)
			if (!inheritor.jump.Contains(i))
				inheritor.jump.Add(i);

		foreach (var i in remove.drop)
			if (!inheritor.drop.Contains(i))
				inheritor.drop.Add(i);




		foreach (var pathFinder in FindObjectsByType<PathFinderManager>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
			pathFinder.nodes.RemoveAll(i => i == remove);
			var arrayPath = pathFinder.nodes.ToArray();


			foreach (var item in arrayPath)
			{
				if (!item)
					continue;

				item.next.Remove(remove);

				if (item.jump.Contains(remove))
				{

					item.jump.Remove(remove);
					if (!item.jump.Contains(inheritor))
						item.jump.Add(inheritor);

					if (!item.drop.Contains(inheritor))
						item.drop.Add(inheritor);

				}
				if (item.drop.Contains(remove))
				{

					item.drop.Remove(remove);
					if (!item.drop.Contains(inheritor))
						item.drop.Add(inheritor);
				}
			}
		}

		inheritor.next.Remove(inheritor);
		inheritor.jump.Remove(inheritor);
		inheritor.drop.Remove(inheritor);

		DestroyImmediate(remove.gameObject);
	}
	public static void NodeFunction_Delete_DisconnectSafely(PathNode node)
	{

		List<PathNode> allNodes = new();
		foreach (var pfs in FindObjectsOfType<PathFinderManager>(true))
		{
			while (pfs.nodes.Contains(node))
				pfs.nodes.Remove(node);
			allNodes.AddRange(pfs.nodes);
			pfs.RenameNodes();
		}


		foreach (var disconnect in allNodes) { 
				while (disconnect.next.Contains(node))
					disconnect.next.Remove(node);

				while (disconnect.jump.Contains(node))
					disconnect.jump.Remove(node);

				while (disconnect.drop.Contains(node))
					disconnect.drop.Remove(node);
			}

		DestroyImmediate(node.gameObject);
	}


	public static void NodeFunction_Connect_BridgeNext(PathNode a, PathNode b)
	{
		void ltr(PathNode l, PathNode r)
		{
			if (!l.next.Contains(r) && l != r)
				l.next.Add(r);
		}

		ltr(a, b);
		ltr(b, a);
	}
	public static void NodeFunction_Connect_BridgeJump(PathNode a, PathNode b)
	{
		void ltr(PathNode l, PathNode r)
		{
			if (!l.jump.Contains(r) && l != r)
				l.jump.Add(r);
		}

		ltr(a, b);
		ltr(b, a);
	}
	public static void NodeFunction_Connect_OneWayJump(PathNode fromGround, PathNode toHigherGround, bool addDrop = false)
	{
		if (!fromGround.jump.Contains(toHigherGround) && fromGround != toHigherGround)
			fromGround.jump.Add(toHigherGround);
		if (addDrop)
			NodeFunction_Connect_Drop(toHigherGround, fromGround);
	}
	public static void NodeFunction_Connect_Drop(PathNode above, PathNode bottom)
	{
		if (!above.drop.Contains(bottom) && above != bottom)
			above.drop.Add(bottom);
	}




	public bool canCreateJump(PathNode other)
	{
		if (other.jump.Contains(this) || jump.Contains(other))
			return false;
		return true;
	}
	public void MakeJumpTo(PathNode other)
	{
		jump.Add(other);
	}

	public bool IsStraightFrom(PathNode item)
	{
		if (!item)
			return false;
		if (item == this)
			return true;
		var posThis = transform.position;
		var posOther = item.transform.position;
		var subt = posThis - posOther;

		var minError = 0.001f;

		var posX = Mathf.Abs(subt.x) < minError;
		var posZ = Mathf.Abs(subt.z) < minError;


		return posX != posZ;
	}


	public PathNode Pathfind_ReviseInterconnection(Vector3 end,  List<PathNode> revised)
	{
		revised.Add(this);
		List<PathNode> listOfConnected = new();
		listOfConnected.AddRange(next);
		listOfConnected.AddRange(jump);
		listOfConnected.AddRange(drop);

		var toEnd_Closest = this;
		var toEnd_Distance = GetSqrDistanceTo(end);

		foreach (var revise in listOfConnected)
		{
			if (revised.Contains(revise))
				continue;
			var inRevision = revise.Pathfind_ReviseInterconnection(end, revised);
			var inRevDistance = inRevision.GetSqrDistanceTo(end);
			if (inRevDistance < toEnd_Distance)
			{
				toEnd_Closest = inRevision;
				toEnd_Distance = inRevDistance;
			}
		}
		return toEnd_Closest;
	}
	public static PathNode Pathfind_Next(PathNode start, Vector3 end)
	{
		var l = Pathfind_List(start, end);
		if (l.Count > 1)
			return l[1];
		return start;
	}
	public static List<PathNode> Pathfind_List(PathNode start, Vector3 end)
	{
		List<PathNode> path = new();
		List<PathNode> ban = new();
		path.Add(start);
		ban.Add(start);
		var attempts = 500;
		var endPoint = start.Pathfind_ReviseInterconnection(end, new());
		var hasReachedEndSpot = false;

		var lastChecked = start;

		if (start == endPoint)
			return path;

		var distanceTraveled = 0f;
		while (!hasReachedEndSpot)
		{
			attempts--;
			if (attempts <= 0)
				break;
			//Make up a new startpoint to check
			List<PathNode> checkClosest = new();
			checkClosest.AddRange(lastChecked.next);
			checkClosest.AddRange(lastChecked.jump);
			checkClosest.AddRange(lastChecked.drop);


			PathNode closest_selected = null;

			hasReachedEndSpot = true;

			var tCost = float.MaxValue;
			foreach (var closest in checkClosest)
			{

				var closesToEnd = closest.GetSqrDistanceTo(end);
				var closestFromNext = closest.GetSqrDistanceTo(lastChecked.transform.position);


				var closestTCost = closesToEnd + closestFromNext;

				if(closest == endPoint)
				{
					closest_selected = closest;
					hasReachedEndSpot = true;
					break;
				}

				if (tCost > closestTCost || closestFromNext >
					closesToEnd)
				{
					hasReachedEndSpot = false;

					if (path.Contains(closest) || ban.Contains(closest))
						continue;

					closest_selected = closest;
					tCost = closestTCost;
				}
			}


			if (closest_selected != null)
			{
				lastChecked = closest_selected;
				path.Add(closest_selected);
			}
			else
			{
				if (lastChecked == start || hasReachedEndSpot)
					break;


				closest_selected = lastChecked;
				path.Remove(lastChecked);
				ban.Add(lastChecked);
				lastChecked = path[path.Count - 1];

				distanceTraveled -= closest_selected.GetSqrDistanceTo(lastChecked.transform.position);

			}
		}


		return path;
	}


	public PathFinderManager ParentID()
	{
		return transform.parent.GetComponent<PathFinderManager>();
	}
	public bool IsInRadius(Transform instanceInRange)
	{
		return IsInRadius(instanceInRange.position);
	}
	public bool IsInRadius(Vector3 positionInRange, float multiplier = 1f)
	{
		var pos = (transform.position - positionInRange);
		pos.y = 0;
		return pos.sqrMagnitude < radius * radius * multiplier * multiplier;
	}


	public bool IsInRadiusOfJump(Transform instanceInRange, float offsetRadius)
	{
		return IsInRadiusOfJump(instanceInRange.position, offsetRadius);
	}
	public bool IsInRadiusOfJump(Vector3 positionInRange, float offsetRadius)
	{
		var pos = (positionInRange - transform.position);
		var jump = transform.position.y > positionInRange.y;
		pos.y = 0f;
		var radiusOffset = Mathf.Abs(offsetRadius);
		return pos.sqrMagnitude < radius * radius + radiusOffset * radiusOffset && jump;
	}
	
	public float GetSqrDistanceTo(Vector3 destination)
	{
		return (destination -transform.position).sqrMagnitude;
	}
	public float GetSqrDistanceTo(PathNode destination)
	{
		return GetSqrDistanceTo(destination.transform.position);
	}

	public float GetDistanceTo(Vector3 destination)
	{
		return (destination - transform.position).magnitude;
	}
	public float GetDistanceTo(PathNode destination)
	{
		return GetDistanceTo(destination.transform.position);
	}
}
