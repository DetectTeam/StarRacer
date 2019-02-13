using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarRacer
{
	public class LevelIncrementor : MonoBehaviour 
	{
		public void SetLevelCount( int value )
		{
			if( LevelCounter.Instance )
				LevelCounter.Instance.LevelCount = value;
		}
	}
}
