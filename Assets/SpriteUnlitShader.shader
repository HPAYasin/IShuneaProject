Shader "Custom/SpriteAdjustShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Gamma ("Gamma", Range(0.1, 3)) = 1.0
        _Contrast ("Contrast", Range(0.0, 2.0)) = 1.0
        _Brightness ("Brightness", Range(0.0, 2.0)) = 1.0 // Яркость
        _Saturation ("Saturation", Range(0.0, 2.0)) = 1.0 // Насыщенность
        _HueShift ("Hue Shift", Range(-1.0, 1.0)) = 0.0 // Оттенок
        _InvertColors ("Invert Colors", Range(0.0, 1.0)) = 0.0 // Инверсия цветов
        _Opacity ("Opacity", Range(0.0, 1.0)) = 1.0 // Прозрачность
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _Gamma;
            float _Contrast;
            float _Brightness;
            float _Saturation;
            float _HueShift;
            float _InvertColors;
            float _Opacity;

            // Функция для смещения оттенка
            fixed3 HueShift(fixed3 color, float shift)
            {
                float angle = shift * 3.14159265 * 2.0; // Конвертируем сдвиг в радианы
                float cosA = cos(angle);
                float sinA = sin(angle);
                float3x3 hueRotationMatrix = float3x3(
                    cosA + (1.0 - cosA) / 3.0, 1.0 / 3.0 * (1.0 - cosA) - sqrt(1.0 / 3.0) * sinA, 1.0 / 3.0 * (1.0 - cosA) + sqrt(1.0 / 3.0) * sinA,
                    1.0 / 3.0 * (1.0 - cosA) + sqrt(1.0 / 3.0) * sinA, cosA + (1.0 - cosA) / 3.0, 1.0 / 3.0 * (1.0 - cosA) - sqrt(1.0 / 3.0) * sinA,
                    1.0 / 3.0 * (1.0 - cosA) - sqrt(1.0 / 3.0) * sinA, 1.0 / 3.0 * (1.0 - cosA) + sqrt(1.0 / 3.0) * sinA, cosA + (1.0 - cosA) / 3.0
                );

                return mul(hueRotationMatrix, color);
            }

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Применение инверсии цветов
                if (_InvertColors > 0.5)
                {
                    col.rgb = 1.0 - col.rgb;
                }

                // Гамма-коррекция
                col.rgb = pow(col.rgb, 1.0 / _Gamma);

                // Применение контраста
                col.rgb = (col.rgb - 0.5) * _Contrast + 0.5;

                // Применение яркости
                col.rgb *= _Brightness;

                // Применение насыщенности (линейная интерполяция между оттенками серого и оригинальными цветами)
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114)); // Перевод в оттенки серого
                col.rgb = lerp(float3(gray, gray, gray), col.rgb, _Saturation);

                // Применение смещения оттенка
                col.rgb = HueShift(col.rgb, _HueShift);

                // Применение прозрачности
                col.a *= _Opacity;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}
