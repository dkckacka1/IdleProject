using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterLevelUpPanel : UIPanel, IUISelectCharacterUpdatable
    {
        [SerializeField] private Transform slotContent;
        [SerializeField] private float longClickTime = 2f;
        [SerializeField] private float maxLongClickTime = 4f;
        
        private readonly List<ConsumableItemSlot> _slotList = new();

        private UISlider _expSlider;
        private ConsumableItemSlot _clickedSlot;
        
        private bool _isClickOver = false;
        private float _usePotionInterval = 0.5f;
        
        private const float DEFAULT_USE_POTION_INTERVAL = 0.5f;
        private const float LONG_CLICK_USE_POTION_INTERVAL = 0.1f;
        private const float MAX_LONG_CLICK_USE_POTION_INTERVAL = 0.025f;
        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CharacterLevelUpButton").Button.onClick.AddListener(LevelUp);
            UIManager.Instance.GetUI<UIButton>("ResetUseExpPotionButton").Button.onClick.AddListener(ResetUseExpPotion);
            _expSlider = UIManager.Instance.GetUI<UISlider>("CharacterExpSlider");
            var expItemDataList = DataManager.Instance.GetDataList<StaticConsumableItemData>()
                .Where(data => data.consumableType == ConsumableType.CharacterExp);

            foreach (var itemData in expItemDataList)
            {
                var slot = CreateSlot(itemData);
                slot.SlotUI.PublishEvent<PointerEventData>(EventTriggerType.PointerUp, OnSlotPointerUp);
                slot.SlotUI.PublishEvent<PointerEventData>(EventTriggerType.PointerDown, OnSlotPointerDown);
                _slotList.Add(slot);
            }

            SetSlotsItemCountByPlayer();
        }

        private ConsumableItemSlot CreateSlot(StaticConsumableItemData itemData)
        {
            var slot = SlotUI.GetSlotUI<ConsumableItemSlot>(slotContent);
            slot.SetData(itemData);
            return slot.GetComponent<ConsumableItemSlot>();
        }

        private void LevelUp()
        {
            Debug.Log("LevelUp");
        }
        
        private void OnSlotPointerDown(PointerEventData eventData, SlotUI slot)
        {
            _isClickOver = true;
            _clickedSlot = slot.GetComponent<ConsumableItemSlot>();
            ClickOver().Forget();
        }

        private void OnSlotPointerUp(PointerEventData eventData, SlotUI slot)
        {
            _isClickOver = false;
            if(_clickedSlot)
                UseExp(_clickedSlot);
        }

        private async UniTaskVoid ClickOver()
        {
            var timer = 0f;
            var intervalTimer = 0f;
            while (_isClickOver)
            {
                await UniTask.Yield();
                timer += Time.deltaTime;
                intervalTimer += Time.deltaTime;
                
                if (timer > maxLongClickTime)
                {
                    _usePotionInterval = MAX_LONG_CLICK_USE_POTION_INTERVAL;
                }
                else if (timer > longClickTime)
                {
                    _usePotionInterval = LONG_CLICK_USE_POTION_INTERVAL;
                }
                
                if (intervalTimer >= _usePotionInterval)
                {
                    UseExp(_clickedSlot);
                    intervalTimer = 0f;
                }
            }

            _usePotionInterval = DEFAULT_USE_POTION_INTERVAL;
        }

        private void UseExp(ConsumableItemSlot targetSlot)
        {
            if (targetSlot.CurrentCount > 0)
            {
                targetSlot.SetCount(targetSlot.CurrentCount - 1);
            }
        }

        private void ResetUseExpPotion()
        {
            SetSlotsItemCountByPlayer();
        }

        private void SetSlotsItemCountByPlayer()
        {
            foreach (var slot in _slotList)
            {
                var data = slot.SlotUI.GetData<StaticConsumableItemData>();
                var playerHaveData = DataManager.Instance.DataController.Player.PlayerData.GetItem(data.itemName);
                slot.SetCount(playerHaveData.itemCount, true);
            }
        }
        
        public void SetCharacter(DynamicCharacterData selectData)
        {
            
        }
    }
}