using System.Collections.Generic;
using System.Linq;
using IdleProject.Battle.AI;
using IdleProject.Battle.Spawn;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.UI
{
    public class PickCharacterPanel : UIPanel
    {
        [SerializeField] private ScrollRect pickCharacterScrollView;
        [FormerlySerializedAs("dorpSlot")] [SerializeField] private SlotUI dropSlot;

        private readonly List<SlotUI> _slotList = new();

        private DynamicCharacterData _pickData;
        private Camera _mainCamera;
        private BattleManager _battleManager;
        
        public override void Initialized()
        {
            _mainCamera = Camera.main;
            _battleManager = GameManager.GetCurrentSceneManager<BattleManager>();

            var userMainCharacterList = DataManager.Instance.DataController.Player.GetPlayerFormation().GetCharacterNameList();
            var characterList = DataManager.Instance.DataController.Player.GetCharacterList;
            
            foreach (var character in characterList)
            {
                var createSlot = CreateSlot(character);
                createSlot.SetOrganization(userMainCharacterList.Any(mainCharacterName => mainCharacterName == character.StaticData.addressValue.characterName));
                _slotList.Add(createSlot.SlotUI);
            }
            
            UIManager.Instance.GetUI<UIButton>("BattleStartButton").Button.onClick.AddListener(StartBattle);
            
            dropSlot.gameObject.SetActive(false);
        }
        
        private OrganizationSlot CreateSlot(DynamicCharacterData characterData)
        {
            var slot = SlotUI.GetSlotUI<OrganizationSlot>(pickCharacterScrollView.content);
            slot.SetData(characterData);
            slot.PublishEvent<PointerEventData>(EventTriggerType.PointerClick, OnSlotClick);
            slot.PublishEvent<PointerEventData>(EventTriggerType.BeginDrag, OnSlotDragBegin);
            slot.PublishEvent<PointerEventData>(EventTriggerType.Drag, OnSlotDrag);
            slot.PublishEvent<PointerEventData>(EventTriggerType.EndDrag, OnSlotDragEnd);
            
            return slot.GetSlotParts<OrganizationSlot>();
        }

        private void OnSlotDragBegin(PointerEventData eventData, SlotUI slot)
        {
            dropSlot.gameObject.SetActive(true);

            var selectCharacterData = slot.GetData<DynamicCharacterData>();

            if (selectCharacterData is not null)
            {
                _pickData = selectCharacterData;
            }
            
            dropSlot.SetData(selectCharacterData);
        }
        
        private void OnSlotDragEnd(PointerEventData eventData, SlotUI slot)
        {
            if (_pickData is null) return;
            
            dropSlot.gameObject.SetActive(false);

            var ray = _mainCamera.ScreenPointToRay(eventData.position);
            var hits = Physics.RaycastAll(ray, 100f);
            
            foreach (var hit in hits)
            {
                var targetSpawnPosition = hit.collider.gameObject.GetComponent<SpawnPosition>();
                if (!targetSpawnPosition) 
                    continue;
                
                
                if (targetSpawnPosition.SpawnAIType == CharacterAIType.Player)
                {
                    var organizationParts = slot.GetSlotParts<OrganizationSlot>();
                    if (!IsCharacterSpawnedSlot(organizationParts, out var spawnedCharacter))
                        // 스폰된 캐릭터가 없다면 생성
                    {
                        organizationParts.SetOrganization(true);
                        _battleManager.spawnController
                            .SpawnCharacterBySpawnPosition(_pickData, targetSpawnPosition).Forget();
                    }
                    else
                        // 스폰된 캐릭터가 있다면
                    {
                        var spawnedPosition = _battleManager.spawnController
                            .GetSpawnPosition(spawnedCharacter, CharacterAIType.Player);

                        if (spawnedPosition == targetSpawnPosition)
                            // 동일한 위치를 선택했다면
                            return;
                        
                        // 다른 위치를 선택했다면
                        _battleManager.spawnController.SwapCharacter(spawnedPosition, targetSpawnPosition);
                    }
                }
            }

            _pickData = null;
        }

        private void OnSlotDrag(PointerEventData eventData, SlotUI slot)
        {
            dropSlot.transform.position = eventData.position;
        }


        
        private void OnSlotClick(PointerEventData eventData, SlotUI slot)
        {
            var organizationSlotParts = slot.GetSlotParts<OrganizationSlot>();

            if (IsCharacterSpawnedSlot(organizationSlotParts, out var character))
            {
                _battleManager.spawnController.RemoveCharacter(character, CharacterAIType.Player);
                
                organizationSlotParts.SetOrganization(false);
            }
        }
        
        private void StartBattle()
        {
            _battleManager.BattleStateEventBus.ChangeEvent(BattleStateType.Battle);
            _battleManager.GameStateEventBus.ChangeEvent(BattleGameStateType.Play);
            
            ClosePanel();
        }

        private bool IsCharacterSpawnedSlot(OrganizationSlot organizationSlot, out CharacterController character)
        {
            var data = organizationSlot.SlotUI.GetData<DynamicCharacterData>();
            var characterName = data.StaticData.addressValue.characterName;

            var characterList = _battleManager.GetCharacterList(CharacterAIType.Player);
            character = characterList.FirstOrDefault(character => character.StatSystem.CharacterName == characterName);

            return character is not null;
        }
    }
}