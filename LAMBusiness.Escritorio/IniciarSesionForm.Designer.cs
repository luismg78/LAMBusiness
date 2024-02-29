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
            LogoPictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // SalirButton
            // 
            SalirButton.Location = new Point(33, 403);
            SalirButton.Margin = new Padding(3, 4, 3, 4);
            SalirButton.Name = "SalirButton";
            SalirButton.Size = new Size(165, 51);
            SalirButton.TabIndex = 5;
            SalirButton.Text = "Salir";
            SalirButton.UseVisualStyleBackColor = true;
            SalirButton.Click += SalirButton_Click;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PasswordTextBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PasswordTextBox.Location = new Point(33, 283);
            PasswordTextBox.Margin = new Padding(3, 4, 3, 4);
            PasswordTextBox.MaxLength = 200;
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PasswordChar = '*';
            PasswordTextBox.Size = new Size(330, 30);
            PasswordTextBox.TabIndex = 4;
            PasswordTextBox.KeyPress += PasswordTextBox_KeyPress;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new Point(33, 255);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(83, 20);
            PasswordLabel.TabIndex = 3;
            PasswordLabel.Text = "Contraseña";
            // 
            // CorreoElectronicoTextBox
            // 
            CorreoElectronicoTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CorreoElectronicoTextBox.CharacterCasing = CharacterCasing.Lower;
            CorreoElectronicoTextBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CorreoElectronicoTextBox.Location = new Point(33, 209);
            CorreoElectronicoTextBox.Margin = new Padding(3, 4, 3, 4);
            CorreoElectronicoTextBox.MaxLength = 200;
            CorreoElectronicoTextBox.Name = "CorreoElectronicoTextBox";
            CorreoElectronicoTextBox.PlaceholderText = "ejemplo@correo.com";
            CorreoElectronicoTextBox.Size = new Size(330, 30);
            CorreoElectronicoTextBox.TabIndex = 2;
            CorreoElectronicoTextBox.KeyPress += CorreoElectronicoTextBox_KeyPress;
            // 
            // IniciarSesionButton
            // 
            IniciarSesionButton.BackColor = Color.FromArgb(255, 65, 82);
            IniciarSesionButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            IniciarSesionButton.ForeColor = Color.White;
            IniciarSesionButton.Location = new Point(198, 403);
            IniciarSesionButton.Margin = new Padding(3, 4, 3, 4);
            IniciarSesionButton.Name = "IniciarSesionButton";
            IniciarSesionButton.Size = new Size(165, 51);
            IniciarSesionButton.TabIndex = 6;
            IniciarSesionButton.Text = "Iniciar Sesión";
            IniciarSesionButton.UseVisualStyleBackColor = false;
            IniciarSesionButton.Click += IniciarSesionButton_ClickAsync;
            // 
            // CorreoElectronicoLabel
            // 
            CorreoElectronicoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CorreoElectronicoLabel.AutoSize = true;
            CorreoElectronicoLabel.Location = new Point(33, 183);
            CorreoElectronicoLabel.Name = "CorreoElectronicoLabel";
            CorreoElectronicoLabel.Size = new Size(132, 20);
            CorreoElectronicoLabel.TabIndex = 1;
            CorreoElectronicoLabel.Text = "Correo Electrónico";
            // 
            // NotificacionLabel
            // 
            NotificacionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            NotificacionLabel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            NotificacionLabel.ForeColor = Color.FromArgb(255, 65, 82);
            NotificacionLabel.Location = new Point(33, 314);
            NotificacionLabel.Name = "NotificacionLabel";
            NotificacionLabel.Size = new Size(331, 68);
            NotificacionLabel.TabIndex = 11;
            NotificacionLabel.Text = "Notificación";
            NotificacionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LogoPictureBox
            // 
            LogoPictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            LogoPictureBox.Image = Properties.Resources.LAM;
            LogoPictureBox.Location = new Point(122, 13);
            LogoPictureBox.Margin = new Padding(3, 4, 3, 4);
            LogoPictureBox.Name = "LogoPictureBox";
            LogoPictureBox.Size = new Size(153, 147);
            LogoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            LogoPictureBox.TabIndex = 12;
            LogoPictureBox.TabStop = false;
            // 
            // IniciarSesionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.FromArgb(234, 237, 239);
            CancelButton = SalirButton;
            ClientSize = new Size(397, 483);
            ControlBox = false;
            Controls.Add(LogoPictureBox);
            Controls.Add(NotificacionLabel);
            Controls.Add(SalirButton);
            Controls.Add(PasswordTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(CorreoElectronicoTextBox);
            Controls.Add(IniciarSesionButton);
            Controls.Add(CorreoElectronicoLabel);
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MaximumSize = new Size(415, 530);
            MinimumSize = new Size(415, 530);
            Name = "IniciarSesionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Iniciar Sesión";
            Load += IniciarSesionForm_Load;
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).EndInit();
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
        private PictureBox LogoPictureBox;
    }
}