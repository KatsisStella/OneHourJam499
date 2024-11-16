using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace OneHourJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        private readonly List<(SpriteRenderer, Box)> _boxes = new();

        private float _timeBeforeNextSpawn = 2f;

        private int _amountLeft = 0;

        [SerializeField]
        private GameObject _boxPrefab;

        [SerializeField]
        private LevelInfo[] _levels;

        [SerializeField]
        private HandInfo[] _hands;

        [SerializeField]
        private Sprite _idleHand;

        [SerializeField]
        private Image _handImage;

        private void Awake()
        {
            Instance = this;

            StartCoroutine(SpawnBoxes());
        }

        private void Register(SpriteRenderer sr, Box box)
        {
            _boxes.Add((sr, box));
        }

        private IEnumerator SpawnBoxes()
        {
            var bounds = CalculateBounds(Camera.main);
            while (_amountLeft < 15)
            {
                yield return new WaitForSeconds(_timeBeforeNextSpawn);
                var go = Instantiate(_boxPrefab, new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y)), Quaternion.identity);
                var sr = go.GetComponent<SpriteRenderer>();

                var i = _amountLeft * (_levels.Length + 1) / 15;
                if (i >= _levels.Length) i = _levels.Length - 1;
                var b = _levels[i];
                var s = b.Sprites[Random.Range(0, b.Sprites.Length)];
                sr.sprite = s.Normal;

                Register(sr, s);

                _timeBeforeNextSpawn -= .05f;
            }

            // You won
        }

        public void FightBoxes(Vector2Int v)
        {
            StartCoroutine(Punch(_hands.First(x => x.Direction == v).Punch));
            for (int i = _boxes.Count - 1; i >= 0; i--)
            {
                var b = _boxes[i];
                Vector2Int dir;
                if (Mathf.Abs(b.Item1.transform.position.x) > Mathf.Abs(b.Item1.transform.position.y))
                {
                    dir = b.Item1.transform.position.x > 0f ? Vector2Int.right : Vector2Int.left;
                }
                else
                {
                    dir = b.Item1.transform.position.y > 0f ? Vector2Int.up : Vector2Int.down;
                }

                if (dir == v)
                {
                    _boxes.RemoveAt(i);
                    StartCoroutine(GetPunched(b.Item1, b.Item2));
                }
            }
        }

        private IEnumerator GetPunched(SpriteRenderer s, Box b)
        {
            s.sprite = b.Punched;
            yield return new WaitForSeconds(.1f);
            s.enabled = false;
            yield return new WaitForSeconds(.1f);
            s.enabled = true;
            yield return new WaitForSeconds(.5f);
            Destroy(s.gameObject);
        }

        private IEnumerator Punch(Sprite s)
        {
            _handImage.gameObject.SetActive(true);
            _handImage.sprite = s;
            yield return new WaitForSeconds(.25f);
            _handImage.gameObject.SetActive(false);
        }

        // http://answers.unity.com/answers/502236/view.html
        public static Bounds CalculateBounds(Camera cam)
        {
            float screenAspect = Screen.width / (float)Screen.height;
            float cameraHeight = cam.orthographicSize * 2;
            Bounds bounds = new(
                cam.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }

        public void OnFightUp(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) FightBoxes(Vector2Int.up);
        }
        public void OnFightRight(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) FightBoxes(Vector2Int.right);
        }
        public void OnFightLeft(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) FightBoxes(Vector2Int.left);
        }
        public void OnFightDown(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) FightBoxes(Vector2Int.down);
        }
    }

    [System.Serializable]
    public class HandInfo
    {
        public Sprite Punch;
        public Vector2Int Direction;
    }

    [System.Serializable]
    public class LevelInfo
    {
        public Box[] Sprites;
    }

    [System.Serializable]
    public class Box
    {
        public Sprite Normal, Punched;
    }
}
