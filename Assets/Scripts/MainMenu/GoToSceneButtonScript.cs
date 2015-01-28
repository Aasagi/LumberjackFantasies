using UnityEngine;

public class GoToSceneButtonScript : MonoBehaviour
{
    public string UnitySceneToTransitionTo = "Insert Scene";

    #region Methods
    private void OnClick()
    {
        Application.LoadLevel(UnitySceneToTransitionTo);
    }
    #endregion
}
