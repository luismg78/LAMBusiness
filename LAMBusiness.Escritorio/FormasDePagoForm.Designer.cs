namespace LAMBusiness.Escritorio
{
    partial class FormasDePagoForm
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
            FormasDePagoLabel = new Label();
            FormasDePagoDataGridView = new DataGridView();
            VentaIdDataGridColumna = new DataGridViewTextBoxColumn();
            FormaDePagoDataGridColumna = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)FormasDePagoDataGridView).BeginInit();
            SuspendLayout();
            // 
            // FormasDePagoLabel
            // 
            FormasDePagoLabel.Location = new Point(14, 12);
            FormasDePagoLabel.Name = "FormasDePagoLabel";
            FormasDePagoLabel.Size = new Size(376, 45);
            FormasDePagoLabel.TabIndex = 2;
            FormasDePagoLabel.Text = "Seleccione el registro y presione enter o doble click del mouse para obtener la nueva forma de pago.";
            // 
            // FormasDePagoDataGridView
            // 
            FormasDePagoDataGridView.AllowUserToAddRows = false;
            FormasDePagoDataGridView.AllowUserToDeleteRows = false;
            FormasDePagoDataGridView.AllowUserToResizeColumns = false;
            FormasDePagoDataGridView.AllowUserToResizeRows = false;
            FormasDePagoDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            FormasDePagoDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            FormasDePagoDataGridView.BackgroundColor = Color.White;
            FormasDePagoDataGridView.BorderStyle = BorderStyle.Fixed3D;
            FormasDePagoDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            FormasDePagoDataGridView.Columns.AddRange(new DataGridViewColumn[] { VentaIdDataGridColumna, FormaDePagoDataGridColumna });
            FormasDePagoDataGridView.Location = new Point(14, 61);
            FormasDePagoDataGridView.Margin = new Padding(3, 4, 3, 4);
            FormasDePagoDataGridView.MultiSelect = false;
            FormasDePagoDataGridView.Name = "FormasDePagoDataGridView";
            FormasDePagoDataGridView.ReadOnly = true;
            FormasDePagoDataGridView.RowHeadersWidth = 62;
            FormasDePagoDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            FormasDePagoDataGridView.Size = new Size(376, 361);
            FormasDePagoDataGridView.TabIndex = 3;
            FormasDePagoDataGridView.CellDoubleClick += FormasDePagoDataGridView_CellDoubleClick;
            FormasDePagoDataGridView.KeyDown += FormasDePagoDataGridView_KeyDown;
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
            // FormaDePagoDataGridColumna
            // 
            FormaDePagoDataGridColumna.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            FormaDePagoDataGridColumna.HeaderText = "Forma de pago";
            FormaDePagoDataGridColumna.MinimumWidth = 8;
            FormaDePagoDataGridColumna.Name = "FormaDePagoDataGridColumna";
            FormaDePagoDataGridColumna.ReadOnly = true;
            // 
            // FormasDePagoForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(403, 440);
            Controls.Add(FormasDePagoDataGridView);
            Controls.Add(FormasDePagoLabel);
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormasDePagoForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Formas De Pago";
            FormClosing += FormasDePagoForm_FormClosing;
            Load += FormasDePagoForm_Load;
            ((System.ComponentModel.ISupportInitialize)FormasDePagoDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label FormasDePagoLabel;
        private DataGridView FormasDePagoDataGridView;
        private DataGridViewTextBoxColumn VentaIdDataGridColumna;
        private DataGridViewTextBoxColumn FormaDePagoDataGridColumna;
    }
}