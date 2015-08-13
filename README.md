#Passive Virtual Desktop Spatial Learning Task
Ian T. Ruginski Master's Project, University of Utah, Dept. of Psychology (Cognition and Neural Science)

Built and compiled using Unity version 4.6.4
Scripts coded in C#

Short description:

####Files and Folders
1. Assets

      -This houses all of the graphical assets and scripts that make up the Unity environment, such as objects, terrain, trees, etc.
  
      -The "Scene" subdirectory contains the scripts for the Video Recording Unity .exe (IETesterInfo.cs) and the Experimental Unity .exe (IEExperiment.cs)\
  
      -The "Scripts" subdirectory contains all the scripts for experimental procedure, i.e. the instructions that appear on the screen.

2.Ruginski_Masters_Experiment_Data and Ruginski_Masters_VideoRecord_Data

     -Houses configuration files for experimental .exe ("Ruginski_Masters_Experiment")

     -And for video recording .exe ("Ruginski_Masters_VideoRecord"), respectively

3. "Library" and "ProjectSettings"

    -Contain config files and metadata for the project.

4. Experiment Data

    -This houses the per-participant datafiles once the experiment has been completed for one participant

#####Functionality and Building the Project

    -Currently, the video recordings (passive spatial learning task) are pre-recorded to match Ian Ruginski's Masters Experiment

    -The project can be modified and built into a portable *.exe file with less files and folders by...
1. Going to the "bin" folder
2. Opening "IanVersion.exe"
3. Building the file from Unity. This should be the fully editable scene file. There is an option in the top right of preferences for "IanVersion." 

    -Checked will build a version that can record the videos. 

    -Unchecked will build the experiment itself and use the "Ian_Replay.dat" and "Ian_Replay2.dat"

    -Note: For counterbalancing the video order, you will have to create a new folder, copy over all subdirectories and files, 

4. If you want to change the video recordings, you must use the "Ian_Masters_VideoRecord.exe" and record two new videos using the menu from that build.

######ConverterForIan folder
    Note: Requires latest version of Microsoft Visual Studio

    -Passive Spatial Learning Task Data Converter (participant file data converter to a more easily readable format for data analysis programs)

    You will need:
    1. The *.dat file that you want to convert
    2. The IanConverter.exe (in the Bin Folder)

    Steps:
        1. Open the *.dat file with the IanConverter.exe program
        2. The program will create a new *.dat.converted file for you
        3. This is just a csv file ready for data cleaning/analysis.
