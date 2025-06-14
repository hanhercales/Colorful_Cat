Shader "Custom/GrayscaleShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1.000000,1.000000,1.000000,1.000000)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        // === THÊM PROPERTY GRAYSCALE ===
        _GrayscaleAmount ("Grayscale Amount", Range(0,1)) = 0 // 0 = full color, 1 = grayscale
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Sprite"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

            #include "UnityCG.cginc"
            #include "UnitySprites.cginc"

            // Removed redundant appdata_t and v2f structs as UnitySprites.cginc defines them.
            // Removed redundant _MainTex, _Color, _AlphaTex, _EnableExternalAlpha declarations
            // as UnityCG.cginc/UnitySprites.cginc likely defines them for this context.

            // --- CHỈ THÊM BIẾN GRAYSCALE ---
            float _GrayscaleAmount; // This is the only new uniform variable we need to declare.


            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                // _Color is defined by UnitySprites.cginc for the sprite context.
                OUT.color = IN.color * _Color; 

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // _MainTex is defined by UnityCG.cginc/UnitySprites.cginc.
                fixed4 c = tex2D (_MainTex, IN.texcoord);

                #if ETC1_EXTERNAL_ALPHA
                // _AlphaTex and _EnableExternalAlpha are also defined by Unity's includes for this context.
                fixed4 alpha = tex2D (_AlphaTex, IN.texcoord);
                c.a = lerp (c.a, alpha.r, _EnableExternalAlpha);
                #endif

                c.rgb *= c.a;
                c *= IN.color;

                // ============== THÊM LOGIC GRAYSCALE TẠI ĐÂY ==============
                float grayscale = dot(c.rgb, float3(0.299, 0.587, 0.114));
                c.rgb = lerp(c.rgb, grayscale.xxx, _GrayscaleAmount);
                // ==========================================================

                return c;
            }
            ENDCG
        }
    }
}