using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class RaycastController : MonoBehaviour
{
    public LayerMask CollisionMask;
    public float Skin = 0.015f;
    public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;
    public float HorizontalRaySpacing;
    public float VerticalRaySpacing;

    public BoxCollider2D PlayerCollider;

    public RaycastOrigins raycastOrigins;

    // Use this for initialization
    void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void UpdateRaycastOrigins()
    {
        Bounds Bounds = PlayerCollider.bounds;
        Bounds.Expand(Skin * -2);

        raycastOrigins.bottomLeft = new Vector2(Bounds.min.x, Bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(Bounds.max.x, Bounds.min.y);
        raycastOrigins.topLeft = new Vector2(Bounds.min.x, Bounds.max.y);
        raycastOrigins.topRight = new Vector2(Bounds.max.x, Bounds.max.y);
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
