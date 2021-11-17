using UnityEngine;
using UnityEngine.SceneManagement;

public class NoReboot : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("NoReboot");
        if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
    }
    void OnLevelWasLoaded(int level)
    {
        if(level != 4)
        {
            Destroy(this.gameObject);
        }
    }
}
