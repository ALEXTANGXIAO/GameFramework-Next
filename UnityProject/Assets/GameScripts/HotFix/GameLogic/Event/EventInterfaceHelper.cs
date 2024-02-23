using System;
using UnityGameFramework.Runtime;

namespace GameLogic
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventInterfaceImpAttribute : BaseAttribute
    {
        private EEventGroup _eGroup;
        public EEventGroup EventGroup => _eGroup;

        public EventInterfaceImpAttribute(EEventGroup group)
        {
            _eGroup = group;
        }
    }

    public class EventInterfaceHelper
    {
        public static void Init()
        {
            RegisterEventInterface_Logic.Register(GameEvent.EventMgr);
            RegisterEventInterface_UI.Register(GameEvent.EventMgr);
        }
    }
}