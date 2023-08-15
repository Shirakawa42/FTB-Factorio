Shader "Custom/BlockMaterialSolid"
{
    Properties
    {
        _MainTex ("Texture Array", 2DArray) = "" {}
        _North ("North Border", Range(0,1)) = 0
        _East ("East Border", Range(0,1)) = 0
        _West ("West Border", Range(0,1)) = 0
        _South ("South Border", Range(0,1)) = 0
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
                float2 uv3 : TEXCOORD2;
                float2 uv4 : TEXCOORD3;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                int textureIndex : TEXCOORD1;
                float light : TEXCOORD2;
                float north : TEXCOORD3;
                float south : TEXCOORD4;
                float east : TEXCOORD5;
                float west : TEXCOORD6;
            };

            UNITY_DECLARE_TEX2DARRAY(_MainTex);
            float _Daylight;
            int _IsUnderground;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.textureIndex = (int)v.uv2.x;
                o.light = v.uv2.y;
                o.north = v.uv3.x * 0.1;
                o.south = v.uv3.y * 0.1;
                o.east = v.uv4.x * 0.1;
                o.west = v.uv4.y * 0.1;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvRepeat = fmod(i.uv, 1.0);
                if (_IsUnderground == 1) {
                    _Daylight = 0.0;
                }
                if (i.light < 8.0) {
                    i.light = 0.0;
                }


                fixed4 ret = UNITY_SAMPLE_TEX2DARRAY(_MainTex, float3(uvRepeat, i.textureIndex)) * 0.3 * max(_Daylight, (i.light / 255.0));
                if (uvRepeat.x < i.west || uvRepeat.x > 1.0 - i.east || uvRepeat.y < i.south || uvRepeat.y > 1.0 - i.north)
                {
                    return ret * 0.3;
                }
                return ret;
            }
            ENDCG
        }
    }
}
