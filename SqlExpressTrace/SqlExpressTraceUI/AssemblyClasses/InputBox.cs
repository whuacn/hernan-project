/************************************************ 2014 Pete_H *******************************************************
 * 
 * This software released under the Code Project Open License. Refer to: http://www.codeproject.com/info/cpol10.aspx
 * or refer to the copy of the Code Project Open License (CPOL.htm) included with this solution. 
 * 
 * This code and the compiled components including libraries and the demonstration application have been made 
 * available only for the purpose of learning, sharing and demonstrating ideas and NOT to imply, recommend or 
 * suggest usage of any part of the code or components.
 * 
 * No claim of suitability, guarantee, or any warranty whatsoever is provided. The software is provided "as-is"
 * Usage of any of this code or components is entirely at your own risk.
 * 
 ********************************************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SqlTraceExpressUI
{
	public class InputBox
	{

		/// <summary>
		/// Displays a modal dialog for getting user input text.
		/// </summary>
		/// <param name="owner">Specifies the owner of the modal dialog. Can be null.</param>
		/// <param name="title">Specifies the caption to use for the modal dialog.</param>
		/// <param name="promptText">Specifies the prompt to display above the input textbox as a guide.</param>
		/// <param name="value">A reference to a string that will contain the input text if the user did not cancel.</param>
		/// <param name="usePasswordChars">Specifies whether this input is for a password and should to mask the input text.</param>
		/// <returns></returns>
		public static DialogResult ShowDialog(IWin32Window owner, string title, string promptText,
			ref string value, bool usePasswordChars = false)
		{
			Form inputDialog = new Form();
			Label promptLabel = new Label();
			TextBox inputTextBox = new TextBox();
			Button okButton = new Button();
			Button cancelButton = new Button();

			inputDialog.Text = title;
			promptLabel.Text = promptText;
			inputTextBox.Text = value;

			okButton.Text = "OK";
			cancelButton.Text = "Cancel";
			okButton.DialogResult = DialogResult.OK;
			cancelButton.DialogResult = DialogResult.Cancel;

			promptLabel.SetBounds(9, 20, 372, 13);
			inputTextBox.SetBounds(12, 36, 372, 20);
			okButton.SetBounds(228, 72, 75, 23);
			cancelButton.SetBounds(309, 72, 75, 23);

			promptLabel.AutoSize = true;
			inputTextBox.Anchor = inputTextBox.Anchor | AnchorStyles.Right;
			okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			inputDialog.ClientSize = new Size(396, 107);
			inputDialog.Controls.AddRange(new Control[] { promptLabel, inputTextBox, okButton, cancelButton });
			inputDialog.ClientSize = new Size(Math.Max(300, promptLabel.Right + 10), inputDialog.ClientSize.Height);
			inputDialog.FormBorderStyle = FormBorderStyle.FixedDialog;

			inputDialog.StartPosition = FormStartPosition.CenterParent;
			inputDialog.MinimizeBox = false;
			inputDialog.MaximizeBox = false;
			inputDialog.AcceptButton = okButton;
			inputDialog.CancelButton = cancelButton;

			inputTextBox.UseSystemPasswordChar = usePasswordChars;

			DialogResult dialogResult = inputDialog.ShowDialog(owner);

			value = inputTextBox.Text;
			return dialogResult;
		}
	}
}