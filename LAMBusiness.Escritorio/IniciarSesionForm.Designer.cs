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
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // SalirButton
            // 
            SalirButton.Location = new Point(29, 302);
            SalirButton.Name = "SalirButton";
            SalirButton.Size = new Size(144, 38);
            SalirButton.TabIndex = 5;
            SalirButton.Text = "Salir";
            SalirButton.UseVisualStyleBackColor = true;
            SalirButton.Click += SalirButton_Click;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PasswordTextBox.BorderStyle = BorderStyle.None;
            PasswordTextBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            PasswordTextBox.Location = new Point(40, 220);
            PasswordTextBox.MaxLength = 200;
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PasswordChar = '*';
            PasswordTextBox.Size = new Size(267, 18);
            PasswordTextBox.TabIndex = 4;
            PasswordTextBox.KeyPress += PasswordTextBox_KeyPress;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new Point(29, 191);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(67, 15);
            PasswordLabel.TabIndex = 3;
            PasswordLabel.Text = "Contraseña";
            // 
            // CorreoElectronicoTextBox
            // 
            CorreoElectronicoTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CorreoElectronicoTextBox.BorderStyle = BorderStyle.None;
            CorreoElectronicoTextBox.CharacterCasing = CharacterCasing.Lower;
            CorreoElectronicoTextBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CorreoElectronicoTextBox.Location = new Point(40, 154);
            CorreoElectronicoTextBox.MaxLength = 200;
            CorreoElectronicoTextBox.Name = "CorreoElectronicoTextBox";
            CorreoElectronicoTextBox.PlaceholderText = "ejemplo@correo.com";
            CorreoElectronicoTextBox.Size = new Size(267, 18);
            CorreoElectronicoTextBox.TabIndex = 2;
            CorreoElectronicoTextBox.KeyPress += CorreoElectronicoTextBox_KeyPress;
            // 
            // IniciarSesionButton
            // 
            IniciarSesionButton.BackColor = Color.FromArgb(255, 65, 82);
            IniciarSesionButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            IniciarSesionButton.ForeColor = Color.White;
            IniciarSesionButton.Location = new Point(173, 302);
            IniciarSesionButton.Name = "IniciarSesionButton";
            IniciarSesionButton.Size = new Size(144, 38);
            IniciarSesionButton.TabIndex = 6;
            IniciarSesionButton.Text = "Iniciar Sesión";
            IniciarSesionButton.UseVisualStyleBackColor = false;
            IniciarSesionButton.Click += IniciarSesionButton_ClickAsync;
            // 
            // CorreoElectronicoLabel
            // 
            CorreoElectronicoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CorreoElectronicoLabel.AutoSize = true;
            CorreoElectronicoLabel.Location = new Point(29, 128);
            CorreoElectronicoLabel.Name = "CorreoElectronicoLabel";
            CorreoElectronicoLabel.Size = new Size(105, 15);
            CorreoElectronicoLabel.TabIndex = 1;
            CorreoElectronicoLabel.Text = "Correo Electrónico";
            // 
            // NotificacionLabel
            // 
            NotificacionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            NotificacionLabel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            NotificacionLabel.ForeColor = Color.FromArgb(255, 65, 82);
            NotificacionLabel.Location = new Point(29, 251);
            NotificacionLabel.Name = "NotificacionLabel";
            NotificacionLabel.Size = new Size(290, 48);
            NotificacionLabel.TabIndex = 11;
            NotificacionLabel.Text = "Notificación";
            NotificacionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LogoPictureBox
            // 
            LogoPictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            LogoPictureBox.Image = Properties.Resources.LAM;
            LogoPictureBox.Location = new Point(107, 10);
            LogoPictureBox.Name = "LogoPictureBox";
            LogoPictureBox.Size = new Size(134, 111);
            LogoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            LogoPictureBox.TabIndex = 12;
            LogoPictureBox.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Location = new Point(30, 146);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(289, 37);
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.White;
            pictureBox2.Location = new Point(30, 211);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(289, 37);
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            // 
            // IniciarSesionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(234, 237, 239);
            CancelButton = SalirButton;
            ClientSize = new Size(349, 368);
            ControlBox = false;
            Controls.Add(PasswordTextBox);
            Controls.Add(CorreoElectronicoTextBox);
            Controls.Add(pictureBox1);
            Controls.Add(LogoPictureBox);
            Controls.Add(NotificacionLabel);
            Controls.Add(SalirButton);
            Controls.Add(PasswordLabel);
            Controls.Add(IniciarSesionButton);
            Controls.Add(CorreoElectronicoLabel);
            Controls.Add(pictureBox2);
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MaximumSize = new Size(365, 407);
            MinimumSize = new Size(365, 407);
            Name = "IniciarSesionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Iniciar Sesión";
            Load += IniciarSesionForm_Load;
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
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
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}