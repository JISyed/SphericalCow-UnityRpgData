// (c) 2013 Alastair Aitchison
// This shader combines passes from the following shaders:
// Outline: Overdrawn
// Fill: Smoothed Grayscale

// Example use : INK material

Shader "Hand-Drawn/Fill+Outline/Overdrawn Outline + Smoothed Greyscale Fill" {
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

       // Diffuse object texture
       	_MainTex ("Base (RGB)", 2D) = "white" {}
		
		// Degree of Quantisation (lower numbers = less discrete colours)
		_QuantizationLevels ("Quantization Levels", float) = 3.0
		
		// Contrast Modifier
		_ContrastBoost ("Contrast Boost", float) = 2.0
		
	}

	SubShader {
    	// Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
        	"Queue" = "Geometry+100" // Render this object after geometry queue, but before transparent objects
        	"RenderType" = "Opaque" // The material is opaque
        	"IgnoreProjector" = "True" // Don't let projectors affect this object
        }
        // Outline Pass
        UsePass "Hand-Drawn/Outline/Simple/INTERIORMASK"
        UsePass "Hand-Drawn/Fill/SmoothedGreyscale/INTERIOR"
        
        // Fill Passes
        UsePass "Hand-Drawn/Outline/Overdrawn/OUTLINE1"
        UsePass "Hand-Drawn/Outline/Overdrawn/OUTLINE2"
        UsePass "Hand-Drawn/Outline/Overdrawn/OUTLINE3"
        UsePass "Hand-Drawn/Outline/Overdrawn/OUTLINE4"
		
	}
}
