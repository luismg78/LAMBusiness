namespace LAMBusiness.Escritorio
{
    partial class CancelarVentaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CancelarVentaLabel = new Label();
            CancelarVentaButton = new Button();
            GuardarVentaButton = new Button();
            GuardarVentaLabel = new Label();
            SuspendLayout();
            // 
            // CancelarVentaLabel
            // 
            CancelarVentaLabel.Location = new Point(34, 35);
            CancelarVentaLabel.Margin = new Padding(4, 0, 4, 0);
            CancelarVentaLabel.Name = "CancelarVentaLabel";
            CancelarVentaLabel.Size = new Size(387, 90);
            CancelarVentaLabel.TabIndex = 0;
            CancelarVentaLabel.Text = "Presione el botón cancelar si desea anular la venta actual e iniciar una nueva venta.";
            // 
            // CancelarVentaButton
            // 
            CancelarVentaButton.BackColor = Color.Brown;
            CancelarVentaButton.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CancelarVentaButton.ForeColor = Color.White;
            CancelarVentaButton.Location = new Point(34, 115);
            CancelarVentaButton.Margin = new Padding(4, 5, 4, 5);
            CancelarVentaButton.Name = "CancelarVentaButton";
            CancelarVentaButton.Size = new Size(387, 64);
            CancelarVentaButton.TabIndex = 1;
            CancelarVentaButton.Text = "Cancelar venta";
            CancelarVentaButton.UseVisualStyleBackColor = false;
            CancelarVentaButton.Click += CancelarVentaButton_Click;
            // 
            // GuardarVentaButton
            // 
            GuardarVentaButton.BackColor = Color.Gold;
            GuardarVentaButton.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            GuardarVentaButton.Location = new Point(34, 340);
            GuardarVentaButton.Margin = new Padding(4, 5, 4, 5);
            GuardarVentaButton.Name = "GuardarVentaButton";
            GuardarVentaButton.Size = new Size(387, 61);
            GuardarVentaButton.TabIndex = 3;
            GuardarVentaButton.Text = "Guardar venta";
            GuardarVentaButton.UseVisualStyleBackColor = false;
            GuardarVentaButton.Click += GuardarVentaButton_Click;
            // 
            // GuardarVentaLabel
            // 
            GuardarVentaLabel.Location = new Point(34, 227);
            GuardarVentaLabel.Margin = new Padding(4, 0, 4, 0);
            GuardarVentaLabel.Name = "GuardarVentaLabel";
            GuardarVentaLabel.Size = new Size(387, 98);
            GuardarVentaLabel.TabIndex = 2;
            GuardarVentaLabel.Text = "Presione el botón guardar si desea almacenar temporalmente la venta actual e iniciar una nueva venta.";
            // 
            // CancelarVentaForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(454, 480);
            Controls.Add(GuardarVentaButton);
            Controls.Add(GuardarVentaLabel);
            Controls.Add(CancelarVentaButton);
            Controls.Add(CancelarVentaLabel);
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CancelarVentaForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cancelar venta";
            FormClosing += CancelarVentaForm_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private Label CancelarVentaLabel;
        private Button CancelarVentaButton;
        private Button GuardarVentaButton;
        private Label GuardarVentaLabel;
    }
}