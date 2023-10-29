namespace LAMBusiness.Escritorio
{
    partial class IniciarSesionForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SalirButton = new Button();
            PasswordTextBox = new TextBox();
            PasswordLabel = new Label();
            CorreoElectronicoTextBox = new TextBox();
            IniciarSesionButton = new Button();
            CorreoElectronicoLabel = new Label();
            SuspendLayout();
            // 
            // SalirButton
            // 
            SalirButton.Location = new Point(43, 398);
            SalirButton.Margin = new Padding(4, 5, 4, 5);
            SalirButton.Name = "SalirButton";
            SalirButton.Size = new Size(181, 48);
            SalirButton.TabIndex = 10;
            SalirButton.Text = "Salir";
            SalirButton.UseVisualStyleBackColor = true;
            SalirButton.Click += SalirButton_Click;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PasswordTextBox.Location = new Point(43, 228);
            PasswordTextBox.Margin = new Padding(4, 5, 4, 5);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PasswordChar = '*';
            PasswordTextBox.Size = new Size(387, 31);
            PasswordTextBox.TabIndex = 7;
            PasswordTextBox.Text = "12345";
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new Point(43, 193);
            PasswordLabel.Margin = new Padding(4, 0, 4, 0);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(101, 25);
            PasswordLabel.TabIndex = 8;
            PasswordLabel.Text = "Contraseña";
            // 
            // CorreoElectronicoTextBox
            // 
            CorreoElectronicoTextBox.CharacterCasing = CharacterCasing.Lower;
            CorreoElectronicoTextBox.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            CorreoElectronicoTextBox.Location = new Point(43, 105);
            CorreoElectronicoTextBox.Margin = new Padding(4, 5, 4, 5);
            CorreoElectronicoTextBox.MaxLength = 200;
            CorreoElectronicoTextBox.Name = "CorreoElectronicoTextBox";
            CorreoElectronicoTextBox.PlaceholderText = "ejemplo@correo.com";
            CorreoElectronicoTextBox.Size = new Size(387, 35);
            CorreoElectronicoTextBox.TabIndex = 6;
            CorreoElectronicoTextBox.Text = "luismg78@gmail.com";
            // 
            // IniciarSesionButton
            // 
            IniciarSesionButton.Location = new Point(250, 398);
            IniciarSesionButton.Margin = new Padding(4, 5, 4, 5);
            IniciarSesionButton.Name = "IniciarSesionButton";
            IniciarSesionButton.Size = new Size(181, 48);
            IniciarSesionButton.TabIndex = 9;
            IniciarSesionButton.Text = "Iniciar Sesión";
            IniciarSesionButton.UseVisualStyleBackColor = true;
            IniciarSesionButton.Click += IniciarSesionButton_Click;
            // 
            // CorreoElectronicoLabel
            // 
            CorreoElectronicoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CorreoElectronicoLabel.AutoSize = true;
            CorreoElectronicoLabel.Location = new Point(43, 72);
            CorreoElectronicoLabel.Margin = new Padding(4, 0, 4, 0);
            CorreoElectronicoLabel.Name = "CorreoElectronicoLabel";
            CorreoElectronicoLabel.Size = new Size(157, 25);
            CorreoElectronicoLabel.TabIndex = 5;
            CorreoElectronicoLabel.Text = "Correo Electrónico";
            // 
            // IniciarSesionForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = SalirButton;
            ClientSize = new Size(470, 485);
            ControlBox = false;
            Controls.Add(SalirButton);
            Controls.Add(PasswordTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(CorreoElectronicoTextBox);
            Controls.Add(IniciarSesionButton);
            Controls.Add(CorreoElectronicoLabel);
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new Size(492, 541);
            MinimumSize = new Size(492, 541);
            Name = "IniciarSesionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Iniciar Sesión";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button SalirButton;
        private TextBox PasswordTextBox;
        private Label PasswordLabel;
        private TextBox CorreoElectronicoTextBox;
        private Button IniciarSesionButton;
        private Label CorreoElectronicoLabel;
    }
}