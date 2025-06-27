using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterLevelUpPanel : UIPanel, IUISelectCharacterUpdatable, IUISelectEquipmentItemUpdatable
    {
        [SerializeField] private Transform slotContent;
        [SerializeField] private float longClickTime = 2f;
        [SerializeField] private float maxLongClickTime = 4f;

        private readonly List<SlotUI> _slotList = new();

        private ConsumableItemSlot _clickedSlot;
        private CharacterExpChangerUI _expChangerUI;

        private bool _isClickOver;
        private float _usePotionInterval = 0.5f;
        private int _expAmount = 0;

        private const float DEFAULT_USE_POTION_INTERVAL = 0.5f;
        private const float LONG_CLICK_USE_POTION_INTERVAL = 0.1f;
        private const float MAX_LONG_CLICK_USE_POTION_INTERVAL = 0.025f;

        private DynamicCharacterData GetSelectCharacter =>
            UIManager.Instance.GetUI<SelectCharacterPanel>().SelectedCharacter;
        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CharacterLevelUpButton").Button.onClick.AddListener(LevelUp);
            UIManager.Instance.GetUI<UIButton>("ResetUseExpPotionButton").Button.onClick.AddListener(ResetUseExpPotion);

            _expChangerUI = UIManager.Instance.GetUI<CharacterExpChangerUI>("CharacterExpChangerUI");

            var expItemDataList = DataManager.Instance.GetDataList<StaticConsumableItemData>()
                .Where(data => data.consumableType == ConsumableType.CharacterExp);

            foreach (var itemData in expItemDataList)
            {
                var slot = CreateSlot(itemData);
                slot.PublishEvent<PointerEventData>(EventTriggerType.PointerUp, OnSlotPointerUp);
                slot.PublishEvent<PointerEventData>(EventTriggerType.PointerDown, OnSlotPointerDown);
                _slotList.Add(slot);
            }

            SetSlotsItemCountByPlayer();
        }

        public override void ClosePanel()
        {
            base.ClosePanel();
            
            ResetUseExpPotion();
        }

        private SlotUI CreateSlot(StaticConsumableItemData itemData)
        {
            var slot = SlotUI.GetSlotUI<ConsumableItemSlot>(slotContent);
            slot.BindData(itemData);
            return slot;
        }

        private void LevelUp()
        {
            var selectCharacter = GetSelectCharacter;
            
            // 들어온 경험치양 만큼 캐릭터 레벨업
            // 레벨업 된 캐릭터 플레이어에 적용
            selectCharacter.AddExp(_expAmount);
            _expAmount = 0;
            selectCharacter.UpdateCharacter(selectCharacter.Level);
            
            // 소비아이템 사용해주기
            foreach (var slot in _slotList)
            {
                AcceptExpPotion(slot.GetSlotParts<ConsumableItemSlot>());
            }
            
            // UI 업데이트
            UIManager.Instance.GetUIsOfType<IUISelectCharacterUpdatable>()
                .ForEach(ui => ui.SelectCharacterUpdatable(selectCharacter));
            
            // 세이브
            DataManager.Instance.SaveController.SaveCharacter(selectCharacter);
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
            if (_clickedSlot)
                UseExpPotion(_clickedSlot);
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
                    UseExpPotion(_clickedSlot);
                    intervalTimer = 0f;
                }
            }

            _usePotionInterval = DEFAULT_USE_POTION_INTERVAL;
        }

        private void UseExpPotion(ConsumableItemSlot itemSlot)
        {
            if (itemSlot.CurrentCount > 0)
            {
                itemSlot.SetCount(itemSlot.CurrentCount - 1, true);
                var amount = itemSlot.SlotUI.GetData<StaticConsumableItemData>().value;
                _expChangerUI.AddExp(amount);
                _expAmount += amount;
            }
        }

        private void AcceptExpPotion(ConsumableItemSlot itemSlot)
        {
            var itemIndex = itemSlot.SlotUI.GetData<StaticConsumableItemData>().Index;

            var userItem = DataManager.Instance.DataController.Player.PlayerConsumableItemDataDic[itemIndex];
            userItem.itemCount = itemSlot.CurrentCount;
            DataManager.Instance.SaveController.SaveConsumableItem(userItem);
        }

        private void ResetUseExpPotion()
        {
            SetSlotsItemCountByPlayer();

            var selectedCharacter = GetSelectCharacter;
            if (selectedCharacter != null)
            {
                _expChangerUI.SetPlayerCharacter(selectedCharacter);
            }
            
            _expAmount = 0;
        }

        private void SetSlotsItemCountByPlayer()
        {
            foreach (var slot in _slotList)
            {
                var data = slot.GetData<StaticConsumableItemData>();
                var playerHaveData = DataManager.Instance.DataController.Player.PlayerConsumableItemDataDic[data.Index];
                slot.GetSlotParts<ConsumableItemSlot>().SetCount(playerHaveData.itemCount, true);
            }
        }

        public void SelectCharacterUpdatable(DynamicCharacterData selectCharacter)
        {
            _expChangerUI.SetPlayerCharacter(selectCharacter);

            ResetUseExpPotion();
        }
        
        public void SelectEquipmentItemUpdatable(DynamicEquipmentItemData item)
        {
            ClosePanel();
        }
    }
}