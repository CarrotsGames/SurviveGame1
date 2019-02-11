using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

public class RaycastController : MonoBehaviour {
    //TODO Find new place for these
  

    public Vector3 StartOffset;

    //spacing inbetween raycast 
    public const float skinWidth = .015f;
    public int horizonalRayCount = 4;
    public int verticalRayCount = 4;
    [HideInInspector]
    public float horizontalRaySpacing;
    public float verticalRaySpacing;
    public BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;
    public virtual void Start()
    {
        //word collider = collider object "BoxCollider2D"
        collider = GetComponent<BoxCollider2D>();

        //grabs CalulateRaySpacing Void
        CalulateRaySpacing();
         //where the players location is = where the level starts + its offset
    }
   public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);
        // Raycasts collisions (AABB) 
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topRight = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

 
    }
    //------------------------------------------------------------------------------------------------------------------
    //		CalulateRaySpacing()
    // Positions raycasting rays correctly
    //
    // Param:
    //				None
    // Return:
    //				Void
    //------------------------------------------------------------------------------------------------------------------
    public void CalulateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizonalRayCount = Mathf.Clamp(horizonalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizonalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
    //------------------------------------------------------------------------------------------------------------------
    //		RaycastOrgins()
    // hold method 
    //
    // Param:
    //				None
    // Return:
    //				struct
    //------------------------------------------------------------------------------------------------------------------
    public struct RaycastOrigins
    {

        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;

    }
}
