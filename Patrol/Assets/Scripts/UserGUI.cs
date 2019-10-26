using UnityEngine;

public class UserGUI : MonoBehaviour
{
    GUIStyle scoreStyle;
    GUIStyle buttonStyle;
    GUIStyle countDownStyle;
    GUIStyle finishStyle;
    
    void Start() {
        scoreStyle = new GUIStyle();
        scoreStyle.fontSize = 40;
        scoreStyle.normal.textColor = Color.blue;
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 15;
        buttonStyle.normal.textColor = Color.white;
        countDownStyle = new GUIStyle();
        countDownStyle.fontSize = 25;
        countDownStyle.normal.textColor = Color.white;
        finishStyle = new GUIStyle();
        finishStyle.fontSize = 40;
        finishStyle.normal.textColor = Color.white;
    }
    private void Update() {
        IUserAction action = SSDirector.Instance.currentSceneController as IUserAction;
        ISceneController controller = SSDirector.Instance.currentSceneController as ISceneController;
        if (controller.GetGameState().Equals(GameState.Running)) {
            float translationX = Input.GetAxis("Horizontal");
            float translationZ = Input.GetAxis("Vertical");
            action?.MovePlayer(translationX, translationZ);
        }
    }
    
    private void OnGUI() {
        string buttonText = "";
        ISceneController controller = SSDirector.Instance.currentSceneController as ISceneController;

        GUI.Label(new Rect(0, 0, 100, 50),
            "Score: " + controller.GetScore().ToString(), scoreStyle);

    }
}