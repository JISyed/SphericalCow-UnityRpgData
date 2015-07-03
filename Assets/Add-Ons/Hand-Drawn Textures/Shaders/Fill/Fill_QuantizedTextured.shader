// Quantized Textured Fill Shader
// (c) 2013 Alastair Aitchison
//
// - Unlit shader - not affected by scene lighting
// - Object is rendered using RGB of base texture, after quantization and contrast boost
// - Opacity of output is determined by brightness of _FillTex, in screenspace coordinates

// Example use : CRAYON material

Shader "Hand-Drawn/Fill/QuantizedTextured" {

    // Shaderlab properties are exposed in Unity's material inspector
	Properties {
	
		// The diffuse material texture - used to colour the model
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		// Additional fill texture - used to texture the model
		_FillTex ("Fill Texture (RGB)", 2D) = "white" {}
		
		// Degree of colour quantization. Lower values = smaller output palette
		_QuantizationLevels ("Quantization Levels", float) = 3.0
		
		// Modifier to boost contrast
		_ContrastBoost ("Contrast Boost", float) = 2.0
	}

	SubShader {
	
    	// Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
			"Queue" = "Geometry+100" // Render this object after the geometry queue, but before transparent objects are drawn
        	"RenderType" = "Opaque" // The material is opaque
        	"IgnoreProjector" = "True" // Don't let projectors affect this object
        }
	
		Pass {
			// This pass renders the interior of the model
			Name "INTERIOR"
			// Use only front faces
			Cull Back
			// Not affected by scene lighting
            Lighting Off
			// Blend the output onto the background using alpha blending
			Blend SrcAlpha OneMinusSrcAlpha

			// Begin the CG code block to define the custom shader
			CGPROGRAM
    		// The vertex shader will be a function called "vert"
    		#pragma vertex vert
   
    		// The fragment shader will be a function called "frag"
    		#pragma fragment frag
			
			// GLSL precision hint to maximise performance (at possible expense of precision)
			// See http://www.opengl.org/registry/specs/NV/fragment_program4.txt
			#pragma fragmentoption ARB_precision_hint_fastest
			
			// UnityCG.inc includes various common macros, matrices etc.
			#include "UnityCG.cginc"

   			// Define simple structure to pass data from Unity to vertex shader
    		struct a2v {
      			float4 vertex : POSITION;
				float4 texcoord : TEXCOORD;
			};

   			// Access Shaderlab properties
			uniform sampler2D _MainTex;
			uniform sampler2D _FillTex;
			uniform half _QuantizationLevels;
			uniform half _ContrastBoost;
			
			// _MainTex_ST and _FillTex_ST are populated by Unity automatically with texture scale (x,y) and offset (z,w)
			// values of _MainTex and _FillTex set in the material inspector, and are required for TRANSFORM_TEX macro to work
			float4 _MainTex_ST;
			float4 _FillTex_ST;

			// Define Struct to hold data passed from the vertex shader to the fragment shader
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 pos2 : TEXCOORD1;
			};
			
            // VERTEX SHADER
			v2f vert(a2v v)
			{
				// Declare the output structure
               	v2f o;
               	
                // Transform from model coordinates to clip coordinates by applying Model-View-Projection matrix 
               	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
               	
               	// Use TRANSFORM_TEX macro from UnityCG.inc to apply texture scale & offset
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				
				// Pass position coordinates as seperate member to apply texture in screen-space rather than model-space
				o.pos2 = o.pos;
				
               	// Pass the output to the fragment shader
               	return o;
			}

			// FRAGMENT SHADER
			fixed4 frag (v2f IN) : COLOR
			{
				// Get the base RGB value from the diffuse texture
				half3 base = tex2D(_MainTex, IN.uv).rgb;

				// Compute screenspace texture coordinates
			    float2 screenPos = IN.pos2.xy / IN.pos2.w;   // screenPos ranges from -1 to 1
    			screenPos.xy = (screenPos.xy + 1) * 0.5;   // Adjust range to be from 0 to 1

				// Scale coordinates to account for Tiling parameter chosen in the material inspector		
				screenPos.xy *= _FillTex_ST.xy;

				// Get the base RGB value from the diffuse texture
				half3 backgroundtex = tex2D(_FillTex, screenPos).rgb;
	
				// Quantise the RGB value based on the specified quantisation level parameter 
				base = round(base * _QuantizationLevels) / _QuantizationLevels;

				// Boost contrast
				base = (base - 0.5) * _ContrastBoost + 0.5;

				// Return the result, with opacity set by the R channel of background texture (inverted, as assumed to be black on white)
				return fixed4(base, 1-backgroundtex.r );
			}

			ENDCG
		} // End Pass
	} // End SubShader
} // End Shader