using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{

		public int MovementRange = 100;
		[HideInInspector] public Vector2 InputAxis;
		Vector3 m_StartPos;
		bool m_UseX = true; // Toggle for using the x axis
		bool m_UseY = true; // Toggle for using the Y axis

        void Start()
        {
            m_StartPos = transform.position;
        }

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;

			InputAxis = new Vector2(-delta.x, delta.y);

		}


		public void OnDrag(PointerEventData data)
		{
			Vector3 newPos = Vector3.zero;

			if (m_UseX)
			{
				int delta = (int)(data.position.x - m_StartPos.x);
				delta = Mathf.Clamp(delta, - MovementRange, MovementRange);
				newPos.x = delta;
			}

			if (m_UseY)
			{
				int delta = (int)(data.position.y - m_StartPos.y);
				delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
				newPos.y = delta;
			}
			transform.position = new Vector3(m_StartPos.x + newPos.x, m_StartPos.y + newPos.y, m_StartPos.z + newPos.z);
			UpdateVirtualAxes(transform.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
			transform.position = m_StartPos;
			UpdateVirtualAxes(m_StartPos);
		}


		public void OnPointerDown(PointerEventData data) { }
	}
}