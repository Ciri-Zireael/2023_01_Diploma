using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class SlidesDropdown : MonoBehaviour
{
    public TMP_Dropdown presentationDropdown; // Reference to the Dropdown UI element
    public string presentationsFolder = ""; // Folder relative to Assets/Resources
    private List<string> presentationPaths = new List<string>();

    void Start()
    {
        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        // Clear existing options
        presentationDropdown.ClearOptions();

        // Get folder path in the Resources directory
        string folderPath = Path.Combine(Application.dataPath, "Resources", presentationsFolder);

        if (Directory.Exists(folderPath))
        {
            // Find subfolders or files (e.g., for each presentation)
            var directories = Directory.GetDirectories(folderPath);

            // Extract directory names and store full paths
            List<string> presentationNames = new List<string>();
            foreach (var dir in directories)
            {
                string folderName = Path.GetFileName(dir);
                presentationNames.Add(folderName);
                presentationPaths.Add(folderName); 
            }

            // Add the names to the dropdown
            presentationDropdown.AddOptions(presentationNames);
        }
        else
        {
            Debug.LogWarning("Presentations folder not found at: " + folderPath);
        }
    }

    public void OnDropdownValueChanged(int index)
    {
        LoadPresentation(presentationPaths[index]);
    }

    void LoadPresentation(string folderPath)
    {
        SlideHolder.SetPresentationPath(folderPath);
    }
}
