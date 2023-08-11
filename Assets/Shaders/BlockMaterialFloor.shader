Shader "Custom/BlockMaterialFloor"
{
    Properties
    {
        _MainTex ("Texture Array", 2DArray) = "" {}
    }
    SubShader
    {
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
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                int textureIndex : TEXCOORD1;
                float light : TEXCOORD2;
            };

            UNITY_DECLARE_TEX2DARRAY(_MainTex);
            float _Daylight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.textureIndex = (int)v.uv2.x;
                o.light = v.uv2.y;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvRepeat = fmod(i.uv, 1.0);
                return UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(uvRepeat, i.textureIndex)) * max(_Daylight, (i.light / 255.0));
            }
            ENDCG
        }
    }
}
