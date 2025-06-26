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
    public class CharacterLevelUpPanel : UIPanel, IUISelectCharacterUpdatable
    {
        [SerializeField] private Transform slotContent;
        [SerializeField] private float longClickTime = 2f;
        [SerializeField] private float maxLongClickTime = 4f;

        private readonly List<ConsumableItemSlot> _slotList = new();

        private ConsumableItemSlot _clickedSlot;
        private CharacterExpChangerUI _expChangerUI;
        private DynamicCharacterData _selectCharacter;

        private bool _isClickOver;
        private float _usePotionInterval = 0.5f;
        private int _expAmount = 0;

        private const float DEFAULT_USE_POTION_INTERVAL = 0.5f;
        private const float LONG_CLICK_USE_POTION_INTERVAL = 0.1f;
        private const float MAX_LONG_CLICK_USE_POTION_INTERVAL = 0.025f;

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
                slot.SlotUI.PublishEvent<PointerEventData>(EventTriggerType.PointerUp, OnSlotPointerUp);
                slot.SlotUI.PublishEvent<PointerEventData>(EventTriggerType.PointerDown, OnSlotPointerDown);
                _slotList.Add(slot);
            }

            SetSlotsItemCountByPlayer();
        }

        public override void ClosePanel()
        {
            base.ClosePanel();
            
            ResetUseExpPotion();
        }

        private ConsumableItemSlot CreateSlot(StaticConsumableItemData itemData)
        {
            var slot = SlotUI.GetSlotUI<ConsumableItemSlot>(slotContent).SlotUI;
            slot.SetData(itemData);
            return slot.GetComponent<ConsumableItemSlot>();
        }

        private void LevelUp()
        {
            var playerCharacter =
                DataManager.Instance.DataController.Player.PlayerData.GetCharacter(_selectCharacter.CharacterData
                    .addressValue.characterName);

            // 들어온 경험치양 만큼 캐릭터 레벨업
            // 레벨업 된 캐릭터 플레이어에 적용
            playerCharacter.AddExp(_expAmount);
            _expAmount = 0;
            _selectCharacter.UpdateCharacter(playerCharacter.level);
            
            // 소비아이템 사용해주기
            foreach (var slot in _slotList)
            {
                AcceptExpPotion(slot);                
            }
            
            // UI 업데이트
            UIManager.Instance.GetUIsOfType<IUISelectCharacterUpdatable>()
                .ForEach(ui => ui.SetCharacter(_selectCharacter));
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
            var itemName = itemSlot.SlotUI.GetData<StaticConsumableItemData>().itemName;

            var userItem = DataManager.Instance.DataController.Player.PlayerData.GetItem(itemName);
            userItem.itemCount = itemSlot.CurrentCount;
        }

        private void ResetUseExpPotion()
        {
            SetSlotsItemCountByPlayer();

            if (_selectCharacter != null)
            {
                var playerCharacter =
                    DataManager.Instance.DataController.Player.PlayerData.GetCharacter(_selectCharacter.CharacterData.name);
                
                _expChangerUI.SetPlayerCharacter(playerCharacter);
            }
            
            _expAmount = 0;
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

        public void SetCharacter(DynamicCharacterData character)
        {
            var playerCharacter =
                DataManager.Instance.DataController.Player.PlayerData.GetCharacter(character.CharacterData.name);

            _expChangerUI.SetPlayerCharacter(playerCharacter);
            _selectCharacter = character;

            ResetUseExpPotion();
        }
    }
}