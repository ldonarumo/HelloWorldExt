using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace HelloWorldExt
{
    [Guid("333d9ef0-b210-434d-967e-0f2332895741")]
    public class CustomTitleToolWindow : ToolWindowPane
    {
        public CustomTitleToolWindow() : base(null)
        {
            this.Caption = "Custom Tab Title";
            this.Content = new CustomTitleControl();
        }

        public void UpdateTitle(string newTitle)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.Caption = newTitle;
        }
    }
}
