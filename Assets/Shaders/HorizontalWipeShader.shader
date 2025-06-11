// Shader for a 2D sprite that reveals from a center vertical line outwards.
Shader "Unlit/HorizontalWipeShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        // Controls the wipe amount (0=invisible, 1=fully visible).
        _WipeAmount ("Wipe Amount", Range(0.0, 1.0)) = 0.0
    }
    SubShader
    {
        // Standard setup for transparent 2D sprites.
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _WipeAmount;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the sprite's color from the texture.
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // --- Curtain Wipe Logic ---
                // 1. Calculate the pixel's horizontal distance from the center (0.5).
                // The result ranges from 0.0 (at the center) to 0.5 (at the edges).
                float dist_from_center = abs(i.uv.x - 0.5);

                // 2. We want our _WipeAmount (0 to 1) to cover this 0 to 0.5 range.
                // So, we multiply our control value by 0.5.
                float wipe_threshold = _WipeAmount * 0.5;

                // 3. If the pixel's distance is less than the threshold, it's visible.
                // step() returns 1 if threshold >= dist_from_center, and 0 otherwise.
                col.a *= step(dist_from_center, wipe_threshold);

                return col;
            }
            ENDCG
        }
    }
}