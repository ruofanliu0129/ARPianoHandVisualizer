using UnityEngine;

/// <summary>
/// Unity单例模式基础类<br/>
/// 此类继承了MonoBehaviour，所以需要单例模式的类直接继承此类即可，同时也相当于集成了MonoBehaviour
/// </summary>
/// <typeparam name="T">子类泛型</typeparam>
public class UnitySingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T I
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject obj = new GameObject();

                    //obj.hideFlags = HideFlags.DontSave;
                    obj.name = typeof(T).ToString();
                    //obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = (T) obj.AddComponent(typeof(T));
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            if (_instance!=I)
            {
                Debug.Log(gameObject.name+"作为单例，重复部分被删了");
                Destroy(gameObject);
            }
            
        }
    }
}