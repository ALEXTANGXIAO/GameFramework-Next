using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFramework;
using UnityEngine;

namespace GameLogic
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager _instance;

        public static PoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PoolManager>();
                }

                if (_instance == null)
                {
                    GameObject gameObject = new GameObject
                    {
                        name = nameof(PoolManager)
                    };
                    _instance = gameObject.AddComponent<PoolManager>();
                    _instance.poolRootObj = gameObject;
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        [SerializeField] private GameObject poolRootObj;
        private readonly Dictionary<string, GameObjectPoolData> _gameObjectPoolDic = new Dictionary<string, GameObjectPoolData>();
        private readonly Dictionary<string, ObjectPoolData> _objectPoolDic = new Dictionary<string, ObjectPoolData>();

        public GameObject GetGameObject(string assetName, Transform parent = null)
        {
            GameObject obj = null;
            if (_gameObjectPoolDic.TryGetValue(assetName, out var gameObjectPoolData) && gameObjectPoolData.poolQueue.Count > 0)
            {
                obj = gameObjectPoolData.GetObj(parent);
            }

            if (obj == null)
            {
                obj = GameModule.Resource.LoadGameObject(assetName, parent: parent);
                obj.name = assetName;
            }

            return obj;
        }

        public async UniTask<GameObject> GetGameObjectAsync(string assetName, Transform parent = null)
        {
            GameObject obj = null;
            if (_gameObjectPoolDic.TryGetValue(assetName, out var gameObjectPoolData) && gameObjectPoolData.poolQueue.Count > 0)
            {
                obj = gameObjectPoolData.GetObj(parent);
            }

            if (obj == null)
            {
                obj = await GameModule.Resource.LoadGameObjectAsync(assetName, parent: parent);
                obj.name = assetName;
            }

            return obj;
        }

        public void PushGameObject(GameObject obj)
        {
            string objName = obj.name;
            if (_gameObjectPoolDic.TryGetValue(objName, out var gameObjectPoolData))
            {
                gameObjectPoolData.PushObj(obj);
            }
            else
            {
                _gameObjectPoolDic.Add(objName, new GameObjectPoolData(obj, poolRootObj));
            }
        }

        public T GetObject<T>() where T : class, new()
        {
            string fullName = typeof(T).FullName;
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(fullName))
            {
                throw new GameFrameworkException($"GetObject full name is invalid.");
            }
#endif
            return CheckObjectCache<T>() ? (T)_objectPoolDic[fullName].GetObj() : new T();
        }

        public void PushObject(object obj)
        {
            string fullName = obj.GetType().FullName;
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(fullName))
            {
                throw new GameFrameworkException($"GetObject full name is invalid.");
            }
#endif
            if (_objectPoolDic.TryGetValue(fullName, out var objectPoolData))
            {
                objectPoolData.PushObj(obj);
            }
            else
            {
                _objectPoolDic.Add(fullName, new ObjectPoolData(obj));
            }
        }

        private bool CheckObjectCache<T>()
        {
            string fullName = typeof(T).FullName;
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(fullName))
            {
                throw new GameFrameworkException($"GetObject full name is invalid.");
            }
#endif
            return _objectPoolDic.ContainsKey(fullName) && _objectPoolDic[fullName].poolQueue.Count > 0;
        }

        public void Clear(bool clearGameObject = true, bool clearCObject = true)
        {
            if (clearGameObject)
            {
                for (int index = 0; index < poolRootObj.transform.childCount; ++index)
                {
                    Destroy(poolRootObj.transform.GetChild(index).gameObject);
                }

                _gameObjectPoolDic.Clear();
            }

            if (!clearCObject)
            {
                return;
            }

            _objectPoolDic.Clear();
        }

        public void ClearAllGameObject() => Clear(clearCObject: false);

        public void ClearGameObject(string prefabName)
        {
            GameObject obj = poolRootObj.transform.Find(prefabName).gameObject;
            if (obj == null)
            {
                return;
            }

            Destroy(obj);
            _gameObjectPoolDic.Remove(prefabName);
        }

        public void ClearGameObject(GameObject prefab) => ClearGameObject(prefab.name);

        public void ClearAllObject() => Clear(false);

        public void ClearObject<T>() => _objectPoolDic.Remove(typeof(T).FullName);

        public void ClearObject(Type type) => _objectPoolDic.Remove(type.FullName);
    }


    public class ObjectPoolData
    {
        public readonly Queue<object> poolQueue = new Queue<object>();

        public ObjectPoolData(object obj) => PushObj(obj);

        public void PushObj(object obj) => poolQueue.Enqueue(obj);

        public object GetObj() => poolQueue.Dequeue();
    }

    public class GameObjectPoolData
    {
        public readonly GameObject fatherObj;
        public readonly Queue<GameObject> poolQueue;

        public GameObjectPoolData(GameObject obj, GameObject poolRootObj)
        {
            fatherObj = new GameObject(obj.name);
            fatherObj.transform.SetParent(poolRootObj.transform);
            poolQueue = new Queue<GameObject>();
            PushObj(obj);
        }

        public GameObjectPoolData(GameObject fatherObj)
        {
            this.fatherObj = fatherObj;
        }

        public void PushObj(GameObject obj)
        {
            poolQueue.Enqueue(obj);
            obj.transform.SetParent(fatherObj.transform);
            obj.SetActive(false);
        }

        public GameObject GetObj(Transform parent = null)
        {
            GameObject go = poolQueue.Dequeue();
            go.SetActive(true);
            go.transform.SetParent(parent);
            if (parent == null)
            {
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            }

            return go;
        }
    }
}