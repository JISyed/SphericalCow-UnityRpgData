// Diffuse Shader with Textured Shadow Fill
// (c) 2013 Alastair Aitchison
//
// - Surface shader uses base pass forward lighting only
// - Object is rendered using basic diffuse texture
// - Custom lighting model applies shading in dark areas based on supplied texture in screenspace coordinates
// - Inspired by Valkyria Chronicles

Shader "Hand-Drawn/Fill/Textured Shadow Fill" {
    // Shaderlab properties are exposed in Unity's material inspector
	Properties {
	
		// The diffuse material texture
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		// Texture with which to draw shade on the model
		_ShadeTex ("Shade Texture (RGB)", 2D) = "black" {}
		
		// Lighting ramp
		_Ramp ("Lighting Ramp (RGB)", 2D) = "gray" {}
		
		// How many times per second should the shadow texture be redrawn
        _RedrawRate ("Redraw Rate", float) = 0.0 
		
	}

	SubShader {
	
    	// Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
        	"RenderType" = "Opaque" // The material is opaque
        	"IgnoreProjector" = "True" // Don't let projectors affect this object
        }

		// Here begins the Cg shader code
		CGPROGRAM
		
		// surface surf : The surface shader will be a function called surf()
		// Toon : It will use the lighting model provided by the LightingToon function
		// exclude_path:prepass : Exclude deferred rendering (since it does not provide light direction required by toon lighting model)
		// nolightmap : Disable lightmap support (makes shader smaller)
		// noforwardadd : Disable Forward rendering additive pass. This makes the shader support one full directional light, with all other lights computed per-vertex/SH.
		#pragma surface surf Toon exclude_path:prepass nolightmap noforwardadd

		// Access the shaderlab properties
		sampler2D _Ramp;
		sampler2D _MainTex;
		sampler2D _ShadeTex;
		float4 _ShadeTex_ST;
		fixed _RedrawRate;

		// We need to define a custom surfaceoutput structure to enable us
		// to pass screenPos to the lighting function
    	struct SurfaceOutputCustom
    	{
    		// These properties must always be present
        	fixed3 Albedo;
        	fixed3 Normal;
        	fixed3 Emission;
        	half Specular;
        	fixed Alpha;
        	// Following is a custom property
        	fixed4 screenPos;
    	};

		// Function to return a pseudorandom number in the range 0.0 - 1.0 
		// Used to animate the texture mapping
        float rand(float2 co){
	    	return frac(sin(dot(co.xy ,float2(12.9898,78.233)))  * 43758.5453 );
	    }

		// Lighting model
		inline half4 LightingToon (SurfaceOutputCustom s, half3 lightDir, half atten)
		{
			#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
			#endif

			// Determine the proportion of incident light reflected off the surface
			// Calculated by dot product of surface normal vector and light direction vector
			// Normally referred to as "N dot L".
			half NdotL = dot(s.Normal, lightDir);

			// "Wrapped" diffuse
			half diff = NdotL * 0.5 + 0.5;
			 
			// Calculate the apparent brightness of the light source	
			half lightBrightness = dot(_LightColor0.rgb, half3(0.3, 0.59, 0.11));
			
			// Intensity depends on light brightness, attenuation, and diffuse reflectivity 
			half intensity = lightBrightness * (diff * atten * 2);
			
			// Clamp intensity to the range 0 - 1
			intensity = saturate(intensity);
			
			// Create a new output structure
			half4 output;

			// Output RGB is product of Albedo and intensity
			output.rgb = s.Albedo * intensity;
			output.a = 1.0;
			
			// Now calculate shading
			// First, calculate the screen coordinates at which to sample the texture
			float2 uv = s.screenPos.xy / s.screenPos.w;
			// Adjust for scaling parameters set in material inspector
			uv *= _ShadeTex_ST.xy ;
			// Now add a random UV offset based on requested redraw rate
			uv += rand(floor(_Time.y * _RedrawRate));
	
			// Look up the value from the ramp texture corresponding to the diffuse light level
			half3 ramp = tex2D (_Ramp, float2(diff, 0.5)).rgb;
			
			// Use the ramp value to lerp between the sampled shade texture and white
			// The brighter the ramp texture, shade becomes more white
			// The darker the ramp texture, shade becomes more like ShadeTex 
			half3 shade = lerp(tex2D(_ShadeTex, uv).rgb, fixed3(1, 1, 1), ramp);
			
			// Multiply the output colour by the shade (darkens the output)
			output.rgb *= shade;
			
			return output;
		}

		// Define input structure containing properties required by the surface shader
		struct Input {
			float2 uv_MainTex;
			float2 uv_ShadowTex;
			float4 screenPos;
		};

		// Surface shader function - refreshingly small!
		void surf (Input IN, inout SurfaceOutputCustom o) {
			// Set the surface Albedo based on the main texture
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			// Pass the screen position the the SurfaceOutputCustom structure
			// so that it may be used in the lighting model
			o.screenPos = IN.screenPos;
		}
		ENDCG
	} // End SubShader
} // End Shader