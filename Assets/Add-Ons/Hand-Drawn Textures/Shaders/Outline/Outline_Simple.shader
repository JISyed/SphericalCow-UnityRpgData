// Simple Outline Fill Shader
// (c) 2013 Alastair Aitchison
//
// - Unlit shader - not affected by scene lighting
// - Outline of object is rendered based on expanded backfaces, using given colour
// - Interior is masked to create "Outline only" effect

Shader "Hand-Drawn/Outline/Simple" {
    // Shaderlab properties are exposed in Unity's material inspector
    Properties {
   
        // Base outline colour
        _OutlineColour ("Outline Colour", Color) = (0.95,0.5,0.5,0.5)

        // Base outline width
        _OutlineWidth ("Outline Width", Float) = 0.01                                                               
    }
   
    // CGINCLUDE block contains code that is shared between all passes
    CGINCLUDE

    // If set, outline will remain constant thickness however far object is from camera (i.e. constant outline thickness in world space)
    // Comment this line out to set thickness based on model space instead
    #define CONSTANT_THICKNESS

    // The vertex shader will be a function called "vert"
    #pragma vertex vert
   
    // The fragment shader will be a function called "frag"
    #pragma fragment frag

	// GLSL precision hint to maximise performance (at possible expense of precision)
	// See http://www.opengl.org/registry/specs/NV/fragment_program4.txt
	#pragma fragmentoption ARB_precision_hint_fastest
         
    // Define simple structure to pass data from Unity to vertex shader
    struct a2v {
       float4 vertex : POSITION;
       float3 normal : NORMAL;
    };
   
	// Define Struct to hold data passed from the vertex shader to the fragment shader
    struct v2f {
    	// Vertex coordinates transformed into clip space
        float4 pos : SV_POSITION;
    };
   
    // Access Shaderlab properties
    uniform fixed4 _OutlineColour;
    uniform half _OutlineWidth;
   
    // Fragment shader is same across all passes
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
            ENDCG
        }
		
        Pass {
        	// This pass renders the outline
            Name "OUTLINE"
            // Only use backfaces
            Cull Front
            // Take the depth further from camera. See http://docs.unity3d.com/Documentation/Components/SL-CullAndDepth.html
            Offset 5, 10
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

                // If CONSTANT_THICKNESS is defined, outline width is set in clip space coordinates
                #ifdef CONSTANT_THICKNESS 
                
                	// Transform vertex coordinates into clip space
                	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                	
                	// Transform normals into eye space
					float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
					
					// Transform eye space normals into clip space
					float2 vOffset = TransformViewToProjection(norm.xy);
					
					// Apply the offset in clip space
					o.pos.xy += vOffset * o.pos.z * _OutlineWidth;

               	// If CONSTANT_THICKNESS is not defined, outline width is set in model space coordinates
                #else
                
            		// Calculate offset for each vertex based on its normal direction, _Scribbliness variable, and redraw rate
                	float4 vOffset = float4(v.normal,0) * (_OutlineWidth);
                	
                	// Apply offset and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix 
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex + vOffset);
				
				#endif
				
               	// Pass the output to the fragment shader
                return o;
            }
            ENDCG
        }   
    }
}