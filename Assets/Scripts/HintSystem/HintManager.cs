using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames
{
    [Serializable]
    public class HintManager
    {
        [SerializeField] private HintData hintData;

        public event Action HintIntervalReachedAction;
        public event Action<HintItem>  WrongItemPlacedOnTheSlotAction;
        public event Action<HintItem> CorrectItemPlacedOnTheSlotAction;
        public event Action<HintItem> SlotIsBlankAction;
        public event Action<HintItem>  ShowDescriptionOnCorrectAction;
        public event Action<HintItem>  ShowHintOnWrongAction;

        public event Action AllSlotsFilledCorrectly;


        private WaitForSeconds _hintWaitForSeconds;
        private Coroutine _hintIntervalCoroutine;
        private MonoBehaviour _hintIntervalCoroutineHandler;
        private MagnetSlot _currentActiveSlot;

        public MagnetSlot CurrentActiveSlot => _currentActiveSlot;

        public bool ShowOneSlotAtATime => hintData.ShowOneSlotAtATime;
        public bool ShowHintOnWrong => hintData.ShowHintOnWrong;
        public bool ShowDescriptionOnCorrect => hintData.ShowDescriptionOnCorrect;

        public HintData.SlotState GetActiveSlotState()
        {
            return CheckSlotState(_currentActiveSlot, false);
        }

        public HintItem GetActiveSlotHintItem()
        {
            return GetSlotHint(_currentActiveSlot);
        }
        
        private IEnumerator HintOnInterval()
        {
            while (true)
            {
                yield return _hintWaitForSeconds;
                HintIntervalReachedAction?.Invoke();
            }
        }
        
        

        private void ActivateNextSlot(HintItem currentSlotHintItem)
        {
            LockSlot(currentSlotHintItem.MagnetSlot, true);
            ActivateFirstBlankSlot();
        }

        public void Initialize()
        {
            hintData.Initialize();
            _hintWaitForSeconds = new WaitForSeconds(hintData.HintIntervalSeconds);
            if (ShowOneSlotAtATime)
            {
                DeactivateAllSlots();
                ActivateFirstBlankSlot();
                CorrectItemPlacedOnTheSlotAction += ActivateNextSlot;
            }
            
        }

        public void ActivateAllSlots()
        {
            foreach (MagnetSlot slot in hintData.SortedSlotsList)
            {
                slot.gameObject.SetActive(true);
            }
        }
        
        public void DeactivateAllSlots()
        {
            foreach (MagnetSlot slot in hintData.SortedSlotsList)
            {
                slot.gameObject.SetActive(false);
            }
        }
        
        public void DeactivateSlot(MagnetSlot slot)
        {
            slot.gameObject.SetActive(false);
        }

        public HintItem GetSlotHint(MagnetSlot slot)
        {
            return hintData.GetSlotHintItem(slot);
        }

        public void LockSlot(MagnetSlot slot, bool isLocked)
        {
            slot.LockSlot(isLocked);
        }
        
        public void ActivateFirstBlankSlot()
        {
            HintItem hintItem = hintData.GetFirstHintItemBlank();
            Debug.Log($"AAA {hintItem}");
            if(hintItem != null)
            {
                _currentActiveSlot = hintItem.MagnetSlot;
                _currentActiveSlot.gameObject.SetActive(true);
                LockSlot(_currentActiveSlot, false);
            }
            else
            {
                Debug.Log(1);
                AllSlotsFilledCorrectly?.Invoke();
            }
            
        }

        public void DeactivateNotCorrectSlots()
        {
            foreach (MagnetSlot slot in hintData.SortedSlotsList)
            {
                if (hintData.GetSlotState(slot) == HintData.SlotState.Correct)
                {
                    continue;
                }
                
                slot.gameObject.SetActive(false);
            }
        }
        
        public HintData.SlotState CheckSlotState(MagnetSlot slot, bool invokeActions)
        {
            HintData.SlotState slotState = hintData.GetSlotState(slot);
            if (!invokeActions)
            {
                return slotState;
            }
            HintItem slotHint = hintData.GetSlotHintItem(slot);
            switch (slotState)
            {
                case HintData.SlotState.Blank:
                {
                    SlotIsBlankAction?.Invoke(slotHint);
                    break;
                }
                case HintData.SlotState.Correct:
                {
                    CorrectItemPlacedOnTheSlotAction?.Invoke(slotHint);
                    if(ShowDescriptionOnCorrect){
                        ShowDescriptionOnCorrectAction?.Invoke(slotHint);
                    }
                    break;
                }
                case HintData.SlotState.Wrong:
                {
                    WrongItemPlacedOnTheSlotAction?.Invoke(slotHint);
                    if(ShowHintOnWrong)
                    {
                        ShowHintOnWrongAction?.Invoke(slotHint);
                    }
                    break;
                }
            }

            return slotState;
        }

        public void StartHintInterval(MonoBehaviour coroutineHandler)
        {
            _hintIntervalCoroutine = coroutineHandler.StartCoroutine(HintOnInterval());
            _hintIntervalCoroutineHandler = coroutineHandler;
        }

        public void StopHintInterval()
        {
            if (_hintIntervalCoroutine != null)
            {
                _hintIntervalCoroutineHandler.StopCoroutine(_hintIntervalCoroutine);
                _hintIntervalCoroutine = null;
                _hintIntervalCoroutineHandler = null;
            }
        }

        public void RestartHintInterval(MonoBehaviour coroutineHandler)
        {
            StopHintInterval();
            StartHintInterval(coroutineHandler);
        }
    }
}
