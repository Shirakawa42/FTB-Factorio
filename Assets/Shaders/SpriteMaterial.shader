Shader "Custom/Sprites"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Daylight ("Daylight Intensity", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha // Add blending for transparency
        ZWrite Off // Turn off depth writing

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
                float light : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float light : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Daylight;
            int _IsUnderground;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.light = v.light;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv) * _Color;
                if (_IsUnderground == 1)
                    _Daylight = 0.0;
                col.rgb *= max(_Daylight, (1.0 / 255.0));
                return col;
            }
            ENDCG
        }
    }
}
