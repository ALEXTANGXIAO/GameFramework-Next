using System;
using System.Collections.Generic;
using System.Reflection;
using UnityGameFramework.Runtime;

namespace GameLogic
{
    public interface IUiController
    {
        void RegUIMessage();
    }

    public class UIController
    {
        private static readonly List<IUiController> m_ListController = new List<IUiController>();

        public static void RegisterAllController()
        {
            var targetType = typeof(IUiController);
            List<IUiController> result = new List<IUiController>();
            var allTypes = Assembly.GetCallingAssembly().GetTypes();
            foreach (var t in allTypes)
            {
                Type[] tfs = t.GetInterfaces();
                foreach (var tf in tfs)
                {
                    if (tf.FullName == targetType.FullName)
                    {
                        IUiController a = Activator.CreateInstance(t) as IUiController;
                        result.Add(a);
                    }
                }
            }

            foreach (var uiController in result)
            {
                AddController(uiController);
            }
        }

        private static void AddController<T>() where T : IUiController, new()
        {
            foreach (var controllerType in m_ListController)
            {
                var type = controllerType.GetType();
                if (type == typeof(T))
                {
                    Log.Fatal("repeat controller type: {0}", typeof(T).Name);
                    return;
                }
            }

            var controller = new T();
            m_ListController.Add(controller);
            controller.RegUIMessage();
        }

        private static void AddController(IUiController uiController)
        {
#if UNITY_EDITOR
            foreach (var controller in m_ListController)
            {
                var type = controller.GetType();
                if (type == uiController.GetType())
                {
                    Log.Fatal("repeat controller type: {0}", uiController.GetType().Name);
                    return;
                }
            }
#endif
            m_ListController.Add(uiController);
            uiController.RegUIMessage();
        }
    }
}