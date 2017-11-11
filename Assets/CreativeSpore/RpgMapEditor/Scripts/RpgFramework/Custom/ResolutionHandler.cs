using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionHandler : MonoBehaviour {

	public class ScreenSize : MonoBehaviour 
	{ 
		public float m_fHeight = 1600f; 
		public float m_fWidth = 900f; 

		// Use this for initialization 
		void Start () 
		{ 
			GetComponent<Camera>().aspect = m_fHeight/m_fWidth; 
		} 
	}
}
