using UnityEngine;
using UnityEngine.SceneManagement;

namespace Temp
{
    public class HackRestart : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}