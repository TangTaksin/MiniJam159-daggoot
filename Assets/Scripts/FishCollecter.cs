using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishCollecter : MonoBehaviour
{
    float score;
    public TextMeshProUGUI ScoreDisplay;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject, 1);
        var fM = other.GetComponent<FishMass>();
        AddScore(fM.GetFinalMass(), fM.GetMass());
    }

    void AddScore(float mass, float oriMass)
    {
        var modi = 1 - (Mathf.Clamp(mass, 0, oriMass) / oriMass);
        var scoreToAdd = mass * modi;

        score += scoreToAdd;
        ScoreDisplay.text = string.Format("Score : {0}", (int)score);
    }
}
