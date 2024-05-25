using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FishMass : MonoBehaviour
{
    [SerializeField] float mass;
    [SerializeField] int dullValue = 5;
    [SerializeField] Material xSectionMat;
    float finalMass;
    Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        CalculateMass();
    }

    public float GetMass()
    {
        return mass;
    }

    public void SetMass(float _mass)
    {
        mass = _mass;
    }

    public float GetFinalMass()
    {
        return finalMass;
    }

    public Material GetXMat()
    {
        return xSectionMat;
    }

    public int GetDull()
    {
        return dullValue;
    }

    void CalculateMass()
    {
        print(_collider.bounds.size);
        finalMass = (_collider.bounds.size.x * _collider.bounds.size.z) * mass;
    }

}
