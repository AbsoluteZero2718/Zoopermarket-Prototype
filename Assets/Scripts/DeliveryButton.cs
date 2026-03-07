using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliveryButton : MonoBehaviour
{
   public void LoadSceneByName(string DeliveryScene)
    {
        SceneManager.LoadScene(DeliveryScene);
    }

   public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
