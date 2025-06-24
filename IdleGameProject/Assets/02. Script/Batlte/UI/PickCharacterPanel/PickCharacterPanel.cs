using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Battle.Spawn;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Data;
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

        [SerializeField] private PickCharacterPanelSlot slotPrefab;
        [FormerlySerializedAs("dorpSlot")] [SerializeField] private SlotUI dropSlot;

        private readonly List<PickCharacterPanelSlot> _slotList = new();

        private CharacterData _pickData;
        private Camera _mainCamera;
        private BattleManager _battleManager;
        
        public override async UniTask Initialized()
        {
            _mainCamera = Camera.main;
            _battleManager = GameManager.GetCurrentSceneManager<BattleManager>();

            var userMainCharacterList = DataManager.Instance.DataController.userData.UserFormation.GetCharacterNameList();
            var heroList = DataManager.Instance.DataController.userData.UserHeroList.Select(hero => hero.heroName);
            
            foreach (var userHeroName in heroList)
            {
                var createSlot = CreateSlot(DataManager.Instance.GetData<CharacterData>(userHeroName));
                createSlot.SetChoice(userMainCharacterList.Any(mainCharacterName => mainCharacterName == userHeroName));
                _slotList.Add(createSlot);
            }
            
            UIManager.Instance.GetUI<UIButton>("BattleStartButton").Button.onClick.AddListener(StartBattle);
            
            dropSlot.gameObject.SetActive(false);
        }
        
        private PickCharacterPanelSlot CreateSlot(CharacterData characterData)
        {
            var slot = Instantiate(slotPrefab, pickCharacterScrollView.content);
            slot.SetData(characterData);
            slot.beginDragEvent.AddListener(OnSlotDragBegin);
            slot.endDragEvent.AddListener(OnSlotDragEnd);
            slot.dragEvent.AddListener(OnSlotDrag);
            slot.clickEvent.AddListener(OnSlotClick);

            return slot;
        }

        private void OnSlotDragBegin(PointerEventData eventData)
        {
            dropSlot.gameObject.SetActive(true);

            var selectCharacterData = eventData.pointerDrag.GetComponent<SlotUI>().GetData<CharacterData>();

            if (selectCharacterData)
            {
                _pickData = selectCharacterData;
            }
            
            dropSlot.SetData(selectCharacterData);
        }
        
        private void OnSlotDragEnd(PointerEventData eventData)
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
                    var slot = eventData.pointerDrag.GetComponent<PickCharacterPanelSlot>();
                    if (!IsCharacterSpawnedSlot(slot, out var spawnedCharacter))
                        // 스폰된 캐릭터가 없다면 생성
                    {
                        slot.SetChoice(true);
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

        private void OnSlotDrag(PointerEventData eventData)
        {
            dropSlot.transform.position = eventData.position;
        }

        private void OnSlotClick(PointerEventData eventData)
        {
            var slot = ExecuteEvents.GetEventHandler<IPointerClickHandler>(eventData.pointerCurrentRaycast.gameObject)
                .GetComponent<PickCharacterPanelSlot>();

            if (IsCharacterSpawnedSlot(slot, out var character))
            {
                _battleManager.spawnController.RemoveCharacter(character, CharacterAIType.Player);
                
                slot.SetChoice(false);
            }
        }
        
        private void StartBattle()
        {
            _battleManager.BattleStateEventBus.ChangeEvent(BattleStateType.Battle);
            _battleManager.GameStateEventBus.ChangeEvent(GameStateType.Play);
            
            ClosePanel();
        }

        private bool IsCharacterSpawnedSlot(SlotUI slot, out CharacterController character)
        {
            var data = slot.GetData<CharacterData>();
            var characterName = data.addressValue.characterName;

            var characterList = _battleManager.GetCharacterList(CharacterAIType.Player);
            character = characterList.FirstOrDefault(character => character.StatSystem.CharacterName == characterName);

            return character is not null;
        }
    }
}