// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/FireShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _SomeTex ("Some Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha One
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #define iTime _Time.y
            #define iResolution _ScreenParams

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _SomeTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float4 _SomeTex_ST;

            fixed3 rgb2hsv(fixed3 c);
            fixed3 hsv2rgb(fixed3 c);
            float rand(fixed2 n);
            float noise(fixed2 n);
            float fbm(fixed2 n);
            fixed4 mainImage(fixed2 fragCoord);

            fixed3 rgb2hsv(fixed3 c)
            {
                fixed4 K = fixed4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                fixed4 p = lerp(fixed4(c.bg, K.wz), fixed4(c.gb, K.xy), step(c.b, c.g));
                fixed4 q = lerp(fixed4(p.xyw, c.r), fixed4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return fixed3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            fixed3 hsv2rgb(fixed3 c)
            {
                fixed4 K = fixed4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                fixed3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
            }

            float rand(fixed2 n) {
                return frac(sin(cos(dot(n, fixed2(12.9898,12.1414)))) * 83758.5453);
            }

            float noise(fixed2 n) {
                const fixed2 d = fixed2(0.0, 1.0);
                fixed2 b = floor(n);
                fixed2 f = smoothstep(fixed2(0.0, 0.0), fixed2(1.0, 1.0), frac(n));
                return lerp(lerp(rand(b), rand(b + d.yx), f.x), lerp(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
            }

            float fbm(fixed2 n) {
                float total = 0.0, amplitude = 1.0;
                for (int i = 0; i <5; i++) {
                    total += noise(n) * amplitude;
                    n += n*1.7;
                    amplitude *= 0.47;
                }
                return total;
            }

            fixed4 mainImage(fixed2 fragCoord) {

                const fixed3 c1 = fixed3(0.5, 0.0, 0.1);
                const fixed3 c2 = fixed3(0.9, 0.1, 0.0);
                const fixed3 c3 = fixed3(0.2, 0.1, 0.7);
                const fixed3 c4 = fixed3(1.0, 0.9, 0.1);
                const fixed3 c5 = fixed3(0.1, 0.1, 0.1);
                const fixed3 c6 = fixed3(0.9, 0.9, 0.9);

                fixed2 speed = fixed2(1.2, 0.1);
                float shift = 1.327+sin(iTime*2.0)/2.4;
                float alpha = 2.1;
                
                //change the constant term for all kinds of cool distance versions,
                //make plus/minus to switch between 
                //ground fire and fire rain!
                float dist = 3.5-sin(iTime*0.4)/1.89;
                
                fixed2 p = fragCoord.xy * dist / iResolution.xx;
                p.x -= iTime/1.1;
                float q = fbm(p - iTime * 0.01+1.0*sin(iTime)/10.0);
                float qb = fbm(p - iTime * 0.002+0.1*cos(iTime)/5.0);
                float q2 = fbm(p - iTime * 0.44 - 5.0*cos(iTime)/7.0) - 6.0;
                float q3 = fbm(p - iTime * 0.9 - 10.0*cos(iTime)/30.0)-4.0;
                float q4 = fbm(p - iTime * 2.0 - 20.0*sin(iTime)/20.0)+2.0;
                q = (q + qb - .4 * q2 -2.0*q3  + .6*q4)/3.8;
                
                fixed2 r = fixed2(fbm(p + q /2.0 + iTime * speed.x - p.x - p.y), fbm(p + q - iTime * speed.y));
                fixed3 c = lerp(c1, c2, fbm(p + r)) + lerp(c3, c4, r.x) - lerp(c5, c6, r.y);
                fixed3 color = fixed3(c * cos(shift * fragCoord.y / iResolution.y));
                color += .05;
                color.r *= .8;
                fixed3 hsv = rgb2hsv(color);
                hsv.y *= hsv.z  * 1.1;
                hsv.z *= hsv.y * 1.13;
                hsv.y = (2.2-hsv.z*.9)*1.20;
                color = hsv2rgb(hsv);
                //fixed4 camColor = tex2D(_SomeTex, fragCoord.xy / iResolution.xy);
                return fixed4(color.x, color.y, color.z, alpha) * 0.2;
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _SomeTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_SomeTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                fixed2 c = IN.texcoord.xy * iResolution.xy;
                c.y *= -1;
                color = mainImage(c);
                color.xyz *= 2.5;
                //color.a *= sqrt(IN.texcoord.y);
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}