using System;
using UnityGameFramework.Runtime;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupLogic)]
    interface IActorLogicEvent
    {
        void OnMainPlayerNameChange();

        void OnMainPlayerLevelChange();

        void OnMainPlayerGoldChange(uint oldVal, uint newVal);

        void OnMainPlayerDiamondChange(uint oldVal, uint newVal);

        void OnMainPlayerBindDiamondChange(uint oldVal, uint newVal);

        void OnMainPlayerExpChange(ulong oldVal, ulong newVal);

        void OnMainPlayerFightValChange();

        void OnMainPlayerDataChange();

        void OnShowFightValueChange(ulong oldVal, ulong newVal);

        void OnMainPlayerEquipChange();

        void OnMainPlayerBagDataChange();

        void OnMainPlayerAttrDataChange();

        void OnMainPlayerEquipDataChange();

        void OnGameActorCreate(object actor);

        void OnMainPlayerLevelDataChange();

        void OnMainPlayerLoginSuccess();

        void OnMainPlayerDressChange();
    }
}