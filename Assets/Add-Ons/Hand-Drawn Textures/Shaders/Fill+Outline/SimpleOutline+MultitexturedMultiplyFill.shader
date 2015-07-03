// (c) 2013 Alastair Aitchison
// This shader combines passes from the following shaders:
// Outline: Simple
// Fill: Multitextured Multiply Fill
// Example use : BALLPOINT PEN material


Shader "Hand-Drawn/Fill+Outline/Simple Outline + Multitextured Multiply Fill" {
    Properties {
    
    	// Outline properties
        _OutlineWidth ("Outline Width", float) = 0.02 // Width of outline, specified in model coordinates
        _OutlineColour ("Outline Colour (A=Opacity)", Color) = (0.1607, 0.1607, 0.7922, 1)   // Colour of outline (including alpha)

		// Fill properties
        _MainTex ("Base (RGB)", 2D) = "white" {} // The base material texture
        _FillTex ("Fill Texture RGBA", 2D) = "white" {}  // Six levels of fill texture, increasing in density    
        _FillColour ("Fill Colour (A=Opacity)", Color) = (0.1607, 0.1607, 0.7922, 1)   // Fill tint colour
       	_IntensityModifier ("Intensity Modifier", Range(-1.0,1.0)) = 0.0 // Modifier to increase brightness (i.e. to lighten texture density)
        _RedrawRate ("Redraw Rate", float) = 0.0 // How many times per second should the fill be redrawn                                                    
    }
    SubShader {   
        Tags {
        	"Queue" = "Geometry+100" // Render this object after geometry queue but before transparent objects
        	"RenderType" = "Opaque" // The material is opaque
        	"IgnoreProjector" = "True" // Don't let projectors affect this object
        }
        
        // Use "Multiply" blend
		Blend DstColor Zero

        // Outline
        UsePass "Hand-Drawn/Outline/Simple/INTERIORMASK"
        UsePass "Hand-Drawn/Outline/Simple/OUTLINE"

		// Fill
		UsePass  "Hand-Drawn/Fill/MultiTextured(Multiply)/FORWARD"	

		
        
        
    }
} 