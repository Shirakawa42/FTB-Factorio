Shader "Custom/TextureArrayShader"
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            UNITY_DECLARE_TEX2DARRAY(_MainTex);
            int _TextureIndex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvRepeat = fmod(i.uv, 1.0); // Repeat the texture
                return UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(uvRepeat, _TextureIndex));
            }
            ENDCG
        }
    }
}
