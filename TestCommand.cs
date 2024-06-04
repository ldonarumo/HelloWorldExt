using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace HelloWorldExt
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class TestCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("2bf6683e-e53d-4228-94c7-3ee0c85ed82d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly HelloWorldExtPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private TestCommand(HelloWorldExtPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static TestCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(HelloWorldExtPackage package)
        {
            // Switch to the main thread - the call to AddCommand in TestCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new TestCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var newTitle = PromptForNewName();
            if (!string.IsNullOrEmpty(newTitle))
            {

                var customTitleToolWindow = (CustomTitleToolWindow)package.ShowCustomTitleToolWindow(); // (CustomTitleToolWindow)ShowCustomTitleToolWindow();
                if (customTitleToolWindow != null)
                {
                    customTitleToolWindow.UpdateTitle(newTitle);
                    ((CustomTitleControl)customTitleToolWindow.Content).SetTitle(newTitle);
                }
            }
        }



        private string PromptForNewName()
        {
            using (var inputForm = new Form())
            {
                inputForm.Width = 400;
                inputForm.Height = 150;
                inputForm.Text = "Rename Tab";

                var inputBox = new TextBox() { Left = 50, Top = 20, Width = 300 };
                var okButton = new Button() { Text = "OK", Left = 250, Width = 100, Top = 50, DialogResult = DialogResult.OK };
                var cancelButton = new Button() { Text = "Cancel", Left = 150, Width = 100, Top = 50, DialogResult = DialogResult.Cancel };

                inputForm.Controls.Add(inputBox);
                inputForm.Controls.Add(okButton);
                inputForm.Controls.Add(cancelButton);
                inputForm.AcceptButton = okButton;
                inputForm.CancelButton = cancelButton;

                return inputForm.ShowDialog() == DialogResult.OK ? inputBox.Text : null;
            }
        }
    }
}
