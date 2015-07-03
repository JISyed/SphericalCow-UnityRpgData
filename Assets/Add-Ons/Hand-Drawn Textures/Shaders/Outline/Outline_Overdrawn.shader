// Overdrawn Outline Shader
// (c) 2013 Alastair Aitchison
//
// - Unlit shader - not affected by scene lighting
// - Outline is rendered four times with varying random offset based on vertex UV coordinates
// - Offsets recalculated every _RedrawRate seconds
// - When outline colour is semi-opaque (i.e. _OutlineColour alpha is < 1.0), this generates effect
//   of untidy repeated tracing of the outline

Shader "Hand-Drawn/Outline/Overdrawn" {

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
    	// Vertex coordinates transformed into clip space
        float4 pos : SV_POSITION;
    };
   
    // Access the shaderlab properties set in the material inspector
    uniform fixed4 _OutlineColour;
    uniform half _OutlineWidth;
    uniform half _RedrawRate;
    uniform half _Scribbliness;
   
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

		// We want to draw an *unfilled* exterior edge around the model. To prevent the interior of
		// the outline being filled, we'll use a Pass to write values to the z buffer to create a mask.
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
            	// Calculate offset for each vertex based on its normal direction, _Scribbliness variable, and redraw rate
               	float4 vOffset = float4(v.normal,0) * (_Scribbliness * hash(v.texcoord.xx + floor(_Time.y * _RedrawRate)) );
                // Apply offset and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix 
               	o.pos = mul(UNITY_MATRIX_MVP, v.vertex - vOffset);
               	// Pass the output to the fragment shader
               	return o;
            }
            ENDCG
        }

        Pass {
        	// The first pass outline
            Name "OUTLINE1"
            // Only use backfaces
            Cull Front
            // Not affected by lighting
            Lighting Off
            // Blend the outline colour onto the background using alpha blending
            Blend SrcAlpha OneMinusSrcAlpha
 			// Begin the CG code block to define the custom outline shader
            CGPROGRAM
            v2f vert (a2v v)
            {
	         	// Declare the output structure
                v2f o;
            	// Calculate offset for each vertex based on its normal direction, _Scribbliness variable, and redraw rate
                float4 vOffset = float4(v.normal,0) * (_OutlineWidth + _Scribbliness * (hash(v.texcoord.xy + floor(_Time.y * _RedrawRate))-0.5)    );
                // Apply offset and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex + vOffset);
               	// Pass the output to the fragment shader
                return o;
            }
            ENDCG
        }

        Pass {
        	// An additional overdrawn pass outline
            Name "OUTLINE2"
            // Only use backfaces
            Cull Front
            // Not affected by lighting
            Lighting Off
            // Blend the outline colour onto the background using alpha blending
            Blend SrcAlpha OneMinusSrcAlpha
 			// Begin the CG code block to define the custom outline shader
            CGPROGRAM
            v2f vert (a2v v)
            {
	         	// Declare the output structure
                v2f o;
            	// Calculate offset for each vertex based on its normal direction, _Scribbliness variable, and redraw rate
                float4 vOffset = float4(v.normal,0) * (_OutlineWidth + _Scribbliness * (hash(v.texcoord.xz + floor(_Time.y * _RedrawRate))-0.5)    );
                // Apply offset and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex + vOffset);
               	// Pass the output to the fragment shader
                return o;
            }
            ENDCG
        }

        Pass {
        	// An additional overdrawn pass outline
            Name "OUTLINE3"
            // Only use backfaces
            Cull Front
            // Not affected by lighting
            Lighting Off
            // Blend the outline colour onto the background using alpha blending
            Blend SrcAlpha OneMinusSrcAlpha
 			// Begin the CG code block to define the custom outline shader
            CGPROGRAM
            v2f vert (a2v v)
            {
	         	// Declare the output structure
                v2f o;
            	// Calculate offset for each vertex based on its normal direction, _Scribbliness variable, and redraw rate
                float4 vOffset = float4(v.normal,0) * (_OutlineWidth + _Scribbliness * (hash(v.texcoord.yy + floor(_Time.y * _RedrawRate))-0.5)    );
                // Apply offset and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex + vOffset);
               	// Pass the output to the fragment shader
                return o;
            }
            ENDCG
        }

        Pass {
        	// An additional overdrawn pass outline
            Name "OUTLINE4"
            // Only use backfaces
            Cull Front
            // Not affected by lighting
            Lighting Off
            // Blend the outline colour onto the background using alpha blending
            Blend SrcAlpha OneMinusSrcAlpha
 			// Begin the CG code block to define the custom outline shader
            CGPROGRAM
            v2f vert (a2v v)
            {
	         	// Declare the output structure
                v2f o;
            	// Calculate offset for each vertex based on its normal direction, _Scribbliness variable, and redraw rate
                float4 vOffset = float4(v.normal,0) * (_OutlineWidth + _Scribbliness * (hash(v.texcoord.yz + floor(_Time.y * _RedrawRate))-0.5)    );
                // Apply offset and transform from model coordinates to clip coordinates by applying Model-View-Projection matrix 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex + vOffset);
               	// Pass the output to the fragment shader
                return o;
            }
            ENDCG
        }
    }
}
