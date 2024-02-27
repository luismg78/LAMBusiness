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
            NotificacionLabel = new Label();
            SuspendLayout();
            // 
            // SalirButton
            // 
            SalirButton.Location = new Point(43, 382);
            SalirButton.Margin = new Padding(4, 5, 4, 5);
            SalirButton.Name = "SalirButton";
            SalirButton.Size = new Size(181, 64);
            SalirButton.TabIndex = 5;
            SalirButton.Text = "Salir";
            SalirButton.UseVisualStyleBackColor = true;
            SalirButton.Click += SalirButton_Click;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PasswordTextBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PasswordTextBox.Location = new Point(43, 198);
            PasswordTextBox.Margin = new Padding(4, 5, 4, 5);
            PasswordTextBox.MaxLength = 200;
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PasswordChar = '*';
            PasswordTextBox.Size = new Size(387, 34);
            PasswordTextBox.TabIndex = 4;
            PasswordTextBox.Text = "12345";
            PasswordTextBox.KeyPress += PasswordTextBox_KeyPress;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new Point(43, 163);
            PasswordLabel.Margin = new Padding(4, 0, 4, 0);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(101, 25);
            PasswordLabel.TabIndex = 3;
            PasswordLabel.Text = "Contraseña";
            // 
            // CorreoElectronicoTextBox
            // 
            CorreoElectronicoTextBox.CharacterCasing = CharacterCasing.Lower;
            CorreoElectronicoTextBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CorreoElectronicoTextBox.Location = new Point(43, 105);
            CorreoElectronicoTextBox.Margin = new Padding(4, 5, 4, 5);
            CorreoElectronicoTextBox.MaxLength = 200;
            CorreoElectronicoTextBox.Name = "CorreoElectronicoTextBox";
            CorreoElectronicoTextBox.PlaceholderText = "ejemplo@correo.com";
            CorreoElectronicoTextBox.Size = new Size(387, 34);
            CorreoElectronicoTextBox.TabIndex = 2;
            CorreoElectronicoTextBox.Text = "luismg78@gmail.com";
            CorreoElectronicoTextBox.KeyPress += CorreoElectronicoTextBox_KeyPress;
            // 
            // IniciarSesionButton
            // 
            IniciarSesionButton.BackColor = Color.Brown;
            IniciarSesionButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            IniciarSesionButton.ForeColor = Color.White;
            IniciarSesionButton.Location = new Point(250, 382);
            IniciarSesionButton.Margin = new Padding(4, 5, 4, 5);
            IniciarSesionButton.Name = "IniciarSesionButton";
            IniciarSesionButton.Size = new Size(181, 64);
            IniciarSesionButton.TabIndex = 6;
            IniciarSesionButton.Text = "Iniciar Sesión";
            IniciarSesionButton.UseVisualStyleBackColor = false;
            IniciarSesionButton.Click += IniciarSesionButton_ClickAsync;
            // 
            // CorreoElectronicoLabel
            // 
            CorreoElectronicoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CorreoElectronicoLabel.AutoSize = true;
            CorreoElectronicoLabel.Location = new Point(43, 72);
            CorreoElectronicoLabel.Margin = new Padding(4, 0, 4, 0);
            CorreoElectronicoLabel.Name = "CorreoElectronicoLabel";
            CorreoElectronicoLabel.Size = new Size(157, 25);
            CorreoElectronicoLabel.TabIndex = 1;
            CorreoElectronicoLabel.Text = "Correo Electrónico";
            // 
            // NotificacionLabel
            // 
            NotificacionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            NotificacionLabel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            NotificacionLabel.ForeColor = Color.Brown;
            NotificacionLabel.Location = new Point(43, 259);
            NotificacionLabel.Margin = new Padding(4, 0, 4, 0);
            NotificacionLabel.Name = "NotificacionLabel";
            NotificacionLabel.Size = new Size(389, 36);
            NotificacionLabel.TabIndex = 11;
            NotificacionLabel.Text = "Notificación";
            NotificacionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // IniciarSesionForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            CancelButton = SalirButton;
            ClientSize = new Size(467, 473);
            ControlBox = false;
            Controls.Add(NotificacionLabel);
            Controls.Add(SalirButton);
            Controls.Add(PasswordTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(CorreoElectronicoTextBox);
            Controls.Add(IniciarSesionButton);
            Controls.Add(CorreoElectronicoLabel);
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new Size(489, 529);
            MinimumSize = new Size(489, 529);
            Name = "IniciarSesionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Iniciar Sesión";
            Load += IniciarSesionForm_Load;
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
        private Label NotificacionLabel;
    }
}