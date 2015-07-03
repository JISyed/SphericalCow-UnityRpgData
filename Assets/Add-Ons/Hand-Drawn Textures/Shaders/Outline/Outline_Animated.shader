// Animated Outline Shader
// (c) 2013 Alastair Aitchison
//
// - Unlit shader - not affected by scene lighting
// - Outline of object is rendered after applying random offsets to back-facing vertices
// - Offsets recalculated every _RedrawRate seconds, based on simple hash of vertex UV coordinates

Shader "Hand-Drawn/Outline/Animated" {

    // Shaderlab properties are exposed in Unity's material inspector
    Properties {
   
        // Base outline colour
        _OutlineColour ("Outline Colour", Color) = (0.95,0.5,0.5,0.5)

        // Base outline width
        _OutlineWidth ("Outline Width", Float) = 0.01
       
        // The maximum amount by which vertices are randomly moved
        _Scribbliness ("Scribbliness", Float) = 0.01
       
        // The rate at which vertices are moved (per second)
        // 0.0 = not animated
        _RedrawRate ("Redraw Rate", Float) = 6.0
                                                                
    }
   
    // CGINCLUDE block contains code that is shared between all passes
    CGINCLUDE

    // If set, outline will remain constant thickness however far object is from camera (i.e. constant outline thickness in world space)
    // Comment this line out to set thickness based on model space instead
    //#define CONSTANT_THICKNESS

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
    };
   
    // Access Shaderlab properties
    fixed4 _OutlineColour;
    uniform half _OutlineWidth;
    uniform half _RedrawRate;
    uniform half _Scribbliness;
   
    // FRAGMENT SHADER (same across all passes)
    fixed4 frag (v2f IN) : COLOR
    {
        return _OutlineColour;
    }
   
    ENDCG

    SubShader {
        // Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
        	"Queue" = "Geometry+100" // Draw after geometry but before transparent objects
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
            // UnityCG.inc includes various common macros, matrices etc.
            #include "UnityCG.cginc"
            // VERTEX SHADER
            v2f vert (a2v v)
            {
            	// Declare the output structure
                v2f o;
                
                // If CONSTANT_THICKNESS is defined, outline width is set in clip space coordinates
                #ifdef CONSTANT_THICKNESS
                
                	// Transform vertex coordinates into clip space
                	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                	
                	// Transform normals into eye space
					float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
					
					// Calculate offset in clip space based on normal direction, Scribbliness, and Redraw rate
					float2 vOffset = TransformViewToProjection(norm.xy) * o.pos.z * _Scribbliness * hash(v.texcoord.xx + floor(_Time.y * _RedrawRate));
					
					// Apply the offset 
					// As this is the interior mask, we *subtract* the offset from the vertex coordinates
					o.pos.xy -= vOffset;
					
               	// If CONSTANT_THICKNESS is not defined, outline width is set in model space coordinates
                #else
                
                	// Calculate offset for each vertex based on its normal direction, Scribbliness, and Redraw rate
                	float4 vOffset = float4(v.normal,0) * _Scribbliness * hash(v.texcoord.xx + floor(_Time.y * _RedrawRate));
                	
                	// Apply offset and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix
                	// Note that offset is *subtracted* from interior mask 
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex - vOffset);
					
				#endif
                
                // Pass the output to the fragment shader
                return o;
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
 
 			// Begin the CG code block to define the custom shader
            CGPROGRAM
            // VERTEX SHADER
            v2f vert (a2v v)
            {
            	// Declare the output structure
                v2f o;
                
                // If CONSTANT_THICKNESS is defined, outline width is set in clip space coordinates
                #ifdef CONSTANT_THICKNESS
                
                	// Transform vertex coordinates into clip space
                	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                
                	// Transform normals into eye space
					float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				
					// Transform eye space normals into clip space
					float2 vOffset = TransformViewToProjection(norm.xy);
				
					// Apply the offset in clip space
					// As this is the outline, we *add* the offset to the vertex coordinates
					o.pos.xy += vOffset * o.pos.z * (_OutlineWidth + _Scribbliness * (hash(v.texcoord.xy + floor(_Time.y * _RedrawRate))-0.5)    );

				// If CONSTANT_THICKNESS is not defined, outline width is set in model space coordinates
                #else
                
                	// Calculate offset for each vertex based on its normal direction, _Scribbliness, and Redraw rate
                	float4 vOffset = float4(v.normal,0) * (_OutlineWidth + _Scribbliness * (hash(v.texcoord.xy + floor(_Time.y * _RedrawRate))-0.5)    );

             		// Apply offset and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex + vOffset);

				#endif
				
				// Return output
                return o;
            }
            ENDCG
        }       
    }
}
