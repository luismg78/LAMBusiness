namespace LAMBusiness.Escritorio
{
    partial class RecuperarVentasForm
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
            this.RecuperarVentasDataGridView = new System.Windows.Forms.DataGridView();
            this.VentaIdDataGridColumna = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FechaDataGridColumna = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecuperarVentasLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RecuperarVentasDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // RecuperarVentasDataGridView
            // 
            this.RecuperarVentasDataGridView.AllowUserToAddRows = false;
            this.RecuperarVentasDataGridView.AllowUserToDeleteRows = false;
            this.RecuperarVentasDataGridView.AllowUserToResizeColumns = false;
            this.RecuperarVentasDataGridView.AllowUserToResizeRows = false;
            this.RecuperarVentasDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.RecuperarVentasDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.RecuperarVentasDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.RecuperarVentasDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RecuperarVentasDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentaIdDataGridColumna,
            this.FechaDataGridColumna});
            this.RecuperarVentasDataGridView.Location = new System.Drawing.Point(12, 47);
            this.RecuperarVentasDataGridView.MultiSelect = false;
            this.RecuperarVentasDataGridView.Name = "RecuperarVentasDataGridView";
            this.RecuperarVentasDataGridView.ReadOnly = true;
            this.RecuperarVentasDataGridView.RowTemplate.Height = 25;
            this.RecuperarVentasDataGridView.Size = new System.Drawing.Size(329, 271);
            this.RecuperarVentasDataGridView.TabIndex = 0;
            this.RecuperarVentasDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RecuperarVentasDataGridView_CellDoubleClick);
            this.RecuperarVentasDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RecuperarVentasDataGridView_KeyDown);
            // 
            // VentaIdDataGridColumna
            // 
            this.VentaIdDataGridColumna.Frozen = true;
            this.VentaIdDataGridColumna.HeaderText = "Id";
            this.VentaIdDataGridColumna.Name = "VentaIdDataGridColumna";
            this.VentaIdDataGridColumna.ReadOnly = true;
            this.VentaIdDataGridColumna.Visible = false;
            // 
            // FechaDataGridColumna
            // 
            this.FechaDataGridColumna.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.FechaDataGridColumna.Frozen = true;
            this.FechaDataGridColumna.HeaderText = "Fecha";
            this.FechaDataGridColumna.Name = "FechaDataGridColumna";
            this.FechaDataGridColumna.ReadOnly = true;
            this.FechaDataGridColumna.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FechaDataGridColumna.Width = 286;
            // 
            // RecuperarVentasLabel
            // 
            this.RecuperarVentasLabel.Location = new System.Drawing.Point(12, 9);
            this.RecuperarVentasLabel.Name = "RecuperarVentasLabel";
            this.RecuperarVentasLabel.Size = new System.Drawing.Size(329, 34);
            this.RecuperarVentasLabel.TabIndex = 1;
            this.RecuperarVentasLabel.Text = "Seleccione el registro y presione enter o doble click del mouse para obtener el d" +
    "etalle de la venta.";
            // 
            // RecuperarVentasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 330);
            this.Controls.Add(this.RecuperarVentasLabel);
            this.Controls.Add(this.RecuperarVentasDataGridView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecuperarVentasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recuperar ventas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RecuperarVentasForm_FormClosing);
            this.Load += new System.EventHandler(this.RecuperarVentasForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RecuperarVentasDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView RecuperarVentasDataGridView;
        private Label RecuperarVentasLabel;
        private DataGridViewTextBoxColumn VentaIdDataGridColumna;
        private DataGridViewTextBoxColumn FechaDataGridColumna;
    }
}