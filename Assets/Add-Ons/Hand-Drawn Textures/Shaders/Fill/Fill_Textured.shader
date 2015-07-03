// Textured Fill Shader
// (c) 2013 Alastair Aitchison
//
// - Unlit shader - not affected by scene lighting
// - Object is rendered using a replacement texture, with opacity determined by brightness of base diffuse texture
// - Replacement texture is tinted by specified _FillColour
// - Replacement texture drawn in screen space coordinates rather than model coordinates

// Example use : SHADED PENCIL material

Shader "Hand-Drawn/Fill/Textured" {
    // Shaderlab properties are exposed in Unity's material inspector
	Properties {
	
		// The diffuse material texture
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		// Texture with which to fill the interior of the model
		_FillTex ("Fill Texture (RGB)", 2D) = "white" {}
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
			// This pass renders a mask over the interior of the model
			Name "INTERIORMASK"
			// Only use frontfaces
	    	Cull Back
	    	// We don't want the mask to be visible, so colour it in using whatever colour is already on the framebuffer
			Blend Zero One
			// Not affected by scene lighting
            Lighting Off 
            
 			// Begin the CG code block to define the custom shader
            CGPROGRAM
            // The vertex shader will be a function called "vert"
    		#pragma vertex vert
    		// The fragment shader will be a function called "frag"
    		#pragma fragment frag
			// GLSL precision hint to maximise performance (at possible expense of precision)
			// See http://www.opengl.org/registry/specs/NV/fragment_program4.txt
			#pragma fragmentoption ARB_precision_hint_fastest
            
            // Define simplest structure to pass vertex data from Unity to vertex shader
   			struct a2v {
       			float4 vertex : POSITION;
    		};
			// Define struct to hold data passed from the vertex shader to the fragment shader
    		struct v2f {
    			// Vertex coordinates transformed into clip space
        		float4 pos : SV_POSITION;
    		};
			// VERTEX SHADER
            v2f vert (a2v v)
            {
            	// Declare the output structure
               	v2f o;
                // Transform from model coordinates to clip coordinates by applying Model-View-Projection matrix 
               	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
               	// Pass the output to the fragment shader
               	return o;
            }
            // Fragment shader is same across all passes
    		fixed4 frag (v2f IN) : COLOR
    		{
    			// Doesn't matter what colour value is output, since blend mode zero one will
    			// cause it to be ignored anyway.
        		return fixed4(0.0,0.0,0.0,0.0);
    		}
            ENDCG
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
				float4 texcoord : TEXCOORD0;
			};

   			// Access Shaderlab properties
			uniform sampler2D _MainTex;
			uniform sampler2D _FillTex;
			
			// _xxx_ST variables are populated by Unity automatically with texture scale (x,y) and offset (z,w) values
			// of corresponding xxx texture, and are required for TRANSFORM_TEX macro to work
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
			half4 frag (v2f IN) : COLOR
			{
				// Get the base RGBA value from the diffuse texture
				half3 original = tex2D(_MainTex, IN.uv);

				// Determine the apparent brightness of this pixel
				// Human eyes are more sensitive to green light, then red, then blue light, in approx ratio 0.59 : 0.3 : 0.11)
				half brightness = dot(original, half3(0.3, 0.59, 0.11));
	
				// Compute the texture coordinates
			    float2 screenPos = IN.pos2.xy / IN.pos2.w;   // screenPos ranges from -1 to 1
    			screenPos = (screenPos + 1) * 0.5;   // Adjust range to be from 0 to 1

				// Scale coordinates to account for Tiling parameter chosen in the material inspector		
				screenPos *= _FillTex_ST.xy;
		
				// Get the background texture corresponding to these screen coordinates
				half3 backgroundtex = tex2D(_FillTex, screenPos).rgb;

				// Combine the background texture with the chosen fill colour
				return half4(backgroundtex, 1-brightness);
			}

			ENDCG
		} // End Pass
	} // End SubShader
} // End Shader