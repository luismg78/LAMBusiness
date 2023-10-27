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
            this.SalirButton = new System.Windows.Forms.Button();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.CorreoElectronicoTextBox = new System.Windows.Forms.TextBox();
            this.IniciarSesionButton = new System.Windows.Forms.Button();
            this.CorreoElectronicoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SalirButton
            // 
            this.SalirButton.Location = new System.Drawing.Point(30, 239);
            this.SalirButton.Name = "SalirButton";
            this.SalirButton.Size = new System.Drawing.Size(127, 29);
            this.SalirButton.TabIndex = 10;
            this.SalirButton.Text = "Salir";
            this.SalirButton.UseVisualStyleBackColor = true;
            this.SalirButton.Click += new System.EventHandler(this.SalirButton_Click);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PasswordTextBox.Location = new System.Drawing.Point(30, 142);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(272, 23);
            this.PasswordTextBox.TabIndex = 7;
            this.PasswordTextBox.Text = "12345";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(30, 116);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(67, 15);
            this.PasswordLabel.TabIndex = 8;
            this.PasswordLabel.Text = "Contraseña";
            // 
            // CorreoElectronicoTextBox
            // 
            this.CorreoElectronicoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CorreoElectronicoTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.CorreoElectronicoTextBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CorreoElectronicoTextBox.Location = new System.Drawing.Point(30, 67);
            this.CorreoElectronicoTextBox.MaxLength = 200;
            this.CorreoElectronicoTextBox.Name = "CorreoElectronicoTextBox";
            this.CorreoElectronicoTextBox.Size = new System.Drawing.Size(272, 26);
            this.CorreoElectronicoTextBox.TabIndex = 6;
            this.CorreoElectronicoTextBox.Text = "luismg78@gmail.com";
            // 
            // IniciarSesionButton
            // 
            this.IniciarSesionButton.Location = new System.Drawing.Point(175, 239);
            this.IniciarSesionButton.Name = "IniciarSesionButton";
            this.IniciarSesionButton.Size = new System.Drawing.Size(127, 29);
            this.IniciarSesionButton.TabIndex = 9;
            this.IniciarSesionButton.Text = "Iniciar Sesión";
            this.IniciarSesionButton.UseVisualStyleBackColor = true;
            this.IniciarSesionButton.Click += new System.EventHandler(this.IniciarSesionButton_Click);
            // 
            // CorreoElectronicoLabel
            // 
            this.CorreoElectronicoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CorreoElectronicoLabel.AutoSize = true;
            this.CorreoElectronicoLabel.Location = new System.Drawing.Point(30, 43);
            this.CorreoElectronicoLabel.Name = "CorreoElectronicoLabel";
            this.CorreoElectronicoLabel.Size = new System.Drawing.Size(105, 15);
            this.CorreoElectronicoLabel.TabIndex = 5;
            this.CorreoElectronicoLabel.Text = "Correo Electrónico";
            // 
            // IniciarSesionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.SalirButton;
            this.ClientSize = new System.Drawing.Size(335, 308);
            this.ControlBox = false;
            this.Controls.Add(this.SalirButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.CorreoElectronicoTextBox);
            this.Controls.Add(this.IniciarSesionButton);
            this.Controls.Add(this.CorreoElectronicoLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(351, 347);
            this.MinimumSize = new System.Drawing.Size(351, 347);
            this.Name = "IniciarSesionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Iniciar Sesión";
            this.ResumeLayout(false);
            this.PerformLayout();

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