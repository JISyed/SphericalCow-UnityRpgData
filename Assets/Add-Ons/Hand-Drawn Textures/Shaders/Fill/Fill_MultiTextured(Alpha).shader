// Multitextured Replacement Shader with Alpha Blending
// (c) 2014 Alastair Aitchison
//
// This shader calculates the apparent brightness of each part of the game scene, and replaces
// it with a texture interpolated from a sequence of monochrome images of  progressively increasing density, which is then
// tinted with a chosen fill colour.
// The monochrome textures are encoded in the RGBA channels of a single "Replacement Texture", creating a Tonal Art Map
// see http://alastaira.wordpress.com/2013/11/01/hand-drawn-shaders-and-creating-tonal-art-maps/
// The shader is suitable for e.g. creating a hatched shading effect.

// Example use : CHALK material

// - First pass creates an interior mask to prevent distant faces of the object showing through transparent front faces.
// - Object is  rendered using forward rendering base pass (http://docs.unity3d.com/Documentation/Components/RenderTech-ForwardRendering.html)
//   which accounts for base texture lit by ambient, vertx, SH, and main directional light in scene.
// - The brightness of the resulting fragment is then used to determine the replacement fill texture
//   Increasing density is represented by RGBA channels of supplied fill texture:
//   r channel = least dense, a channel = most dense
// - Texture may be applied based on model coordinates or screen coordinates, based on SCREEN_ALIGNED definition
// - The replacement texture is tinted based on _FillColour parameter
// - Final result is blended onto the screen using alpha blending (Blend SrcAlpha OneMinusSrcAlpha)
//
// For more information on the technique used here, see:
//  - http://research.microsoft.com/en-us/um/people/hoppe/hatching.pdf
//  - http://www.cs.utah.edu/~emilp/papers/ToneCtrlHatching.pdf

Shader "Hand-Drawn/Fill/MultiTextured(AlphaBlend)" {
    Properties {
    
    	// The diffuse material texture. This will be used, in conjunction with lighting calculations etc.,
    	// to identify the density of fill texture used over the surface of the model. 
        _MainTex ("Base (RGB)", 2D) = "white" {}
        
        // Four channel image containg levels of fill texture, increasing in density from R->G->B->A
        _FillTex ("Fill Texture RGBA", 2D) = "white" {}

        // Colour applied to fill texture
        _FillColour ("Fill Colour (A=Opacity)", Color) = (0.1607, 0.1607, 0.7922, 1)   // A nice blue biro ink colour is (0.1607,0.1607, 0.7922)

		// Modifier to alter texture intensity (i.e. to lighten/darken texture)
       	_IntensityModifier ("Intensity Modifier", Range(-0.5,0.5)) = 0.0

		// How many times per second should the texture be redrawn
        _RedrawRate ("Redraw Rate", float) = 0.0                                                     
    }
    SubShader {
    
    	// Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
        	"Queue" = "Geometry+100" // Render this object after all opaque objects have been drawn
        	"RenderType" = "Opaque" // The material is opaque. Background still shows through, but that's controlled by Blend mode.
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


		// Use conventional alpha blending to overlay the fill texture on the screen
		// See http://docs.unity3d.com/Documentation/Components/SL-Blend.html for more info
		Blend SrcAlpha OneMinusSrcAlpha

		// Begin the CG Program Code
		// Note that no explicit "Pass" is used in the code, since this is a surface shader
		CGPROGRAM

		// Comment the following line to have model-aligned texture rather than screen-aligned texture
		#define SCREEN_ALIGNED
        
		// surface surf: The surface shader will be a function called "surf"
		// Lambert: Use the Lambert lighting model (influenced by scene lights, but without any specular properties)
		// noforwardadd: Don't apply additional forward lighting passes - these would screw up the lighting calculation
		// finalcolor:ApplyFillTexture:  Once all shading computations are done, call the ApplyFillTexture function, which will apply appropriate texture to the final calculated colour
        #pragma surface surf Lambert alpha:fade finalcolor:ApplyFillTexture

		// GLSL precision hint to maximise performance (at possible expense of precision)
		// See http://www.opengl.org/registry/specs/NV/fragment_program4.txt
		#pragma fragmentoption ARB_precision_hint_fastest

		// Function to return a pseudorandom number in the range 0.0 - 1.0 
		// Used to animate the fill texture mapping
        float rand(float2 co){
	    	return frac(sin(dot(co.xy ,float2(12.9898,78.233)))  * 43758.5453 );
	    }

		// Access the shaderlab properties. The "uniform" keyword shows these values were defined
		// by the calling application (Unity), and are not changed within the shader
        uniform sampler2D _MainTex;
        uniform sampler2D _FillTex;
        uniform half _RedrawRate;
        uniform half _IntensityModifier;
        uniform fixed4 _FillColour;
        
        // _xxx_ST variables are populated by Unity automatically with texture scale (x,y) and offset (z,w) values
		// of corresponding xxx texture. We will use these to scale the texture in screen space
		#ifdef SCREEN_ALIGNED
        	float4 _FillTex_ST;
        #endif
        
		// Input struct holds data passed from Unity to the surface shader
		struct Input
        {
           float2 uv_MainTex : TEXCOORD0; // Texture coordinates of main texture
           float2 uv_FillTex; // Additional texture coordinates of first fill texture
           #ifdef SCREEN_ALIGNED
           		float4 screenPos; // Vertex position in screenspace
           #endif
        };

		// SURFACE SHADER
		void surf(Input IN, inout SurfaceOutput o)
		{
			// Simply set material albedo based on main texture
			// The surface shader will automatically calculate effect of lighting, shadows etc.
			o.Albedo = (half3)tex2D(_MainTex, IN.uv_MainTex).rgb;
		}
        
        // This function is called after the surface shader has accounted for all lighting, light probes, etc.
        // and will be used to substitute the calculated colour value with an (interpolated) fill texture based
        // on pixel brightness
		void ApplyFillTexture(Input IN, SurfaceOutput o, inout half4 color)
		{
			// Calculate apparent brightness of pixel calculated by surface shader,
			// after accounting for lighting, texture, shadows etc.
			half brightness = saturate(dot(color.rgb, half3(0.3, 0.59, 0.11)));
	
			// Determine uv coordinates for fill texture
			float2 uv;
			#ifdef SCREEN_ALIGNED
				// Apply texture in screen coordinates
				uv = IN.screenPos.xy / IN.screenPos.w;
				// Adjust range to be from 0 to 1
				uv = (uv + 1) * 0.5;   
				// Scale coordinates to account for Tiling parameter chosen in the material inspector		
				uv *= _FillTex_ST.xy;
			#else
				// Apply texture in model coordinates
				uv = IN.uv_FillTex.xy;
			#endif
            
			// Create a random offset based on requested redraw rate
			half fillTexOffset = rand(floor(_Time.y * _RedrawRate));

			// Sample the RGBA value from all the fill textures at the requested point 
            fixed4 fillTexRGBA = tex2D(_FillTex, uv + fillTexOffset);

			// Determine the intensity scale
			// We have four textures, so scale factor = 4/5 = 0.8
            half fillIntensity = min(brightness, 0.8);
            half baseIntensity = brightness - fillIntensity;
            half unitIntensity = fillIntensity / 0.8;

			// Calculate a vector of weights that the four textures will contribute to the final output
            half4 fillTexWeights = saturate((unitIntensity * 4.0) - half4(3.0, 2.0, 1.0, 0.0)); // weights for fillTex0, fillTex1, fillTex2, fillTex3

			// We currently have a vector of weights in the style (0, t, 1, 1)
			// where t is a value between 0-1 that represents the weight that the corresponding
			// texture should have to be blended into the final output.
			// To get the weight of the *other* texture with which it should be blended, we can subtract
			// the preceding elements of each weight as follows:
            fillTexWeights.yzw -= fillTexWeights.xyz;
            
			// We now have a vector of weights of the style (0, t, 1-t, 0)
			// Sum the weighted fill textures to determine composite fill texture,
			// and apply the base intensity
			half fillTex = saturate(baseIntensity + dot(fillTexRGBA, fillTexWeights.wzyx));

			// Apply chosen RGB colour
			color.rgb = _FillColour.rgb;
			color.a =  + _IntensityModifier + brightness - fillTex;
		}
		ENDCG	
		
		
    }
} 