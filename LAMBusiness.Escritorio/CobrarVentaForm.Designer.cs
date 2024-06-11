namespace LAMBusiness.Escritorio
{
    partial class CobrarVentaForm
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
            PagoConTarjetaLabel = new Label();
            PagoConTarjetaTextBox = new TextBox();
            PorcentajeCobroExtraTextBox = new TextBox();
            PorcentajeCobroExtraLabel = new Label();
            EfectivoTextBox = new TextBox();
            EfectivoLabel = new Label();
            CancelarButton = new Button();
            AceptaButton = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // PagoConTarjetaLabel
            // 
            PagoConTarjetaLabel.AutoSize = true;
            PagoConTarjetaLabel.Location = new Point(12, 24);
            PagoConTarjetaLabel.Name = "PagoConTarjetaLabel";
            PagoConTarjetaLabel.Size = new Size(141, 15);
            PagoConTarjetaLabel.TabIndex = 0;
            PagoConTarjetaLabel.Text = "Pago con tarjeta bancaria";
            // 
            // PagoConTarjetaTextBox
            // 
            PagoConTarjetaTextBox.Location = new Point(12, 42);
            PagoConTarjetaTextBox.Name = "PagoConTarjetaTextBox";
            PagoConTarjetaTextBox.Size = new Size(320, 23);
            PagoConTarjetaTextBox.TabIndex = 1;
            // 
            // PorcentajeCobroExtraTextBox
            // 
            PorcentajeCobroExtraTextBox.BackColor = Color.White;
            PorcentajeCobroExtraTextBox.Location = new Point(12, 103);
            PorcentajeCobroExtraTextBox.Name = "PorcentajeCobroExtraTextBox";
            PorcentajeCobroExtraTextBox.ReadOnly = true;
            PorcentajeCobroExtraTextBox.Size = new Size(320, 23);
            PorcentajeCobroExtraTextBox.TabIndex = 3;
            PorcentajeCobroExtraTextBox.TabStop = false;
            // 
            // PorcentajeCobroExtraLabel
            // 
            PorcentajeCobroExtraLabel.AutoSize = true;
            PorcentajeCobroExtraLabel.Location = new Point(12, 85);
            PorcentajeCobroExtraLabel.Name = "PorcentajeCobroExtraLabel";
            PorcentajeCobroExtraLabel.Size = new Size(206, 15);
            PorcentajeCobroExtraLabel.TabIndex = 2;
            PorcentajeCobroExtraLabel.Text = "Porcentaje por uso de tarjeta bancaria";
            // 
            // EfectivoTextBox
            // 
            EfectivoTextBox.Location = new Point(12, 162);
            EfectivoTextBox.Name = "EfectivoTextBox";
            EfectivoTextBox.Size = new Size(320, 23);
            EfectivoTextBox.TabIndex = 5;
            // 
            // EfectivoLabel
            // 
            EfectivoLabel.AutoSize = true;
            EfectivoLabel.Location = new Point(12, 144);
            EfectivoLabel.Name = "EfectivoLabel";
            EfectivoLabel.Size = new Size(95, 15);
            EfectivoLabel.TabIndex = 4;
            EfectivoLabel.Text = "Pago en efectivo";
            // 
            // CancelarButton
            // 
            CancelarButton.Location = new Point(130, 320);
            CancelarButton.Name = "CancelarButton";
            CancelarButton.Size = new Size(98, 35);
            CancelarButton.TabIndex = 6;
            CancelarButton.Text = "Cancelar";
            CancelarButton.UseVisualStyleBackColor = true;
            // 
            // AceptaButton
            // 
            AceptaButton.BackColor = Color.FromArgb(255, 65, 82);
            AceptaButton.ForeColor = Color.White;
            AceptaButton.Location = new Point(234, 320);
            AceptaButton.Name = "AceptaButton";
            AceptaButton.Size = new Size(98, 35);
            AceptaButton.TabIndex = 7;
            AceptaButton.Text = "Aceptar";
            AceptaButton.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            label1.ImageAlign = ContentAlignment.MiddleRight;
            label1.Location = new Point(12, 220);
            label1.Name = "label1";
            label1.Size = new Size(320, 57);
            label1.TabIndex = 8;
            label1.Text = "$0.00";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CobrarVentaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(344, 378);
            Controls.Add(label1);
            Controls.Add(AceptaButton);
            Controls.Add(CancelarButton);
            Controls.Add(EfectivoTextBox);
            Controls.Add(EfectivoLabel);
            Controls.Add(PorcentajeCobroExtraTextBox);
            Controls.Add(PorcentajeCobroExtraLabel);
            Controls.Add(PagoConTarjetaTextBox);
            Controls.Add(PagoConTarjetaLabel);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CobrarVentaForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Realizar Cobro";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label PagoConTarjetaLabel;
        private TextBox PagoConTarjetaTextBox;
        private TextBox PorcentajeCobroExtraTextBox;
        private Label PorcentajeCobroExtraLabel;
        private TextBox EfectivoTextBox;
        private Label EfectivoLabel;
        private Button CancelarButton;
        private Button AceptaButton;
        private Label label1;
    }
}