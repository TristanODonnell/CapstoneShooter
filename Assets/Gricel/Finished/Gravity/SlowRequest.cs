using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlowRequest
{
	public byte framesToUpdate = 10;
	private byte update = 10;


	public SlowRequest(byte updateInFrames)
	{
		framesToUpdate = update = updateInFrames;
	}
	public bool ExecuteRequest()
	{
		--update;

		if (update > 0) 
			return false;
		update = framesToUpdate;
		return true;
	}
}
