using RapidGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleUI : MonoBehaviour {

    [SerializeField]
    protected CustomClass custom = new CustomClass();

    protected PopupWindow popup;

    #region declarations
    [System.Serializable]
    public class CustomClass {

        public string name;

        public List<string> shapes = new List<string>();
    }
    #endregion

    #region unity
    private void OnEnable() {
        popup = new PopupWindow("popup");
        popup.Add(() => RGUI.Field(custom, typeof(CustomClass)));
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) popup.SetOpen(!popup.isOpen);
    }
    private void OnGUI() {
        popup.DoGUI();
    }
    #endregion
}
