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
            RecuperarVentasDataGridView = new DataGridView();
            RecuperarVentasLabel = new Label();
            VentaIdDataGridColumna = new DataGridViewTextBoxColumn();
            FechaDataGridColumna = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)RecuperarVentasDataGridView).BeginInit();
            SuspendLayout();
            // 
            // RecuperarVentasDataGridView
            // 
            RecuperarVentasDataGridView.AllowUserToAddRows = false;
            RecuperarVentasDataGridView.AllowUserToDeleteRows = false;
            RecuperarVentasDataGridView.AllowUserToResizeColumns = false;
            RecuperarVentasDataGridView.AllowUserToResizeRows = false;
            RecuperarVentasDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            RecuperarVentasDataGridView.BackgroundColor = Color.White;
            RecuperarVentasDataGridView.BorderStyle = BorderStyle.Fixed3D;
            RecuperarVentasDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            RecuperarVentasDataGridView.Columns.AddRange(new DataGridViewColumn[] { VentaIdDataGridColumna, FechaDataGridColumna });
            RecuperarVentasDataGridView.Location = new Point(17, 78);
            RecuperarVentasDataGridView.Margin = new Padding(4, 5, 4, 5);
            RecuperarVentasDataGridView.MultiSelect = false;
            RecuperarVentasDataGridView.Name = "RecuperarVentasDataGridView";
            RecuperarVentasDataGridView.ReadOnly = true;
            RecuperarVentasDataGridView.RowHeadersWidth = 62;
            RecuperarVentasDataGridView.RowTemplate.Height = 25;
            RecuperarVentasDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RecuperarVentasDataGridView.Size = new Size(470, 452);
            RecuperarVentasDataGridView.TabIndex = 0;
            RecuperarVentasDataGridView.CellDoubleClick += RecuperarVentasDataGridView_CellDoubleClick;
            RecuperarVentasDataGridView.KeyDown += RecuperarVentasDataGridView_KeyDown;
            // 
            // RecuperarVentasLabel
            // 
            RecuperarVentasLabel.Location = new Point(17, 15);
            RecuperarVentasLabel.Margin = new Padding(4, 0, 4, 0);
            RecuperarVentasLabel.Name = "RecuperarVentasLabel";
            RecuperarVentasLabel.Size = new Size(470, 57);
            RecuperarVentasLabel.TabIndex = 1;
            RecuperarVentasLabel.Text = "Seleccione el registro y presione enter o doble click del mouse para obtener el detalle de la venta.";
            // 
            // VentaIdDataGridColumna
            // 
            VentaIdDataGridColumna.Frozen = true;
            VentaIdDataGridColumna.HeaderText = "Id";
            VentaIdDataGridColumna.MinimumWidth = 8;
            VentaIdDataGridColumna.Name = "VentaIdDataGridColumna";
            VentaIdDataGridColumna.ReadOnly = true;
            VentaIdDataGridColumna.Visible = false;
            // 
            // FechaDataGridColumna
            // 
            FechaDataGridColumna.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            FechaDataGridColumna.HeaderText = "Fecha";
            FechaDataGridColumna.MinimumWidth = 8;
            FechaDataGridColumna.Name = "FechaDataGridColumna";
            FechaDataGridColumna.ReadOnly = true;
            // 
            // RecuperarVentasForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(504, 550);
            Controls.Add(RecuperarVentasLabel);
            Controls.Add(RecuperarVentasDataGridView);
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RecuperarVentasForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Recuperar ventas";
            FormClosing += RecuperarVentasForm_FormClosing;
            Load += RecuperarVentasForm_Load;
            ((System.ComponentModel.ISupportInitialize)RecuperarVentasDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView RecuperarVentasDataGridView;
        private Label RecuperarVentasLabel;
        private DataGridViewTextBoxColumn VentaIdDataGridColumna;
        private DataGridViewTextBoxColumn FechaDataGridColumna;
    }
}