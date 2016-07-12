using System;

namespace uFrame.Editor.Platform
{
    public interface IPlatformOperations
    {
        void OpenScriptFile(string filePath);
        void OpenLink(string link);
        string GetAssetPath(object graphData);
        bool MessageBox(string title, string message, string ok);
        bool MessageBox(string title, string message, string ok, string cancel);

        void ComplexMessageBox(string title, string message, string optionA, Action actionA,
                                                             string optionB, Action actionB,
                                                             string optionC, Action actionC);
        void SaveAssets();
        void RefreshAssets();
        void Progress(float progress, string message);
    }
}
