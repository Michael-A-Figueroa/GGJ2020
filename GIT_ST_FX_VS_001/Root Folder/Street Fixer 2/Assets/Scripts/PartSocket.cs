using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSocket : MonoBehaviour
{
    public PartType part;

    int repairCurNeed;
    public float transitionTime = 1f;
    public bool isFixed;
    MeshRenderer matRend;
    GAMEMANAGER gm;

    private void Start()
    {
        matRend = GetComponent<MeshRenderer>();
        gm = FindObjectOfType<GAMEMANAGER>();
        repairCurNeed = Random.Range(0, 2);
        if (repairCurNeed == 0) { isFixed = true; gm.CheckFixedParts(); }
    }


    void Update()
    {
        #region Value
        if (repairCurNeed == 2 && matRend.material.color != gm.warningColor) ChangeColor(gm.warningColor);
        else if (repairCurNeed == 1 && matRend.material.color != gm.alertColor) ChangeColor(gm.alertColor);
        else if (repairCurNeed == 0 && matRend.material.color != gm.goodColor) { ChangeColor(gm.goodColor); if (!isFixed) { isFixed = true; gm.CheckFixedParts(); } }
        #endregion
    }

    public void Repair()
    {
        if (repairCurNeed > 0) repairCurNeed--;
        else if(repairCurNeed == 0) isFixed = true;

        gm.CheckFixedParts();
    }

    public void Damage()
    {
        if (repairCurNeed == 0 || repairCurNeed < 2) repairCurNeed++;

        gm.AddPartToFix(gameObject.GetComponent<PartSocket>());
    }

    void ChangeColor(Color newColor)
    {
        matRend.material.color = Color.Lerp(matRend.material.color, newColor, transitionTime);
    }

}
