using UnityEngine;

public class Debug : UnityEngine.Debug
{
    public static void DrawCircle(Vector3 position, Quaternion rotation,  float radius, float segments, Color color)
    {
        if (radius <= 0.0f || segments <= 0)
        {
            return;
        }
        float angleStep = (360.0f / segments);

        // –езультат умножаетс€ на константу Mathf.Deg2Rad, котора€ преобразует градусы в радианы
        // которые требуютс€ дл€ методов тригонометрии класса Mathf в Unity
        angleStep *= Mathf.Deg2Rad;

        Vector3 lineStart = Vector3.zero;
        Vector3 lineEnd = Vector3.zero;

        for (int i = 0; i < segments; i++)
        {
            // Ќачало линии определ€етс€ как начальный угол текущего отрезка (i).
            lineStart.x = Mathf.Cos(angleStep * i);
            lineStart.y = Mathf.Sin(angleStep * i);
            lineStart.z = 0.0f;

            //  онец линии определ€етс€ углом наклона следующего отрезка (i+1).
            lineEnd.x = Mathf.Cos(angleStep * (i + 1));
            lineEnd.y = Mathf.Sin(angleStep * (i + 1));
            lineEnd.z = 0.0f;

            // –езультаты умножаютс€ таким образом, чтобы они соответствовали желаемому радиусу
            lineStart *= radius;
            lineEnd *= radius;

            //–езультаты умножаютс€ на кватернион вращени€, чтобы повернуть их
            // поскольку эта операци€ не €вл€етс€ коммутативной, результат должен быть
            // переназначен, вместо использовани€ оператора присваивани€ умножени€ (*
            lineStart = rotation * lineStart;
            lineEnd = rotation * lineEnd;

            // –езультаты смещаютс€ в зависимости от желаемого положени€/начала координат
            lineStart += position;
            lineEnd += position;

            DrawLine(lineStart, lineEnd, color);
        }
    }

    public static void DrawSector(Vector3 position, Quaternion orientation, float startAngle, float endAngle, 
        float radius, Color color, int arcSegments = 32)
    {
        float arcSpan = Mathf.DeltaAngle(startAngle, endAngle);

        // ѕоскольку Mathf.DeltaAngle возвращает значение угла со знаком кратчайшего пути между двум€ углами, необходимо
        // сместить его на 360,0 градусов, чтобы получить положительное значение
        if (arcSpan <= 0)
        {
            arcSpan += 360.0f;
        }

        // шаг угла рассчитываетс€ путем делени€ длины дуги на количество сегментов аппроксимацииts
        float angleStep = (arcSpan / arcSegments) * Mathf.Deg2Rad;
        float stepOffset = startAngle * Mathf.Deg2Rad;

        float stepStart = 0.0f;
        float stepEnd = 0.0f;
        Vector3 lineStart = Vector3.zero;
        Vector3 lineEnd = Vector3.zero;

        Vector3 arcStart = Vector3.zero;
        Vector3 arcEnd = Vector3.zero;
        Vector3 arcOrigin = position;

        for (int i = 0; i < arcSegments; i++)
        {
            // ¬ычислите начало и конец сегмента аппроксимации и сместите их на начальный угол
            stepStart = angleStep * i + stepOffset;
            stepEnd = angleStep * (i + 1) + stepOffset;

            lineStart.x = Mathf.Cos(stepStart);
            lineStart.y = Mathf.Sin(stepStart);
            lineStart.z = 0;

            lineEnd.x = Mathf.Cos(stepEnd);
            lineEnd.y = Mathf.Sin(stepEnd);
            lineEnd.z = 0;

            lineStart *= radius;
            lineEnd *= radius;

            //–езультаты умножаютс€ на кватернион ориентации, чтобы повернуть их
            // поскольку эта операци€ не €вл€етс€ коммутативной, результат должен быть
            // переназначен, вместо использовани€ оператора присваивани€ умножени€
            lineStart = orientation * lineStart;
            lineEnd = orientation * lineEnd;

            lineStart += position;
            lineEnd += position;

            if (i == 0)
            {
                arcStart = lineStart;
            }

            if (i == arcSegments - 1)
            {
                arcEnd = lineEnd;
            }

            DrawLine(lineStart, lineEnd, color);
        }

        DrawLine(arcStart, arcOrigin, color);
        DrawLine(arcEnd, arcOrigin, color);
    }
}
