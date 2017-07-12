using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DragObjects : MonoBehaviour {

	[SerializeField] GameObject Player;
	[SerializeField] Transform dragPos;
	[SerializeField] float force = 50; // сила броска
	[SerializeField] float sensitivity = 25; // регулировка скорости при перемещении объекта
	[SerializeField] float heightValue = 0.35f; // чувствительность при изменении высоты колесиком мышки
	[SerializeField] float heightValueMax = 2; // насколько можно поднять объект, относительно стартовой точки
	[SerializeField] float heightValueMin = 1; // насколько можно опустить объект, относительно стартовой точки
	[SerializeField] float distance = 10; // максимальная дистанция, с которой можно схватить объект
	[SerializeField] float maxMass = 10; // максимальная масса объекта, который можно поднять
	[SerializeField] float stopDistance = 3; // допустимое расстояние от точки назначения и текущего положения объекта, при перемещении (чтобы нельзя было перетащить предмет сквозь графику)

	[Space(20)]
	[SerializeField] AudioSource _as;
	[SerializeField] AudioClip impuls;
	[SerializeField] AudioClip drag;
	[SerializeField] GameObject dragEffect;
	[SerializeField] Collider ignoreCol;

	Rigidbody body;
	float mass, curHeight, curForce;
	Transform clone, local;
	static bool _get;

	public static bool isDrag
	{
		get{ return _get; }
	}
	public Rigidbody getBody
	{
		get{ return body; }
	}



	void OnDisable()
	{
		ClearObjWithoutImpuls ();
	}

	public void ClearObjWithoutImpuls(){
		if (!body)
			return;
		Rigidbody tmpBody = body;
		Clear();
		tmpBody.gameObject.SendMessage ("SyncPos", tmpBody, SendMessageOptions.DontRequireReceiver);
	}

	public void ClearObjWithImpuls(){
		Rigidbody tmpBody = body;
		Clear();
		tmpBody.velocity = Camera.main.transform.TransformDirection(Vector3.forward) * curForce;
		tmpBody.gameObject.SendMessage ("SyncPos", tmpBody, SendMessageOptions.DontRequireReceiver);
		//_as.Stop ();
		_as.PlayOneShot (impuls);
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(1))
		{
			body = GetRigidbody();
		}
		else if(Input.GetMouseButtonUp(1) && body)
		{
			ClearObjWithoutImpuls ();
		}
		else if(Input.GetMouseButtonDown(0) && body)
		{
			ClearObjWithImpuls ();
		}

		/*if(Input.GetAxis("Mouse ScrollWheel") > 0 && body)
		{
			NewHeight(heightValue);
		}
		else if(Input.GetAxis("Mouse ScrollWheel") < 0 && body)
		{
			NewHeight(-heightValue);
		}*/
		if (body)
			clone.position = Vector3.Lerp (clone.position, dragPos.position, Time.deltaTime * 5);
	}

	void NewHeight(float value) // изменение высоты
	{
		curHeight += value;
		curHeight = Mathf.Clamp(curHeight, heightValueMin, heightValueMax);
		if(curHeight == heightValueMin || curHeight == heightValueMax) return;
		clone.position += new Vector3(0, value, 0);
	}

	Rigidbody GetRigidbody()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
		if(Physics.Raycast(ray, out hit, distance,1<<Convert.ToInt32 (Player.layer) ))
		{
			if(hit.rigidbody && !hit.rigidbody.isKinematic && hit.rigidbody.mass <= maxMass && hit.collider.CompareTag("DragObject"))
			{
				mass = hit.rigidbody.mass;
				if(mass < 2) mass = 2; // сила броска, зависит от массы, поэтому проверяем, чтобы минимальная масса не была равна или меньше единицы
				curForce = force / mass;
				hit.rigidbody.useGravity = false;
				hit.rigidbody.freezeRotation = true;
				clone.position =hit.point;// dragPos.position;//hit.point; // устанавливаем точку, где "подхватили" объект
				if(dragEffect)
					dragEffect.SetActive (true);
				//hit.rigidbody.position=dragPos.position;
				return hit.rigidbody;
			}
			
		}
		return null;
	}

	void SetLocal() // копируем позицию и вращение, оригинала
	{
		if(_get) return;
		local.rotation = body.rotation;
		local.position = dragPos.position;
		_get = true;
	}

	float RoundTo(float f, int to) // округлить до
	{
		return ((int)(f*to))/(float)to;
	}

	void FixedUpdate()
	{
		if(!body) return;


		Vector3 lookAt = Camera.main.transform.position;
		lookAt.y = clone.position.y;
		clone.LookAt(lookAt);
		SetLocal();

		//body.position =Vector3.Lerp(body.position, local.position,Time.deltaTime*sensitivity);
		body.velocity = Vector3.Lerp(body.velocity, (local.position - body.position) * sensitivity,Time.deltaTime*20);
		body.rotation = local.rotation;


		float dist = Vector3.Distance(body.position, local.position);
		dist = RoundTo(dist, 100); // округляем до сотых, для исключения погрешностей
		if(dist > stopDistance) // сброс, при попытке протащить предмет сквозь стену или если резко повернуть камеру
		{
			body.velocity = Vector3.zero; // обязательный сброс скорости, иначе объект может улететь в стратосферу
			Clear();
		}
	}

	void CheckVelocity() // проверка скорости, когда мы отпускаем объект во время движения камеры, чтобы его скорость не могла превышать силы броска
	{
		Vector3 velocity = body.velocity.normalized * curForce;
		if(body.velocity.sqrMagnitude > velocity.sqrMagnitude)
		{
			body.velocity = velocity;
		}
	}

	void ClearMessage()
	{
		if (!body)
			return;
		Rigidbody tmpBody = body;
		Clear();
		if(tmpBody)
			tmpBody.gameObject.SendMessage ("SyncPos", tmpBody, SendMessageOptions.DontRequireReceiver);
	}

	void Clear()
	{
		curHeight = 0;
		_get = false;
		clone.localPosition = Vector3.zero;
		local.localPosition = Vector3.zero;
		if(!body) return;
		CheckVelocity();
		body.useGravity = true;
		body.freezeRotation = false;
		body = null;
		if(dragEffect)			
			dragEffect.SetActive (false);
	}

	void Awake()
	{
		Player = gameObject;
		if(!clone) // создание вспомогательных точек
		{
			local = new GameObject().transform;
			clone = new GameObject().transform;
			local.parent = clone;
			clone.parent = Camera.main.transform;
		}

		heightValueMin = -Mathf.Abs(heightValueMin);
		heightValueMax = Mathf.Abs(heightValueMax);

		Clear();
	}
}