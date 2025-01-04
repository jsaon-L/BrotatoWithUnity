using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogFormTest : DialogForm<int>
{
    public TMP_InputField height;
    protected override int GetDialogResuleData()
    {
        return int.Parse(height.text);
    }
}
