using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishCollecter : MonoBehaviour
{
    float score;
    float smallestCut;

    public TextMeshProUGUI ScoreDisplay;
    public TextMeshProUGUI ResultScoreDisplay;
    public TextMeshProUGUI ResultSmallestDisplay;

    private void OnEnable()
    {
        smallestCut = Mathf.Infinity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject, 1);
        var fM = other.GetComponent<FishMass>();
        AddScore(fM.GetFinalMass(), fM.GetMass(), fM.GetMultiplier());

        CompareCut(fM.GetFinalMass());
    }

    void AddScore(float mass, float oriMass, int multiplier)
    {
        var modi = 1 - (Mathf.Clamp(mass, 0, oriMass) / oriMass);
        var scoreToAdd = mass * modi * multiplier;

        score += scoreToAdd;
        ScoreDisplay.text = string.Format("Score : {0} pts", (int)score);
        ResultScoreDisplay.text = string.Format("{0} pts", (int)score);
        ResultSmallestDisplay.text = string.Format("{0} mg(s)", smallestCut);

    }

    void CompareCut(float mass)
    {
        if (mass < smallestCut)
             smallestCut = mass;
    }
}
