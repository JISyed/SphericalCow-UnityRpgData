// (c) 2013 Alastair Aitchison
// This shader combines passes from the following shaders:
// Outline: Simple
// Fill: Textured

// Example use : PENCIL SHADE material with OUTLINE


Shader "Hand-Drawn/Fill+Outline/Simple Outline + Textured Interior" {
	Properties {

		// Base outline colour
        _OutlineColour ("Outline Colour", Color) = (0.95,0.5,0.5,0.5)

        // Base outline width
        _OutlineWidth ("Outline Width", Float) = 0.01
       
        // Diffuse object texture
       	_MainTex ("Base (RGB)", 2D) = "white" {}
		
		// Textured interior fill
        _FillTex  ("Fill Texture (RGB)", 2D) = "white" {}
	}

	SubShader {
    	// Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
        	"Queue" = "Geometry+100" // Render this object after geometry objects, but before transparent objects
        	"RenderType" = "Opaque" // The material is opaque
        	"IgnoreProjector" = "True" // Don't let projectors affect this object
        }
             
        // Fill Pass
        UsePass "Hand-Drawn/Fill/Textured/INTERIORMASK"
		UsePass "Hand-Drawn/Fill/Textured/INTERIOR"
		// Outline Pass
		UsePass "Hand-Drawn/Outline/Simple/OUTLINE"
	}
}
