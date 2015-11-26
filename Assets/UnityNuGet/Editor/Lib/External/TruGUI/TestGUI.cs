using UnityEngine;
using System.Collections;
using TruGUI.Controls;
using TruGUI.Utilities;
using TruGUI;
using System.Linq;

public class TestGUI : MonoBehaviour
{
    public Button theBtn = new Button(new GUIElementOptions("theBtn", new GUIContent("hi"), null, ElementLayoutOption.Width(100), ElementLayoutOption.Height(50)));

    void Start()
    {
        theBtn.OnActionTaken += new TruGUIElement<bool>.Action<bool>(theBtn_OnActionTaken);
    }

    void theBtn_OnActionTaken(TruGUIElement<bool> sender, bool result)
    {
        theBtn.ColapseAnimate(TruGUIElement.ColapseExpandOptions.Both, .5f);
    }


    void OnGUI()
    {
        theBtn.Draw();
    }
}
