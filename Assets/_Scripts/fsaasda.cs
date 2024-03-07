using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fsaasda : MonoBehaviour
{
    private bool isTouching = false;
    private int touchCount = 0;

    private void Update()
    {
        // Проверьте, что считывается более 2 пальцев.
        if (touchCount > 2)
        {
            // Ваш код для обработки более чем двух пальцев.
        }

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    // Создайте RaycastHit2D для получения информации о столкновении с коллайдерами.
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                    if (hit.collider != null)
                    {
                        // Проверьте, что столкновение произошло с Collider2D вашего объекта.
                        if (hit.collider.CompareTag("YourTag"))
                        {
                            //isTouching[touch.fingerId] = true;
                            // Здесь можно добавить код, выполняемый при касании UI элемента.
                            Debug.Log("Палец касается UI элемента " + hit.collider.gameObject.name);
                        }
                    }
                    else
                    {
                        Debug.Log("hui");
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    isTouching = false;
                    touchCount--;

                    // Здесь можно добавить код, выполняемый при отпускании пальца с экрана.

                    Debug.Log("Палец отпущен с экрана.");
                }
            }
        }
    }
}
