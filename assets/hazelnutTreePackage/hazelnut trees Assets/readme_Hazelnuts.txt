
1. Optimized Textures and Materials
--------------------------------------------
Please note that all tree prefabs share the same materials and textures in order to save texture memory and texture load at runtime.

Usually unity automatically creates materials and textures for each tree.
I have just deleted all those texture folders except for the first one, then assigned the materials "Optimized Bark Meterial" and "Optimized Leaf Material" both located in the Prefab "xyz__donnotdelete" to each tree by dragging them from the project tab to the proper slot of each tree in the inspector tab.

Changing any parameter on any tree will unity force to recalculate the textures so the additional folders will be recreated. In this case just delete the new folders and reassign the original materials like described above.

Those materials use the textures you find in the folder "xyz__donnotdelete_Textures". Please note the special texture settings of those textures: High Aniso Level [9] on the diffuse texture and kaiser-mipmapping.


2. Creating highly detailed trunks and branches
--------------------------------------------
Unfortunately Unity’s tree creator does not support smooth geometry on trunks whose radius is lower than about  0.4m. usually those trunks are simple cuboids – whereas you might want to have e.g. hexagons as far as the cross section is concerned. you either can live with this restriction or work with a simple trick: do not model your trees 1:1 but 2:1 or even 3:1!
just set the prefab’s xyz-scale to 0.5 and work on your tree as you would do with any other tree.
but please note: using those trees within the terrain engine you will have to set the tree height and tree width to 50 within the "terrain" --> "place trees settings" before you can start painting those trees onto the terrain. otherwise they would just get placed in their original size.
have a look at "Hazelnut 5m 2X highres" for the details.