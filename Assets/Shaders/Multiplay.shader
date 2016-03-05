Shader "Maciex/TexVertexColor" { 
   Properties {
   	_Color ("MainColor", Color) = (1,1,1,1) 
   	_MainTex ( "Main Texture", 2D ) = "white" {}
   }
   SubShader { 
   Tags { "Queue" = "Transparent" }
             ZWrite OFF
             Blend SrcAlpha OneMinusSrcAlpha 
             
      Pass { 
      
      	Cull off
         CGPROGRAM

         #pragma vertex vert 
         
         #pragma fragment frag
   		 uniform float4 _Color; 
   		 uniform sampler2D _MainTex;
     	 uniform float4 _MainTex_ST; 
     	  
				
		 struct VertOut
         {
             float4 position : POSITION;
             float4 color : COLOR;
             half2 uv : TEXCOORD0;
            
         };
         struct VertIn
         {
             float4 vertex : POSITION;
             float4 color : COLOR;
             float4 texcoord : TEXCOORD0;
         };		
	
         VertOut vert(VertIn vertexIn) 
         {
        	VertOut output;
        	output.position = mul(UNITY_MATRIX_MVP, vertexIn.vertex);
        	output.color = vertexIn.color;
        	output.uv =  vertexIn.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
            return output;
         }

         float4 frag(VertOut input) : COLOR // fragment shader
         {
            return tex2D( _MainTex, input.uv )*_Color*input.color;        
         }

         ENDCG 
      }
   }
}
