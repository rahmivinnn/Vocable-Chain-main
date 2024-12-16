using UnityEngine;
using System.Collections;

public class ButtonPlay : MyButton {
    public int gotoIndex = 1;
    public bool useScreenFader;
    int world, subWorld, level;

    public string xboxButton = "A"; 

    protected override void Start()
    {
        base.Start();
        world = Prefs.unlockedWorld;
        subWorld = Prefs.unlockedSubWorld;
        level = Prefs.unlockedLevel;

    }

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

#if UNITY_WEBGL
        CUtils.LoadScene(gotoIndex, useScreenFader);
#else
        CUtils.LoadScene(world == 0 && subWorld == 0 && level == 0 ? 3 : gotoIndex, useScreenFader);
#endif
    }
}
