using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneHourJam.Manager
{
    public class AAA : MonoBehaviour
    {
        public bool IsDead;

        private void Awake()
        {
            StartCoroutine(Loose());
        }

        private IEnumerator Loose()
        {
            yield return new WaitForSeconds(5f);
            if (!IsDead) SceneManager.LoadScene("Lost");
        }
    }
}
