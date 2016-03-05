//Flat color texture, does not support alpha channels
     Shader "Maciex/SimpleTexture"
     {
         Properties
         {
             _MainTex ( "Main Texture", 2D ) = "white" {}
             _Color ("MainColor", Color) = (1,1,1,1) 
         }
         
         SubShader
         {
             Tags { "Queue" = "Transparent" }
             ZWrite OFF
             Blend SrcAlpha OneMinusSrcAlpha
             
             Pass
             {
     CGPROGRAM
     #pragma vertex vert
     #pragma fragment frag
     
     #include "UnityCG.cginc"
     
     
     // uniforms
     uniform sampler2D _MainTex;
     uniform float4 _MainTex_ST; 
     uniform float4 _Color; 
     
     struct vertexInput
     {
         float4 vertex : POSITION; 
         float4 texcoord : TEXCOORD0;
     };
     
     
     struct fragmentInput
     {
         float4 pos : SV_POSITION;
         half2 uv : TEXCOORD0;
     };
     
     
     fragmentInput vert( vertexInput i )
     {
         fragmentInput o;
         o.pos = mul( UNITY_MATRIX_MVP, i.vertex );
         o.uv =  i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
     
         return o;
     }
     
     
     half4 frag( fragmentInput i ) : COLOR
     {
         return tex2D( _MainTex, i.uv )*_Color;
     }
     
     ENDCG
             } 
         } 
         
         FallBack "Diffuse"
     }
 