using UnityEngine;
using UnityEngine.SceneManagement;

public class LodeScenes : MonoBehaviour
{
    public void Load_Scene(int sceneIndex)
    {
       if (sceneIndex == 1)
       {
            SceneManager.LoadScene("Slide 1");
       }
       else if(sceneIndex == 2)
       {
            SceneManager.LoadScene("Slide 2");
       }
       else if(sceneIndex == 3)
       {
            SceneManager.LoadScene("Slide 3");
       }
       else if(sceneIndex == 4)
       {
            SceneManager.LoadScene("Slide 4");
       }
       else if(sceneIndex == 5)
       {
            SceneManager.LoadScene("Slide 5");
       }
        
    }
}
