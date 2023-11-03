namespace LAMBusiness.Escritorio
{
    partial class BuscarForm
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
            this.NombreDelProductoLabel = new System.Windows.Forms.Label();
            this.PrecioVentaLabel = new System.Windows.Forms.Label();
            this.ExistenciaLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NombreDelProductoLabel
            // 
            this.NombreDelProductoLabel.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.NombreDelProductoLabel.Location = new System.Drawing.Point(40, 34);
            this.NombreDelProductoLabel.Name = "NombreDelProductoLabel";
            this.NombreDelProductoLabel.Size = new System.Drawing.Size(444, 120);
            this.NombreDelProductoLabel.TabIndex = 0;
            this.NombreDelProductoLabel.Text = "Nombre del producto";
            // 
            // PrecioVentaLabel
            // 
            this.PrecioVentaLabel.Font = new System.Drawing.Font("Segoe UI", 31.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.PrecioVentaLabel.Location = new System.Drawing.Point(40, 169);
            this.PrecioVentaLabel.Name = "PrecioVentaLabel";
            this.PrecioVentaLabel.Size = new System.Drawing.Size(444, 64);
            this.PrecioVentaLabel.TabIndex = 1;
            this.PrecioVentaLabel.Text = "$0.00";
            this.PrecioVentaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExistenciaLabel
            // 
            this.ExistenciaLabel.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ExistenciaLabel.Location = new System.Drawing.Point(40, 284);
            this.ExistenciaLabel.Name = "ExistenciaLabel";
            this.ExistenciaLabel.Size = new System.Drawing.Size(444, 64);
            this.ExistenciaLabel.TabIndex = 2;
            this.ExistenciaLabel.Text = "Existencia : 0";
            this.ExistenciaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BuscarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 390);
            this.Controls.Add(this.ExistenciaLabel);
            this.Controls.Add(this.PrecioVentaLabel);
            this.Controls.Add(this.NombreDelProductoLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BuscarForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buscar producto";
            this.Load += new System.EventHandler(this.BuscarForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Label NombreDelProductoLabel;
        private Label PrecioVentaLabel;
        private Label ExistenciaLabel;
    }
}