using UnityEngine;
using System.Collections;

public class GibScript : MonoBehaviour {

	public float m_fYMaxForce = 10.0f;
	public float m_fYMinForce = 1.0f;
	
	public float m_fXMaxForce = 10.0f;
	public float m_fXMinForce = 1.0f;
	
	public float m_fZMaxForce = 10.0f;
	public float m_fZMinForce = 1.0f;
	
	
	

	public float m_fYMaxScale = 10.0f;
	public float m_fYMinScale = 1.0f;
	
	public float m_fXMaxScale= 10.0f;
	public float m_fXMinScale = 1.0f;
	
	public float m_fZMaxScale = 10.0f;
	public float m_fZMinScale = 1.0f;

	
	// Use this for initialization
	void Start () {
		this.rigidbody.AddForce(Vector3.up + new Vector3(	(Random.value * (m_fXMaxForce - m_fXMinForce) + m_fXMinForce),
																				(Random.value * (m_fYMaxForce - m_fYMinForce) + m_fYMinForce),
																				(Random.value * (m_fZMaxForce - m_fZMinForce) + m_fZMinForce)	), ForceMode.Acceleration);
			
		this.transform.localScale = new Vector3(	Random.value * (m_fXMaxScale - m_fXMinScale) + m_fXMinScale,
																	Random.value * (m_fYMaxScale - m_fYMinScale) + m_fYMinScale,
																	Random.value * (m_fZMaxScale - m_fZMinScale) + m_fZMinScale);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
