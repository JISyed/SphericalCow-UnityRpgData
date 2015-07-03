// Smoothed Greyscale Fill Shader
// (c) 2013 Alastair Aitchison
//
// - Unlit shader - not affected by scene lighting
// - Object is rendered using RGB of base texture, after quantization, contrast boost, and conversion to greyscale.
// - Opacity of output is determined by brightness of _MainTex

// Example use : INK material

Shader "Hand-Drawn/Fill/SmoothedGreyscale" {

	Properties {
	
		// The base material texture
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		// The amount by which final colour output will be quantised.
		// Lower values = less discrete colours used in ouput palette
		_QuantizationLevels ("Quantization Levels", float) = 3.0
		
		// Contrast Modifier
		_ContrastBoost ("Contrast Boost", float) = 2.0
		
	}

	SubShader {

    	// Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
        	"Queue" = "Geometry+100" // Render this object after other geometry objects, but before transparent objects
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
			
			// If enabled, this smooths texture by averaging neighbouring texels
			#define SMOOTHED_TEXTURE
			
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
      			float4 vertex : POSITION; // Vertex coordinates
				float4 texcoord : TEXCOORD; // Texture coordinates
			};

			// Access Shaderlab properties
			uniform sampler2D _MainTex;
			uniform half _ContrastBoost;
			uniform half _QuantizationLevels;
			
			// Following required for TRANSFORM_TEX macro to work
			float4 _MainTex_ST;
			
			// __MainTex texel size. i.e. If _MainTex is 1k x 1k, both x and y will be 1.0/1024.0 
			uniform float4 _MainTex_TexelSize;
			
			// Define Struct to hold data passed from the vertex shader to the fragment shader
			struct v2f {
				float4 pos : POSITION; // Vertex coordinates
				float2 uv : TEXCOORD0; // Texture coordinates for this texel
				#ifdef SMOOTHED_TEXTURE
					float2 neighbour_uv[6] : TEXCOORD1; // Texture coordinates for neighbouring texels
				#endif
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
				
				// If texture smoothing is enabled, supply additional UV coordinates of neighbouring
				// texels to the fragment shader
				#ifdef SMOOTHED_TEXTURE
					o.neighbour_uv[0] = o.uv + float2(-_MainTex_TexelSize.x*4, -_MainTex_TexelSize.y*4);
					o.neighbour_uv[1] = o.uv + float2(-_MainTex_TexelSize.x*2, _MainTex_TexelSize.y*2);
					o.neighbour_uv[2] = o.uv + float2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y);
					o.neighbour_uv[3] = o.uv + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
					o.neighbour_uv[4] = o.uv + float2(_MainTex_TexelSize.x*2, -_MainTex_TexelSize.y*2);
					o.neighbour_uv[5] = o.uv + float2(_MainTex_TexelSize.x*4, _MainTex_TexelSize.y*4);
				#endif
				
               	// Pass the output to the fragment shader
               	return o;
			}

			// FRAGMENT SHADER
			fixed4 frag (v2f IN) : COLOR
			{
				// If texture smoothing is enabled, calculate weighted average texture from
				// this and surrounding texels
				#ifdef SMOOTHED_TEXTURE
					half3 output = tex2D(_MainTex, IN.uv).rgb * 0.5;
					output += tex2D(_MainTex, IN.neighbour_uv[0]).rgb * 0.05;
					output += tex2D(_MainTex, IN.neighbour_uv[1]).rgb * 0.1;
					output += tex2D(_MainTex, IN.neighbour_uv[2]).rgb * 0.2;
					output += tex2D(_MainTex, IN.neighbour_uv[3]).rgb * 0.2;
					output += tex2D(_MainTex, IN.neighbour_uv[4]).rgb * 0.1;
					output += tex2D(_MainTex, IN.neighbour_uv[5]).rgb * 0.05;
				// Otherwise lookup the simple RGB value of this texel alone 
				#else
					half3 output = tex2D(_MainTex, IN.uv).rgb;
				#endif
				
				// Quantise the RGB value based on the specified quantisation level parameter 
				output = round(output.rgb*_QuantizationLevels)/_QuantizationLevels;

				// Boost contrast
				output = (output - 0.5) * _ContrastBoost + 0.5;

				// Calculate the brightness of the calculated value
				half brightness = dot(output.rgb, half3(0.3, 0.59, 0.11));

				// Return fully opaque greyscale value based on brightness
				return fixed4(brightness.rrr, 1-brightness.r);	
			}
			ENDCG
		} // End Pass
	} // End SubShader
} // End Shader