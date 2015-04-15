using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;		// This allows for the use of lists, like <GameObject>
using pseudoSinCos;
using System.Linq;


public class verts
{
	public float angle {get;set;}
	public int location {get;set;} // 1= left end point    0= middle     -1=right endpoint
	public Vector3 pos {get;set;}
	public bool endpoint { get; set;}

}

[ExecuteInEditMode]

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]

[Serializable]

public class DynamicLight : MonoBehaviour {

	public delegate void OnReachedDelegate(GameObject[] go);
	public event OnReachedDelegate OnReachedGameObjects;

	public string version = "1.1.2 (Unity 5)";

	public Material lightMaterial;
	public PolygonCollider2D[] allMeshes;									// Array for all of the meshes in our scene
	public List<verts> allVertices = new List<verts>();								// Array for all of the vertices in our meshes
	public float lightRadius = 20f;
	public int lightSegments = 8;
	public LayerMask Layer; // Mesh for our light mesh
	//int Layer;

	// -- OPTIMISATIONS BOOLS --//
	public bool notifyGameObjectsReached = false;
	public bool intelliderConvex = false;

	// -- OPTIONALS BOOLS --//
	public bool recalculateNormals = true;

	
	[HideInInspector] public int vertexWorking;
	public float RangeAngle = 360f;


	// Private variables

	Mesh lightMesh;	
	//public PolygonCollider2D[] allMeshes;
	PolygonCollider2D[] temArrayAllMeshes;

	List <verts> tempVerts = new List<verts>(); // -- temporal vertices of whole mesh --//

	List<GameObject> objReached = new List<GameObject>(); // -- save all GO reache by current light --//


	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "bright.png", true);
	}


	public void setMainMaterial(Material m){
		lightMaterial = m;
	}

	public void setLayerMask(){
		#if UNITY_EDITOR
		if(!Application.isPlaying && Layer.value <= 0){
			Layer = 1<< LayerMask.NameToLayer("ShadowLayer");
		}
		#endif
	}
	


	public void Rebuild () {


		//--mesh filter--//
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter==null){
			//Debug.LogError("MeshFilter not found!");
			return;
		}
		
		Mesh mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.Clear();



		PseudoSinCos.initPseudoSinCos();
		
		//-- Step 1: obtain all active meshes in the scene --//
		//---------------------------------------------------------------------//

		//MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();		// Add a Mesh Renderer component to the light game object so the form can become visible

		lightMesh = new Mesh();																	// create a new mesh for our light mesh
		meshFilter.mesh = lightMesh;															// Set this newly created mesh to the mesh filter
		lightMesh.name = "Light Mesh";															// Give it a name
		lightMesh.MarkDynamic ();




		
	}


	void Start(){
		//-- Set Layer mask --//
		setLayerMask();

		Rebuild();
	}

	void Update(){


		fixedLimitations();

		if(lightMesh){
			getAllMeshes();
			setLight ();
			renderLightMesh ();
			resetBounds ();
		}



	}


	void fixedLimitations(){
		gameObject.transform.localScale = Vector3.one;
		//gameObject.transform.localEulerAngles = Vector3.zero;

		Vector3 pos = gameObject.transform.localPosition;
		pos.z = 0;
		gameObject.transform.localPosition = pos;

		// Angle
		if(RangeAngle > 360.0001f)
			RangeAngle = 360;


	}

	void getAllMeshes(){

		allMeshes = FindObjectsOfType<PolygonCollider2D>();
	}

	void resetBounds(){
		Bounds b = lightMesh.bounds;
		b.center = Vector3.zero;
		lightMesh.bounds = b;
	}
	void setLight () {

		bool sortAngles = false;

		//objectsReached.Clear(); // sweep all last objects reached

		allVertices.Clear();// Since these lists are populated every frame, clear them first to prevent overpopulation

	



		//--Step 2: Obtain vertices for each mesh --//
		//---------------------------------------------------------------------//
	
		// las siguientes variables usadas para arregla bug de ordenamiento cuando
		// los angulos calcuados se encuentran en cuadrantes mixtos (1 y 4)
		bool lows = false; // check si hay menores a -0.5
		bool his = false; // check si hay mayores a 2.0
		float magRange = 0.15f;

		// -- CLEAR TEMPVERTS --// ver 1.1.0v
		tempVerts.Clear();



		// reset counter vertices;
		vertexWorking = 0;

		for (int m = 0; m < allMeshes.Length; m++) {
		//for (int m = 0; m < 1; m++) {
			tempVerts.Clear();
			PolygonCollider2D mf = allMeshes[m];

			// las siguientes variables usadas para arregla bug de ordenamiento cuando
			// los angulos calcuados se encuentran en cuadrantes mixtos (1 y 4)
			lows = false; // check si hay menores a -0.5
			his = false; // check si hay mayores a 2.0

			if(notifyGameObjectsReached == true) // work only in neccesary cases -- optimization ver 1.1.0--
				objReached.Clear();



			// Method for check every point in each collider
			// if is closer from light, any point, then add collider to work.

			bool mfInWorks = false;

			var points = Enumerable.Range(0, mf.pathCount).SelectMany(x => mf.GetPath(x));
			foreach (var point in points) {
				Vector3 worldPoint = mf.transform.TransformPoint(point);
				if((worldPoint - gameObject.transform.position).sqrMagnitude <= lightRadius* lightRadius){

					//float angleWorldPoint = getVectorAngle(true, worldPoint.x,worldPoint.y);
					//if((angleWorldPoint*Mathf.Rad2Deg > startAngle) && (angleWorldPoint * Mathf.Rad2Deg < endAngle)){
						//Debug.Log(angleWorldPoint*Mathf.Rad2Deg + "   " + startAngle + "  " + endAngle);
						mfInWorks = true;
						break;
					//}

				}
			}



			if(mfInWorks == true)

			{
				if(((1 << mf.transform.gameObject.layer) & Layer) != 0){

					// Add all vertices that interact
					vertexWorking += mf.GetTotalPointCount();

					for (int i = 0; i < mf.GetTotalPointCount(); i++) {								   // ...and for ever vertex we have of each mesh filter...

						verts v = new verts();
						
						Vector3 worldPoint = mf.transform.TransformPoint(mf.points[i]);
						
						// Reforma fecha 24/09/2014 (ultimo argumento lighradius X worldPoint.magnitude (expensivo pero preciso))
						RaycastHit2D ray = Physics2D.Raycast(transform.position, worldPoint - transform.position, (worldPoint - transform.position).magnitude, Layer);
						
						
						if(ray){
							v.pos = ray.point;
														
							if( worldPoint.sqrMagnitude >= (ray.point.sqrMagnitude - magRange) && worldPoint.sqrMagnitude <= (ray.point.sqrMagnitude + magRange) )
								v.endpoint = true;

							if(notifyGameObjectsReached == true) // work only in neccesary cases -- optimization ver 1.1.0--
								// GO touched -> pass to list //
								objReached.Add(ray.collider.gameObject.transform.parent.gameObject);

						}else{
							v.pos =  worldPoint;
							v.endpoint = true;
						}
						
//						Debug.DrawLine(transform.position, v.pos, Color.white);	
						
						//--Convert To local space for build mesh (mesh craft only in local vertex)
						v.pos = transform.InverseTransformPoint(v.pos); 
						//--Calculate angle
						v.angle = getVectorAngle(true,v.pos.x, v.pos.y);
						
						
						
						// -- bookmark if an angle is lower than 0 or higher than 2f --//
						//-- helper method for fix bug on shape located in 2 or more quadrants
						if(v.angle < 0f )
							lows = true;
						
						if(v.angle > 2f)
							his = true;
						
						
						//--Add verts to the main array
						//-- AVOID EXTRA CALCULOUS OF Vector3.angle --//

						if(360 != Mathf.RoundToInt(RangeAngle)){ 
							if (Vector3.Angle(v.pos, Vector3.up) < RangeAngle*.5f) {	// Light angle restriction
								if((v.pos).sqrMagnitude <= lightRadius*lightRadius){
									tempVerts.Add(v);
//									Debug.DrawLine(transform.position, transform.TransformPoint(v.pos), Color.white);
								}
							}
						}else{
							if((v.pos).sqrMagnitude <= lightRadius*lightRadius){
								tempVerts.Add(v);
//								Debug.DrawLine(transform.position, transform.TransformPoint(v.pos), Color.white);
							}
						}


						
						
						
						if(sortAngles == false)
							sortAngles = true;
						

					}
				}
				
				
				
				
				
				// Indentify the endpoints (left and right)
				if(tempVerts.Count > 0){
					
					sortList(tempVerts); // sort first
					
					int posLowAngle = 0; // save the indice of left ray
					int posHighAngle = 0; // same last in right side
					
					//Debug.Log(lows + " " + his);
					
					if(his == true && lows == true){  //-- FIX BUG OF SORTING CUANDRANT 1-4 --//

						if(tempVerts.Count > 1){

							float lowestAngle = -1f;//tempVerts[0].angle; // init with first data
							float highestAngle = tempVerts[0].angle;
							
							
							for(int d=0; d<tempVerts.Count; d++){
								
								
								
								if(tempVerts[d].angle < 1f && tempVerts[d].angle > lowestAngle){
									lowestAngle = tempVerts[d].angle;
									posLowAngle = d;
								}
								
								if(tempVerts[d].angle > 2f && tempVerts[d].angle < highestAngle){
									highestAngle = tempVerts[d].angle;
									posHighAngle = d;
								}
							}
						}


						
						
					}else{
						//-- convencional position of ray points
						// save the indice of left ray
						posLowAngle = 0; 
						posHighAngle = tempVerts.Count-1;
						
					}

					//-- fix error when sort vertex with only 1 tempvert AND rangeAngle < 360 --//
					// --------   ver 1.0.7    ---------//
					//--------------------------------------------------------------------------//
					int endPointLimit = 2;

					if(tempVerts.Count == 1){ 
						endPointLimit = 1;
						tempVerts[0].location = 7; // --lucky se7en
						// --------------------------------------------------------------------------------------------- //
						// --------------------------------------------------------------------------------------------- //

					}else{
						// -- more than one... --//
						tempVerts[posLowAngle].location = 1; // right
						tempVerts[posHighAngle].location = -1; // left
					}

					
					
					//--Add vertices to the main meshes vertexes--//
					if(intelliderConvex == true && endPointLimit > 1){
						allVertices.Add(tempVerts[posLowAngle]);
						allVertices.Add(tempVerts[posHighAngle]);
					}else{
						allVertices.AddRange(tempVerts);
					}
					 

					
					
					
					// -- r ==0 --> right ray
					// -- r ==1 --> left ray

					 
					for(int r = 0; r<endPointLimit; r++){
						
						//-- Cast a ray in same direction continuos mode, start a last point of last ray --//
						Vector3 fromCast = new Vector3();
						bool isEndpoint = false;
						
						if(r==0){
							fromCast = transform.TransformPoint(tempVerts[posLowAngle].pos);
							isEndpoint = tempVerts[posLowAngle].endpoint;
							
						}else if(r==1){
							fromCast = transform.TransformPoint(tempVerts[posHighAngle].pos);
							isEndpoint = tempVerts[posHighAngle].endpoint;
						}
						
						
						
						
						
						if(isEndpoint == true){
							Vector3 dir = (fromCast - transform.position);
							fromCast += (dir * .001f);
							
							
							
							float mag = (lightRadius);// - fromCast.magnitude;
							//float mag = fromCast.magnitude;
							RaycastHit2D rayCont = Physics2D.Raycast(fromCast, dir, mag, Layer);
							//Debug.DrawLine(fromCast, dir.normalized*mag ,Color.green);
							
							
							Vector3 hitp;
							if(rayCont){
								//-- IMPROVED REACHED OBJECTS --// VERSION 1.1.2
								hitp = rayCont.point;   //world p
								
								if(notifyGameObjectsReached == true){ // work only in neccesary cases -- optimization ver 1.1.0--
									if((hitp - transform.position ).sqrMagnitude < (lightRadius * lightRadius)){
										//-- GO reached --> adding to mail list --//
										objReached.Add(rayCont.collider.gameObject.transform.parent.gameObject);
									}		
								}
								
//								Debug.DrawLine(fromCast, hitp, Color.green);
							}else{
								//-- FIX ERROR WEIRD MESH WHEN ENDPOINT COLLIDE OUTSIDE RADIUS VERSION 1.1.2 --//
								//-- NEW INSTANCE OF DIR VECTOR3 ADDED --//
								Vector3 newDir = transform.InverseTransformDirection(dir);	//local p
								hitp = transform.TransformPoint( newDir.normalized * mag); //world p
//								Debug.DrawLine(fromCast, hitp, Color.blue);
							}

							// --- VER 1.0.6 -- //
							//--- this fix magnitud of end point ray (green) ---//

							if((hitp - transform.position ).sqrMagnitude > (lightRadius * lightRadius)){
								//-- FIX ERROR WEIRD MESH WHEN ENDPOINT COLLIDE OUTSIDE RADIUS VERSION 1.1.2  --//
								dir = transform.InverseTransformDirection(dir);	//local p
								hitp = transform.TransformPoint( dir.normalized * mag);
							}

//							Debug.DrawLine(fromCast, hitp, Color.green);	
							
							verts vL = new verts();
							vL.pos = transform.InverseTransformPoint(hitp);
							
							vL.angle = getVectorAngle(true,vL.pos.x, vL.pos.y);
							allVertices.Add(vL);
							
							
							
							
							
							
							
						}
						
						
					}
					
					
				}

				if(notifyGameObjectsReached == true){
					//notify if not null
					if(OnReachedGameObjects != null){
						OnReachedGameObjects(objReached.ToArray());
					}
				}

				
				
			}
			}


		




		//--Step 3: Generate vectors for light cast--//
		//---------------------------------------------------------------------//
		
		int theta = 0;
		//		int amount = 360 / lightSegments;
		float amount = RangeAngle / lightSegments;
		
		
		
		for (int i = 0; i <= lightSegments; i++)  {
			
			theta = Mathf.RoundToInt(amount * i);
			if(theta >= 360) theta = 0;
			
			verts v = new verts();

			// THIS FOLLOWING LINE CONSUME 7-8 FPS by LIGHT 
			PseudoSinCos.initPseudoSinCos();

			v.pos = new Vector3((PseudoSinCos.SinArray[theta]), (PseudoSinCos.CosArray[theta]), 0); // in dregrees (previous calculate)

			Quaternion quat = Quaternion.AngleAxis(RangeAngle*.5f + transform.eulerAngles.z, Vector3.forward);
			v.pos = quat * v.pos;

			//quat = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);
			//v.pos = quat * v.pos;

			v.pos *= lightRadius;
			v.pos += transform.position;
			
			RaycastHit2D ray = Physics2D.Raycast(transform.position,v.pos - transform.position,lightRadius, Layer);
			
			if (!ray){
				
				//Debug.DrawLine(transform.position, v.pos, Color.blue);
				v.pos = transform.InverseTransformPoint(v.pos);
				v.angle = getVectorAngle(true,v.pos.x, v.pos.y);					// store angle without object rotation -> consistency for sorting
				allVertices.Add(v);
				
			} else {

				v.pos = transform.InverseTransformPoint(ray.point);
				v.angle = getVectorAngle(true,v.pos.x, v.pos.y);					// store angle without object rotation -> consistency for sorting
				allVertices.Add(v);

				//if(theta == 0 || theta == Mathf.RoundToInt(RangeAngle)) {			// we only need the two border angle rays, if they collide
					//Debug.DrawLine(transform.position, ray.point, Color.red);
				//	v.pos = transform.InverseTransformPoint(ray.point);
				//	v.angle = getVectorAngle(true,v.pos.x, v.pos.y);				// store angle without object rotation -> consistency for sorting
					//allVertices.Add(v);
				//}
			}
			
		}



		//-- Step 4: Sort each vertice by angle (along sweep ray 0 - 2PI)--//
		//---------------------------------------------------------------------//
		//if (sortAngles == true) {
			sortList(allVertices);
		//}
		//-----------------------------------------------------------------------------


		//--auxiliar step (change order vertices close to light first in position when has same direction) --//
		float rangeAngleComparision = 0.0001f;
		for(int i = 0; i< allVertices.Count; i+=1){
			
			verts uno = allVertices[i];
			verts dos = allVertices[(i +1) % allVertices.Count];

			// -- Comparo el angulo local de cada vertex y decido si tengo que hacer un exchange-- //
			if(uno.angle >= (dos.angle-rangeAngleComparision) && uno.angle <= (dos.angle + rangeAngleComparision)){

				// -- FIX BUG 1.0.7 ( exchange when rangeAngle is less than 360)  -- //
				// ----------------------------------------------------------------- //

				if(uno.location == 7){
					//Debug.Log("7");
					if(uno.angle <= allVertices[allVertices.Count/2].angle){
						uno.location = 1;
					}else{
						uno.location = -1;
					}
				}
				if(dos.location == 7){
					//Debug.Log("7");
					if(dos.angle <= allVertices[allVertices.Count/2].angle){
						dos.location = 1;
					}else{
						dos.location = -1;
					}
				}

				//--------------------------------------------------------------------------//
				//--------------------------------------------------------------------------//


				if(dos.location == -1){ // Right Ray
					
					if(uno.pos.sqrMagnitude > dos.pos.sqrMagnitude){
							allVertices[i] = dos;
							allVertices[(i +1) % allVertices.Count] = uno;
					}
				}
				

				// ALREADY DONE!!
				if(uno.location == 1){ // Left Ray

					if(uno.pos.sqrMagnitude < dos.pos.sqrMagnitude){
						allVertices[i] = dos;
						allVertices[(i +1) % allVertices.Count] = uno;

					}
				}
				
				
				
			}


		}



	}

	void renderLightMesh(){
		//-- Step 5: fill the mesh with vertices--//
		//---------------------------------------------------------------------//
		
		//interface_touch.vertexCount = allVertices.Count; // notify to UI
		
		Vector3 []initVerticesMeshLight = new Vector3[allVertices.Count+1];
		
		initVerticesMeshLight [0] = Vector3.zero;
		
		
		for (int i = 0; i < allVertices.Count; i++) { 
			//Debug.Log(allVertices[i].angle);
			initVerticesMeshLight [i+1] = allVertices[i].pos;
			
			//if(allVertices[i].endpoint == true)
			//Debug.Log(allVertices[i].angle);
			
		}
		
		lightMesh.Clear ();
		lightMesh.vertices = initVerticesMeshLight;
		
		Vector2 [] uvs = new Vector2[initVerticesMeshLight.Length];
		for (int i = 0; i < initVerticesMeshLight.Length; i++) {
			uvs[i] = new Vector2(initVerticesMeshLight[i].x, initVerticesMeshLight[i].y);		
		}
		lightMesh.uv = uvs;
		
		// triangles
		int idx = 0;
		int [] triangles = new int[(allVertices.Count * 3)];
		for (int i = 0; i < (allVertices.Count*3); i+= 3) {
			
			triangles[i] = 0;
			triangles[i+1] = idx+1;
			
			
			if(i == (allVertices.Count*3)-3){
				//-- if is the last vertex (one loop)
				if(Mathf.RoundToInt(RangeAngle) == 360) {
					triangles[i+2] = 1;							// last triangle closes full round
				} else {
					triangles[i+2] = 0;							// no closing when light angle < 360°
				}
			}else{
				triangles[i+2] = idx+2; //next next vertex	
			}
			
			idx++;
		}
		
		
		lightMesh.triangles = triangles;

		if(recalculateNormals == true)
			lightMesh.RecalculateNormals();
			
		var pc = GetComponent<PolygonCollider2D>();
		pc.SetPath(0, lightMesh.vertices.Select(x => (Vector2)x).ToArray());

		GetComponent<Renderer>().sharedMaterial = lightMaterial;
	}

	void sortList(List<verts> lista){
			lista.Sort((item1, item2) => (item2.angle.CompareTo(item1.angle)));
	}

	void drawLinePerVertex(){
		for (int i = 0; i < allVertices.Count; i++)
		{
			if (i < (allVertices.Count -1))
			{
				Debug.DrawLine(allVertices [i].pos , allVertices [i+1].pos, new Color(i*0.02f, i*0.02f, i*0.02f));
			}
			else
			{
				Debug.DrawLine(allVertices [i].pos , allVertices [0].pos, new Color(i*0.02f, i*0.02f, i*0.02f));
			}
		}
	}

	float getVectorAngle(bool pseudo, float x, float y){
		float ang = 0;
		if(pseudo == true){
			ang = pseudoAngle(x, y);
		}else{
			ang = Mathf.Atan2(y, x);
		}
		return ang;
	}
	
	float pseudoAngle(float dx, float dy){
		// Hight performance for calculate angle on a vector (only for sort)
		// APROXIMATE VALUES -- NOT EXACT!! //
		float ax = Mathf.Abs (dx);
		float ay = Mathf.Abs (dy);
		float p = dy / (ax + ay);
		if (dx < 0){
			p = 2 - p;

		}
		return p;
	}

}