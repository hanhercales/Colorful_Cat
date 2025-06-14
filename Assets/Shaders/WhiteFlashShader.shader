// Simple shader that adds a white color overlay to a sprite.
Shader "Unlit/WhiteFlashShader"
{
    Properties
    {
        // The main sprite texture.
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        // The normal color tint.
        _Color ("Tint", Color) = (1,1,1,1)
        // How much white to add (0 = normal, 1 = fully white).
        _FlashAmount ("Flash Amount", Range(0.0, 1.0)) = 0.0
    }
    SubShader
    {
        // Standard setup for a transparent sprite.
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _FlashAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                
                // Lerp (linear interpolate) between the original color and full white,
                // based on the _FlashAmount.
                fixed4 flashColor = lerp(col, fixed4(1,1,1,col.a), _FlashAmount);
                
                // Ensure the final alpha is the original sprite's alpha.
                flashColor.a = col.a;

                return flashColor;
            }
            ENDCG
        }
    }
}