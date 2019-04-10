using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugLoadLevel : MonoBehaviour
{


    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl)) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene("FutureLevelShocase");
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                SceneManager.LoadScene("JapanLevelShowcase");
            }
        }
    }
}
