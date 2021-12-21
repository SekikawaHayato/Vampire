Shader "Custom/Mask"
{
    Properties
    {
        // テクスチャー
        [NoScaleOffset] _MainTex ("Texture",2D)="white"{}
        // Mask
        [NoScaleOffset] _MaskTex("Mask Texture(RGB)",2D)="white"{}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // 頂点シェーダーに渡ってくる頂点データ
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv1 : TEXCOORD0;  
            };

            // フラグメントシェーダーに渡すデータ
            struct v2f
            {
                float2 uv1 : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;

            // 頂点シェーダー
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1 = v.uv1;
                return o;
            }

            // フラグメントシェーダ
            fixed4 frag(v2f i):SV_Target
            {
                fixed4 mask = tex2D(_MaskTex,i.uv1);
                clip(mask.a-0.5);
                fixed4 col = tex2D(_MainTex,i.uv1);
                return col*mask;
            }
            ENDCG
        }
    }
}
