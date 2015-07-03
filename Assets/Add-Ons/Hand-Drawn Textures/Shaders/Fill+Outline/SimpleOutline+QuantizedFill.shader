// (c) 2013 Alastair Aitchison
// This shader combines passes from the following shaders:
// Outline: Simple
// Fill: Quantized
// Example use : FELT-TIP material


Shader "Hand-Drawn/Fill+Outline/Simple Outline + Quantized Interior" {
	Properties {

		// Base outline colour
        _OutlineColour ("Outline Colour", Color) = (0.95,0.5,0.5,0.5)

        // Base outline width
        _OutlineWidth ("Outline Width", Float) = 0.01

		// Base fill texture
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		// Degree of quantization
		_QuantizationLevels ("Quantization Levels", float) = 3.0

		// Scale factor to increase contrast in output colour range
		_ContrastBoost ("Contrast Boost", float) = 3.0

	}

	SubShader {
	
    	// Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
        	"Queue" = "Geometry+100" // Render this object after other geometry objects, but before transparent objects
        	"RenderType" = "Opaque" // The material is opaque
        	"IgnoreProjector" = "True" // Don't let projectors affect this object
        }
        
        // Simple Outline
        UsePass "Hand-Drawn/Outline/Simple/OUTLINE"
        // Quantized Fill
		UsePass "Hand-Drawn/Fill/Quantized/INTERIOR"
	}
}
