using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[EditorWindowTitle(title = "tools/Pathfinder")]
public class PathFinderControl : EditorWindow
{
    [MenuItem("tools/Pathfinder Generator")]
    public static void ShowWindow()
	{
        GetWindow<PathFinderControl>("Pathfinder");
	}
	private void OnInspectorUpdate()
	{
		Repaint();
	}
	PathNode[] nodes;
	private void OnGUI()
	{
		var selection = Selection.objects;
		nodes = new PathNode[selection.Length];
		for (var i = 0; i < selection.Length; i++)
		{
			PathNode nodeSelected;
			if (selection[i].GetType() != typeof(GameObject))
				return;
			nodeSelected = ((GameObject)selection[i]).GetComponent<PathNode>();
			if (nodeSelected)
			{
				nodes[i] = nodeSelected;
				continue;
			}
			nodes = new PathNode[0];
			break;
		}


		if (nodes.Length == selection.Length && selection.Length > 0)
			Make_NodesSelected();
		else
			Make_NodeManager();

		if (forceName)
		{
			forceName = false;
			RenameAllPaths();
		}
	}
	private void Make_NodesSelected()
	{
		GUILayout.Label("Radius");
		if(nodes.Length == 1)
		{
			var n = nodes[0];
			var edited = n.radius;

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("x0.5"))
				n.radius *= 0.5f;
			if (GUILayout.Button("fixed"))
				n.radius = Mathf.Round(n.radius * 4f) / 4f;
			if (GUILayout.Button("x2  "))
				n.radius *= 2f;
			GUILayout.EndHorizontal();

			n.radius = EditorGUILayout.Slider(n.radius, 0.01f, 40f);

			if (edited != n.radius)
				SceneView.RepaintAll();
		}

		GUILayout.Label("New Nodes");

		if (Button("New Walk Node",
			"Creates a node where the two can walk towards each other",
			nodes.Length == 1))
		{
			PathNode.NodeFunction_Connect_BridgeNext(createNewNodeFrom(nodes[0]), nodes[0]);
		}
		if (Button("New Bidirectional jump Node",
			"Creates a node where a path finding object can jump over an obstacle like a hole",
			nodes.Length == 1))
		{
			PathNode.NodeFunction_Connect_BridgeJump(createNewNodeFrom(nodes[0]), nodes[0]);
		}
		if (Button($"Jump from {nodes[0].name} to NewNode",
			"Creates a New node where the currently selected acts as a ground level spot and a new node becomes a higher platform",
			nodes.Length == 1))
		{
			var nNode = createNewNodeFrom(nodes[0]);
			PathNode.NodeFunction_Connect_OneWayJump(nodes[0], nNode, true);

			nNode.transform.position += Vector3.up * nNode.radius;
			Selection.activeGameObject = nNode.gameObject;
		}
		if (Button($"Jump from New Node to {nodes[0].name}",
			"Creates a New node where the currently selected acts as a higher platform level spot and a new node becomes a ground level spot",
			nodes.Length == 1))
		{
			var nNode = createNewNodeFrom(nodes[0]);
			PathNode.NodeFunction_Connect_OneWayJump(nNode, nodes[0], true);

			nNode.transform.position -= Vector3.up * nNode.radius;
			Selection.activeGameObject = nNode.gameObject;
		}
		if (Button("New Drop Down Node",
			"Creates a New node where the previously selected node can Drop down, (the instance currently selected is the higher platform)",
			nodes.Length == 1))
		{

			var nNode = createNewNodeFrom(nodes[0]);

			PathNode.NodeFunction_Connect_Drop(nodes[0], nNode);
			nNode.transform.position -= Vector3.up * nNode.radius;
			Selection.activeGameObject = nNode.gameObject;
		}



		GUILayout.Label("Connect");
		if(nodes.Length > 1)
		{
			if(Button("Connect Nodes for walk",
				"Makes an interconnectd path for the pathfinder using all selected nodes"))
			{
				foreach (var a in nodes)
					foreach (var b in nodes)
						if (a != b)
							PathNode.NodeFunction_Connect_BridgeNext(a, b);
			}


			if (Button("Connect Nodes for jumping",
				"Makes an interconnectd path for the pathfinder using all selected nodes",
				nodes.Length == 2))
			{
				var a = nodes[0];
				var b = nodes[1];

				if (a != b)
					PathNode.NodeFunction_Connect_BridgeJump(a, b);
			}



			if (Button($"Make a single jump from {nodes[0]} towards {nodes[1]}",
				$"Makes an interconnectd path for the pathfinder using all selected nodes",
				nodes.Length == 2 &&
				Mathf.Abs(nodes[0].transform.position.y - nodes[1].transform.position.y) > 0.25f))
			{
				var a = nodes[0];
				var b = nodes[1];

				if(a.transform.position.y > b.transform.position.y)
				{
					var c = a;
					a = b;
					b = c;
				}


				if (a != b)
					PathNode.NodeFunction_Connect_OneWayJump(a, b, true);
			}
			if (Button($"Make a drop down from {nodes[0]} towards {nodes[1]}",
				$"Makes a connection where one node acts as a hole and the other as a higher platform",
				nodes.Length == 2 &&
				Mathf.Abs(nodes[0].transform.position.y - nodes[1].transform.position.y) > 0.25f))
			{
				var a = nodes[0];
				var b = nodes[1];

				if (!a.IsHigherThan(b))
				{
					var c = a;
					a = b;
					b = c;
				}


				if (a != b)
					PathNode.NodeFunction_Connect_Drop(a, b);
			}

		}


		if (Button("Fix jumps and drops",
			"some jumps and drops might be in the wrong spots, this setting recalculates the selected nodes"))
		{
			foreach (var fix in nodes)
			{
				var j = fix.jump.ToArray();
				foreach (var jumps in j)
				{
					if (fix.next.Contains(jumps))
					{
						jumps.next.Remove(fix);
						fix.next.Remove(jumps);
					}


					if (!jumps.IsHigherThan(fix))
						if (fix.jump.Contains(jumps))
						{
							fix.jump.Remove(jumps);
							jumps.jump.Add(fix);
						}
				}


				var d = fix.drop.ToArray();
				foreach (var drops in d)
				{
					if (fix.next.Contains(drops))
					{
						drops.next.Remove(fix);
						fix.next.Remove(drops);
					}


					if (drops.IsHigherThan(fix))
						if (fix.drop.Contains(drops))
						{
							fix.drop.Remove(drops);
							drops.drop.Add(fix);
						}
				}
			}
		}



		GUILayout.Label("Subdivide");
		if(Button("Split in 4",
			"Cuts down the selected node into four nodes fully walkable between each other"))
		{
			List<PathNode> splitNodes = new();

			foreach (var split in nodes)
			{
				Vector3[] _x = { Vector3.right, Vector3.left };
				Vector3[] _y = { Vector3.back, Vector3.forward };
				List<PathNode> currentSplit = new();
				foreach (var x in _x)
					foreach (var y in _y) {
						var position = split.transform.position + (x + y) * split.radius * 0.5f;
						var parent = split.ParentID();
						var nNode = PathNode.Create("n", position, parent.transform, split.radius * 0.5f);
						splitNodes.Add(nNode);
						currentSplit.Add(nNode);

						parent.nodes.Add(nNode);
					}

				foreach (var a in currentSplit)
					foreach (var b in currentSplit)
						PathNode.NodeFunction_Connect_BridgeNext(a, b);
			}

			void ExecuteOnContain(ref List<PathNode> l, PathNode owner)
			{
				foreach (var split in nodes)
					if(l.Contains(split))
					{
						var distanceMin = float.MaxValue;
						var setReplacement = splitNodes[0];
						foreach (var replacement in splitNodes)
							if(distanceMin > owner.GetSqrDistanceTo(replacement))
							{
								distanceMin = owner.GetSqrDistanceTo(replacement);
								setReplacement = replacement;
							}
						l.Add(setReplacement);

						if (split.next.Contains(owner) && !setReplacement.next.Contains(owner))
							setReplacement.next.Add(owner);
						if (split.jump.Contains(owner) && !setReplacement.jump.Contains(owner))
							setReplacement.jump.Add(owner);
						if (split.drop.Contains(owner) && !setReplacement.drop.Contains(owner))
							setReplacement.drop.Add(owner);


						while (l.Contains(split))
							l.Remove(split);
					}

			}
			var allPNs = Get_AllPathNodes();
			foreach (var allNodes in allPNs)
			{
				ExecuteOnContain(ref allNodes.next, allNodes);
				ExecuteOnContain(ref allNodes.jump, allNodes);
				ExecuteOnContain(ref allNodes.drop, allNodes);
			}


			foreach (var delete in nodes)
				PathNode.NodeFunction_Delete_DisconnectSafely(delete);

		}

		GUILayout.Label("Merge");
		if (Button("Merge As Line",
			"Make all selected nodes into a straight line",
			nodes.Length > 2))
		{

			//Find out the nodes on the longest extents
			PathNode pointA = nodes[0];
			PathNode pointB = nodes[1];
			{
				float distanceToEachOther = 0f;
				foreach (var pA in nodes)
					foreach (var pB in nodes) {
						var distance_AToB = pA.GetSqrDistanceTo(pB.transform.position);
						if (pB != pA && distanceToEachOther < distance_AToB)
						{
							pointA = pA;
							pointB = pB;
							distanceToEachOther = distance_AToB;
						}
					}
			}

			//Merge any other node into A or B
			foreach (var merge in nodes)
			{
				if (merge == pointA || merge == pointB)
					continue;
				var distanceToA = pointA.GetSqrDistanceTo(merge.transform.position);
				var distanceToB = pointB.GetSqrDistanceTo(merge.transform.position);

				if (distanceToA < distanceToB)
					PathNode.NodeFunction_SafelyMerge(pointA, merge);
				else
					PathNode.NodeFunction_SafelyMerge(pointB, merge);
			}

			foreach (var pfs in Get_AllPathFinders())
				pfs.RenameNodes();
		}

		if(Button("Merge As Block",
			"Makes a block out of all the selected nodes",
			nodes.Length > 1))
		{
			//Get two reference nodes
			var pointA = nodes[0];
			var pointB = nodes[1];
			var distance = 0f;
			foreach (var pA in nodes)
				foreach (var pB in nodes) {
					var distance_AB = pA.GetSqrDistanceTo(pB.transform.position);
					if (pA != pB && distance_AB > distance)
					{
						distance = distance_AB;
						pointA = pA;
						pointB = pB;
					}
				}

			var position = (pointA.transform.position + pointB.transform.position) * 0.5f;
			var radius = pointA.GetDistanceTo(pointB);



			var createdNode = PathNode.Create("new", position, pointA.ParentID().transform, 2f);
			pointA.ParentID().nodes.Add(createdNode);
			createdNode.radius = radius * 0.5f;

			foreach (var nodeToErase in nodes)
				PathNode.NodeFunction_SafelyMerge(createdNode, nodeToErase);
		}

		if (nodes.Length == 2)
			if (!nodes[0] || !nodes[1])
				if (Button($"Disconnect {nodes[0].name} & {nodes[1].name} From each other",
					"Disconnects all selected items from the other nodes"))
				{
					foreach (var disconnect in nodes)
					{
						var otherNode = disconnect == nodes[0] ? nodes[1] : nodes[0];

						while (disconnect.next.Contains(otherNode))
							disconnect.next.Remove(otherNode);
						while (disconnect.jump.Contains(otherNode))
							disconnect.jump.Remove(otherNode);
						while (disconnect.drop.Contains(otherNode))
							disconnect.drop.Remove(otherNode);
					}
				}


		if (Button($"Disconnect {nodes.Length}",
			"Disconnects all selected items from the other nodes"))
		{
			foreach (var disconnect in nodes)
			{
				disconnect.next = new();
				disconnect.jump = new();
				disconnect.drop = new();

				foreach (var allNodes in Get_AllPathNodes())
				{
					while (allNodes.next.Contains(disconnect))
						allNodes.next.Remove(disconnect);
					while (allNodes.jump.Contains(disconnect))
						allNodes.jump.Remove(disconnect);
					while (allNodes.drop.Contains(disconnect))
						allNodes.drop.Remove(disconnect);
				}
			}
		}
		
	}
	private void Make_NodeManager()
	{
		PathFinderManager selected = null;
		if (Selection.activeGameObject)
			selected = Selection.activeGameObject.GetComponent<PathFinderManager>();

		if (Button("Create New Node Manager",
			"Makes a new node manager gameobject to allow editing the scenario or create floors"))
		{
			var nNodeManager = Resources.Load<PathFinderManager>("NodeMap");
			if (selected)
				nNodeManager = selected;

			var desiredPosition = nNodeManager.transform.position;
			nNodeManager = Instantiate<PathFinderManager>(nNodeManager);


			if (selected)
				nNodeManager.RemoveNodes();

			nNodeManager.transform.position = desiredPosition;
			Selection.activeGameObject = nNodeManager.gameObject;

			var nNode = PathNode.Create("", desiredPosition, nNodeManager.transform, 1f);
			nNodeManager.nodes.Add(nNode);
		}

		if (!selected)
			return;

		if (Button("Make a starting node",
			"Makes a new node manager gameobject to allow editing the scenario or create floors",
			selected.nodes.Count == 0))
		{
			var nNode = PathNode.Create("", selected.transform.position, selected.transform, 1f);
			selected.nodes.Add(nNode);

		}

		if (Button("Clear All",
			"Gets rid of all nodes from the selected PathfinderManager",
			selected.nodes.Count > 0))
		{
			selected.RemoveNodes();
		}
		if (Button("Bake Nodes",
			"Creates pathNodes all around the interior of the Manager gameobject automatically interconnecting them and setting up jumps"))
		{
			selected.Bake();
		}
		if (Button("Recalculate Jumps and drops",
			"Some nodes might be in the wrong spot, being this that some jumping nodes might be above and some drop down nodes below, this option fixes it"))
		{

			foreach (var fix in selected.nodes)
			{
				var j = fix.jump.ToArray();
				foreach (var jumps in j)
				{
					if (fix.next.Contains(jumps))
					{
						jumps.next.Remove(fix);
						fix.next.Remove(jumps);
					}

					if (!jumps.IsHigherThan(fix))
						if(fix.jump.Contains(jumps))
						{
							jumps.jump.Add(fix);
							fix.jump.Remove(jumps);
						}
				}


				var d = fix.drop.ToArray();
				foreach (var drops in d)
				{
					if (fix.next.Contains(drops))
					{
						drops.next.Remove(fix);
						fix.next.Remove(drops);
					}


					if (drops.IsHigherThan(fix))
						if (fix.drop.Contains(drops))
						{
							drops.drop.Add(fix);
							fix.drop.Remove(drops);
						}
				}
			}
		}
	}


	private PathNode createNewNodeFrom(PathNode other)
	{
		var n = PathNode.Create("copy", other.transform.position, other.transform.parent, other.radius);
		other.ParentID().nodes.Add(n);
		Selection.activeGameObject = n.gameObject;
		return n;
	}
	private PathFinderManager [] Get_AllPathFinders()
	{
		return FindObjectsByType<PathFinderManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
	}
	private PathNode [] Get_AllPathNodes()
	{
		List<PathNode> l = new();
		foreach (var pfs in Get_AllPathFinders())
			foreach (var ns in pfs.nodes)
				if (!l.Contains(ns))
					l.Add(ns);
		return l.ToArray();
	}
	private void RenameAllPaths()
	{
		foreach (var pfs in Get_AllPathFinders())
			pfs.RenameNodes();
	}
	bool forceName = false;
	private bool Button(string button, string description, bool condition = true)
	{
		if (!condition)
			return false;
		GUILayout.Label("");
		EditorGUILayout.HelpBox(description, MessageType.Info);

		if (GUILayout.Button(button))
		{
			foreach (var dirt in Selection.objects)
				EditorUtility.SetDirty(dirt);
			forceName = true;
			return true;
		}
		return false;
	}

}
