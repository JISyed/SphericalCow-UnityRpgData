// (c) 2013 Alastair Aitchison
// This shader combines passes from the following shaders:
// Outline: Simple
// Fill: Bumped Diffuse (Unity Built-in)
// Example use : HIGHLIGHTER material


Shader "Hand-Drawn/Fill+Outline/Simple Outline + Bumped Diffuse Fill" {
	Properties {
		// Fill colour
		_Color ("Main Color", Color) = (1,1,1,1)
	
		// Fill diffuse texture
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		// Fill bumpmap
		_BumpMap ("Normalmap", 2D) = "bump" {}
		
		// Outline colour
        _OutlineColour ("Outline Colour", Color) = (0.95,0.5,0.5,0.5)

        // Outline width
        _OutlineWidth ("Outline Width", Float) = 0.01
	}

	SubShader {
	
    	// Tags provide additional information to the rendering engine to affect how and when
    	// this shader should be processed
        Tags {
        	"Queue" = "Geometry+100" // Render this object after geometry objects but before transparent objects
        	"RenderType" = "Opaque" // The material is opaque
        	"IgnoreProjector" = "True" // Don't let projectors affect this object
        }
        
        // Use the simple outline
        UsePass "Hand-Drawn/Outline/Simple/OUTLINE"
        
        // Use the Bumped Diffuse forward rendering fill
		UsePass "Bumped Diffuse/FORWARD"
	}
}
