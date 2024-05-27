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

    public Vector3 boxHalfExtents = new Vector3(2, .5f, 2);

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
        Rotate();
        SliceAndSplit();

        if (Input.GetKeyDown(KeyCode.Mouse1) && !sharpening)
        {
            StartCoroutine(RefillGuage());
            AudioManager.Instance.PlaySFX(AudioManager.Instance.knifeRain);
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
                AudioManager.Instance.PlaySFX(AudioManager.Instance.noKnife);

                return;
            }

            // ui slice anim
            uiAnimator.Play("chop");
            // play slice sfx here 



            var hit = Physics.OverlapBox(transform.position, boxHalfExtents, transform.rotation, sliceableMask);
            print(hit.Length);

            if (hit.Length > 0)
            {
                AudioManager.Instance.PlaySliceSFX();

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
                        upperHull.layer = 7;
                        var u_Col = upperHull.AddComponent<BoxCollider>();

                        var u_Rb = upperHull.AddComponent<Rigidbody>();
                        u_Rb.AddForce(Vector3.left * 2, ForceMode.Impulse);

                        var u_Mass = upperHull.AddComponent<FishMass>();
                        u_Mass.SetMass(fMass.GetMass());
                        u_Mass.SetXMat(fMass.GetXMat());

                        Destroy(col.gameObject);
                    }

                }
            }
            else if (hit.Length <= 0)
            {   //miss penelty
                curSharpness -= missPenelty;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.noKnife);
            }


        }
    }

    public void Rotate()
    {
        var mouseInput = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, mouseInput, 0));
    }

    void OnDrawGizmos()
    {
        // Set the color for the Gizmos
        Gizmos.color = Color.red;

        // Calculate the box center
        Vector3 boxCenter = transform.position;

        // Draw the box
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2); // Gizmos.DrawWireCube needs full size, so we multiply by 2
    }

}
