using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SlicingMechanic : MonoBehaviour
{
    public LayerMask sliceableMask;
    Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    public SlicedHull Slice(GameObject objectToSlice)
    {
        return objectToSlice.Slice(transform.position, transform.forward);
    }

    public void Update()
    {
        CutterPosition();
        SliceAndSplit();
    }

    private void CutterPosition()
    {
        var m2W = camera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(m2W.x, 0);
    }

    private void SliceAndSplit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics.OverlapBox(transform.position, new Vector3(10, .1f, 10), transform.rotation, sliceableMask);

            if (hit.Length > 0)
            {
                foreach (var col in hit)
                {
                    print("slice!");
                    var hull = Slice(col.gameObject);

                    if (hull != null)
                    {
                        GameObject bottomHull = hull.CreateLowerHull(col.gameObject);
                        bottomHull.layer = 7;
                        var b_Col = bottomHull.AddComponent<BoxCollider>();


                        GameObject upperHull = hull.CreateUpperHull(col.gameObject);
                        upperHull.layer = 7;
                        var u_Col = upperHull.AddComponent<BoxCollider>();
                        var u_Rb = upperHull.AddComponent<Rigidbody>();

                        u_Rb.AddExplosionForce(1, transform.position, 5f);

                        Destroy(col.gameObject);
                    }

                }
            }
            else
                return;
        }
    }
}
