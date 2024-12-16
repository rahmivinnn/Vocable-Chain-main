using UnityEngine;
using System.Collections;

public class ButtonOpenDialog : MyButton {

    public DialogType dialogType;
    public DialogShow dialogShow;
    public string xboxButton = "A";

    private void Update()
    {
      
        if (Input.GetButtonDown(xboxButton))
        {
            OnButtonClick();
        }
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        DialogController.instance.ShowDialog(dialogType, dialogShow);
    }
}
