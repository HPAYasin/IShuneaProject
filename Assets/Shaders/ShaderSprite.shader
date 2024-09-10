Shader "Custom/AdvancedSpriteShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}

        // �������� ���������
        _EnableColorAdjust ("Enable Color Adjustment", Float) = 0
        _Gamma ("Gamma", Range(0.1, 3)) = 1.0
        _Brightness ("Brightness", Range(0.0, 2.0)) = 1.0
        _Saturation ("Saturation", Range(0.0, 2.0)) = 1.0
        _HueShift ("Hue Shift", Range(-1.0, 1.0)) = 0.0

        // �������� ������
        _EnableInvert ("Enable Inversion", Float) = 0

        // ������� (Outline)
        _EnableOutline ("Enable Outline", Float) = 0
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.05

        // ������ �������� (Glow)
        _EnableGlow ("Enable Glow", Float) = 0
        _GlowColor ("Glow Color", Color) = (1,1,0,1)
        _GlowStrength ("Glow Strength", Range(0, 5)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
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
            float _EnableColorAdjust;
            float _Gamma;
            float _Brightness;
            float _Saturation;
            float _HueShift;
            float _EnableInvert;

            float _EnableOutline;
            float4 _OutlineColor;
            float _OutlineThickness;

            float _EnableGlow;
            float4 _GlowColor;
            float _GlowStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // ������� ��� �������� ���������
            fixed4 ApplyColorAdjustment(fixed4 color)
            {
                if (_EnableColorAdjust > 0.5)
                {
                    // �����-���������
                    color.rgb = pow(color.rgb, 1.0 / _Gamma);

                    // ���������� �������
                    color.rgb *= _Brightness;

                    // ���������� ������������
                    float gray = dot(color.rgb, float3(0.299, 0.587, 0.114)); // ������� ������
                    color.rgb = lerp(float3(gray, gray, gray), color.rgb, _Saturation);

                    // ���������� ������ �������
                    color.rgb = lerp(color.rgb, float3(1, 0, 0), _HueShift); // ������� ����� �������
                }
                return color;
            }

            // ������� ��� �������� ������
            fixed4 ApplyInvert(fixed4 color)
            {
                if (_EnableInvert > 0.5)
                {
                    color.rgb = 1.0 - color.rgb;
                }
                return color;
            }

            // ������� ��� ������� (outline)
            fixed4 ApplyOutline(fixed4 color, float2 uv)
            {
                if (_EnableOutline > 0.5)
                {
                    float outline = step(_OutlineThickness, uv.x) * step(_OutlineThickness, uv.y) * step(1 - _OutlineThickness, uv.x) * step(1 - _OutlineThickness, uv.y);
                    color = lerp(_OutlineColor, color, outline);
                }
                return color;
            }

            // ������� ��� �������� (glow)
            fixed4 ApplyGlow(fixed4 color)
            {
                if (_EnableGlow > 0.5)
                {
                    float glowAmount = _GlowStrength * (1.0 - dot(color.rgb, float3(0.299, 0.587, 0.114)));
                    color.rgb += _GlowColor.rgb * glowAmount;
                }
                return color;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);

                // ���������� ��������
                color = ApplyInvert(color);

                // ���������� �������� ���������
                color = ApplyColorAdjustment(color);

                // ���������� �������
                color = ApplyOutline(color, i.uv);

                // ���������� ��������
                color = ApplyGlow(color);

                return color;
            }
            ENDCG
        }
    }

    FallBack "Sprites/Default"
}
