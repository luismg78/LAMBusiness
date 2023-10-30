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
            this.CancelarVentaLabel = new System.Windows.Forms.Label();
            this.CancelarVentaButton = new System.Windows.Forms.Button();
            this.GuardarVentaButton = new System.Windows.Forms.Button();
            this.GuardarVentaLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CancelarVentaLabel
            // 
            this.CancelarVentaLabel.Location = new System.Drawing.Point(24, 21);
            this.CancelarVentaLabel.Name = "CancelarVentaLabel";
            this.CancelarVentaLabel.Size = new System.Drawing.Size(271, 54);
            this.CancelarVentaLabel.TabIndex = 0;
            this.CancelarVentaLabel.Text = "Presione el botón cancelar si desea anular la venta actual e iniciar una nueva ve" +
    "nta.";
            // 
            // CancelarVentaButton
            // 
            this.CancelarVentaButton.Location = new System.Drawing.Point(24, 78);
            this.CancelarVentaButton.Name = "CancelarVentaButton";
            this.CancelarVentaButton.Size = new System.Drawing.Size(115, 28);
            this.CancelarVentaButton.TabIndex = 1;
            this.CancelarVentaButton.Text = "Cancelar venta";
            this.CancelarVentaButton.UseVisualStyleBackColor = true;
            this.CancelarVentaButton.Click += new System.EventHandler(this.CancelarVentaButton_Click);
            // 
            // GuardarVentaButton
            // 
            this.GuardarVentaButton.Location = new System.Drawing.Point(24, 204);
            this.GuardarVentaButton.Name = "GuardarVentaButton";
            this.GuardarVentaButton.Size = new System.Drawing.Size(115, 28);
            this.GuardarVentaButton.TabIndex = 3;
            this.GuardarVentaButton.Text = "Guardar venta";
            this.GuardarVentaButton.UseVisualStyleBackColor = true;
            this.GuardarVentaButton.Click += new System.EventHandler(this.GuardarVentaButton_Click);
            // 
            // GuardarVentaLabel
            // 
            this.GuardarVentaLabel.Location = new System.Drawing.Point(24, 136);
            this.GuardarVentaLabel.Name = "GuardarVentaLabel";
            this.GuardarVentaLabel.Size = new System.Drawing.Size(271, 59);
            this.GuardarVentaLabel.TabIndex = 2;
            this.GuardarVentaLabel.Text = "Presione el botón guardar si desea almacenar temporalmente la venta actual e inic" +
    "iar una nueva venta.";
            // 
            // CancelarVentaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 288);
            this.Controls.Add(this.GuardarVentaButton);
            this.Controls.Add(this.GuardarVentaLabel);
            this.Controls.Add(this.CancelarVentaButton);
            this.Controls.Add(this.CancelarVentaLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CancelarVentaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cancelar venta";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CancelarVentaForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private Label CancelarVentaLabel;
        private Button CancelarVentaButton;
        private Button GuardarVentaButton;
        private Label GuardarVentaLabel;
    }
}