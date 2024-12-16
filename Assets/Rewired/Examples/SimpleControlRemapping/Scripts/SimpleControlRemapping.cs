﻿namespace Rewired.Demos {

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;
    using System.Collections;

    [AddComponentMenu("")]
    public class SimpleControlRemapping : MonoBehaviour {

        private const string category = "Default";
        private const string layout = "Default";
        private const string uiCategory = "UI";

        private InputMapper inputMapper = new InputMapper();

        public GameObject buttonPrefab;
        public GameObject textPrefab;
        public RectTransform fieldGroupTransform;
        public RectTransform actionGroupTransform;
        public Text controllerNameUIText;
        public Text statusUIText;

        private ControllerType selectedControllerType = ControllerType.Keyboard;
        private int selectedControllerId = 0;
        private List<Row> rows = new List<Row>();

        private Player player { get { return ReInput.players.GetPlayer(0); } }
        private ControllerMap controllerMap {
            get {
                if(controller == null) return null;
                return player.controllers.maps.GetMap(controller.type, controller.id, category, layout);
            }
        }
        private Controller controller { get { return player.controllers.GetController(selectedControllerType, selectedControllerId); } }

        public RectTransform movableObject;  
        public float moveSpeed = 200f;  

        private void OnEnable() {
            if(!ReInput.isReady) return;

            inputMapper.options.timeout = 5f;
            inputMapper.options.ignoreMouseXAxis = true;
            inputMapper.options.ignoreMouseYAxis = true;
            
            ReInput.ControllerConnectedEvent += OnControllerChanged;
            ReInput.ControllerDisconnectedEvent += OnControllerChanged;
            inputMapper.InputMappedEvent += OnInputMapped;
            inputMapper.StoppedEvent += OnStopped;

            InitializeUI();
        }

        private void OnDisable() {
            inputMapper.Stop();
            inputMapper.RemoveAllEventListeners();
            ReInput.ControllerConnectedEvent -= OnControllerChanged;
            ReInput.ControllerDisconnectedEvent -= OnControllerChanged;
        }

        private void RedrawUI() {
            if(controller == null) { 
                ClearUI();
                return;
            }

            controllerNameUIText.text = controller.name;

            for(int i = 0; i < rows.Count; i++) {
                Row row = rows[i];
                InputAction action = rows[i].action;

                string name = string.Empty;
                int actionElementMapId = -1;

                foreach(var actionElementMap in controllerMap.ElementMapsWithAction(action.id)) {
                    if(actionElementMap.ShowInField(row.actionRange)) {
                        name = actionElementMap.elementIdentifierName;
                        actionElementMapId = actionElementMap.id;
                        break;
                    }
                }

                row.text.text = name;
                row.button.onClick.RemoveAllListeners(); 
                int index = i;
                row.button.onClick.AddListener(() => OnInputFieldClicked(index, actionElementMapId));
            }
        }

        private void ClearUI() {
            if(selectedControllerType == ControllerType.Joystick) controllerNameUIText.text = "No joysticks attached";
            else controllerNameUIText.text = string.Empty;

            for(int i = 0; i < rows.Count; i++) {
                rows[i].text.text = string.Empty;
            }
        }

        private void InitializeUI() {
            foreach(Transform t in actionGroupTransform) {
                Object.Destroy(t.gameObject);
            }
            foreach(Transform t in fieldGroupTransform) {
                Object.Destroy(t.gameObject);
            }

            foreach(var action in ReInput.mapping.ActionsInCategory(category)) {
                if(action.type == InputActionType.Axis) {
                    CreateUIRow(action, AxisRange.Full, action.descriptiveName);
                    CreateUIRow(action, AxisRange.Positive, !string.IsNullOrEmpty(action.positiveDescriptiveName) ? action.positiveDescriptiveName : action.descriptiveName + " +");
                    CreateUIRow(action, AxisRange.Negative, !string.IsNullOrEmpty(action.negativeDescriptiveName) ? action.negativeDescriptiveName : action.descriptiveName + " -");
                } else if(action.type == InputActionType.Button) {
                    CreateUIRow(action, AxisRange.Positive, action.descriptiveName);
                }
            }

            RedrawUI();
        }

        private void CreateUIRow(InputAction action, AxisRange actionRange, string label) {
            GameObject labelGo = Object.Instantiate<GameObject>(textPrefab);
            labelGo.transform.SetParent(actionGroupTransform);
            labelGo.transform.SetAsLastSibling();
            labelGo.GetComponent<Text>().text = label;

            GameObject buttonGo = Object.Instantiate<GameObject>(buttonPrefab);
            buttonGo.transform.SetParent(fieldGroupTransform);
            buttonGo.transform.SetAsLastSibling();

            rows.Add(
                new Row() {
                    action = action,
                    actionRange = actionRange,
                    button = buttonGo.GetComponent<Button>(),
                    text = buttonGo.GetComponentInChildren<Text>()
                }
            );
        }

        private void SetSelectedController(ControllerType controllerType) {
            bool changed = false;

            if(controllerType != selectedControllerType) { 
                selectedControllerType = controllerType;
                changed = true;
            }

            int origId = selectedControllerId;
            if(selectedControllerType == ControllerType.Joystick) {
                if(player.controllers.joystickCount > 0) selectedControllerId = player.controllers.Joysticks[0].id;
                else selectedControllerId = -1;
            } else {
                selectedControllerId = 0;
            }
            if(selectedControllerId != origId) changed = true;

            if(changed) {
                inputMapper.Stop();
                RedrawUI();
            }
        }

        public void OnControllerSelected(int controllerType) {
            SetSelectedController((ControllerType)controllerType);
        }

        private void OnInputFieldClicked(int index, int actionElementMapToReplaceId) {
            if(index < 0 || index >= rows.Count) return;
            if(controller == null) return;

            StartCoroutine(StartListeningDelayed(index, actionElementMapToReplaceId));
        }

        private IEnumerator StartListeningDelayed(int index, int actionElementMapToReplaceId) {
            yield return new WaitForSeconds(0.1f);

            inputMapper.Start(
                new InputMapper.Context() {
                    actionId = rows[index].action.id,
                    controllerMap = controllerMap,
                    actionRange = rows[index].actionRange,
                    actionElementMapToReplace = controllerMap.GetElementMap(actionElementMapToReplaceId)
                }
            );

            player.controllers.maps.SetMapsEnabled(false, uiCategory);

            statusUIText.text = "Listening...";
        }

        private void OnControllerChanged(ControllerStatusChangedEventArgs args) {
            SetSelectedController(selectedControllerType);
        }

        private void OnInputMapped(InputMapper.InputMappedEventData data) {
            RedrawUI();
        }

        private void OnStopped(InputMapper.StoppedEventData data) {
            statusUIText.text = string.Empty;
            player.controllers.maps.SetMapsEnabled(true, uiCategory);
        }

        private void Update() {
          
            if (controller != null) {
                
                float moveX = player.GetAxis("MoveX"); 
                float moveY = player.GetAxis("MoveY"); 
                Vector2 moveInput = new Vector2(moveX, moveY);

                MoveObject(moveInput);
            }
        }

        private void MoveObject(Vector2 moveInput) {
            if (movableObject == null) return;

            movableObject.anchoredPosition += moveInput * moveSpeed * Time.deltaTime;
        }

        private class Row {
            public InputAction action;
            public AxisRange actionRange;
            public Button button;
            public Text text;
        }
    }
}
