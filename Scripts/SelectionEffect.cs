using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Control
{
    public class SelectionEffect : MonoBehaviour
    {
        [SerializeField] PlayerController player;
        [SerializeField] Fighter playerFighter;
        [SerializeField] GameObject ui_object;
        [SerializeField] float offSetY = 1.5f;
        RectTransform canvasRect;
        private void Awake()
        {
            canvasRect = GetComponent<RectTransform>();
        }
        void Update()
        {
            if (!player.InteractWithComponent()) return;
            if (playerFighter.GetTarget != null)
            {
                GameObject target = playerFighter.GetTarget.gameObject;

                float offSetPosY = target.transform.position.y + offSetY;

                Vector3 offSetPos = new Vector3(target.transform.position.x, offSetPosY, target.transform.position.z);

                Vector2 canvasPos;

                Vector2 screenPoint = Camera.main.WorldToScreenPoint(offSetPos);
                
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
                //no idea wtf this does

                //Vector2 viewportPosition = Camera.main.WorldToViewportPoint(target.transform.position);
                //Vector2 objectScreenPos = new Vector2(((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)), ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject ui_selected = Instantiate(ui_object, transform);
                    RectTransform position = ui_selected.GetComponent<RectTransform>();
                    print(canvasPos);
                    ui_selected.transform.parent = this.transform;
                    ui_selected.GetComponent<RectTransform>().anchoredPosition = canvasPos;
                }
            }
        }
    }
}