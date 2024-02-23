using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace GameLogic
{
    class RegisterEventInterface_Logic
    {
        public static void Register(EventMgr mgr)
        {
            var disp = mgr.Dispatcher;

            HashSet<Type> types = CodeTypes.Instance.GetTypes(typeof(EventInterfaceImpAttribute));

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(EventInterfaceImpAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                EventInterfaceImpAttribute httpHandlerAttribute = (EventInterfaceImpAttribute)attrs[0];

                if (httpHandlerAttribute.EventGroup != EEventGroup.GroupLogic)
                {
                    continue;
                }

                object obj = Activator.CreateInstance(type, disp);

                mgr.RegWrapInterface(obj.GetType().GetInterfaces()[0]?.FullName, obj);
            }
        }
    }
}