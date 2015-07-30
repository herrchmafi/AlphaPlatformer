using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class RaycastController : MonoBehaviour {
	
	public LayerMask collisionMask;
	
	protected const float skinWidth = .015f;
	public int horizontalRayCount;
	public int verticalRayCount;
	
	protected float horizontalRaySpacing;
	
	protected float verticalRaySpacing;
	
	public BoxCollider2D boxCollider;
	
	protected RaycastOrigins raycastOrigins;
	
	//Prevent null reference exceptions because this is referenced in other scripts' Start methods
	public virtual void Awake() {
		this.boxCollider = GetComponent<BoxCollider2D> ();
	}
	
	public virtual void Start() {
		this.CalculateRaySpacing ();
	}
	//Calculate references to specific positions of gameobject bounds
	protected void UpdateRaycastOrigins() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand (skinWidth * -2);
		
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}
	
	//Determine spacing of rays based on ray count of respective axis
	protected void CalculateRaySpacing() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand (skinWidth * -2);
		
		this.horizontalRayCount = Mathf.Clamp (this.horizontalRayCount, 2, int.MaxValue);
		this.verticalRayCount = Mathf.Clamp (this.verticalRayCount, 2, int.MaxValue);
		
		this.horizontalRaySpacing = bounds.size.y / (this.horizontalRayCount - 1);
		this.verticalRaySpacing = bounds.size.x / (this.verticalRayCount - 1);
	}
	
	protected struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
