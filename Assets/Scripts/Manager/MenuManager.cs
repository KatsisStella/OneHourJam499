using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneHourJam.Manager
{
    public class MenuManager : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
