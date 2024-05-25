using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.UI;

public class SlicingMechanic : MonoBehaviour
{
    public LayerMask sliceableMask;
    public float maxSharpness = 100;

    public float sharpenTime = 2;
    float sharpTimer;

    float curSharpness;
    public Image SharpnessImage;
    public Image SharpenTimeImage;

    bool sharpening;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        curSharpness = maxSharpness;
    }

    public SlicedHull Slice(GameObject objectToSlice, Material xMat)
    {
        return objectToSlice.Slice(transform.position, transform.forward, xMat);
    }

    public void Update()
    {
        SliceAndSplit();

        if (Input.GetKeyDown(KeyCode.R) && !sharpening)
        {
            StartCoroutine(RefillGuage());
        }

        if (sharpening)
        {
            sharpTimer -= Time.deltaTime;
        }

        SharpnessImage.fillAmount = curSharpness / maxSharpness;
        SharpenTimeImage.fillAmount = sharpTimer / sharpenTime;
    }

    IEnumerator RefillGuage()
    {
        sharpening = true;
        sharpTimer = sharpenTime;

        yield return new WaitForSeconds(sharpenTime);

        sharpTimer = 0;

        curSharpness = maxSharpness;
        sharpening = false;
    }

    private void SliceAndSplit()
    {
        if (Input.GetMouseButtonDown(0) && !sharpening)
        {
            if (curSharpness <= 0)
            {
                // notify dullness
                return;
            }

            var hit = Physics.OverlapBox(transform.position, new Vector3(10, 10f, 10), transform.rotation, sliceableMask);

            if (hit.Length > 0)
            {
                foreach (var col in hit)
                {
                    var fMass = col.GetComponent<FishMass>();

                    curSharpness -= fMass.GetDull();

                    print("slice!");
                    var hull = Slice(col.gameObject, fMass.GetXMat());

                    if (hull != null)
                    {
                        GameObject bottomHull = hull.CreateLowerHull(col.gameObject);
                        bottomHull.layer = 7;
                        var b_Col = bottomHull.AddComponent<BoxCollider>();
                        var b_Rb = bottomHull.AddComponent<Rigidbody>();

                        if (!bottomHull.GetComponent<FishMass>())
                        {
                            var b_Mass = bottomHull.AddComponent<FishMass>();
                            b_Mass.SetMass(fMass.GetMass());
                        }

                        GameObject upperHull = hull.CreateUpperHull(col.gameObject);
                        var u_Col = upperHull.AddComponent<BoxCollider>();
                        var u_Rb = upperHull.AddComponent<Rigidbody>();
                        var u_Mass = upperHull.AddComponent<FishMass>();
                        u_Mass.SetMass(fMass.GetMass());

                        u_Rb.AddForce(Vector3.left*2, ForceMode.Impulse);

                        Destroy(col.gameObject);
                    }

                }
            }

        }
    }
}
