using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// Calls from UI button
        /// </summary>
        public void Reload()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}