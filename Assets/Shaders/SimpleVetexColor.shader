Shader "Maciex/SimpleVertexColor" { 
   Properties {
   	_Color ("MainColor", Color) = (1,1,1,1) 
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
				
		 struct VertOut
         {
             float4 position : POSITION;
             float4 color : COLOR;
         };
         struct VertIn
         {
             float4 vertex : POSITION;
             float4 color : COLOR;
         };		
	
         VertOut vert(VertIn vertexIn) 
         {
        	VertOut output;
        	output.position = mul(UNITY_MATRIX_MVP, vertexIn.vertex);
        	output.color = vertexIn.color;
            return output;
         }

         float4 frag(VertOut input) : COLOR // fragment shader
         {
            return _Color*input.color;        
         }

         ENDCG 
      }
   }
}
