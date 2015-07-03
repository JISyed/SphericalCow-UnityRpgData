// Animated Textured Outline Shader
// (c) 2013 Alastair Aitchison
//
// - Unlit shader - not affected by scene lighting
// - Outline is rendered using RGB of _OutlineColour
// - Opacity of output is determined by brightness of _OutlineTex, a texture applied in screenspace coordinates
// - Outline can be animated by applying random offset to backfacing vertices

Shader "Hand-Drawn/Outline/Animated+Textured" {

    // Shaderlab properties are exposed in Unity's material inspector
    Properties {
   
        // Base outline colour
        _OutlineColour ("Outline Colour", Color) = (0.95,0.5,0.5,0.5)

        // Base outline width
        _OutlineWidth ("Outline Width", Float) = 0.01

        // Outline texture to be combined with colour
        _OutlineTex ("Outline Texture", 2D) = "white" {}
       
        // The maximum amount by which vertices are randomly moved
        _Scribbliness ("Scribbliness", Float) = 0.01
       
        // The rate at which vertices are moved (per second)
        // 0.0 = not animated
        _RedrawRate ("Redraw Rate", Float) = 6.0
                                                                
    }
   
    // CGINCLUDE block contains code that is shared between all passes
    CGINCLUDE
   
    // The vertex shader will be a function called "vert"
    #pragma vertex vert
   
    // The fragment shader will be a function called "frag"
    #pragma fragment frag
			
	// GLSL precision hint to maximise performance (at possible expense of precision)
	// See http://www.opengl.org/registry/specs/NV/fragment_program4.txt
	#pragma fragmentoption ARB_precision_hint_fastest
			
    // Returns pseudorandom number in range 0.0 - 1.0
    float hash(float2 seed){
        return frac(sin(dot(seed.xy ,float2(12.9898,78.233))) * 43758.5453);
    }
   
    // Define simple structure to pass data from Unity to vertex shader
    struct a2v {
       float4 vertex : POSITION;
       float3 normal : NORMAL;
       float4 texcoord : TEXCOORD;
    };

	// Define Struct to hold data passed from the vertex shader to the fragment shader
    struct v2f {
        float4 pos : SV_POSITION;
        float4 pos2 : TEXCOORD0;
    };
   
    // Access Shaderlab properties
    uniform fixed4 _OutlineColour;
    uniform sampler2D _OutlineTex;
    uniform half _OutlineWidth;
    uniform half _RedrawRate;
    uniform half _Scribbliness;

    
    // __OutlineTex_ST is populated by Unity automatically with texture scale (x,y) and offset (z,w)
	// values of _OutlineTex set in the material inspector
	float4 _OutlineTex_ST;
            
    ENDCG

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
			// VERTEX SHADER
            v2f vert (a2v v)
            {
           		// Declare the output structure
                v2f o;
                
                // Calculate a random offset for each vertex along their normal based on a hash of their texcoord
                // that lies somewhere in the range 0 - _Scribbliness, recalculated every _RedrawRate seconds
                float4 vOffset = float4(v.normal,0) * _Scribbliness * hash(v.texcoord.xx + floor(_Time.y * _RedrawRate));

      			// Subtract the offset, and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix     
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex - vOffset);
                
                // Pass additional set of position coordinates (will be used for screenspace coordinate calculation in fragment shader)
                o.pos2 = o.pos;
                
                // Pass the output to the fragment shader
                return o;
            }
            // FRAGMENT SHADER
            fixed4 frag (v2f IN) : COLOR
			{
				// Return any colour - the Blend mode Zero One means it won't be seen anyway 
    			return fixed4(0,0,0,0);
    		}
            
            ENDCG
        }

        Pass {
        	// This pass renders the outline
            Name "OUTLINE"
            // Only use backfaces
            Cull Front
            // Not affected by lighting
            Lighting Off
            // Blend the outline colour onto the background using alpha blending
            Blend SrcAlpha OneMinusSrcAlpha
 
            // Begin the CG code block to define the custom outline shader
            CGPROGRAM
            // UnityCG.inc includes various common macros, matrices etc.
            #include "UnityCG.cginc"
            // VERTEX SHADER
            v2f vert (a2v v)
            {
                // Declare the output structure
                v2f o;

				// Calculate a random offset for each vertex along their normal based on a hash of their texcoord
                // that lies somewhere in the range 0 - _Scribbliness, recalculated every _RedrawRate seconds
                float4 vOffset = float4(v.normal,0) * (_OutlineWidth + _Scribbliness * (hash(v.texcoord.xy + floor(_Time.y * _RedrawRate))-0.5)    );

				// Add the offset, and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix     
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex + vOffset);

                // Pass additional set of position coordinates (will be used for screenspace coordinate calculation in fragment shader)
     			o.pos2 = o.pos;

				// Pass result to fragment shader
                return o;
            }
            
            // FRAGMENT SHADER
            fixed4 frag (v2f IN) : COLOR
    		{
			    // Compute screenspace texture coordinates
			    float2 screenPos = IN.pos2.xy / IN.pos2.w;   // screenPos ranges from -1 to 1
    			screenPos.xy = (screenPos.xy + 1) * 0.5;   // Adjust range to be from 0 to 1
		
				// Scale coordinates to account for Tiling parameter chosen in the material inspector		
				screenPos.xy *= _OutlineTex_ST.xy;
		
				// Get the base RGB value from the diffuse texture
				half3 outlineTex = tex2D(_OutlineTex, screenPos).rgb;
	
				// Adjust opacity based on the brightness of the outline texture
				_OutlineColour.a -= outlineTex.rgb;
				
				// Return the output
     			return _OutlineColour;
    		}
            ENDCG
        }       
    }
}
