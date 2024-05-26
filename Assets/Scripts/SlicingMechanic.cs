using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.UI;

public class SlicingMechanic : MonoBehaviour
{
    public LayerMask sliceableMask;
    public float maxSharpness = 100;
    public float missPenelty = 10;
    public float sharpenTime = 2;
    float sharpTimer = 0;

    float curSharpness;
    public Image SharpnessImage;
    public Gradient SharpnessGradient;
    public Image SharpenTimeImage;
    public Animator uiAnimator;

    bool sharpening;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        curSharpness = maxSharpness;
    }

    public SlicedHull Slice(GameObject objectToSlice)
    {
        return objectToSlice.Slice(transform.position, transform.forward);
    }

    public void Update()
    {
        SliceAndSplit();

        if (Input.GetKeyDown(KeyCode.Mouse1) && !sharpening)
        {
            StartCoroutine(RefillGuage());
        }

        if (sharpening)
        {
            sharpTimer -= Time.deltaTime;
            SharpenTimeImage.fillAmount = 1 - (sharpTimer / sharpenTime);
        }

        var sharpColor = curSharpness / maxSharpness;

        SharpnessImage.fillAmount = sharpColor;
        SharpnessImage.color = SharpnessGradient.Evaluate(sharpColor);

        
    }

    IEnumerator RefillGuage()
    {
        sharpening = true;
        sharpTimer = sharpenTime;

        uiAnimator.Play("start_resharp");

        //play resharpening sfx here

        yield return new WaitForSeconds(sharpenTime);

        uiAnimator.SetTrigger("End Sharp");

        sharpTimer = 0;
        SharpenTimeImage.fillAmount = 0;

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
                uiAnimator.Play("dull");
                // Play dull sfx here 

                return;
            }
            
            // ui slice anim
            uiAnimator.Play("chop");
            // play slice sfx here 


            var hit = Physics.OverlapBox(transform.position, new Vector3(10, 10f, 10), transform.rotation, sliceableMask);

            if (hit.Length > 0)
            {
                foreach (var col in hit)
                {
                    var fMass = col.GetComponent<FishMass>();

                    var hull = Slice(col.gameObject);

                    if (hull != null)
                    {
                        curSharpness -= fMass.GetDull();

                        GameObject bottomHull = hull.CreateLowerHull(col.gameObject, fMass.GetXMat());
                        bottomHull.layer = 7;
                        var b_Col = bottomHull.AddComponent<BoxCollider>();
                        var b_Rb = bottomHull.AddComponent<Rigidbody>();

                        if (!bottomHull.GetComponent<FishMass>())
                        {
                            var b_Mass = bottomHull.AddComponent<FishMass>();
                            b_Mass.SetMass(fMass.GetMass());
                            b_Mass.SetXMat(fMass.GetXMat());
                        }

                        GameObject upperHull = hull.CreateUpperHull(col.gameObject, fMass.GetXMat());
                        var u_Col = upperHull.AddComponent<BoxCollider>();

                        var u_Rb = upperHull.AddComponent<Rigidbody>();
                        u_Rb.AddForce(Vector3.left*2, ForceMode.Impulse);

                        var u_Mass = upperHull.AddComponent<FishMass>();
                        u_Mass.SetMass(fMass.GetMass());
                        u_Mass.SetXMat(fMass.GetXMat());

                        Destroy(col.gameObject);
                    }

                }
            }
            //miss penelty
            curSharpness -= missPenelty;

        }
    }
}
