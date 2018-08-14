using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public static class StringExtensions  
{
	
	//Reset StringBuilder
	public static void Clear( this StringBuilder value )
	{
		value.Length = 0;
		value.Capacity = 0;
	}
	
}
