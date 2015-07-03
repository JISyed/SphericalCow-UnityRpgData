// (c) 2013 Alastair Aitchison
// This shader combines passes from the following shaders:
// Outline: Animated + Textured
// Fill: Quantized Textured
// Example use : CRAYON material


Shader "Hand-Drawn/Fill+Outline/Textured Outline + Quantized Textured Interior" {
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

       // Diffuse object texture
       	_MainTex ("Base (RGB)", 2D) = "white" {}
		
		// Texture of the fill
       _FillTex  ("Fill Texture (RGB)", 2D) = "white" {}
       
       // Degree of fill colour quantization
		_QuantizationLevels ("Quantization Levels", float) = 3.0
		
		// Contrast modifier
		_ContrastBoost ("Contrast Boost", float) = 2.0
	}

	SubShader {
    	// Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
        	"Queue" = "Geometry+100" // Render this object after geometry objects, but before transparent objects
        	"RenderType" = "Opaque" // The material is opaque
        	"IgnoreProjector" = "True" // Don't let projectors affect this object
        }
        
        
        // Render the fill
		UsePass "Hand-Drawn/Fill/QuantizedTextured/INTERIOR"
		
		// Render the outline
        UsePass "Hand-Drawn/Outline/Animated+Textured/INTERIORMASK"
        UsePass "Hand-Drawn/Outline/Animated+Textured/OUTLINE"
	}
}
