using UnityEngine;
using System.Collections;
[RequireComponent(typeof(BoxCollider2D))]

public class RaycastController : MonoBehaviour
{
    public LayerMask CollisionMask;
    public float Skin = 0.015f;
    public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;
    [HideInInspector]
    public float HorizontalRaySpacing;
    [HideInInspector]

    public float VerticalRaySpacing;
   public BoxCollider2D PlayerCollider;

    public RaycastOrigins raycastOrigins;

    // Use this for initialization
    public virtual void Start()
    {

         PlayerCollider = GetComponent<BoxCollider2D>();
         CalculateRaySpacing();

    }

 
  public  void UpdateRaycastOrigins()
    {
        // Bounds are NOT equal to the cubes box collider 
        Bounds Bounds = PlayerCollider.bounds;
        Bounds.Expand(Skin * -2);

        // Points of the collider 
        //[WORKS PERFECTLY]
        // Bot left collision
        raycastOrigins.bottomLeft = new Vector2(Bounds.min.x, Bounds.min.y);
        // Bot Right collision
        raycastOrigins.bottomRight = new Vector2(Bounds.max.x, Bounds.min.y);

        //[DOESNET WORK PERFECTLY]
        // Top Left
        raycastOrigins.topLeft = new Vector2(Bounds.min.x, Bounds.max.y  );
       // top Right
        raycastOrigins.topRight = new Vector2(Bounds.max.x, Bounds.max.y);

        // DEBUG raycast is in scene for a better look  
        // Attaching to untiy and playing scene will take you to the stuff thats not working 
        // EG Bounds transform is not displaying the cubes transfom
    }
    public void CalculateRaySpacing()
    {
        Bounds Bounds = PlayerCollider.bounds;
        Bounds.Expand(Skin * -2);

        HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, 2, int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

        HorizontalRaySpacing = Bounds.size.y / (HorizontalRayCount - 1);
        VerticalRaySpacing = Bounds.size.x / (VerticalRayCount - 1);
    }


   public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
